using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Math;

namespace Fusee.Engine
{
    class StereoModeOculus : IStereoMode
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
            get { return Stereo3DMode.Oculus; }
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

        #region Shader
        // variables and shader for Oculus Rift
        private const float K0 = 1.0f;
        private const float K1 = 0.22f;
        private const float K2 = 0.24f;
        private const float K3 = 0.0f;

        private IShaderParam _lensCenterParam;
        private IShaderParam _screenCenterParam;
        private IShaderParam _scaleParam;
        private IShaderParam _scaleInParam;
        private IShaderParam _hdmWarpParam;

        private const string OculusVs = @"
            attribute vec3 fuVertex;
            attribute vec2 fuUV;
            attribute vec4 fuColor;

            varying vec2 vUV;

            void main()
            {
                vUV = fuUV;
                gl_Position = vec4(fuVertex, 1);
            }";

        private const string OculusPs = @"
            uniform sampler2D vTexture;

            uniform vec2 LensCenter;
            uniform vec2 ScreenCenter;
            uniform vec2 Scale;
            uniform vec2 ScaleIn;
            uniform vec4 HmdWarpParam;

            varying vec2 vUV;

            vec2 HmdWarp(vec2 texIn)
            {
                vec2 theta = (texIn - LensCenter) * ScaleIn;
                float rSq = theta.x * theta.x + theta.y * theta.y;
                vec2 theta1 = theta * (HmdWarpParam.x + HmdWarpParam.y * rSq + HmdWarpParam.z * rSq * rSq + HmdWarpParam.w * rSq * rSq * rSq);
                return LensCenter + Scale * theta1;
            }

            void main()
            {
                vec2 tc = HmdWarp(vUV.xy);
	            if (any(bvec2(clamp(tc,ScreenCenter-vec2(0.25,0.5), ScreenCenter+vec2(0.25,0.5)) - tc)))
	            {
		            gl_FragColor = vec4(0.2, 0.2, 0.2, 1.0);
		            return;
	            }

	            gl_FragColor = texture(vTexture, tc);
            }";
#endregion

        public StereoModeOculus(Stereo3D s3d, Stereo3DMode mode, int width, int height)
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

            _guiLImage = new GUIImage(null, 0, 0, _screenWidth/2, _screenHeight);
            _guiLImage.AttachToContext(rc);
            _guiLImage.Refresh();

            _guiRImage = new GUIImage(null, _screenWidth/2, 0, _screenWidth/2, _screenHeight);
            _guiRImage.AttachToContext(rc);
            _guiRImage.Refresh();

            _shaderProgram = _rc.CreateShader(OculusVs, OculusPs);
            _shaderTexture = _shaderProgram.GetShaderParam("vTexture");

            _lensCenterParam = _shaderProgram.GetShaderParam("LensCenter");
            _screenCenterParam = _shaderProgram.GetShaderParam("ScreenCenter");
            _scaleParam = _shaderProgram.GetShaderParam("Scale");
            _scaleInParam = _shaderProgram.GetShaderParam("ScaleIn");
            _hdmWarpParam = _shaderProgram.GetShaderParam("HmdWarpParam");
        }

        public void DetachFromContext(RenderContext rc)
        {
            if (_guiLImage != null)
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
            _currentEye = eye;

            switch (eye)
            {
                case Stereo3DEye.Left:
                    _rc.Viewport(0, cuttingEdge, _screenWidth / 2, _screenHeight - cuttingEdge);
                    break;

                case Stereo3DEye.Right:
                    _rc.Viewport(_screenWidth / 2, cuttingEdge, _screenWidth / 2, _screenHeight - cuttingEdge);
                    break;
            }


            _rc.ClearColor = _clearColor;
            _rc.Clear(ClearFlags.Color | ClearFlags.Depth);
        }

        public void Save()
        {
            const int picTrans = 81;
            switch (_currentEye)
            {
                case Stereo3DEye.Left:
                    _rc.GetBufferContent(new Rectangle(-picTrans, 0, _screenWidth - picTrans, _screenHeight),
                        _contentLTex);
                    break;
                case Stereo3DEye.Right:
                    _rc.GetBufferContent(new Rectangle(+picTrans, 0, _screenWidth + picTrans, _screenHeight),
                        _contentRTex);
                    break;
            }

            _rc.Viewport(0, 0, _screenWidth, _screenHeight);
        }

        public void Display()
        {
            if (CheckState())
                return;

            _rc.ClearColor = new float4(0, 0, 0, 0); // _clearColor
            _rc.Clear(ClearFlags.Color | ClearFlags.Depth);

            var currShader = _rc.CurrentShader;

            _rc.SetShader(_shaderProgram);

            RenderEye(Stereo3DEye.Left);
            RenderEye(Stereo3DEye.Right);

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

        private void RenderEye(Stereo3DEye eye)
        {
            var scale = new float2(0.1469278f, 0.2350845f);
            var scaleIn = new float2(2, 2.5f);
            var hdmWarp = new float4(K0, K1, K2, K3);

            float2 lensCenter;
            float2 screenCenter;

            if (eye == Stereo3DEye.Left)
            {
                _rc.SetShaderParamTexture(_shaderTexture, _contentLTex);

                lensCenter = new float2(0.3125f, 0.5f);
                screenCenter = new float2(0.25f, 0.5f);
            }
            else
            {
                _rc.SetShaderParamTexture(_shaderTexture, _contentRTex);

                lensCenter = new float2(0.6875f, 0.5f);
                screenCenter = new float2(0.75f, 0.5f);
            }

            _rc.SetShaderParam(_lensCenterParam, lensCenter);
            _rc.SetShaderParam(_screenCenterParam, screenCenter);
            _rc.SetShaderParam(_scaleParam, scale);
            _rc.SetShaderParam(_scaleInParam, scaleIn);
            _rc.SetShaderParam(_hdmWarpParam, hdmWarp);

            _rc.Render(eye == Stereo3DEye.Left ? _guiLImage.GUIMesh : _guiRImage.GUIMesh);
        }
    }
}
