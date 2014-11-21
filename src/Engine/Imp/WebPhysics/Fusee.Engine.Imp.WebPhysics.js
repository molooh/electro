/* This file contains the hand generated part of the FUSEE implementation.
	 Classes defined here are used and called by the JSIL cross compiled part of FUSEE.
	 This file creates the connection to the underlying WebPhyiscs part.

	Just for the records: The first version of this file was generated using 
	JSIL v0.6.0 build 16283. From then on it was changed and maintained manually.
*/

var $WebPhysicsImp = JSIL.DeclareAssembly("Fusee.Engine.Imp.WebPhysics");

var $fuseeCore = JSIL.GetAssembly("Fusee.Engine.Core");
var $fuseeCommon = JSIL.GetAssembly("Fusee.Engine.Common");
var $fuseeMath = JSIL.GetAssembly("Fusee.Math.Core");

JSIL.DeclareNamespace("Fusee");
JSIL.DeclareNamespace("Fusee.Engine");



JSIL.MakeClass($jsilcore.TypeRef("System.Object"), "Fusee.Engine.WebPhysicsImp", true, [], function ($interfaceBuilder) {
    $ = $interfaceBuilder;

    $.Field({ Static: false, Public: true}, "World", $.Object, null);
    $.Field({ Static: false, Public: true }, "Gravity", $fuseeMath.TypeRef("Fusee.Math.float3"), null);
    


    $.Method({ Static: false, Public: true }, ".ctor",
        new JSIL.MethodSignature(null, []),
        function _ctor() {
            var collisionConfiguration = new Ammo.btDefaultCollisionConfiguration();
            var dispatcher = new Ammo.btCollisionDispatcher(collisionConfiguration);
            var overlappingPairCache = new Ammo.btDbvtBroadphase();
            var solver = new Ammo.btSequentialImpulseConstraintSolver();
            this.$WebPhysicsImp$World = new Ammo.btDiscreteDynamicsWorld(dispatcher, overlappingPairCache, solver, collisionConfiguration);
            this.$WebPhysicsImp$World.setGravity(new Ammo.btVector3(0, -10, 0));
        }
    );



    $.Method({ Static: false, Public: true }, "StepSimulation",
        new JSIL.MethodSignature(null, [$.Single, $.Single, $.Single]),
        function WebPhysicsImp_StepSimulation(timeSteps, maxSubSteps, fixedTimeSteps) {
            this.$WebPhysicsImp$World.stepSimulation(timeSteps, maxSubSteps);
        }
    );

    $.Method({ Static: false, Public: true }, "get_DynamicWorld",
        new JSIL.MethodSignature($.Object, []),
        function get_DynamicWorld() {
            return this.$WebPhysicsImp$World$value;
        }
    );
    
    $.Method({ Static: false, Public: true }, "set_Gravity",
        new JSIL.MethodSignature(null, [$fuseeMath.TypeRef("Fusee.Math.float3")]),
        function set_Gravity(value) {
            this.$WebPhysicsImp$Gravity$value = value;
        }
    );

    $.Method({ Static: false, Public: true }, "get_Gravity",
        new JSIL.MethodSignature($fuseeMath.TypeRef("Fusee.Math.float3"), []),
        function get_Gravity() {
            return this.$WebPhysicsImp$Gravity$value;
        }
    );

    $.Method({ Static: false, Public: true }, "Dispose",
        new JSIL.MethodSignature(null, []),
        function WebPhysicsImp_Dispose() {
           //Not implemented
        }
    );

    $.ImplementInterfaces(
        $fuseeCommon.TypeRef("Fusee.Engine.IDynamicWorldImp")
    );

    return function (newThisType) { $thisType = newThisType; };
});

