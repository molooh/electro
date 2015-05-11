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

namespace Examples.Quest_test_physics
{
    public class Quest_test_physics : RenderCanvas
    {
        // angle variables
        private static float _angleHorz, _angleVert, _angleVelHorz, _angleVelVert;

        private const float RotationSpeed = 1f;
        private const float Damping = 0.92f;

        // model variables
        private Mesh _meshLab;
        private Mesh _meshBall;

        // variables for shader
        private ShaderProgram _spColor;
        private ShaderProgram _spTexture;

        private IShaderParam _colorParam;
        private IShaderParam _textureParam;

        private ITexture _iTex;

        //Physic
        private Physic _physic;

        private SceneRenderer _sr;
        private SceneContainer _scene;

        // is called on startup
        public override void Init()
        {
            RC.ClearColor = new float4(1, 1, 1, 1);
           
            var ser = new Serializer();
            using (var file = File.OpenRead(@"Assets/Sphere.fus"))
            {
                var scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
                MeshComponent mc = scene.Children.FindComponents<MeshComponent>(comp => true).First();
                _meshBall = SceneRenderer.MakeMesh(mc);
            }


            using (var file = File.OpenRead(@"Assets/lab_rot_inner.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
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

            _physic = new Physic();
           
            _physic.InitScene();
        }

        // is called once a frame
        public override void RenderAFrame()
        {

            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            //var rb2 = _physic.World.GetRigidBody(_physic.World.NumberRigidBodies() - 1);
            //var rb2 = _physic.World.GetRigidBody(_physic.World.NumberRigidBodies() -5);
            var rb2 = _physic.GetBall();

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


            // move ball per keyboard
            if (Input.Instance.IsKeyUp(KeyCodes.Left))
            {
                rb2.ApplyCentralImpulse = new float3(-10, 0, 0);
            }

            //onkeyup
            if (Input.Instance.IsKeyUp(KeyCodes.Right))
            {
                rb2.ApplyCentralImpulse = new float3(10, 0, 0);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.Up))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, 10);
            }


            if (Input.Instance.IsKeyUp(KeyCodes.Down))
            {
                rb2.ApplyCentralImpulse = new float3(0, 0, -10);
            }

            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            //var mtxCam = float4x4.LookAt(0, 200, -500, 0, 0, 0, 0, 1, 0);
            var mtxCam = float4x4.LookAt(0, 100, -300, 0, 0, 0, 0, 1, 0) * mtxRot;

            RC.SetShader(_spColor);

            if (rb2.CollisionShape is SphereShape)
            {
                var shape = (SphereShape)rb2.CollisionShape;
                var pos = float4x4.Transpose(rb2.WorldTransform);
                RC.ModelView = mtxCam*pos;// * float4x4.Scale(shape.Radius);
                RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
                RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                RC.Render(_meshBall);

            }

            RC.SetShaderParam(_colorParam, new float4(0.8f, 0.1f, 0.1f, 1));
            //RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
            RC.ModelView = mtxCam;//* float4x4.CreateTranslation(-100, 0, 0); ;
            _sr.Render(RC);
/* 
            var inner = _physic.GetInnerWall();
            var matrix = float4x4.Transpose(inner.WorldTransform);
           Mesh _mesh =  MeshReader.LoadMesh(@"Assets/inner.obj.model");
           
                if (inner.CollisionShape is Fusee.Engine.BoxShape)
                {
                    var shape = (BoxShape) inner.CollisionShape;
                    RC.ModelView = mtxCam * matrix * float4x4.Scale(shape.HalfExtents.x / 100, shape.HalfExtents.y / 100, shape.HalfExtents.z / 100);

                    RC.SetShader(_spColor);
                    RC.SetShaderParam(_colorParam, new float4(0.9f, 0.9f, 0.0f, 1));
                    RC.SetRenderState(new RenderStateSet {AlphaBlendEnable = false, ZEnable = true});
                    RC.Render(_mesh);
                }
          
*/             
/*            
 * if (rb.CollisionShape is Fusee.Engine.BoxShape)
                {
                    var shape = (BoxShape) rb.CollisionShape;
                    RC.ModelView = mtxCam * matrix * float4x4.Scale(shape.HalfExtents.x / 100, shape.HalfExtents.y / 100, shape.HalfExtents.z / 100);
     
                   
                    RC.SetShader(_spColor);
                    RC.SetShaderParam(_colorParam, new float4(0.9f, 0.9f, 0.0f, 1));
                    RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                    RC.Render(_meshCube);
                }
            // render ball mesh
            for (int i = 0; i < _physic.World.NumberRigidBodies(); i++)
            {
                var rb = _physic.World.GetRigidBody(i);
                var matrix = float4x4.Transpose(rb2.WorldTransform);
                

                if (rb.CollisionShape is SphereShape)
                {
                    var shape = (SphereShape) rb.CollisionShape;
                    var pos = float4x4.Transpose(rb.WorldTransform);
                    RC.ModelView = mtxCam*pos*float4x4.Scale(shape.Radius);
                    RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
                    RC.SetRenderState(new RenderStateSet {AlphaBlendEnable = false, ZEnable = true});
                    RC.Render(_meshBall);

                }

                else if (rb.CollisionShape is ConvexHullShape)
                {
                    var shape = (ConvexHullShape)rb.CollisionShape;
                    var pos = float4x4.Transpose(rb.WorldTransform);
                    RC.ModelView = mtxCam*pos;// * float4x4.CreateRotationX(180); //*mtxRot;// * float4x4.CreateTranslation(150, 0, 0);
                    RC.SetShaderParam(_colorParam, new float4(0.8f, 0.1f, 0.1f, 1));
                    RC.SetRenderState(new RenderStateSet {AlphaBlendEnable = false, ZEnable = true});
                    //RC.Render(_meshLab);
                }
                else if (rb.CollisionShape is BoxShape)
                {
                    
                    var shape = (BoxShape)rb.CollisionShape;
                    var pos = float4x4.Transpose(rb.WorldTransform);
                    //Debug.WriteLine(pos);
                    RC.ModelView = mtxCam * pos;// * float4x4.CreateTranslation(wallX[counter], wallY[counter], wallZ[counter]);// * float4x4.CreateRotationX(180); //*mtxRot;// * float4x4.CreateTranslation(150, 0, 0);
                    RC.SetShaderParam(_colorParam, new float4(0.8f, 0.1f, 0.1f, 1));
                    RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
                    if (counter == 0)
                    {
                        
                    }
                    else if (counter > 0 && counter <= 2)
                    {
                        RC.Render(_meshWall90);
                        
                    }
                    else if (counter >= 4)
                    {
                        RC.Render(_meshWall);
                        
                    }
                    counter++;
                     
                }


            }
 */          
            //ball
            /*
            RC.ModelView = mtxCam;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });
            RC.Render(_meshBall);
             * 
            //walls
            RC.SetShaderParam(_colorParam, new float4(0.8f, 0.1f, 0.1f, 1));
            RC.SetRenderState(new RenderStateSet { AlphaBlendEnable = false, ZEnable = true });

            RC.ModelView = mtxCam;//* float4x4.CreateTranslation(-100, 0, 0); ;
            RC.Render(_meshLab);
*/
/*
            var modelViewMesh1 = mtxCam * mtxRot * float4x4.CreateTranslation(-150, 150, 0) * float4x4.CreateTranslation(0, -50, 0);
            RC.ModelView = modelViewMesh1;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.8f, 0, 1));
            RC.Render(_meshBall);

            // render labyrinth mesh
            var modelViewMesh2 = mtxCam * mtxRot * float4x4.CreateTranslation(150, 0, 0);
            RC.ModelView = modelViewMesh2;
            RC.SetShaderParam(_colorParam, new float4(0.5f, 0.1f, 0.5f, 1));
            RC.Render(_meshLab);
*/
            // swap buffers
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
            var app = new Quest_test_physics();
            app.Run();
        }

        public override void DeInit()
        {
            
            base.DeInit();
            _physic.World.Dispose();    //???
        }
    }
}
