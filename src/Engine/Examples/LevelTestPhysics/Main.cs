using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;

namespace Examples.LevelTestPhysics
{
    public class LevelTestPhysics : RenderCanvas
    {
        // angle variables
        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;

        private const float RotationSpeed = 1f;
        private const float Damping = 0.92f;

        // model variables
        private Mesh _meshLab;
        private Mesh _meshBall;
        private Mesh _Rechteck;
        private Mesh _way;

        float3 camMin;
        float3 camMax;

        // variables for shader
        private ShaderProgram _spColor;
        private ShaderProgram _spTexture;

        private IShaderParam _colorParam;
        private IShaderParam _textureParam;

        private ITexture _iTex;

        //Physic
        private Physic _physic;

        private SceneRenderer _sr;
        private SceneRenderer _srSphere;
        private SceneContainer _scene;
        private SceneContainer _sceneSphere;

        //Animation Way
        private RigidBody[] _rbWay;

        // is called on startup
        public override void Init()
        {

            RC.ClearColor = new float4(0.1f, 0.1f, 0.5f, 1);
            _Rechteck = MeshReader.LoadMesh(@"Assets/Rechteck.obj.model");

            _way = MeshReader.LoadMesh(@"Assets/way.obj.model");

            var ser = new Serializer();
            using (var file = File.OpenRead(@"Assets/SphereB.fus"))
            {
                _sceneSphere = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
                //MeshComponent mc = scene.Children.FindComponents<MeshComponent>(comp => true).First();
                //_meshBall = SceneRenderer.MakeMesh(mc);
            }
            _srSphere = new SceneRenderer(_sceneSphere, "Assests");

            var serI = new Serializer();
            using (var file = File.OpenRead(@"Assets/Island_split.fus"))
            {
                _scene = serI.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
                //MeshComponent mc = _scene.Children.FindComponents<MeshComponent>(comp => true).First();
                //_meshLab = SceneRenderer.MakeMesh(mc);
            }

            _sr = new SceneRenderer(_scene, "Assets");
            /* 
           _meshBall = MeshReader.LoadMesh(@"Assets/Sphere.obj.model");
          _meshLab = MeshReader.LoadMesh(@"Assets/SimpleLab45.obj.model");
*/
            _spColor = MoreShaders.GetDiffuseColorShader(RC);
            _colorParam = _spColor.GetShaderParam("color");

            _spTexture = MoreShaders.GetTextureShader(RC);
            _textureParam = _spTexture.GetShaderParam("texture1");

            _physic = new Physic();

            _physic.InitScene();
        }

        // is called once a frame
        public override void RenderAFrame()
        {

            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            _physic.World.StepSimulation((float)Time.Instance.DeltaTime, (Time.Instance.FramePerSecondSmooth / 60), 1 / 60);//???

            // move scene per mouse
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

            var rb = _physic.GetBall();
            var rb2 = _physic.GetBall2();
            // move ball per keyboard
            if (Input.Instance.IsKeyUp(KeyCodes.Left))
            {
                rb.ApplyCentralImpulse = new float3(-10, 0, 0);
            }

            //onkeyup
            if (Input.Instance.IsKeyUp(KeyCodes.Right))
            {
                rb.ApplyCentralImpulse = new float3(10, 0, 0);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.Up))
            {
                rb.ApplyCentralImpulse = new float3(0, 0, 10);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.Down))
            {
                rb.ApplyCentralImpulse = new float3(0, 0, -10);
            }

            if (Input.Instance.IsKeyUp(KeyCodes.A))
            {
                rb2.ApplyCentralImpulse = new float3(-10, 0, 0);
            }

            //onkeyup
            if (Input.Instance.IsKeyUp(KeyCodes.D))
            {
                rb2.ApplyCentralImpulse = new float3(10, 0, 0);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.W))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, 10);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.S))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, -10);
            }

            float3 averageNewPos = (rb.Position + rb2.Position) / 2;

            camMin = new float3(averageNewPos.x - 750, 0, averageNewPos.z - 550);
            camMax = new float3(averageNewPos.x + 750, 0, averageNewPos.z + 950);

            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            var mtxCam = float4x4.LookAt(averageNewPos.x + 200, 800, averageNewPos.z - 2500, averageNewPos.x, 0, averageNewPos.z, 0, 1, 0);

            RC.SetShader(_spColor);

            // Rechteck Boden
            var mtxR = float4x4.CreateTranslation(averageNewPos.x, 0, averageNewPos.z + 200);
            RC.ModelView = mtxCam * mtxRot * mtxR;// *float4x4.CreateScale(0.2f);
            RC.Render(_Rechteck);

            if (rb.CollisionShape is SphereShape)
            {
                var shape = (SphereShape)rb.CollisionShape;
                var pos = float4x4.Transpose(rb.WorldTransform);
                Debug.WriteLine("##################################### " + rb.Position);
                RC.ModelView = mtxCam * pos * mtxRot;// *float4x4.Scale(4);
                //RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
                RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                //RC.Render(_meshBall);
                _srSphere.Render(RC);
            }

            if (rb2.CollisionShape is SphereShape)
            {
                var shape = (SphereShape)rb.CollisionShape;
                var pos = float4x4.Transpose(rb2.WorldTransform);
                //Debug.WriteLine("##################################### " + rb.Position);
                RC.ModelView = mtxCam * pos * mtxRot;// *float4x4.Scale(4);
                //RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
                RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                //RC.Render(_meshBall);
                _srSphere.Render(RC);
            }

            //RC.SetShaderParam(_colorParam, new float4(0.8f, 0.1f, 0.1f, 1));
            RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
            RC.ModelView = mtxCam * mtxRot;//* float4x4.CreateScale(0.1f)* float4x4.CreateTranslation(-100, 0, 0); ;
            _sr.Render(RC);

            /*
                        //Animation Way
                        _rbWay = _physic.GetWay();
                        RigidBody way0 = _rbWay[0];
                        way0.Gravity = new float3( 0,0,10);
                        var posWay = float4x4.Transpose(way0.WorldTransform);
                        RC.ModelView = mtxCam * posWay * mtxRot;// *float4x4.CreateScale(0.1f);
                        RC.Render(_way);
            */
            Present();
        }

        // is called when the window was resized
        public override void Resize()
        {
            RC.Viewport(0, 0, Width, Height);

            var aspectRatio = Width / (float)Height;
            var projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 5000);
            RC.Projection = projection;
        }

        public static void Main()
        {
            var app = new LevelTestPhysics();
            app.Run();
        }

        public override void DeInit()
        {

            base.DeInit();
            _physic.World.Dispose();    //???
        }
    }
}
