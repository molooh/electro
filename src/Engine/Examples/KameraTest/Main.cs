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
        private int _bb ;
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
            //Array for Players Position 
            float3 [] playerPos = new float3[3];

            //Array for new Players Position
            float3 [] newPlayerPos = new float3[3];

            //Array for input
            float3[] move = new float3[3];

            //Central Position of all Players
            var averageNewPos = new float3(0,0,0);

            //Camera Minimum and Maximum
            var camMin = new float3(0, 0, 0);
            var camMax = new float3(0, 0, 0);

            playerPos[0].x = _aa;
            playerPos[0].y = 0;
            playerPos[0].z = _bb;

            playerPos[1].x = _cc;
            playerPos[1].y = 0;
            playerPos[1].z = _dd;

            playerPos[2].x = _ee;
            playerPos[2].y = 0;
            playerPos[2].z = _ff;
            



            var inputA = 0;
            var inputB = 0;
            var inputC = 0;
            var inputD = 0;
            var inputE = 0;
            var inputF = 0;
           


            // move per keyboard (arrow keys)
                if (Input.Instance.IsKey(KeyCodes.Left)){
                    inputA = 5;

                    _aa -= (int)inputA;
            }
                  if (Input.Instance.IsKey(KeyCodes.Right)){
                      inputA = 5;
                      _aa += (int)inputA;
              }
                    if (Input.Instance.IsKey(KeyCodes.Up)){
                        inputB = 5;
                        _bb += (int)inputB;
                }
                   if (Input.Instance.IsKey(KeyCodes.Down)){
                       inputB = 5;
                       _bb -= (int)inputB;
               }

            // move per keyboard (W A S D)
            
                  if (Input.Instance.IsKey(KeyCodes.A)){
                      inputC = 5;
                       _cc -= (int)inputC;
              }

           
                    if (Input.Instance.IsKey(KeyCodes.D)){
                        inputC = 5;
                        _cc += (int)inputC;
                }

          
                    if (Input.Instance.IsKey(KeyCodes.W)){
                        inputD = 5;
                        _dd += (int)inputD;
                }

           
                     if (Input.Instance.IsKey(KeyCodes.S)){
                         inputD = 5;
                         _dd -= (int)inputD;

                 }


            //move per keybord U H J K

                 if (Input.Instance.IsKey(KeyCodes.H)) { 

                     inputE = 5;
                     _ee -= (int)inputE;
                        }
                    if (Input.Instance.IsKey(KeyCodes.K)){
                        inputE = 5;
                        _ee += (int)inputE;
                        }

           
                    if (Input.Instance.IsKey(KeyCodes.U)){
                        inputF = 5;
                        _ff += (int)inputF;
                        }
                     if (Input.Instance.IsKey(KeyCodes.J)){
                         inputF = 5;
                         _ff -= (int)inputF;

                        }
                     move[0].x = inputA;
                     move[0].z = inputB;
                     move[1].x = inputC;
                     move[1].z = inputD;
                     move[2].x = inputE;
                     move[2].z = inputF;
                     for (int i = 0; i < playerPos.Length; i++)
                     {
                         newPlayerPos[i].x = playerPos[i].x + move[i].x;
                         newPlayerPos[i].z = playerPos[i].z + move[i].z;
                         averageNewPos += newPlayerPos[i];

                     //  Console.WriteLine(move[i]);
                     }

                     averageNewPos *= (float)(1.0 / playerPos.Length);
                    // Console.WriteLine(averageNewPos);

                     camMin = new float3(averageNewPos.x - 750, 0, averageNewPos.z - 550);
                     camMax = new float3(averageNewPos.x + 750, 0, averageNewPos.z + 950);

                    


                    
            
          
            var mtxCam = float4x4.LookAt(averageNewPos.x, 400, averageNewPos.z - 1500, averageNewPos.x, 0 , averageNewPos.z, 0, 1, 0);

            for (int i = 0; i < playerPos.Length; i++)
            {
                if (newPlayerPos[i].x <= camMin.x)
                {
                    playerPos[i].x = camMin.x;

                }
                else
                {
                    if (newPlayerPos[i].x >= camMax.x)
                    {
                        playerPos[i].x = camMax.x;

                    }
                    else
                    {
                        playerPos[i].x = newPlayerPos[i].x;
                    }
                }

                if (newPlayerPos[i].z <= camMin.z)
                {
                    playerPos[i].z = camMin.z;

                }
                else
                {
                    if (newPlayerPos[i].z >= camMax.z)
                    {
                        playerPos[i].z = camMax.z;

                    }
                    else
                    {
                        playerPos[i].z = newPlayerPos[i].z;
                    }
                }
            }

            // first mesh
            
            var mtxM1 = float4x4.CreateTranslation(playerPos[0].x, 0, playerPos[0].z);

       

            RC.ModelView = mtxCam * mtxM1;
     
            RC.SetShader(_spColor);
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            
           
            RC.Render(_meshCube);
            
         

            // second mesh
            var mtxM2 = float4x4.CreateTranslation( playerPos[1].x, 0, playerPos[1].z);
           
            RC.ModelView = mtxCam * mtxM2;
            RC.SetShaderParam(_colorParam, new float4(0.2f, 0.6f, 0.1f, 1));
            RC.Render(_meshCube2);

            //third mesh 
            var mtxM3 = float4x4.CreateTranslation(playerPos[2].x, 0, playerPos[2].z);

            RC.ModelView = mtxCam * mtxM3;
            RC.SetShaderParam(_colorParam, new float4(0.4f, 0.5f, 0.8f, 1));
            RC.Render(_meshCube3);
            
            
          

            // fourth mesh static
            RC.SetShaderParam(_colorParam, new float4(0.2f, 0.2f, 0.2f, 1));
            var mtxM4 = float4x4.CreateTranslation(-300, 0, 0);
            RC.ModelView = mtxCam * mtxM4;
            RC.Render(_meshCube4);
 

            // Rechteck Boden

            var mtxR = float4x4.CreateTranslation(averageNewPos.x, -101, averageNewPos.z + 200);
            RC.ModelView = mtxCam * mtxR;
            RC.Render(_Rechteck);




     

          

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
