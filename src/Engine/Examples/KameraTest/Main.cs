using System;
using Fusee.Engine;
using Fusee.Math;


namespace Examples.KameraTest
{
    public class KameraTest : RenderCanvas
    {
        // angle variables
        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;

        private const float RotationSpeed = 1f;
        private const float Damping = 0.92f;

        // model variables
        private Mesh _meshCube;
        private Mesh _meshCube2;
        private Mesh _meshCube3;
        private Mesh _meshCube4;
        private Mesh _meshCubeB1;
        private Mesh _meshCubeB2;
        private Mesh _meshCubeB3;
        private Mesh _meshCubeB4;
        private Mesh _meshCubeB5;
        private Mesh _meshCubeB6;
        private Mesh _meshCubeB7;
        private Mesh _Rechteck;

        // variables for shader
        private ShaderProgram _spTexture;
        private ShaderProgram _spColor;
        
        

        private IShaderParam _colorParam;
        private IShaderParam _textureParam;

        private ITexture _iTex;


        private int _aa;
        private int _bb;
        private int _cc = 300;
        private int _dd;
        private int _ee = 600;
        private int _ff;


        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);

            _meshCube = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCube2 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCube3 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCube4 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB1 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB2 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB3 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB4 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB5 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB6 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCubeB7 = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _Rechteck = MeshReader.LoadMesh(@"Assets/Rechteck.obj.model");


            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            // _spTexture = MoreShaders.GetTextureShader(RC);

            _colorParam = _spColor.GetShaderParam("color");
            // _textureParam = _spTexture.GetShaderParam("texture1");

            _spTexture = MoreShaders.GetTextureShader(RC);
         
            _textureParam = _spTexture.GetShaderParam("texture1");
           
        }

        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            var ace = (_aa + _cc + _ee) / 3;
            var bdf = (_bb + _dd + _ff) / 3;
            var ab = (_aa + _bb) / 2;
            var cd = (_cc + _dd) / 2;


            var diffA = ace - _aa;
            var diffB = bdf - _bb;
            Console.WriteLine(_aa + "  " + ace + " " + diffA);

            // move per keyboard (arrow keys)
            if (diffA <= 750) { 
                if (Input.Instance.IsKey(KeyCodes.Left)){
                //_angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;
                _aa -= 5;
            }
            }

                if (diffA >= -750) { 
                  if (Input.Instance.IsKey(KeyCodes.Right)){
                      //_angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;
                      _aa += 5;
              }
}
                if (diffB >= -950) { 
                    if (Input.Instance.IsKey(KeyCodes.Up)){
                        //_angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;
                        _bb += 5;
                }
}
                if (diffB <= 550) { 
                   if (Input.Instance.IsKey(KeyCodes.Down)){
                       // _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;
                       _bb -= 5;
               }
}
            // move per keyboard (W A S D)
            
                  if (Input.Instance.IsKey(KeyCodes.A)){
                      //_angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;
                      _cc -= 5;
              }

           
                    if (Input.Instance.IsKey(KeyCodes.D)){
                        //_angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;
                        _cc += 5;
                }

          
                    if (Input.Instance.IsKey(KeyCodes.W)){
                        //_angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;
                        _dd += 5;
                }

           
                     if (Input.Instance.IsKey(KeyCodes.S)){
                         // _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;
                         _dd -= 5;
                 }


            //move per keybord U H J K

                 if (Input.Instance.IsKey(KeyCodes.H)) { 
                      //_angleHorz -= RotationSpeed * (float)Time.Instance.DeltaTime;
                      _ee -= 5;
                        }
                    if (Input.Instance.IsKey(KeyCodes.K)){
                        //_angleHorz += RotationSpeed * (float)Time.Instance.DeltaTime;
                        _ee += 5;
                        }

           
                    if (Input.Instance.IsKey(KeyCodes.U)){
                        //_angleVert -= RotationSpeed * (float)Time.Instance.DeltaTime;
                        _ff += 5;
                        }
                     if (Input.Instance.IsKey(KeyCodes.J)){
                         // _angleVert += RotationSpeed * (float)Time.Instance.DeltaTime;
                         _ff -= 5;

                        }



            
          
            var mtxCam = float4x4.LookAt(ace, 400, bdf - 1500, ace, 0 , bdf, 0, 1, 0);
            


            // first mesh

            var mtxM1 = float4x4.CreateTranslation(_aa, 0, _bb);

            if (diffA <= -750)
            {
                var _aaNewR = ace + 750;
                mtxM1 = float4x4.CreateTranslation(_aaNewR, 0, _bb);

            }
            if (diffA>=750)
            {
                var _aaNewL = ace - 750;
                mtxM1 = float4x4.CreateTranslation(_aaNewL, 0, _bb);
            }

            if (diffB <= -950)
            {
                var _bbNewO = bdf + 950;
                mtxM1 = float4x4.CreateTranslation(_aa, 0, _bbNewO);
            }

            if (diffB >= 550)
            {
                var _bbNewU = bdf - 550;
                mtxM1 = float4x4.CreateTranslation(_aa, 0, _bbNewU);
            }

            RC.ModelView = mtxCam * mtxM1;
          //  var mtxMVP = RC.ModelViewProjection;
           // float4 vCube1 = mtxMVP * new float4(0, 0, 0, 1);
           // float3 vCube1P = new float3(vCube1.x / vCube1.w, vCube1.y / vCube1.w, vCube1.z / vCube1.w);
           // Console.WriteLine(vCube1P);


            RC.SetShader(_spColor);
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            
           
            RC.Render(_meshCube);
            
          //  Console.WriteLine("_aa: " + _aa + "  _bb: " + _bb + "_cc: "+ _cc + "_dd: " + _dd +"diff: " +diff+"\n");

            // second mesh
            var mtxM2 = float4x4.CreateTranslation( _cc, 0, _dd);
           
            RC.ModelView = mtxCam * mtxM2;
            RC.SetShaderParam(_colorParam, new float4(0.2f, 0.6f, 0.1f, 1));
            RC.Render(_meshCube2);

            //third mesh static 
            var mtxM3 = float4x4.CreateTranslation(_ee, 0, _ff);

            RC.ModelView = mtxCam * mtxM3;
            RC.SetShaderParam(_colorParam, new float4(0.4f, 0.5f, 0.8f, 1));
            RC.Render(_meshCube3);
            
            
          

            // fourth mesh
            RC.SetShaderParam(_colorParam, new float4(0.2f, 0.2f, 0.2f, 1));
            var mtxM4 = float4x4.CreateTranslation(-300, 0, 0);
            RC.ModelView = mtxCam * mtxM4;
            RC.Render(_meshCube4);
 

            // Rechteck Boden

            var mtxR = float4x4.CreateTranslation(ace, -101, bdf + 200);
            RC.ModelView = mtxCam * mtxR;
            RC.Render(_Rechteck);




         /*   //Boden aus Würfeln
            
            var mtxB1 = float4x4.CreateTranslation(-1300, -700, 0);
            RC.ModelView = mtxCam*mtxB1;
            RC.Render(_meshCubeB1);

            var mtxB2 = float4x4.CreateTranslation(-1100, -700, 0);
            RC.ModelView = mtxCam * mtxB2;
            RC.Render(_meshCubeB2);

            var mtxB3 = float4x4.CreateTranslation(-900, -700, 0);
            RC.ModelView = mtxCam * mtxB3;
            RC.Render(_meshCubeB3);

            var mtxB4 = float4x4.CreateTranslation(-700, -700, 0);
            RC.ModelView = mtxCam * mtxB4;
            RC.Render(_meshCubeB4);

            var mtxB5 = float4x4.CreateTranslation(-500, -700, 0);
            RC.ModelView = mtxCam * mtxB5;
            RC.Render(_meshCubeB5);

            var mtxB6 = float4x4.CreateTranslation(-300, -700, 0);
            RC.ModelView = mtxCam * mtxB6;
            RC.Render(_meshCubeB6);

            var mtxB7 = float4x4.CreateTranslation(-100, -700, 0);
            RC.ModelView = mtxCam * mtxB7;
            RC.Render(_meshCubeB7);
           */

          

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
            var app = new KameraTest();
            app.Run();
        }
    }
}
