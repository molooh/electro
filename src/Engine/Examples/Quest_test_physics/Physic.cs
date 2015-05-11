using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    class Physic
    {
        private DynamicWorld _world;

        public DynamicWorld World
        {
            get { return _world; }
            set { _world = value; }
        }

        internal BoxShape WallCollider90, WallCollider;
        internal SphereShape MySphereCollider;
        internal ConvexHullShape MyConvHull;

        private SceneContainer _scene;
        private IEnumerable<SceneNodeContainer> _sceneList;

        internal Mesh SphereMesh, LabMesh;

        public Physic()
        {
            InitScene();
        }

        public void InitWorld()
        {
            _world = new DynamicWorld();
        }
        RigidBody inner;
        public void InitColliders()
        {

            var ser = new Serializer();
            using (var file = File.OpenRead(@"Assets/lab_rot_inner.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;
            }
            //Island
            foreach (SceneNodeContainer node in  _scene.Children.FindNodes(node => node.Name.StartsWith("box")))
            {
                AABBf? aabb = new AABBCalculator(node).GetBox();
                float3 boxSize = aabb.Value.Size;
                float3 boxSizeHalf = boxSize / 2;
                float3 boxCenter = aabb.Value.Center;
                float3 min = aabb.Value.min;
                float3 max = aabb.Value.max;

                

                WallCollider = _world.AddBoxShape(boxSizeHalf.x, boxSizeHalf.y, boxSizeHalf.z);
                //mass, float3 position, float3 orientation, CollisionShape colShape float3.Zero
                var lab = _world.AddRigidBody(0, new float3(boxCenter.x, boxCenter.y, boxCenter.z), new float3(0, 0, 0), WallCollider);

                lab.Restitution = 1;
                lab.Friction = 1;
                lab.SetDrag(0.0f, 0.05f);

            }

            /**/
            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("rot")))
            {
                MeshComponent middle = node.GetMesh();
                float3[] verts = middle.Vertices;
/*
                node.Components.
                
                Components = new List<SceneComponentContainer>(new SceneComponentContainer[]
                {
                    new TransformComponent()
                    {
                        Rotation = new float3(0, 0, 0),
                        Translation = new float3(0.11f, 0.11f, 0),
                        Scale = new float3(1, 1, 1)
                    },
                    aMesh
                });

                float3 rot = new TransformComponent().Rotation;
               
*/
                float3 minVert = verts[0];
                float3 maxVert = verts[0];


                for (int i = 1; i < verts.Length; i++)
                {
                    if (verts[i].x < minVert.x) minVert.x = verts[i].x;
                    if (verts[i].y < minVert.y) minVert.y = verts[i].y;
                    if (verts[i].z < minVert.z) minVert.z = verts[i].z;
                    if (verts[i].x > maxVert.x) maxVert.x = verts[i].x;
                    if (verts[i].y > maxVert.y) maxVert.y = verts[i].y;
                    if (verts[i].z > maxVert.z) maxVert.z = verts[i].z;
                }
                Debug.WriteLine("######## " + maxVert.x + "######## " + maxVert.y + "######## " + maxVert.z );
                Debug.WriteLine("######## " + minVert.x + "######## " + minVert.y + "######## " + minVert.z);

                float3 size = (maxVert/2 - minVert/2);
                float3 center = (maxVert + minVert) * 0.5f;

                

                WallCollider = _world.AddBoxShape(size.x, size.y, size.z);
                //mass, float3 position, float3 orientation, CollisionShape colShape float3.Zero
                inner = _world.AddRigidBody(0, new float3(center.x, center.y, center.z), new float3(0, -0.5f, 0), WallCollider);

                inner.Restitution = 1;
                inner.Friction = 1;
                inner.SetDrag(0.0f, 0.05f);
 /*
               
                //rotation modell + Größe ohne bounding box TransformComponent min max aus vertices mesh (aabb calc?)


                 */
            }
            //MySphereCollider = _world.AddSphereShape(5);
            
        }

        public void GroundPlane(float3 pos, float3 rot)
        {
            var groundShape = _world.AddBoxShape(200, 1, 200);
            var ground = _world.AddRigidBody(0, pos, rot, groundShape);

            ground.Restitution = 0.5f;
            ground.Friction = 0.1f;
        }

        public RigidBody sphere0;

        public void InitScene()
        {
            InitWorld();
            InitColliders();
            //GroundPlane(float3.Zero, float3.Zero);
           
            sphere0 = _world.AddRigidBody(10, new float3(30, 30, 0), float3.Zero, World.AddSphereShape(5));
            sphere0.Restitution = 0.5f;
            //sphere0.Friction = 0.8f;
            //sphere0.CollisionShape = MySphereCollider;

            // sphere.SetDrag(0.0f, 0.05f);

        }

        public RigidBody GetBall()
        {
            return sphere0;
        }

        public RigidBody GetInnerWall()
        {
            return inner;
        }

    }

    
}
