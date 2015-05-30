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

        private RigidBody box;
        private RigidBody sphere;
        //private RigidBody[] Spheres = new RigidBody[10];
        private RigidBody WaterSphere;
        private int sphereCounter = 0;

        private SceneContainer _scene;
       // private SceneContainer _scenePlayer;
        private IEnumerable<SceneNodeContainer> _sceneList;
        private SceneComponentContainer transf;


        public Physic()
        {
            InitScene();
        }

        public void InitWorld()
        {
            _world = new DynamicWorld();
        }

        private void InitSpheres(SceneContainer scenePlayer, string name)//, float3 center = (0,0,0)
        {

            //SphereCollider
            foreach (SceneNodeContainer node in scenePlayer.Children.FindNodes(node => node.Name.StartsWith("sphere") || node.Name.StartsWith("Sphere") || node.Name.Contains("") ))
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

                float3 center = (maxVert + minVert) / 2;

                if (node.Name.Contains("Air"))
                {
                    center = new float3(300, 0, 300);
                }

                float3 radius = ((maxVert - minVert) / 2) * 5;
                Debug.WriteLine("NAME SPHERE ::::::::" + name);

                //Collider 
                SphereCollider = _world.AddSphereShape(radius.x);
                //Parameter: Masse, Position, Rotation
                sphere = _world.AddRigidBody(10, new float3(center.x, 150, center.z), new float3(rot.x, rot.y, rot.z), SphereCollider);

                sphere.Restitution = 0.5f;
                sphere.Friction = 0.2f;
                sphere.SetDrag(0.0f, 0.05f);

                /*
                Spheres[sphereCounter] = sphere;
                sphereCounter++;
                 * */
                if (name == "water")
                {
                    WaterSphere = sphere;
                }
                
            }

        }
        
        public void InitColliders()
        {
            var ser = new Serializer();

            SceneContainer scenePlayer;

            using (var file = File.OpenRead(@"Assets/player_fire.fus"))
            {
                scenePlayer = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }
            InitSpheres(scenePlayer, "fire");
            /*
            using (var file = File.OpenRead(@"Assets/Player_water.fus"))
            {
                scenePlayer = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }
            InitSpheres(scenePlayer, "water");
            */
            WaterSphere = _world.AddRigidBody(10, new float3(0, 150, 0), float3.Zero, World.AddSphereShape(5));
            WaterSphere.Restitution = 0.5f;
            WaterSphere.Friction = 0.2f;

            using (var file = File.OpenRead(@"Assets/player_earth.fus"))
            {
                scenePlayer = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }
            InitSpheres(scenePlayer, "earth");

            using (var file = File.OpenRead(@"Assets/Player_air.fus"))
            {
                scenePlayer = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }
            InitSpheres(scenePlayer, "air");

            
            using (var file = File.OpenRead(@"Assets/Island_split.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }
            int counter = 0;
            //BoxCollider
            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("box") || node.Name.StartsWith("Box")))
            {   //Debug.WriteLine(" ################### box collider " + counter);
                counter++;
            
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

                float3 size = (maxVert / 2 - minVert / 2) * 2;

                if (node.Name.Contains("Baum"))
                {
                    size.y = size.y/4;
                }
                

                float3 center = (maxVert + minVert) / 2;

                if(node.Name.StartsWith("box.Island"))
                {
                    Debug.WriteLine("INSEL SIZE // CENTER MAXVERT " + size + " // " + center + " // " + maxVert);
                   
                }

                //Collider 
                BoxCollider = _world.AddBoxShape(size.x, size.y, size.z);
                //Parameter: Masse, Position, Rotation
                box = _world.AddRigidBody(0, new float3(center.x, center.y, center.z), new float3(rot.x, rot.y, rot.z), BoxCollider);

                box.Restitution = 1;
                box.Friction = 1;
                box.SetDrag(0.0f, 0.05f);
              
            }

           
        }

        public void InitScene()
        {
            InitWorld();
            InitColliders();


        }

       
        /*
        public RigidBody[] GetSphereColliders()
        {
            return Spheres;
        }
        */

        public RigidBody GetWaterSphere()
        {
            return WaterSphere;
        }

    }
}


