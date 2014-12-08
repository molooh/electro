/* This file contains the hand generated part of the FUSEE implementation.
	 Classes defined here are used and called by the JSIL cross compiled part of FUSEE.
	 This file creates the connection to the underlying WebPhyiscs part.

	Just for the records: The first version of this file was generated using 
	JSIL v0.6.0 build 16283. From then on it was changed and maintained manually.
*/

var $WebPhysics = JSIL.DeclareAssembly("Fusee.Engine.Imp.WebPhysics");

var $fuseeEngine = JSIL.GetAssembly("Fusee.Engine");
var $fuseeCore = JSIL.GetAssembly("Fusee.Engine.Core");
var $fuseeCommon = JSIL.GetAssembly("Fusee.Engine.Common");
var $fuseeMath = JSIL.GetAssembly("Fusee.Math.Core");

JSIL.DeclareNamespace("Fusee");
JSIL.DeclareNamespace("Fusee.Engine");



JSIL.MakeClass($jsilcore.TypeRef("System.Object"), "Fusee.Engine.RigidBodyImp", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Property({ Static: false, Public: false }, "UserObject", $.Object, null);
    $.Field({ Static: false, Public: false }, "BtRigidBody", $.Object, null);
    $.Property({ Static: false, Public: false }, "Position", $fuseeMath.TypeRef("Fusee.Math.float3"));
    

    $.Method({ Static: false, Public: true }, ".ctor",
        new JSIL.MethodSignature(null, []),
        function _ctor() {
            //there is nothing to be implemented here
        }
    );

    $.Method({ Static: false, Public: true }, "get_UserObject",
        new JSIL.MethodSignature($.Object, []),
        function get_UserObject() {
            return this.$RigidbodyImp$UserObject$value;
        }
    );
    $.Method({ Static: false, Public: true }, "set_UserObject",
        new JSIL.MethodSignature(null, [$.Object]),
        function set_UserObject(value) {
            this.$RigidbodyImp$UserObject$value = value;
        }
    );

    $.Method({ Static: false, Public: true }, "get_Position",
       new JSIL.MethodSignature($fuseeMath.TypeRef("Fusee.Math.float3"), []),
       function get_Position() {
           console.log("get_Posiotion");
           var trans = new Ammo.btTransform();
           this.BtRigidBody.getMotionState().getWorldTransform(trans);
           var retval = new $fuseeMath.Fusee.Math.float3();
           retval.x = trans.getOrigin().x();
           retval.y = trans.getOrigin().y();
           retval.z = trans.getOrigin().z();
           return retval;
       }
   );
    $.Method({ Static: false, Public: true }, "set_Position",
        new JSIL.MethodSignature(null, [$fuseeMath.TypeRef("Fusee.Math.float3")]),
        function set_Position(value) {
            console.log("set_Position");
            var trans = new Ammo.btTransform();
            trans.setIdentity();
            trans.setOrigin(new Ammo.btVector3(value.x, value.y, value.z));
            this.BtRigidBody.getMotionState().setWorldTransform(trans);
        }
    );

    //TODO: Implement full RigidBodyImp functionality

    $.ImplementInterfaces(
        $fuseeCommon.TypeRef("Fusee.Engine.IRigidBodyImp")
    );

    return function (newThisType) { $thisType = newThisType; };
});


JSIL.MakeClass($jsilcore.TypeRef("System.Object"), "Fusee.Engine.CollisionShapeImp", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Property({ Static: false, Public: true }, "UserObject", $.Object, null);
   // $.Field({ Static: false, Public: false }, "BtCollisionShape", $.Ammo.btCollisionShape, null);

    $.Method({ Static: false, Public: true }, ".ctor",
        new JSIL.MethodSignature(null, []),
        function _ctor() {
            //there is nothing to be implemented here
        }
    );

    $.Method({ Static: false, Public: true }, "get_UserObject",
        new JSIL.MethodSignature($.Object, []),
        function CollisionShapeImp_get_UserObject() {
            return this.$CollisionShapeImp$UserObject;
        }
    );
    $.Method({ Static: false, Public: true }, "set_UserObject",
        new JSIL.MethodSignature(null, [$.Object]),
        function CollisionShapeImp_set_UserObject(value) {
            this.$CollisonShapeImp$UserObject$value = value;
        }
    );

    $.ImplementInterfaces(
        $fuseeCommon.TypeRef("Fusee.Engine.ICollisionShapeImp")
    );

    return function (newThisType) { $thisType = newThisType; };
});


JSIL.MakeClass($WebPhysics.TypeRef("Fusee.Engine.CollisionShapeImp"), "Fusee.Engine.BoxShapeImp", true, [], function($interfaceBuilder) {
    $ = $interfaceBuilder;

    //$.Field({ Static: false, Public: true }, "LocalScaling", $fuseeMath.TypeRef("Fusee.Math.float3"), null);
    $.Property({ Static: false, Public: false }, "UserObject", $.Object, null);
    $.Field({ Static: false, Public: false }, "BtBoxShape", $.Object, null);

    $.Method({ Static: false, Public: true }, ".ctor",
        new JSIL.MethodSignature(null, []),
        function _ctor() {
            //there is nothing to be implemented here
        }
    );

    $.Method({ Static: false, Public: true }, "get_LocalScaling",
        new JSIL.MethodSignature($fuseeMath.TypeRef("Fusee.Math.float3"), []),
        function get_LocalScaling() {
            return this.$BoxShapeImp$LocalScaling;
        }
    );
    $.Method({ Static: false, Public: true }, "set_LocalScaling",
        new JSIL.MethodSignature(null, [$fuseeMath.TypeRef("Fusee.Math.float3")]),
        function set_LocalScaling(value) {
            this.$BoxShapeImp$LocalScaling$value = value;
        }
    );

    $.Method({ Static: false, Public: true }, "get_UserObject",
        new JSIL.MethodSignature($.Object, []),
        function get_UserObject() {
            return this.CollisionShapeImp$UserObject
        }
    );
    $.Method({ Static: false, Public: true }, "set_UserObject",
        new JSIL.MethodSignature(null, [$.Object]),
        function set_UserObject(value) {
            this.$CollisonShapeImp$UserObject$value = value;
        }
    );

    $.ImplementInterfaces(
        $fuseeCommon.TypeRef("Fusee.Engine.IBoxShapeImp")
    );

    return function (newThisType) { $thisType = newThisType; };
});

JSIL.MakeClass($jsilcore.TypeRef("System.Object"), "Fusee.Engine.WebPhysicsImp", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Field({ Static: false, Public: false}, "World", $.Object, null);
    $.Property({ Static: false, Public: false }, "Gravity", $fuseeMath.TypeRef("Fusee.Math.float3"), null);

    $.Method({ Static: false, Public: true }, ".ctor",
        new JSIL.MethodSignature(null, []),
        function _ctor() {
            var collisionConfiguration = new Ammo.btDefaultCollisionConfiguration();
            var dispatcher = new Ammo.btCollisionDispatcher(collisionConfiguration);
            var overlappingPairCache = new Ammo.btDbvtBroadphase();
            var solver = new Ammo.btSequentialImpulseConstraintSolver();
            this.World = new Ammo.btDiscreteDynamicsWorld(dispatcher, overlappingPairCache, solver, collisionConfiguration);
            this.World.Gravity = new Ammo.btVector3(0, -10, 0);
            if (this.World == null) {
                console.log("World couldn't be created");
            } else {
                console.log("Brave New World");
                
            }

       }
    );

    //AddRigidBody
    $.Method({ Static: false, Public: true }, "AddRigidBody",
        new JSIL.MethodSignature($fuseeCommon.TypeRef("Fusee.Engine.IRigidbodyImp"), [$.Single, $fuseeMath.TypeRef("Fusee.Math.float3"), $fuseeMath.TypeRef("Fusee.Math.float3"), $fuseeCommon.TypeRef("Fusee.Engine.ICollisionShapeImp")]),
        function WebPhysicsImp_AddRigidBody(mass, worldTransform, orientation, colShape) {
            console.log("AddRigiBbody");
            //Todo: use the actual CollisionShape that was passed as a prarmeter
            //var colShape = new Ammo.btBoxShape(1);
            var btColShape;
            var shapeType = colShape.toString();
            console.log("ShapeType " + shapeType);
            switch (shapeType) {
                //Primitives
            case "Fusee.Engine.BoxShapeImp":
                var boxShapeImp = new $WebPhysics.Fusee.Engine.BoxShapeImp();
                boxShapeImp = colShape.BtBoxShape.UserObject;
                var btBoxHalfExtents = boxShapeImp.HalfExtents;
                btColShape = new Ammo.btBoxShape(btBoxHalfExtents);
                break;
            default:
                console.log("default");
            }

            //Set the start Position of the Rigidbody
            var startTransform = new Ammo.btTransform();
            startTransform.setIdentity();
            var mass = mass;
            var isDynamic = (mass != 0);
            var localInertia = new Ammo.btVector3(0, 0, 0);
           /* if (isDynamic)
                colShape.calculateLocalInertia(mass, localInertia);*/
            startTransform.setOrigin(new Ammo.btVector3(worldTransform.x, worldTransform.y, worldTransform.z));
            var myMotionState = new Ammo.btDefaultMotionState(startTransform);
            var rbInfo = new Ammo.btRigidBodyConstructionInfo(mass, myMotionState, btColShape, localInertia);
            var body = new Ammo.btRigidBody(rbInfo);
            this.World.addRigidBody(body);

            var retval = new $WebPhysics.Fusee.Engine.RigidBodyImp();
            retval.BtRigidBody = body;
            body.UserObject = retval;
            return retval;
        }
    );

    //AddBoxShape
    $.Method({ Static: false, Public: true }, "AddBoxShape",
        new JSIL.MethodSignature($fuseeCommon.TypeRef("Fusee.Engine.ICollisionShapeImp"), [$.Single]),
        function WebPhysicsImp_AddRigidBody(halfWidth) {
            var btBoxShape = new Ammo.btBoxShape(halfWidth, halfWidth, halfWidth);
            //BtCollisionShapes.Add(btBoxShape) TODO: extra alligned CollisionShape Array ??
            var retval = new $WebPhysics.Fusee.Engine.BoxShapeImp();
            retval.BtBoxShape = btBoxShape;
            btBoxShape.UserObject = retval;
            return retval;
        }
    );

    $.Method({ Static: false, Public: true }, "StepSimulation",
        new JSIL.MethodSignature(null, [$.Single, $.Single, $.Single]),
        function WebPhysicsImp_StepSimulation(timeSteps, maxSubSteps, fixedTimeSteps) {
            //console.log("stepSimulation");
            this.World.stepSimulation(timeSteps, maxSubSteps);
        }
    );

    $.Method({ Static: false, Public: true }, "get_DynamicWorld",
        new JSIL.MethodSignature($.Object, []),
        function get_DynamicWorld() {
            return this.World$;
        }
    );
    
    $.Method({ Static: false, Public: true }, "set_Gravity",
        new JSIL.MethodSignature(null, [$fuseeMath.TypeRef("Fusee.Math.float3")]),
        function set_Gravity(value) {
            this.World.setGravity = new Ammo.btVector3(value.z, value.y, value.z);
        }
    );

    $.Method({ Static: false, Public: true }, "get_Gravity",
        new JSIL.MethodSignature($fuseeMath.TypeRef("Fusee.Math.float3"), []),
        function get_Gravity() {
            console.log("get_Gravity" + this.World.getGravity);
            var retval = new $fuseeMath.Fusee.Math.float3(this.World.getGravity().x, this.World.getGravity().y, this.World.getGravity().z);
            return retval;
        }
    );

    $.Method({ Static: false, Public: true }, "Dispose",
        new JSIL.MethodSignature(null, []),
        function $WebPhysics_Dispose() {
           //Not implemented
        }
    );

    $.ImplementInterfaces(
        $fuseeCommon.TypeRef("Fusee.Engine.IDynamicWorldImp")
    );

    return function (newThisType) { $thisType = newThisType; };
});

