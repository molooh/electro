using System;
using Fusee.Engine;
using Fusee.Math;

namespace Examples.TriggerTests
{
    public class TriggerTests : RenderCanvas
    {

        public delegate void ColliderEventHandler(object sender, ColliderEventArgs e);
        public event ColliderEventHandler Collide;



        private Mesh _meshTea, _meshCube, _meshSphere, _meshCylinder, _meshPlatonic, _meshFire;


        public ShaderProgram _spColor;
        public IShaderParam _colorParam;
        public ShaderProgram _spColor2;
        public IShaderParam _colorParam2;

        private ShaderProgram _spTexture;

        private IShaderParam _textureParam;

        private ITexture _iTex;

        private ShaderProgram _spTexture2;

        private IShaderParam _textureParam2;

        private ITexture _iTex2;


      /*  private float _speed;
        private float3 _posX ;
        private float _posY = 0;
        private float _posZ = 0;*/
       
       

        private PhysicsQuest _physic;
        private int currentScene = 1;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);

            _meshSphere = MeshReader.LoadMesh(@"Assets/Sphere.obj.model");
            _meshTea = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");
            _meshCube = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            _meshCylinder = MeshReader.LoadMesh(@"Assets/Cylinder.obj.model");
            _meshPlatonic = MeshReader.LoadMesh(@"Assets/Platonic.obj.model");
            
           _meshFire = MeshReader.LoadMesh(@"Assets/Fireball_Baked.obj.model");


            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");

            _spColor2 = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam2 = _spColor.GetShaderParam("color");

            _spTexture = MoreShaders.GetTextureShader(RC);
            _textureParam = _spTexture.GetShaderParam("texture1");

            _spTexture2 = MoreShaders.GetTextureShader(RC);
            _textureParam2 = _spTexture.GetShaderParam("texture1");

            /*var imgData = RC.LoadImage("Assets/Kugel_bak_2.jpg");
            _iTex = RC.CreateTexture(imgData);*/

            var imgData = RC.LoadImage("Assets/WasseKugel.jpg");
            _iTex = RC.CreateTexture(imgData);

            var imgData2 = RC.LoadImage("Assets/Waterplants0037_L.jpg");
            _iTex2 = RC.CreateTexture(imgData2);

            
            _physic = new PhysicsQuest();

            _physic.World.Dispose();
            _physic.InitScene1();
            currentScene = 1;
        }


        // is called once a frame
        public override void RenderAFrame()
        {
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            var rb2 = _physic.World.GetRigidBody(_physic.World.NumberRigidBodies() - 1);

          // is called once a frame
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            float fps = Time.Instance.FramePerSecond;
            fps = (float)Math.Round(fps, 2);
            

            _physic.World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);

            // move per keyboard
           

            if (Input.Instance.IsKeyDown(KeyCodes.A))
            {
                rb2.ApplyCentralImpulse = new float3(10, 0, 0);
            }

            if (Input.Instance.IsKeyDown(KeyCodes.D))
            {
                rb2.ApplyCentralImpulse = new float3(-10 , 0, 0);
            }
           
            if (Input.Instance.IsKeyDown(KeyCodes.W))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, -10);
            }
            if (Input.Instance.IsKeyDown(KeyCodes.O))
            {
                rb2.ApplyCentralImpulse = new float3(0, 10, 0);
            }

            if (Input.Instance.IsKeyDown(KeyCodes.L))
            {
                rb2.ApplyCentralImpulse = new float3(0, -10, 0);
            }

            if (Input.Instance.IsKeyDown(KeyCodes.S))
            {
                _physic.World.GetRigidBody(_physic.World.NumberRigidBodies() - 1).ApplyCentralImpulse = new float3(0, 0, 10);
            }
            if (Input.Instance.IsKeyDown(KeyCodes.None))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, 0);
            }

            /* if (Input.Instance.IsKey(KeyCodes.Left))
               _posX += _speedTrans * (float)Time.Instance.DeltaTime;

           if (Input.Instance.IsKey(KeyCodes.Right))
               _posX += -_speedTrans * (float)Time.Instance.DeltaTime;

           if (Input.Instance.IsKey(KeyCodes.O))
               _posY += _speedTrans * (float)Time.Instance.DeltaTime;

           if (Input.Instance.IsKey(KeyCodes.L))
               _posY += -_speedTrans * (float)Time.Instance.DeltaTime;

           if (Input.Instance.IsKey(KeyCodes.Up))
               _posZ += -_speedTrans * (float)Time.Instance.DeltaTime;

           if (Input.Instance.IsKey(KeyCodes.Down))
               _posZ += _speedTrans * (float)Time.Instance.DeltaTime;*/
            
            
            var mtxCam = float4x4.LookAt(0, 50, 70, 0, 0, 0, 0, 1, 0);

            //Model Shapes
            for (int i = 0; i < _physic.World.NumberRigidBodies(); i++)
            {
                var rb = _physic.World.GetRigidBody(i);
                var matrix = float4x4.Transpose(rb.WorldTransform);

               
                if (rb.CollisionShape is BoxShape)
                {
                    var shape = (BoxShape)rb.CollisionShape;
                    RC.ModelView = mtxCam * matrix * float4x4.Scale(shape.HalfExtents.x / 100, shape.HalfExtents.y / 100, shape.HalfExtents.z / 100);
                   // RC.SetShader(_spColor);
                    //RC.SetShaderParam(_colorParam, new float4(0, 0.8f, 0, 0.5f));
                    RC.SetShader(_spTexture2);
                    RC.SetShaderParamTexture(_textureParam2, _iTex2);
                    RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                    RC.Render(_meshCube);
                }
                else if (rb.CollisionShape is SphereShape)
                {
                    var shape = (SphereShape)rb.CollisionShape;
                    RC.ModelView = mtxCam * matrix * float4x4.Scale(shape.Radius);
                    //RC.SetShader(_spColor2);
                    //RC.SetShaderParam(_colorParam2,new float4(0.8f,0,0, 1));
                    RC.SetShader(_spTexture);
                    RC.SetShaderParamTexture(_textureParam, _iTex);
                    RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                    RC.Render(_meshSphere);
                    
                }
                else if (rb.CollisionShape is CylinderShape)
                {
                    var shape = (CylinderShape)rb.CollisionShape;
                    RC.ModelView = mtxCam * matrix * float4x4.Scale(4);
                   // RC.SetShader(_spCustom);
                    //RC.SetShaderParam(_colorCustom, new float4(0.1f, 0.1f, 0.9f, 1));
                    RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                    RC.Render(_meshCylinder);
                }
                else if (rb.CollisionShape is ConvexHullShape)
                {
                    if (currentScene == 2)
                    {
                        RC.ModelView = mtxCam * matrix * float4x4.Scale(1.0f);
                       // RC.SetShader(_spCustom);
                        //RC.SetShaderParam(_colorCustom, new float4(2f, 2f, 2, 1));
                        RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                        RC.Render(_meshPlatonic);
                    }
                    if (currentScene == 4)
                    {
                        RC.ModelView = mtxCam * matrix * float4x4.Scale(0.05f);
                        RC.SetShader(_spColor);
                        RC.SetShaderParam(_colorParam, new float4(0.6f, 0.2f, 0.5f, 1));
                        RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                        RC.Render(_meshTea);
                    }
                }
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

        public static void Main()
        {
            var app = new TriggerTests();
            app.Run();
        }
    }
}
