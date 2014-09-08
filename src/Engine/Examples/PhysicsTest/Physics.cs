using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using Fusee.Engine;
using Fusee.Math;
using Microsoft.Win32;

namespace Examples.PhysicsTest
{
    class Physics
    {
        private DynamicWorld _world;

        public DynamicWorld World
        {
            get { return _world; }
            set { _world = value; }
        }

        internal BoxShape MyBoxCollider;
        internal SphereShape MySphereCollider;
        internal CylinderShape MyCylinderCollider;
        internal ConvexHullShape MyConvHull;
        internal ConvexHullShape TeaPotHull;

        internal Mesh BoxMesh, TeaPotMesh, PlatonicMesh;

        private int _numRB;
        private string shapes;

        public int GetNumRB()
        {
            return _numRB;
        }

        public string GetShapes()
        {
            return shapes;
        }

        public Physics()
        {
            Debug.WriteLine("Physic: Constructor");
            //InitCollisionCallback();
            InitScene1();
            //InitDfo6Constraint();
            //Tester();
        }


        public void InitWorld()
        {
            _world = new DynamicWorld();
            _numRB = 0;
        }

        public void InitColliders()
        {
            MyBoxCollider = _world.AddBoxShape(2);
            MySphereCollider = _world.AddSphereShape(2);
            MyCylinderCollider = _world.AddCylinderShape(new float3(2, 4, 2));

            BoxMesh = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            TeaPotMesh = MeshReader.LoadMesh(@"Assets/Teapot.obj.model");
            PlatonicMesh = MeshReader.LoadMesh(@"Assets/Platonic.obj.model");
            float3[] verts = PlatonicMesh.Vertices;

            MyConvHull = _world.AddConvexHullShape(verts, true);

            float3[] vertsTeaPot = TeaPotMesh.Vertices;
            TeaPotHull = _world.AddConvexHullShape(vertsTeaPot, true);
            TeaPotHull.LocalScaling = new float3(0.05f, 0.05f,0.05f);

        }


        public void InitCollisionCallback()
        {
            InitWorld();
            InitColliders();
            GroundPlane(float3.Zero, float3.Zero);
            CollisionTest();
            shapes = "BoxShape, SphereShape";
        }




        public void InitScene1()
        {
            InitWorld();
            InitColliders();
            GroundPlane(float3.Zero, float3.Zero);
            FallingTower1();
            shapes = "BoxShape";
        }
        public void InitScene2()
        {
            InitWorld();
            InitColliders();
            GroundPlane(new float3(30, 15, 0), new float3(0, 0, (float)Math.PI / 6));
            GroundPlane(new float3(-20, 0, 0), float3.Zero);
            FallingPlatonics();
            InitPoint2PointConstraint();
            InitHingeConstraint();
            shapes = "BoxShape, ConvexHullShape";
        }
        public void InitScene3()
        {
            InitWorld();
            InitColliders();
            GroundPlane(new float3(30, 15, 0), new float3(0,0,(float)Math.PI/6));
            GroundPlane(new float3(-20, 0, 0), float3.Zero);
            FallingSpheres();
            InitKegel();
            shapes = "CylinderShape, SphereShape";
        }

        public void InitScene4()
        {
            InitWorld();
            InitColliders();
            GroundPlane(float3.Zero, float3.Zero);
            FallingTeaPots();
            shapes = "ConvexHullShape";
        }

        public void GroundPlane(float3 pos, float3 rot)
        {
            var groundShape = _world.AddBoxShape(30, 0.1f, 30);
            var ground = _world.AddRigidBody(0, pos, rot, groundShape);
            ground.Restitution = 1f;
            ground.Friction = 1;
            ground.IsTrigger = false;
            _numRB++;
        }

        public void InitKegel()
        {
            var shape = _world.AddCylinderShape(2f, 4, 2f);
            _world.AddRigidBody(1, new float3(-20, 3, 15), float3.Zero, shape);
            _world.AddRigidBody(1, new float3(-25, 3, 10), float3.Zero, shape);
            _world.AddRigidBody(1, new float3(-10, 3, -5), float3.Zero, shape);
            _world.AddRigidBody(1, new float3(-15, 3, 0), float3.Zero, shape);
            _world.AddRigidBody(1, new float3(-10, 3, -10), float3.Zero, shape);
            _numRB += 5;

        }

        public void InitHull()
        {
           float3[] verts = PlatonicMesh.Vertices;
          
           var shape = _world.AddConvexHullShape(verts);
           _world.AddRigidBody(1, new float3(20, 20,0), float3.Zero, shape);
            _numRB++;

        }

        public void FallingTower1()
        {
            for (int k = -1; k < 2; k++)
            {
                for (int h = -1; h < 2; h++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        var pos = new float3((2* h) , 15+ (k * 2), 2 * j);

                        RigidBody cube = _world.AddRigidBody(1, pos, float3.Zero, MyBoxCollider);
                        cube.Friction = 1.0f;
                        cube.Restitution = 0.75f;
                        cube.IsTrigger = false;
                        _numRB++;
                    }
                }
            }
        }

        public void FallingSpheres()
        {
            for (int k = 0; k < 2; k++)
            {
                for (int h = -2; h < 2; h++)
                {
                    for (int j = -2; j < 5; j++)
                    {

                        var pos = new float3((4 * h)+25, 50 + (k * 4), 4 * j);

                        var sphere = _world.AddRigidBody(1, pos, float3.Zero, MySphereCollider);
                        sphere.Friction = 0.5f;
                        sphere.Restitution = 0.8f;
                        sphere.IsTrigger = false;
                        _numRB++;
                    }
                }
            }
        }

        public void FallingPlatonics()
        {
            
            for (int k = 0; k < 1; k++)
            {
                for (int h = -2; h < 2; h++)
                {
                    for (int j = -2; j <2 ; j++)
                    {
                        var pos = new float3((4 * h) + 30, 50 + (k * 4), 4 * j);

                        var sphere = _world.AddRigidBody(1, pos, float3.Zero, MyConvHull);
                        
                        sphere.Friction = 0.5f;
                        sphere.Restitution = 0.2f;
                        _numRB++;
                    }
                }
            }

            
        }

        public void FallingTeaPots()
        {
            for (int k = 0; k < 4; k++)
            {
                for (int h = -2; h < 3; h++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        var pos = new float3((10*h), 20 + (k*10), 10*j);
                        var cube = _world.AddRigidBody(1, pos, float3.Zero, TeaPotHull);
                        cube.Friction = 1.0f;
                        cube.SetDrag(0.0f, 0.05f);
                        _numRB++;
                    }
                }
            }
        }

        public void InitPoint2PointConstraint()
        {
            
            var rbA = _world.AddRigidBody(1, new float3(-20, 15, 0), float3.Zero, MyBoxCollider);
            rbA.LinearFactor = new float3(0,0,0);
            rbA.AngularFactor = new float3(0, 0, 0);

            var rbB = _world.AddRigidBody(1, new float3(-21, 10, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            var p2p = _world.AddPoint2PointConstraint(rbA, rbB, new float3(0, -3f, 0), new float3(0, 2.5f, 0));
            p2p.SetParam(PointToPointFlags.PointToPointFlagsCfm, 0.9f);

            var rbC = _world.AddRigidBody(1, new float3(-21, 5, 2), float3.Zero, MyBoxCollider);
            _numRB++;
            var p2p1 = _world.AddPoint2PointConstraint(rbB, rbC, new float3(0, -2.5f, 0), new float3(0, 2.5f, 0));
  
        }

        public void InitHingeConstraint()
        {
            var rot = new float3(0, (float) Math.PI/4, 0);
            //var mesh = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            var rbA = _world.AddRigidBody(1, new float3(0, 15, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            rbA.LinearFactor = new float3(0, 0, 0);
            rbA.AngularFactor = new float3(0, 0, 0);

            var rbB = _world.AddRigidBody(1, new float3(0, 10, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            
            var hc = _world.AddHingeConstraint(rbA, rbB, new float3(0, -5, 0), new float3(0, 2, 0), new float3(0, 0, 1), new float3(0, 0, 1), false);

            hc.SetLimit(-(float)Math.PI * 0.25f, (float)Math.PI * 0.25f);
        }

        public void InitSliderConstraint()
        {
            var mesh = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            var rbA = _world.AddRigidBody(1, new float3(400, 500, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            rbA.LinearFactor = new float3(0, 0, 0);
            rbA.AngularFactor = new float3(0, 0, 0);

            var rbB = _world.AddRigidBody(1, new float3(200, 500, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            var frameInA = float4x4.Identity;
            frameInA.Row3 = new float4(0,1,0,1);
            var frameInB = float4x4.Identity;
            frameInA.Row3 = new float4(0, 0, 0, 1);
            var sc = _world.AddSliderConstraint(rbA, rbB, frameInA, frameInB, true);

        }

        public void InitGearConstraint()
        {
            var mesh = MeshReader.LoadMesh(@"Assets/Cube.obj.model");
            var rbA = _world.AddRigidBody(0, new float3(0, 150, 0), float3.Zero, MyBoxCollider);
            _numRB++;


            var rbB = _world.AddRigidBody(1, new float3(0, 300, 0), float3.Zero, MyBoxCollider);
            _numRB++;

        }

        public void InitDfo6Constraint()
        {
            InitWorld();
            GroundPlane(new float3(0, 0, 0), float3.Zero);
            var rbB = _world.AddRigidBody(1, new float3(0, 25, 0), float3.Zero, MyBoxCollider);
            _numRB++;
            var framInB = float4x4.CreateTranslation(new float3(0,-10,0));
            var dof6 = _world.AddGeneric6DofConstraint( rbB,  framInB, false);
            dof6.LinearLowerLimit = new float3(0,0,0);
            dof6.LinearUpperLimit = new float3(0,0,0);
            dof6.AngularLowerLimit = new float3(0,0,0);
            dof6.AngularUpperLimit = new float3(0,0,0);
        }

        public void CompoundShape()
        {
            var compShape = _world.AddCompoundShape(true);
            var box = _world.AddBoxShape(25);
            var sphere = _world.AddBoxShape(25);
            var matrixBox = float4x4.Identity;
            var matrixSphere = new float4x4(1, 0, 0, 2, 0, 1, 0, 2, 0, 0, 1, 2, 0, 0, 0, 1);
            compShape.AddChildShape(matrixBox, box);
            compShape.AddChildShape(matrixSphere, sphere);
            var rb = _world.AddRigidBody(1, new float3(0, 150, 0), float3.Zero, compShape);
            _numRB++;
        }

        public void InitGImpacShape()
        {
            var gimp = _world.AddGImpactMeshShape(TeaPotMesh);
            var rbB = _world.AddRigidBody(1, new float3(0, 10, 0), float3.Zero, gimp);
            _numRB++;
        }


        public void CollisionTest()
        {
            RigidBody box1 = _world.AddRigidBody(1, new float3(-10, 30, 0), float3.Zero, MyBoxCollider);
            box1.Friction = 0.5f;
            box1.Restitution = 0.8f;
            box1.IsTrigger = true;
            RigidBody box2 = _world.AddRigidBody(1, new float3(-10, 20, -10), float3.Zero, MyBoxCollider);
            box2.Friction = 0.5f;
            box2.Restitution = 0.8f;
            box2.IsTrigger = true;
            RigidBody box3 = _world.AddRigidBody(1, new float3(-10, 40, -20), float3.Zero, MyBoxCollider);
            box3.Friction = 0.5f;
            box3.Restitution = 0.8f;
            box3.IsTrigger = true;
            RigidBody box4 = _world.AddRigidBody(1, new float3(30, 20, 0), float3.Zero, MyBoxCollider);
            box4.Friction = 0.5f;
            box4.Restitution = 0.8f;
            box4.IsTrigger = true;
            RigidBody box5 = _world.AddRigidBody(1, new float3(-10, 20, 30), float3.Zero, MyBoxCollider);
            box5.Friction = 0.5f;
            box5.Restitution = 0.8f;
            box5.IsTrigger = true;
           
             RigidBody sphere1 = _world.AddRigidBody(1, new float3(10, 20, 0), float3.Zero, MySphereCollider);
            sphere1.Friction = 0.5f;
            sphere1.Restitution = 0.8f;
            sphere1.IsTrigger = true;

            RigidBody sphere2 = _world.AddRigidBody(1, new float3(10, 40, 0), float3.Zero, MySphereCollider);
            sphere2.Friction = 0.5f;
            sphere2.Restitution = 0.8f;
            sphere2.IsTrigger = true;

            RigidBody sphere3 = _world.AddRigidBody(1, new float3(20, 20, 0), float3.Zero, MySphereCollider);
            sphere3.Friction = 0.5f;
            sphere3.Restitution = 0.8f;
            sphere3.IsTrigger = true;

            RigidBody sphere4 = _world.AddRigidBody(1, new float3(10, 20, -20), float3.Zero, MySphereCollider);
            sphere4.Friction = 0.5f;
            sphere4.Restitution = 0.8f;
            sphere4.IsTrigger = true;

            RigidBody sphere5 = _world.AddRigidBody(1, new float3(10, 50, -20), float3.Zero, MySphereCollider);
            sphere5.Friction = 0.5f;
            sphere5.Restitution = 0.8f;
            sphere5.IsTrigger = true;
            
            shapes = "BoxShape, SphereShape";
        }

    }
}
