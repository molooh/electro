using System;
using System.Runtime.Remoting.Channels;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;

namespace Examples.TCP_ServerTest
{

    internal class TCP_ServerTest : RenderCanvas
    {
        private GUIText _guiSubText;
        private GUIText _guiServerText1;
        private IFont _guiLatoBlack;
        private GUIHandler _guiHandler;

                
        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(1, 1, 1, 1);

            Console.WriteLine("Test");

            _guiHandler = new GUIHandler();
            _guiHandler.AttachToContext(RC);

            _guiLatoBlack = RC.LoadFont("Assets/Lato-Black.ttf", 14);
            _guiSubText = new GUIText("FUSEE 3D TCP Server Test", _guiLatoBlack, 20, 20);
            _guiSubText.TextColor = new float4(0, 0, 0, 1f);
            _guiHandler.Add(_guiSubText);

            
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            
            _guiHandler.RenderGUI();
            string cont = AsynchronousSocketListener.content;
            if (Input.Instance.IsKeyDown(KeyCodes.W))
            {
                Console.WriteLine("Test");
                //Render(cont);
            }

           Present();
            var listener = new AsynchronousSocketListener();
           listener.Tcp(new string[] { });
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }

        public static void Main()
        {
            var app = new TCP_ServerTest();
            app.Run();
        }

        public void Render(string cont)
        {
              _guiServerText1.Text = cont;
              _guiHandler.RenderGUI();
                     
        }
    }
    
}
