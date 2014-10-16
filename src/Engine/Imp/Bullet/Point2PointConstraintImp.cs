using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusee.Math;
using BulletSharp;

namespace Fusee.Engine
{
    public class Point2PointConstraintImp : IPoint2PointConstraintImp
    {
        internal Point2PointConstraint _p2pci;
        internal Translater Translater;
        

        public float3 PivotInA
        {
            get
            {
                var retval = new float3(_p2pci.PivotInA.X, _p2pci.PivotInA.Y, _p2pci.PivotInA.Z);
                return retval;
            }
            set
            {
                var pivoA = Translater.Float3ToBtVector3(value);
                var o = (Point2PointConstraintImp)_p2pci.Userobject;
                o._p2pci.SetPivotA(pivoA);
            }
        }
        public float3 PivotInB
        {
            get
            {
                var retval = new float3(_p2pci.PivotInB.X, _p2pci.PivotInB.Y, _p2pci.PivotInB.Z);
                return retval;
            }
            set
            {
                var pivoB = Translater.Float3ToBtVector3(value);
                var o = (Point2PointConstraintImp)_p2pci.Userobject;
                o._p2pci.SetPivotB(pivoB);
            }
            
        }

        public void UpdateRhs(float timeStep)
        {
            var o = (Point2PointConstraintImp)_p2pci.Userobject;
            o._p2pci.UpdateRHS(timeStep);
        }

        public void SetParam(PointToPointFlags param, float value, int axis = -1)
        {
            var o = (Point2PointConstraintImp)_p2pci.Userobject;
            ConstraintParam constraintParam;
            switch (param)
            {
                case PointToPointFlags.PointToPointFlagsErp:
                    constraintParam = ConstraintParam.Erp;
                    break;
                case PointToPointFlags.PointToPointFlagsStopErp:
                    constraintParam = ConstraintParam.StopErp;
                    break;
                case PointToPointFlags.PointToPointFlagsCfm:
                    constraintParam = ConstraintParam.Cfm;
                    break;
                case PointToPointFlags.PointToPointFlagsStopCfm:
                    constraintParam = ConstraintParam.StopCfm;
                    break;
                default:
                    constraintParam = ConstraintParam.Cfm;
                    break;

            }

            o._p2pci.SetParam(constraintParam, value, axis);
        }
        public float GetParam(PointToPointFlags param, int axis = -1)
        {

            var typedConstraint = _p2pci.GetParam(ConstraintParam.Cfm, axis);
            switch (param)
            {
                case PointToPointFlags.PointToPointFlagsErp:
                    typedConstraint = _p2pci.GetParam(ConstraintParam.Erp, axis);
                    //constraintParam = 1;
                    break;
                case PointToPointFlags.PointToPointFlagsStopErp:
                    typedConstraint = _p2pci.GetParam(ConstraintParam.StopErp, axis);
                    //constraintParam = 2;
                    break;
                case PointToPointFlags.PointToPointFlagsCfm:
                    typedConstraint = _p2pci.GetParam(ConstraintParam.Cfm, axis);    
                    //constraintParam = 3;
                    break;
                case PointToPointFlags.PointToPointFlagsStopCfm:
                    typedConstraint = _p2pci.GetParam(ConstraintParam.StopCfm, axis);  
                    //constraintParam = 4;
                    break;
                default:
                    typedConstraint = _p2pci.GetParam(ConstraintParam.Cfm, axis);
                    break; 
            }
            return typedConstraint;
        }

        public IRigidBodyImp RigidBodyA
        {
            get
            {
                var retval = _p2pci.RigidBodyA;
                return (RigidBodyImp)retval.UserObject;
            }
        }
        public IRigidBodyImp RigidBodyB
        {
            get
            {
                var retval = _p2pci.RigidBodyB;
                return (RigidBodyImp)retval.UserObject;
            }
        }
        public int GetUid()
        {
            var retval = _p2pci.Uid;
            return retval;
        }
        private object _userObject;
        public object UserObject
        {
            get { return _userObject; }
            set { _userObject = value; }
        }
    }
}
