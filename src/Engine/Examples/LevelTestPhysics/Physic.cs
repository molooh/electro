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
        internal SphereShape SphereCollider;
        private SceneContainer _scene;
        private RigidBody box;
        public RigidBody sphere;
        public RigidBody sphere2;

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
            using (var file = File.OpenRead(@"Assets/Island_split_ph_woTexture.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }

            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("box")))
            {
                /**/
                if (node.Name.Contains("Auswahl"))
                {
                    continue;
                }
                
                AABBf? aabb = new AABBCalculator(node).GetBox();
                float3 boxSize = aabb.Value.Size;
                float3 boxSizeHalf = boxSize / 2;
                float3 boxCenter = aabb.Value.Center;

                TransformComponent transform = node.GetTransform();
                float3 rot = transform.Rotation;

                //Größe des Modells
                float3[] verts = node.GetMesh().Vertices;
                if (verts == null)
                {
                    continue;
                }
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

                float3 size = (maxVert / 2 - minVert / 2);
                float3 center = (maxVert + minVert) / 2;

                if (node.Name.Contains("Baum"))
                {
                    size.x = 20;
                    size.z = 20;

                }

                Debug.WriteLine("-------# " + " - " + node.Name);
                Debug.WriteLine("BBox Size: " +   boxSizeHalf);
                Debug.WriteLine("Verts Size: " + size);
                Debug.WriteLine("BBox Center: " + boxCenter);
                Debug.WriteLine("Verts center: " + center);
                Debug.WriteLine("Box Rotation: " + rot);
                Debug.WriteLine("-------------------------------");

                BoxCollider = _world.AddBoxShape(size.x, size.y, size.z);
                
                //Parameter: Masse, Position, Rotation
                box = _world.AddRigidBody(0, new float3(boxCenter.x, boxCenter.y, boxCenter.z), new float3(rot.x, rot.y, rot.z), BoxCollider);
             
                box.Restitution = 0.5f;
                box.Friction = 0.2f;
                box.SetDrag(0.0f, 0.05f);

               

            }

            //SphereCollider
            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("sphere") || node.Name.StartsWith("Sphere")))
            {
                MeshComponent mesh = node.GetMesh();

                if (node.Name.Contains("Auswahl"))
                {
                    continue;
                }

                AABBf? aabb = new AABBCalculator(node).GetBox();
                float3 boxSize = aabb.Value.Size;
                float radius = boxSize.x/2;
                float3 center = aabb.Value.Center;

                TransformComponent transform = node.GetTransform();
                float3 rot = transform.Rotation;

                Debug.WriteLine("-------# " + " - " + node.Name);
                Debug.WriteLine("Sphere radius: " + radius);
                Debug.WriteLine("Sphere Center: " + center);
                Debug.WriteLine("-------------------------------");

                SphereCollider = _world.AddSphereShape(radius);
                //Parameter: Masse, Position, Rotation
                var sphere = _world.AddRigidBody(0, new float3(center.x, 150, center.z), new float3(rot.x, rot.y, rot.z), SphereCollider);

                sphere.Restitution = 0.5f;
                sphere.Friction = 0.2f;
                sphere.SetDrag(0.0f, 0.05f);
           
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

            var shape =  World.AddSphereShape(5*4);
            sphere = _world.AddRigidBody(1, new float3(950, 100, 0), float3.Zero, shape);
            sphere.Restitution = 0.5f;
            sphere.Friction = 0.2f;
          
            sphere2 = _world.AddRigidBody(1, new float3(1000, 200, 0), float3.Zero,shape);
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


    }

    
}


