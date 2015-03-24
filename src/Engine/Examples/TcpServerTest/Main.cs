using System;
using System.Text;
using System.Threading;
using Fusee.Engine;
using Fusee.SceneManagement;
using Fusee.Math;

namespace Examples.TcpServerTest
{

    internal class TcpServerTest : RenderCanvas
    {
        private ThreadPoolTcpSrvr _tpts;
        private GUIText _guiSubText;
        private GUIText _serverText;
        private IFont _guiLatoBlack;
        private GUIHandler _guiHandler;

        private GUI _gui;

                
        // is called on startup
        public override void Init()
        {
            //creates thread for TcpServer, sets it as backgroundthread, starts the thread
            var tcpServer = new Thread(StartTcpServer);
            tcpServer.IsBackground = true;
            tcpServer.Start(this);

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
                StringBuilder sb = new StringBuilder();
                foreach (TcpConnection connection in _tpts.GetConnections())
                {
                    sb.Append(connection.Message);
                    sb.Append("// ");
                }
                _gui.RenderMsg(sb.ToString());
            }
            catch(NullReferenceException)
            {
                _gui.RenderMsg("Nichts empfangen!");
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


        public static void StartTcpServer(object self)
        {
            ((TcpServerTest)self)._tpts = new ThreadPoolTcpSrvr();
            ((TcpServerTest)self)._tpts.StartListening();
        }
        
        public static void Main()
        {
            var app = new TcpServerTest();
            app.Run();
        }

        public void Render(string cont)
        {
        _guiHandler.RenderGUI();
                     
        }
    }
    
}
