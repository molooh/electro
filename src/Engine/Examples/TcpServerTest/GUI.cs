using System;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.TcpServerTest
{
    // ReSharper disable once InconsistentNaming
    class GUI
    {
        private readonly GUIHandler _guiHandler;

        private IFont _fontSmall;
        private IFont _fontMedium;
        private IFont _fontBig;

        private readonly GUIPanel _guiPanel;

        private GUIText _fps, _serverMsg;

        private readonly float4 _color1 = new float4(1f, 1f, 1f, 1);
        private readonly float4 _color2 = new float4(0, 0, 0, 1);

        public GUI(RenderContext rc)
        {
            //Basic Init

            _fontSmall = rc.LoadFont("Assets/Lato-Black.ttf", 12);
            _fontMedium = rc.LoadFont("Assets/Lato-Black.ttf", 18);
            _fontBig = rc.LoadFont("Assets/Lato-Black.ttf", 40);

            _guiHandler = new GUIHandler();
            _guiHandler.AttachToContext(rc);

            //Start Pannel Init
            _guiPanel = new GUIPanel("TCP Server Test", _fontMedium, 10, 10, 330, 150);

            _fps = new GUIText("FPS", _fontMedium, 30, 55, _color2);
            _serverMsg = new GUIText("Message received:", _fontMedium, 30, 95, _color2);

            _guiPanel.ChildElements.Add(_fps);
            _guiPanel.ChildElements.Add(_serverMsg);

            ShowGUI();
        }


        public void RenderFps(float fps)
        {
            _fps.Text = "FPS: " + fps; //TODO: manage RAM usage!            
        }

        public void RenderMsg(string serverMsg)
        {
            _serverMsg.Text = "Message received: " + serverMsg; //TODO: manage RAM usage!
            _guiHandler.RenderGUI();
        }




        public void ShowGUI()
        {
            _guiHandler.Clear();
            _guiHandler.Add(_guiPanel);
        }


        public void Resize()
        {
            _guiHandler.Refresh();
        }
    }
}
