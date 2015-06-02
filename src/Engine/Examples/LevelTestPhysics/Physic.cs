using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;
using System;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;

namespace Examples.LevelTestPhysics
{
    class Physic
    {
        private DynamicWorld _world;

        public DynamicWorld World
        {
            get { return _world; }
            set { _world = value; }
        }

        internal BoxShape BoxCollider;
        private SceneContainer _scene;
        private RigidBody box;
        public RigidBody sphere;
        public RigidBody sphere2;
        public RigidBody[] boxesWay = new RigidBody[35];
        int indexWays = 0;

        public Physic()
        {
            InitScene();
        }

        public void InitWorld()
        {
            _world = new DynamicWorld();
        }


        public void InitColliders()
        {

            var ser = new Serializer();
            using (var file = File.OpenRead(@"Assets/Island_split.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }

            // Größe und Position, sowie Rotation durch Vertices und TransformComponent
            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("box") || node.Name.StartsWith("Box")))
            {

                MeshComponent mesh = node.GetMesh();

                if (mesh.Vertices == null)
                {
                    continue;
                }

                float3[] verts = mesh.Vertices;
                TransformComponent transform = node.GetTransform();

                //Rotation des Modells
                float3 rot = transform.Rotation;


                //Größe des Modells
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

                float3 size = ((maxVert / 2 - minVert / 2) * 2);

                if (node.Name.Contains("Baum"))
                {
                    size.y = size.y / 4;
                }


                float3 center = (maxVert + minVert) / 2;

                //Collider 
                BoxCollider = _world.AddBoxShape(size.x, size.y, size.z);
                //Parameter: Masse, Position, Rotation
                box = _world.AddRigidBody(0, new float3(center.x, center.y, center.z), new float3(rot.x, rot.y, rot.z), BoxCollider);
                box.Restitution = 1;
                box.Friction = 1;
                box.SetDrag(0.0f, 0.05f);

                if (node.Name.StartsWith("box.way"))
                {
                    boxesWay[indexWays] = box;
                    indexWays++;
                }

            }
        }

        public void GroundPlane(float3 pos, float3 rot)
        {
            var groundShape = _world.AddBoxShape(200, 1, 200);
            var ground = _world.AddRigidBody(0, pos, rot, groundShape);

            ground.Restitution = 0.5f;
            ground.Friction = 0.1f;
        }



        public void InitScene()
        {
            InitWorld();
            InitColliders();
            //GroundPlane(float3.Zero, float3.Zero);

            sphere = _world.AddRigidBody(10, new float3(0, 50, 0), float3.Zero, World.AddSphereShape(5));
            sphere.Restitution = 0.5f;
            sphere.Friction = 0.2f;


            sphere2 = _world.AddRigidBody(10, new float3(50, 50, 0), float3.Zero, World.AddSphereShape(5));
            sphere2.Restitution = 0.5f;
            sphere2.Friction = 0.2f;

        }

        public RigidBody GetBall()
        {
            return sphere;
        }

        public RigidBody GetBall2()
        {
            return sphere2;
        }

        public RigidBody[] GetWay()
        {
            return boxesWay;
        }

    }

    
}


