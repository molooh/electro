using System;
using Fusee.Engine;
using Fusee.Math;

namespace Fusee.Engine
{
    interface IStereoMode
    {
        /// <summary>
        /// Returns the active StereoMode.
        /// </summary>
        Stereo3DMode ActiveMode { get; }
        
        /// <summary>
        /// Returns the current eye.
        /// </summary>
        Stereo3DEye CurrentEye { get; set; }

        /// <summary>
        /// Returns the width.
        /// </summary>
        int ScreenWidth { get; set; }

        /// <summary>
        /// Returns the height.
        /// </summary>
        int ScreenHeight { get; set; }

        /// <summary>
        /// Attaches the render textures to the context.
        /// Attaches some shaderds.
        /// </summary>
        /// <param name="rc"></param>
        void AttachToContext(RenderContext rc);

        /// <summary>
        /// Detach everything from context.
        /// </summary>
        /// <param name="rc">RenderContext</param>
        void DetachFromContext(RenderContext rc);

        /// <summary>
        /// Prepares the specified eye for 3d rendering.
        /// </summary>
        /// <param name="eye"></param>
        void Prepare(Stereo3DEye eye);

        /// <summary>
        /// Saves a specific buffer region to the render context.
        /// </summary>
        void Save();

        /// <summary>
        /// Displays the result as rendering output on the <see cref="RenderContext"/>.
        /// </summary>
        void Display();

        /// <summary>
        /// Checks the state if clean or not.
        /// </summary>
        bool CheckState();

        /// <summary>
        /// Calculates an aspect ratio for rendering dependend on the StereoMode.
        /// </summary>
        /// <returns></returns>
        float CalculateAspectRatio();

        /// <summary>
        /// Aligns the <see cref="Stereo3DEye"/> to the target point.
        /// </summary>
        /// <param name="eye">The <see cref="Stereo3DEye"/>.</param>
        /// <param name="eyeV">The eye vector.</param>
        /// <param name="target">The target.</param>
        /// <param name="up">Up vector.</param>
        /// <returns>A Matrix that represents the current eye's orientation towards a target point.</returns>
        float4x4 LookAt3D(Stereo3DEye eye, float3 eyeV, float3 target, float3 up);

    }
}
