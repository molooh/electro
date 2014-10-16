using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletSharp;
using Fusee.Math;

namespace Fusee.Engine
{
    public class SliderConstraintImp : ISliderConstraintImp
    {
        internal SliderConstraint _sci;
        internal Translater Translater = new Translater();

        /*public float3 AnchorInA
        {
            get
            {
                var retval = Translater.BtVector3ToFloat3(_sci.AnchorInA);
                return retval;
            }
        }
        public float3 AnchorInB
        {
            get
            {
                var retval = Translater.BtVector3ToFloat3(_sci.AnchorInB);
                return retval;
            }
        }*/

        public float AngularDepth
        {
            get
            {
                var retval = _sci.AngularDepth;
                return retval;
            }
        }
        public float AngularPosition
        {
            get
            {
                var retval = _sci.AngularPosition;
                return retval;
            }
        }

        public void CalculateTransforms(float4x4 transA, float4x4 transB)
        {
            _sci.CalculateTransforms(Translater.Float4X4ToBtMatrix(transA), Translater.Float4X4ToBtMatrix(transB));
        }
        public float4x4 CalculatedTransformA
        {
            get
            {
                var retval = Translater.BtMatrixToFloat4X4(_sci.CalculatedTransformA);
                return retval;
            }
        }
        public float4x4 CalculatedTransformB
        {
            get
            {
                var retval = Translater.BtMatrixToFloat4X4(_sci.CalculatedTransformB);
                return retval;
            }
        }

        public float DampingDirAngular
        {
            get
            {
                var retval = _sci.DampingDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingDirAngular = value;
            }
        }
        public float DampingDirLinear
        {
            get
            {
                var retval = _sci.DampingDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingDirLinear = value;
            }
        }
        public float DampingLimAngular
        {
            get
            {
                var retval = _sci.DampingLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingLimAngular = value;
            }
        }
        public float DampingLimLinear
        {
            get
            {
                var retval = _sci.DampingLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingLimLinear = value;
            }
        }
        public float DampingOrthoAngular
        {
            get
            {
                var retval = _sci.DampingOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingOrthoAngular = value;
            }
        }
        public float DampingOrthoLinear
        {
            get
            {
                var retval = _sci.DampingOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.DampingOrthoLinear = value;
            }
        }

        public float4x4 FrameOffsetA
        {
            get
            {
                var retval = Translater.BtMatrixToFloat4X4(_sci.FrameOffsetA);
                return retval;
            }
        }
        public float4x4 FrameOffsetB
        {
            get
            {
                var retval = Translater.BtMatrixToFloat4X4(_sci.FrameOffsetB);
                return retval;
            }
        }

        public float LinearDepth
        {
            get
            {
                var retval = _sci.LinearDepth;
                return retval;
            }
        }
        public float LinPos
        {
            get
            {
                var retval = _sci.LinearPos;
                return retval;
            }
        }

        public float LowerAngularLimit
        {
            get
            {
                var retval = _sci.LowerAngularLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.LowerAngularLimit = value;
            }
        }
        public float LowerLinearLimit
        {
            get
            {
                var retval = _sci.LowerLinearLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.LowerLinearLimit = value;
            }
        }

        public float MaxAngularMotorForce
        {
            get
            {
                var retval = _sci.MaxAngMotorForce;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.MaxAngMotorForce = value;
            }
        }
        public float MaxLinearMotorForce
        {
            get
            {
                var retval = _sci.MaxLinearMotorForce;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.MaxLinearMotorForce = value;
            }
        }

        public bool PoweredAngularMotor
        {
            get
            {
                var retval = _sci.PoweredAngularMotor;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.PoweredAngularMotor = value;
            }
        }
        public bool PoweredLinearMotor
        {
            get
            {
                var retval = _sci.PoweredLinearMotor;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.PoweredLinearMotor = value;
            }
        }

        public float RestitutionDirAngular
        {
            get
            {
                var retval = _sci.RestitutionDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionDirAngular = value;
            }
        }
        public float RestitutionDirLinear
        {
            get
            {
                var retval = _sci.RestitutionDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionDirLinear = value;
            }
        }
        public float RestitutionLimAngular
        {
            get
            {
                var retval = _sci.RestitutionLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionLimAngular = value;
            }
        }
        public float RestitutionLimLinear
        {
            get
            {
                var retval = _sci.RestitutionLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionLimLinear = value;
            }
        }
        public float RestitutionOrthoAngular
        {
            get
            {
                var retval = _sci.RestitutionOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionOrthoAngular = value;
            }
        }
        public float RestitutionOrthoLinear
        {
            get
            {
                var retval = _sci.RestitutionOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.RestitutionOrthoLinear = value;
            }
        }

        public void SetFrames(float4x4 frameA, float4x4 frameB)
        {
            var o = (SliderConstraintImp)_sci.Userobject;
            o._sci.SetFrames(Translater.Float4X4ToBtMatrix(frameA), Translater.Float4X4ToBtMatrix(frameB));
        }

        public float SoftnessDirAngular
        {
            get
            {
                var retval = _sci.SoftnessDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessDirAngular = value;
            }
        }
        public float SoftnessDirLinear
        {
            get
            {
                var retval = _sci.SoftnessDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessDirLinear = value;
            }
        }
        public float SoftnessLimAngular
        {
            get
            {
                var retval = _sci.SoftnessLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessLimAngular = value;
            }
        }
        public float SoftnessLimLinear
        {
            get
            {
                var retval = _sci.SoftnessLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessLimLinear = value;
            }
        }
        public float SoftnessOrthoAngular
        {
            get
            {
                var retval = _sci.SoftnessOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessOrthoAngular = value;
            }
        }
        public float SoftnessOrthoLinear
        {
            get
            {
                var retval = _sci.SoftnessOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.SoftnessOrthoLinear = value;
            }
        }

        public bool SolveAngularLimit
        {
            get
            {
                var retval = _sci.SolveAngularLimit;
                return retval;
            }  
        }
        public bool SolveLinearLimit
        {
            get
            {
                var retval = _sci.SolveLinearLimit;
                return retval;
            }
        }

        public float TargetAngularMotorVelocity
        {
            get
            {
                var retval = _sci.TargetAngularMotorVelocity;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.TargetAngularMotorVelocity = value;
            }
        }
        public float TargetLinearMotorVelocity
        {
            get
            {
                var retval = _sci.TargetLinearMotorVelocity;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.TargetLinearMotorVelocity = value;
            }
        }

        public void TestAngularLimits()
        {
            _sci.TestAngularLimits();
        }
        public void TestLinearLimits()
        {
            _sci.TestLinearLimits();
        }

        public float UpperAngularLimit
        {
            get
            {
                var retval = _sci.UpperAngularLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.UpperAngularLimit = value;
            }
        }
        public float UpperLinearLimit
        {
            get
            {
                var retval = _sci.UpperLinearLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.UpperLinearLimit = value;
            }
        }
        
        public bool UseFrameOffset
        {
            get
            {
                var retval = _sci.UseFrameOffset;
                return retval;
            }
            set
            {
                var o = (SliderConstraintImp)_sci.Userobject;
                o._sci.UseFrameOffset = value;
            }
        }
        
        public bool UseLinearReferenceFrameA
        {
            get
            {
                var retval = _sci.UseLinearReferenceFrameA;
                return retval;
            }
        }

        #region IConstraintImp
        public IRigidBodyImp RigidBodyA
        {
            get
            {
                var retval = _sci.RigidBodyA;
                return (RigidBodyImp)retval.UserObject;
            }
        }
        public IRigidBodyImp RigidBodyB
        {
            get
            {
                var retval = _sci.RigidBodyB;
                return (RigidBodyImp)retval.UserObject;
            }
        }
        public int GetUid()
        {
            var retval = _sci.Uid;
            return retval;
        }

        private object _userObject;
        public object UserObject
        {
            get { return _userObject; }
            set { _userObject = value; }
        }
        #endregion IConstraintImp 
    }
}
