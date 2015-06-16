
using System.IO;
using Fusee.Engine;
using Fusee.Engine.SimpleScene;
using Fusee.Math;
using Fusee.Serialization;

namespace Examples.LevelTest
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
        private RigidBody _box;
        

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
            using (var file = File.OpenRead(@"Assets/Island_split_edit.fus"))
            {
                _scene = ser.Deserialize(file, null, typeof(SceneContainer)) as SceneContainer;

            }

            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("box")))
            {
                // Polygon-Auswahl ignorieren
                if (node.Name.Contains("Auswahl") || node.Name.Contains("Twigs") || node.Name.Contains("Stamm") || node.Name.Contains("Stamm") || node.Name.Contains("Ast"))
                {
                    continue;
                }
                
                //Position des Modells
                AABBf? aabb = new AABBCalculator(node).GetBox();
                float3 boxCenter = aabb.Value.Center;

                //Rotation des Modells
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

                if (node.Name.Contains("Baum"))
                {
                    size.x = 20;
                    size.z = 20;
                }

                BoxCollider = _world.AddBoxShape(size.x, size.y, size.z);
                //Parameter: Masse, Position, Rotation
                _box = _world.AddRigidBody(0, new float3(boxCenter.x, boxCenter.y, boxCenter.z), new float3(rot.x, rot.y, rot.z), BoxCollider);
                _box.Restitution = 0.5f;
                _box.Friction = 0.2f;
                _box.SetDrag(0.0f, 0.05f);
            }

            //SphereCollider
            foreach (SceneNodeContainer node in _scene.Children.FindNodes(node => node.Name.StartsWith("sphere") || node.Name.StartsWith("Sphere")))
            {

                if (node.Name.Contains("Auswahl"))
                {
                    continue;
                }

                AABBf? aabb = new AABBCalculator(node).GetBox();
                float3 size = aabb.Value.Size;
                float radius = size.x/2;
                float3 center = aabb.Value.Center;

                TransformComponent transform = node.GetTransform();
                float3 rot = transform.Rotation;

                SphereCollider = _world.AddSphereShape(radius);
                //Parameter: Masse, Position, Rotation
                var rbSphere = _world.AddRigidBody(0, new float3(center.x, center.y, center.z), new float3(rot.x, rot.y, rot.z), SphereCollider);
                rbSphere.Restitution = 0.5f;
                rbSphere.Friction = 0.2f;
                rbSphere.SetDrag(0.0f, 0.05f);
           
            }
        }

        public RigidBody InitSphere(float3 position)
        {
            var shape = World.AddSphereShape( 34); //5* 4 *0.2f)

            RigidBody sphereBody = _world.AddRigidBody(1, position, float3.Zero, shape);
            sphereBody.Restitution = 0.5f;
            sphereBody.Friction = 0.2f;

            return sphereBody;
        }

        public void InitScene()
        {
            InitWorld();
            InitColliders();
   
        }
    }
}
