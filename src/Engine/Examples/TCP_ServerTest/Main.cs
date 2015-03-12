using System;
using System.Runtime.Remoting.Channels;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Examples.PhysicsTest;
using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;

namespace Examples.TCP_ServerTest
{

    internal class TCP_ServerTest : RenderCanvas
    {
        private GUIText _guiSubText;
        private GUIText _serverText;
        private IFont _guiLatoBlack;
        private GUIHandler _guiHandler;

        private GUI _gui;

                
        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(1, 1, 1, 1);
            _gui = new GUI(RC);
           
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            float fps = Time.Instance.FramePerSecond;
            _gui.RenderFps(fps);


            try
            {
                var connect = new TcpConnection();
                //string msg = connect.recvMessage.ToString();
                _gui.RenderMsg(connect.teststring);
            }
            catch(NullReferenceException)
            {
                _gui.RenderMsg("Nix da!");
            }
            
           Present();
        }


        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);
        }


        public static void StartFusee()
        {
            var app = new TCP_ServerTest();
            app.Run();
        }

        public static void StartTCPServer()
        {
            ThreadPoolTcpSrvr tpts = new ThreadPoolTcpSrvr();
        }
        
        
        public static void Main()
        {
            Thread fusee = new Thread(StartFusee);
            Thread tcpServer = new Thread(StartTCPServer);
            tcpServer.IsBackground = true;

            tcpServer.Start();
            fusee.Start();
        }

        public void Render(string cont)
        {
        _guiHandler.RenderGUI();
                     
        }
    }
    
}
