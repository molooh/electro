using System;
using Fusee.Math;

namespace Fusee.Engine
{
    /// <summary>
    /// The enum for the eye side selection.
    /// </summary>
    public enum Stereo3DEye
    {
        /// <summary>
        /// The left eye = 0.
        /// </summary>
        Left,
        /// <summary>
        /// The right eye = 1.
        /// </summary>
        Right
    }

    /// <summary>
    /// The enum for the 3D Mode selection.
    /// </summary>
    public enum Stereo3DMode
    {
        /// <summary>
        /// The anaglyph mode = 0.
        /// </summary>
        Anaglyph,
        /// <summary>
        /// The oculus rift mode = 1.
        /// </summary>
        Oculus,
        /// <summary>
        /// The over under mode = 2.
        /// </summary>
        OverUnder,
        /// <summary>
        /// The left right mode = 3.
        /// </summary>
        LeftRight
    }

    public enum StereoRenderState
    {
        /// <summary>
        /// This is the normal state. Everything is okey.
        /// </summary>
        Clean,
        /// <summary>
        /// Something is not okey, instance should be reinitialized.
        /// This can occure after a resize has been done etc.
        /// </summary>
        Dirty
    }

    internal static class Stereo3DParams
    {
        internal static float EyeDistance = 30f;
        internal static float Convergence = 0f;
    }

    /// <summary>
    /// Rendering of stereo 3D graphics in different stereo modes.
    /// </summary>
    public class Stereo3D
    {
        private IStereoMode _stereoHandler;
        private RenderContext _rc;
        
        /// <summary>
        /// Gets the current eye.
        /// </summary>
        /// <value>
        /// The current eye side enum. left=0, right=1.
        /// </value>
        public Stereo3DEye CurrentEye
        {
            get { return _stereoHandler.CurrentEye; }
            set { _stereoHandler.CurrentEye = value; }
        }

        /// <summary>
        /// Returns the screen width the current mode has.
        /// /// Sets the screen width current mode has.
        /// </summary>
        public int ScreenWidth 
        {
            get { return _stereoHandler.ScreenWidth; }
            set { _stereoHandler.ScreenWidth = value; }
        }

        /// <summary>
        /// Returns the screen height the current mode has.
        /// Sets the screen height current mode has.
        /// </summary>
        public int ScreenHeight
        {
            get { return _stereoHandler.ScreenHeight; }
            set { _stereoHandler.ScreenHeight = value; }
        }

        public RenderContext RenderContext
        {
            get { return _rc; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stereo3D"/> class.
        /// </summary>
        /// <param name="mode">The 3D rendering mode. anaglyph=0, oculus=1.</param>
        /// <param name="width">The width of the render output in pixels.</param>
        /// <param name="height">The height of the render output in pixels.</param>
        public Stereo3D(RenderContext rc, Stereo3DMode mode, int width, int height)
        {
            _rc = rc;
            ChangeStereoMode(mode, width, height);
        }

        /// <summary>
        /// Initializes a StereoMode object like Oculus, Anaglyph, OverUnder or LeftRight.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ChangeStereoMode(Stereo3DMode mode, int width, int height)
        {
            if(_stereoHandler != null)
                _stereoHandler.DetachFromContext(_rc);

            switch (mode)
            {
                case Stereo3DMode.Anaglyph:
                    _stereoHandler = new StereoModeAnaglyph(this, mode, width, height);
                break;
                case Stereo3DMode.Oculus:
                    _stereoHandler = new StereoModeOculus(this, mode, width, height);
                break;
                case Stereo3DMode.OverUnder:
                    _stereoHandler = new StereoModeOverUnder(this, mode, width, height);
                break;
                case Stereo3DMode.LeftRight:
                    _stereoHandler = new StereoModeLeftRight(this, mode, width, height);
                break;
                default:
                    _stereoHandler = new StereoModeOverUnder(this, mode, width, height);
                break;
            }
        }

        /// <summary>
        /// Call through.
        /// </summary>
        /// <param name="rc">The <see cref="RenderContext"/> object to be used.</param>
        public void AttachToContext(RenderContext rc)
        {
            _rc = rc;
            _stereoHandler.AttachToContext(rc);
        }

        /// <summary>
        /// Call through.
        /// </summary>
        /// <param name="eye">The <see cref="Stereo3DEye"/>.</param>
        public void Prepare(Stereo3DEye eye)
        {
            _stereoHandler.Prepare(eye);
        }

        /// <summary>
        /// Call through.
        /// </summary>
        public void Save()
        {
            _stereoHandler.Save();
        }

        /// <summary>
        /// Call through.
        /// </summary>
        public void Display()
        {
            _stereoHandler.Display();
        }


        /// <summary>
        /// Call through.
        /// </summary>
        /// <returns></returns>
        public float CalculateAspectRatio()
        {
            return _stereoHandler.CalculateAspectRatio();
        }

        /// <summary>
        /// Call through.
        /// </summary>
        /// <param name="currentEye"></param>
        /// <param name="eyeF"></param>
        /// <param name="targetF"></param>
        /// <param name="upF"></param>
        /// <returns></returns>
        public float4x4 LookAt3D(Stereo3DEye currentEye, float3 eyeF, float3 targetF, float3 upF)
        {
            return _stereoHandler.LookAt3D(currentEye, eyeF, targetF, upF);
        }
    }
}