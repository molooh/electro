using System;
using System.IO;
using System.Linq;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;


namespace Examples.LevelTest
{
    public class LevelTest : RenderCanvas
    {

        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;
        private const float RotationSpeed = 1f;
        private const float Damping = 0.92f;
        
        private Mesh _worldMesh;

        private float4x4 _modelScaleOffset;

       // private ShaderProgram _spColor;
        private ShaderProgram _spTexture;

        private SceneRenderer _sr;
        private SceneContainer _scene;
      

       // private IShaderParam _colorParam;
        //private IShaderParam _textureParam;
       
        //private ITexture _iTex;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);

            _angleHorz = 2;
            _angleVert = 0.2f;

          // _spTexture = RC.CreateShader(Vt, Pt);
          //_worldMesh = MeshReader.LoadMesh(@"Assets/new_Island_finish.obj.model");
    
           var ser = new Serializer();
            using (var file = File.OpenRead(@"Assets/NEW_ISLAND.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
            }

            _sr = new SceneRenderer(_scene, "Assets");
               
         
         _spTexture = MoreShaders.GetTextureShader(RC);
        
         
        //  var imgData = RC.LoadImage("Assets/Island.jpg");
         

       }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            if (Input.Instance.IsButton(MouseButtons.Left))
            {
                _angleVelHorz = -RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseX);
                _angleVelVert = -RotationSpeed * Input.Instance.GetAxis(InputAxis.MouseY);
            }
            else
            {
                var curDamp = (float)Math.Exp(-Damping * Time.Instance.DeltaTime);

                _angleVelHorz *= curDamp;
                _angleVelVert *= curDamp;
            }

            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;

            //TerrainTry
           var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
           var mtxCam = float4x4.LookAt(1700, 900, 20000, 200, 0, 0, 0,1, 0)*mtxRot;
           RC.SetShader(_spTexture);
           RC.ModelView = mtxCam;
           _sr.Render(RC);
         

           
            Present();
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 100000);
        }

        public static void Main()
        {
            var app = new LevelTest();
            app.Run();
        }
    }
}
