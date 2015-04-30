using System;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.LevelTest
{
    public class LevelTest : RenderCanvas
    {

        private static float _angleHorz, _angleVert;
        private Mesh _worldMesh;
        

       // private ShaderProgram _spColor;
        private ShaderProgram _spTexture;

       // private IShaderParam _colorParam;
        private IShaderParam _textureParam;

        private ITexture _iTex;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);

            _angleHorz = 2;
            _angleVert = 0.2f;

            //_spTexture = RC.CreateShader(Vt, Pt);
           _worldMesh = MeshReader.LoadMesh(@"Assets/NEW_ISLAND.obj.model");
           

         // _spColor = MoreShaders.GetDiffuseColorShader(RC);
          // _colorParam = _spColor.GetShaderParam("color");

           _spTexture = MoreShaders.GetTextureShader(RC);
           _textureParam = _spTexture.GetShaderParam("texture1");

           var imgData = RC.LoadImage("Assets/Island.jpg");
          _iTex = RC.CreateTexture(imgData);
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            //TerrainTry
           var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
           var mtxCam = float4x4.LookAt(1700, 700, 10000, 200, -900, 0, 0,1, 0)*mtxRot;
           //RC.SetShader(_spColor);
            RC.SetShader(_spTexture);
           RC.ModelView = mtxCam;
           //RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
           RC.Render(_worldMesh);

          

            Present();
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
            var app = new LevelTest();
            app.Run();
        }
    }
}
