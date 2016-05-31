#define GUI_SIMPLE

using System.Diagnostics;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Imp.Input.NatNet;
using Fusee.Engine.Imp.Input.Vrpn;
using Fusee.Math.Core;
using Fusee.Serialization;
using static Fusee.Engine.Core.Input;
#if GUI_SIMPLE
using Fusee.Engine.Core.GUI;
#endif

namespace Fusee.Engine.Examples.Inputs3D.Core
{

    [FuseeApplication(Name = "3DInputs Example", Description = "A very 3D example.")]
    public class Inputs3D : RenderCanvas
    {
        private SceneContainer _rocketScene;
        private SceneContainer _cubeScene;
        private SceneRenderer _rocketRenderer;
        private SceneRenderer _cubeRenderer;

#if GUI_SIMPLE
        private GUIHandler _guiHandler;

        private GUIButton _guiFuseeLink;
        private GUIImage _guiFuseeLogo;
        private FontMap _guiLatoBlack;
        private GUIText _guiSubText;
        private float _subtextHeight;
        private float _subtextWidth;
#endif
        private NatNetSkeleton _patrick;
        private NatNetRigidbody _axisN;
        private VrpnTrackerDevice _axisV;

        // Init is called on startup. 
        public override void Init()
        {
#if GUI_SIMPLE
            _guiHandler = new GUIHandler();
            _guiHandler.AttachToContext(RC);

            _guiFuseeLink = new GUIButton(6, 6, 157, 87);
            _guiFuseeLink.ButtonColor = new float4(0, 0, 0, 0);
            _guiFuseeLink.BorderColor = new float4(0, 0.6f, 0.2f, 1);
            _guiFuseeLink.BorderWidth = 0;
            _guiFuseeLink.OnGUIButtonDown += _guiFuseeLink_OnGUIButtonDown;
            _guiFuseeLink.OnGUIButtonEnter += _guiFuseeLink_OnGUIButtonEnter;
            _guiFuseeLink.OnGUIButtonLeave += _guiFuseeLink_OnGUIButtonLeave;
            _guiHandler.Add(_guiFuseeLink);
            _guiFuseeLogo = new GUIImage(AssetStorage.Get<ImageData>("FuseeLogo150.png"), 10, 10, -5, 150, 80);
            _guiHandler.Add(_guiFuseeLogo);
            var fontLato = AssetStorage.Get<Font>("Lato-Black.ttf");
            fontLato.UseKerning = true;
            _guiLatoBlack = new FontMap(fontLato, 18);
            _guiSubText = new GUIText("Input3D FUSEE Example", _guiLatoBlack, 100, 100);
            _guiSubText.TextColor = new float4(0.05f, 0.25f, 0.15f, 0.8f);
            _guiHandler.Add(_guiSubText);
            _subtextWidth = GUIText.GetTextWidth(_guiSubText.Text, _guiLatoBlack);
            _subtextHeight = GUIText.GetTextHeight(_guiSubText.Text, _guiLatoBlack);
#endif

            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(1, 1, 1, 1);

            // Load the rocket model
            // var ser = new Serializer();
            // _rocketScene = ser.Deserialize(IO.StreamFromFile(@"Assets/RocketModel.fus", FileMode.Open), null, typeof(SceneContainer)) as SceneContainer;
            _rocketScene = AssetStorage.Get<SceneContainer>("RocketModel.fus");
            _cubeScene = AssetStorage.Get<SceneContainer>("Cube.fus");

            // Wrap a SceneRenderer around the model.
            _rocketRenderer = new SceneRenderer(_rocketScene);
            _cubeRenderer = new SceneRenderer(_cubeScene);

            foreach (var inputDevice in Devices)
            {
                if (inputDevice.Id.Contains("Patrick"))
                {
                    Debug.WriteLine("FOUND: " + inputDevice.Id);
                    _patrick = (NatNetSkeleton)inputDevice;
                }
                else if (inputDevice.Id.Contains("axis@"))
                {
                    Debug.WriteLine("FOUND: " + inputDevice.Id);
                    _axisV = (VrpnTrackerDevice)inputDevice;
                    _axisV.Orientation = CoordinatesystemOrientation.RightHanded;
                }
                else if (inputDevice.Id.Contains("axis_"))
                {
                    Debug.WriteLine("FOUND: " + inputDevice.Id);
                    _axisN = (NatNetRigidbody)inputDevice;
                }
                else
                    Debug.WriteLine(inputDevice.Id);
            }

        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            var mtxRot = float4x4.Identity;
            var mtxCam = float4x4.LookAt(0, 25, -30, 0, 11, 0, 0, 1, 0);
            var mtxTransl = float4x4.Identity;
            var mtxScaleCube = float4x4.CreateScale(0.5f);
            var mtxScaleRocket = float4x4.CreateScale(0.01f);

            if (_patrick != null)
            {
                foreach (var bone in _patrick.Bones)
                {
                    mtxRot = bone.RotationMatrix;
                    mtxTransl = float4x4.CreateTranslation(bone.Position * 10);
                    RC.ModelView = mtxCam * mtxTransl * mtxRot * mtxScaleCube;
                    _cubeRenderer.Render(RC);
                }
            }

            if (_axisV != null)
            {
                mtxRot = _axisV.RotationMatrix;
                mtxTransl = float4x4.CreateTranslation(_axisV.Position * 10);
                RC.ModelView = mtxCam * mtxTransl * mtxRot * mtxScaleRocket;
                _rocketRenderer.Render(RC);
            }

#if GUI_SIMPLE
            _guiHandler.RenderGUI();
#endif

            Present();

            if (Keyboard.GetKey(KeyCodes.Escape))
            {
                this.CloseGameWindow();
            }
        }

        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;

#if GUI_SIMPLE
            _guiSubText.PosX = (int)((Width - _subtextWidth) / 2);
            _guiSubText.PosY = (int)(Height - _subtextHeight - 3);

            _guiHandler.Refresh();
#endif

        }

#if GUI_SIMPLE
        private void _guiFuseeLink_OnGUIButtonLeave(GUIButton sender, GUIButtonEventArgs mea)
        {
            _guiFuseeLink.ButtonColor = new float4(0, 0, 0, 0);
            _guiFuseeLink.BorderWidth = 0;
            SetCursor(CursorType.Standard);
        }

        private void _guiFuseeLink_OnGUIButtonEnter(GUIButton sender, GUIButtonEventArgs mea)
        {
            _guiFuseeLink.ButtonColor = new float4(0, 0.6f, 0.2f, 0.4f);
            _guiFuseeLink.BorderWidth = 1;
            SetCursor(CursorType.Hand);
        }

        void _guiFuseeLink_OnGUIButtonDown(GUIButton sender, GUIButtonEventArgs mea)
        {
            OpenLink("http://fusee3d.org");
        }
#endif
    }
}