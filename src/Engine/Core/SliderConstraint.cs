using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Math;

namespace Fusee.Engine
{
    public class SliderConstraint
    {
        internal ISliderConstraintImp _iSliderConstraintImp;

        /*public float3 AnchorInA
        {
            get
            {
                var retval = _iSliderConstraintImp.AnchorInA;
                return retval;
            }
        }
        public float3 AnchorInB
        {
            get
            {
                var retval = _iSliderConstraintImp.AnchorInB;
                return retval;
            }
        }*/

        public float AngularDepth
        {
            get
            {
                var retval = _iSliderConstraintImp.AngularDepth;
                return retval;
            }
        }
        public float AngularPosition
        {
            get
            {
                var retval = _iSliderConstraintImp.AngularPosition;
                return retval;
            }
        }

        public void CalculateTransforms(float4x4 transA, float4x4 transB)
        {
            _iSliderConstraintImp.CalculateTransforms(transA, transB);
        }
        public float4x4 CalculatedTransformA
        {
            get
            {
                var retval = _iSliderConstraintImp.CalculatedTransformA;
                return retval;
            }
        }
        public float4x4 CalculatedTransformB
        {
            get
            {
                var retval = _iSliderConstraintImp.CalculatedTransformB;
                return retval;
            }
        }

        public float DampingDirAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingDirAngular = value;
            }
        }
        public float DampingDirLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingDirLinear = value;
            }
        }
        public float DampingLimAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingLimAngular = value;
            }
        }
        public float DampingLimLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingLimLinear = value;
            }
        }
        public float DampingOrthoAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingOrthoAngular = value;
            }
        }
        public float DampingOrthoLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.DampingOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.DampingOrthoLinear = value;
            }
        }

        public float4x4 FrameOffsetA
        {
            get
            {
                var retval = _iSliderConstraintImp.FrameOffsetA;
                return retval;
            }
        }
        public float4x4 FrameOffsetB
        {
            get
            {
                var retval = _iSliderConstraintImp.FrameOffsetB;
                return retval;
            }
        }

        public float LinearDepth
        {
            get
            {
                var retval = _iSliderConstraintImp.LinearDepth;
                return retval;
            }
        }
        public float LinPos
        {
            get
            {
                var retval = _iSliderConstraintImp.LinPos;
                return retval;
            }
        }

        public float LowerAngularLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.LowerAngularLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.LowerAngularLimit = value;
            }
        }
        public float LowerLinearLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.LowerLinearLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.LowerLinearLimit = value;
            }
        }

        public float MaxAngularMotorForce
        {
            get
            {
                var retval = _iSliderConstraintImp.MaxAngularMotorForce;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.MaxAngularMotorForce = value;
            }
        }
        public float MaxLinearMotorForce
        {
            get
            {
                var retval = _iSliderConstraintImp.MaxLinearMotorForce;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.MaxLinearMotorForce = value;
            }
        }

        public bool PoweredAngularMotor
        {
            get
            {
                var retval = _iSliderConstraintImp.PoweredAngularMotor;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.PoweredAngularMotor = value;
            }
        }
        public bool PoweredLinearMotor
        {
            get
            {
                var retval = _iSliderConstraintImp.PoweredLinearMotor;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.PoweredLinearMotor = value;
            }
        }

        public float RestitutionDirAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionDirAngular = value;
            }
        }
        public float RestitutionDirLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionDirLinear = value;
            }
        }
        public float RestitutionLimAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionLimAngular = value;
            }
        }
        public float RestitutionLimLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionLimLinear = value;
            }
        }
        public float RestitutionOrthoAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionOrthoAngular = value;
            }
        }
        public float RestitutionOrthoLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.RestitutionOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.RestitutionOrthoLinear = value;
            }
        }

        public void SetFrames(float4x4 frameA, float4x4 frameB)
        {
            var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
            o._iSliderConstraintImp.SetFrames(frameA, frameB);
        }

        public float SoftnessDirAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessDirAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessDirAngular = value;
            }
        }
        public float SoftnessDirLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessDirLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessDirLinear = value;
            }
        }
        public float SoftnessLimAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessLimAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessLimAngular = value;
            }
        }
        public float SoftnessLimLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessLimLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessLimLinear = value;
            }
        }
        public float SoftnessOrthoAngular
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessOrthoAngular;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessOrthoAngular = value;
            }
        }
        public float SoftnessOrthoLinear
        {
            get
            {
                var retval = _iSliderConstraintImp.SoftnessOrthoLinear;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.SoftnessOrthoLinear = value;
            }
        }

        public bool SolveAngularLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.SolveAngularLimit;
                return retval;
            }
        }
        public bool SolveLinearLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.SolveLinearLimit;
                return retval;
            }
        }

        public float TargetAngularMotorVelocity
        {
            get
            {
                var retval = _iSliderConstraintImp.TargetAngularMotorVelocity;
                return retval;
            }
            set
            {
                var o = (SliderConstraint) _iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.TargetAngularMotorVelocity = value;
            }
        }

        public float TargetLinearMotorVelocity
        {
            get
            {
                var retval = _iSliderConstraintImp.TargetLinearMotorVelocity;
                return retval;
            }
            set
            {
                var o = (SliderConstraint) _iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.TargetLinearMotorVelocity = value;
            }
        }

        public void TestAngularLimits()
        {
            _iSliderConstraintImp.TestAngularLimits();
        }
        public void TestLinearLimits()
        {
            _iSliderConstraintImp.TestLinearLimits();
        }

        public float UpperAngularLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.UpperAngularLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.UpperAngularLimit = value;
            }
        }
        public float UpperLinearLimit
        {
            get
            {
                var retval = _iSliderConstraintImp.UpperLinearLimit;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.UpperLinearLimit = value;
            }
        }
       
        public bool UseFrameOffset
        {
            get
            {
                var retval = _iSliderConstraintImp.UseFrameOffset;
                return retval;
            }
            set
            {
                var o = (SliderConstraint)_iSliderConstraintImp.UserObject;
                o._iSliderConstraintImp.UseFrameOffset = value;
            }
        }

        public bool UseLinearReferenceFrameA
        {
            get
            {
                var retval = _iSliderConstraintImp.UseLinearReferenceFrameA;
                return retval;
            }
        }
        #region IConstraintImp
        public RigidBody RigidBodyA
        {
            get
            {
                var retval = _iSliderConstraintImp.RigidBodyA.UserObject;
                return (RigidBody)retval;
            }
        }

        public RigidBody RigidBodyB
        {
            get
            {
                var retval = _iSliderConstraintImp.RigidBodyB.UserObject;
                return (RigidBody)retval;
            }
        }
        public int GetUid()
        {
            var retval = _iSliderConstraintImp.GetUid();
            return retval;
        }
        #endregion IConstraintImp
    }
}
