using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Math;

namespace Fusee.Engine
{
    class StereoModeAnaglyph : IStereoMode
    {
        #region Fields
        private Stereo3D _stereo3D;

        private RenderContext _rc;
        private float4 _clearColor;

        private Stereo3DEye _currentEye;
        private StereoRenderState _renderState;

        private GUIImage _guiLImage;
        private GUIImage _guiRImage;

        private ShaderProgram _shaderProgram;
        private ShaderProgram _spTexture;

        private IShaderParam _shaderTexture;

        private int _screenWidth;
        private int _screenHeight;

        private ITexture _contentLTex;
        private ITexture _contentRTex;
        #endregion

        #region Properties
        /// <summary>
        /// Sets or Gets the ScreenWidth.
        /// Sets the state flag to dirty.
        /// </summary>
        public int ScreenWidth
        {
            get { return _screenWidth; }
            set
            {
                _screenWidth = value;
                _renderState = StereoRenderState.Dirty;
            }
        }

        /// <summary>
        /// Sets or Gets the ScreenHeight.
        /// Sets the state flag to dirty.
        /// </summary>
        public int ScreenHeight
        {
            get { return _screenHeight; }
            set
            {
                _screenHeight = value;
                _renderState = StereoRenderState.Dirty;
            }
        }

        /// <summary>
        /// Returns the current stereo mode.
        /// </summary>
        public Stereo3DMode ActiveMode
        {
            get { return Stereo3DMode.Anaglyph;}
        }

        /// <summary>
        /// Returns the current eye.
        /// </summary>
        public Stereo3DEye CurrentEye
        {
            get { return _currentEye; }
            set { _currentEye = value; }
        }
        #endregion

        #region Anaglyph

        // shader for anagylph mode
        private const string AnaglyphVs = @"
            attribute vec3 fuVertex;
            attribute vec2 fuUV;
            attribute vec4 fuColor;

            varying vec2 vUV;

            void main()
            {
                vUV = fuUV;
                gl_Position = vec4(fuVertex, 1);
            }";

        private const string AnaglyphPs = @"
            #ifdef GL_ES
                precision highp float;
            #endif
        
            uniform sampler2D vTexture;
            varying vec2 vUV;

            void main()
            {
                vec4 colTex = texture2D(vTexture, vUV);
                vec4 _redBalance = vec4(0.1, 0.65, 0.25, 0);
                float _redColor = (colTex.r * _redBalance.r + colTex.g * _redBalance.g + colTex.b * _redBalance.b) * 1.5;
                gl_FragColor = vec4(_redColor, colTex.g, colTex.b, 1) * 1.4; // * dot(vNormal, vec3(0, 0, -1))  lefthanded change???
            }";

        #endregion

        public StereoModeAnaglyph(Stereo3D s3d, Stereo3DMode mode, int width, int height)
        {
            _stereo3D = s3d;
            _screenWidth = width;
            _screenHeight = height;

            _renderState = StereoRenderState.Clean;

            AttachToContext(_stereo3D.RenderContext);
        }

        public void AttachToContext(RenderContext rc)
        {
            _rc = rc;
            _clearColor = rc.ClearColor;

            var imgData = _rc.CreateImage(_screenWidth, _screenHeight, "black");
            _contentLTex = _rc.CreateTexture(imgData);
            _contentRTex = _rc.CreateTexture(imgData);

            _shaderProgram = _rc.CreateShader(AnaglyphVs, AnaglyphPs);
            _shaderTexture = _shaderProgram.GetShaderParam("vTexture");

            _guiLImage = new GUIImage(null, 0, 0, _screenWidth, _screenHeight);
            _guiLImage.AttachToContext(rc);
            _guiLImage.Refresh();
        }

        public void DetachFromContext(RenderContext rc)
        {
            if(_guiLImage != null)
                _guiLImage.DetachFromContext();

            if (_guiRImage != null)
                _guiRImage.DetachFromContext();

            _guiLImage = null;
            _guiRImage = null;
        }

        public void Prepare(Stereo3DEye eye)
        {
            _currentEye = eye;
            const int cuttingEdge = 100;

            _rc.ClearColor = _clearColor;
            _rc.Clear(ClearFlags.Color | ClearFlags.Depth);            
        }

        public void Save()
        {
            _rc.GetBufferContent(new Rectangle(0, 0, _screenWidth, _screenHeight), (_currentEye == Stereo3DEye.Left) ? _contentLTex : _contentRTex);
        }

        public void Display()
        {
            if (CheckState())
                return;

            _rc.ClearColor = new float4(0, 0, 0, 0); // _clearColor
            _rc.Clear(ClearFlags.Color | ClearFlags.Depth);

            var currShader = _rc.CurrentShader;

            _rc.SetShader(_shaderProgram);

            RenderEye(Stereo3DEye.Left, true, false, false, false);
            _rc.Clear(ClearFlags.Depth);
            RenderEye(Stereo3DEye.Right, false, true, true, false);

            _rc.ColorMask(true, true, true, false);

            _rc.SetShader(currShader);
        }

        public bool CheckState()
        {
            // StereoRenderState is propably dirty.
            // so we have to do some clean ups etc.
            if (_renderState == StereoRenderState.Dirty)
            {
                DetachFromContext(_stereo3D.RenderContext);

                AttachToContext(_rc);

                _renderState = StereoRenderState.Clean;

                return true;
            }

            return false;
        }

        public float CalculateAspectRatio()
        {
            return _screenWidth / (float)_screenHeight;  
        }

        public float4x4 LookAt3D(Stereo3DEye eye, float3 eyeV, float3 target, float3 up)
        {
            var x = (eye == Stereo3DEye.Left) ? eyeV.x - Stereo3DParams.EyeDistance : eyeV.x + Stereo3DParams.EyeDistance;

            var newEye = new float3(x, eyeV.y, eyeV.z);
            var newTarget = new float3(target.x, target.y, target.z);

            // change lookat ?? lefthanded change
            return float4x4.LookAt(newEye, newTarget, up);
        }

        private void RenderEye(Stereo3DEye eye, bool red, bool green, bool blue, bool alpha)
        {
            _rc.SetShaderParamTexture(_shaderTexture, eye == Stereo3DEye.Left ? _contentLTex : _contentRTex);
            _rc.ColorMask(red, green, blue, alpha);

            // TODO - change lookat ?? lefthanded change
            _rc.Render(_guiLImage.GUIMesh);
        }

    }
}
