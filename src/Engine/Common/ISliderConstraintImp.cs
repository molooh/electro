using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Fusee.Math;

namespace Fusee.Engine
{
    public interface ISliderConstraintImp : IConstraintImp
    {
        /*float3 AnchorInA { get; }
        float3 AnchorInB { get; }*/
        float AngularDepth { get; }
        float AngularPosition { get; }

        void CalculateTransforms(float4x4 transA, float4x4 transB);
        float4x4 CalculatedTransformA { get; }
        float4x4 CalculatedTransformB { get; }

        float DampingDirAngular { get; set; }
        float DampingDirLinear { get; set; }
        float DampingLimAngular { get; set; }
        float DampingLimLinear { get; set; }
        float DampingOrthoAngular { get; set; }
        float DampingOrthoLinear { get; set; }

        float4x4 FrameOffsetA { get; }
        float4x4 FrameOffsetB { get; }

        float LinearDepth { get; }
        float LinPos { get; }

        float LowerAngularLimit { get; set; }
        float LowerLinearLimit { get; set; }

        float MaxAngularMotorForce { get; set; }
        float MaxLinearMotorForce { get; set; }

        bool PoweredAngularMotor { get; set; }
        bool PoweredLinearMotor { get; set; }

        float RestitutionDirAngular { get; set; }
        float RestitutionDirLinear { get; set; }
        float RestitutionLimAngular { get; set; }
        float RestitutionLimLinear { get; set; }
        float RestitutionOrthoAngular { get; set; }
        float RestitutionOrthoLinear { get; set; }

        void SetFrames(float4x4 frameA, float4x4 frameB);

        float SoftnessDirAngular { get; set; }
        float SoftnessDirLinear { get; set; }
        float SoftnessLimAngular { get; set; }
        float SoftnessLimLinear { get; set; }
        float SoftnessOrthoAngular { get; set; }
        float SoftnessOrthoLinear { get; set; }

        bool SolveAngularLimit { get;}
        bool SolveLinearLimit { get; }

        float TargetAngularMotorVelocity { get; set; }
        float TargetLinearMotorVelocity { get; set; }

        void TestAngularLimits();
        void TestLinearLimits();

        float UpperAngularLimit { get; set; }
        float UpperLinearLimit { get; set; }

        bool UseFrameOffset { get; set; }

        bool UseLinearReferenceFrameA { get; }

    }
}
