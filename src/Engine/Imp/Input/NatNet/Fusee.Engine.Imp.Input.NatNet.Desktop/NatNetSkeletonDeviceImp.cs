using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using Fusee.Engine.Common;
using Fusee.Math.Core;
using NatNetML;

namespace Fusee.Engine.Imp.Input.NatNet.Desktop
{
    public class NatNetSkeletonDeviceImp : IInputDeviceImp
    {
        public string Name { get; }
        private int _natNetId;

        private NatNetDriverImp _natNetDriver;

        private SkeletonData _skeletonData;
        private float[] _lastValues;

        public NatNetSkeletonDeviceImp(string name)
        {
            Name = name;
            _lastValues = new float[AxesCount];
        }

        public string Id => GetType().FullName + "_" + Name + "_" + _natNetId;
        public string Desc => "NatNet Skeleton device for the Baseline marker set.";
        public DeviceCategory Category => DeviceCategory.Skeleton;
        public int AxesCount => 147;
        public IEnumerable<AxisImpDescription> AxisImpDesc
        {
            get
            {
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_X",
                        Id = 0,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_Y",
                        Id = 1,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_Z",
                        Id = 2,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_QX",
                        Id = 3,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_QY",
                        Id = 4,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_QZ",
                        Id = 5,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Hips_QW",
                        Id = 6,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_X",
                        Id = 7,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_Y",
                        Id = 8,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_Z",
                        Id = 9,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_QX",
                        Id = 10,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_QY",
                        Id = 11,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_QZ",
                        Id = 12,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine_QW",
                        Id = 13,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_X",
                        Id = 14,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_Y",
                        Id = 15,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_Z",
                        Id = 16,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_QX",
                        Id = 17,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_QY",
                        Id = 18,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_QZ",
                        Id = 19,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Spine1_QW",
                        Id = 20,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_X",
                        Id = 21,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_Y",
                        Id = 22,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_Z",
                        Id = 23,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_QX",
                        Id = 24,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_QY",
                        Id = 25,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_QZ",
                        Id = 26,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Neck_QW",
                        Id = 27,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_X",
                        Id = 28,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_Y",
                        Id = 29,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_Z",
                        Id = 30,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_QX",
                        Id = 31,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_QY",
                        Id = 32,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_QZ",
                        Id = 33,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "Head_QW",
                        Id = 34,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_X",
                        Id = 35,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_Y",
                        Id = 36,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_Z",
                        Id = 37,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_QX",
                        Id = 38,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_QY",
                        Id = 39,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_QZ",
                        Id = 40,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftShoulder_QW",
                        Id = 41,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_X",
                        Id = 42,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_Y",
                        Id = 43,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_Z",
                        Id = 44,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_QX",
                        Id = 45,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_QY",
                        Id = 46,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_QZ",
                        Id = 47,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftArm_QW",
                        Id = 48,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_X",
                        Id = 49,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_Y",
                        Id = 50,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_Z",
                        Id = 51,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_QX",
                        Id = 52,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_QY",
                        Id = 53,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_QZ",
                        Id = 54,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftForeArm_QW",
                        Id = 55,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_X",
                        Id = 56,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_Y",
                        Id = 57,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_Z",
                        Id = 58,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_QX",
                        Id = 59,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_QY",
                        Id = 60,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_QZ",
                        Id = 61,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftHand_QW",
                        Id = 62,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_X",
                        Id = 63,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_Y",
                        Id = 64,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_Z",
                        Id = 65,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_QX",
                        Id = 66,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_QY",
                        Id = 67,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_QZ",
                        Id = 68,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightShoulder_QW",
                        Id = 69,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_X",
                        Id = 70,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_Y",
                        Id = 71,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_Z",
                        Id = 72,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_QX",
                        Id = 73,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_QY",
                        Id = 74,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_QZ",
                        Id = 75,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightArm_QW",
                        Id = 76,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_X",
                        Id = 77,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_Y",
                        Id = 78,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_Z",
                        Id = 79,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_QX",
                        Id = 80,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_QY",
                        Id = 81,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_QZ",
                        Id = 82,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightForeArm_QW",
                        Id = 83,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_X",
                        Id = 84,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_Y",
                        Id = 85,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_Z",
                        Id = 86,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_QX",
                        Id = 87,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_QY",
                        Id = 88,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_QZ",
                        Id = 89,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightHand_QW",
                        Id = 90,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_X",
                        Id = 91,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_Y",
                        Id = 92,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_Z",
                        Id = 93,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_QX",
                        Id = 94,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_QY",
                        Id = 95,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_QZ",
                        Id = 96,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftUpLeg_QW",
                        Id = 97,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_X",
                        Id = 98,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_Y",
                        Id = 99,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_Z",
                        Id = 100,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_QX",
                        Id = 101,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_QY",
                        Id = 102,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_QZ",
                        Id = 103,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftLeg_QW",
                        Id = 104,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_X",
                        Id = 105,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_Y",
                        Id = 106,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_Z",
                        Id = 107,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_QX",
                        Id = 108,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_QY",
                        Id = 109,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_QZ",
                        Id = 110,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftFoot_QW",
                        Id = 111,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_X",
                        Id = 112,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_Y",
                        Id = 113,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_Z",
                        Id = 114,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_QX",
                        Id = 115,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_QY",
                        Id = 116,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_QZ",
                        Id = 117,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "LeftToeBase_QW",
                        Id = 118,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_X",
                        Id = 119,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_Y",
                        Id = 120,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_Z",
                        Id = 121,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_QX",
                        Id = 122,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_QY",
                        Id = 123,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_QZ",
                        Id = 124,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightUpLeg_QW",
                        Id = 125,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_X",
                        Id = 126,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_Y",
                        Id = 127,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_Z",
                        Id = 128,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_QX",
                        Id = 129,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_QY",
                        Id = 130,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_QZ",
                        Id = 131,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightLeg_QW",
                        Id = 132,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_X",
                        Id = 133,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_Y",
                        Id = 134,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_Z",
                        Id = 135,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_QX",
                        Id = 136,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_QY",
                        Id = 137,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_QZ",
                        Id = 138,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightFoot_QW",
                        Id = 139,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_X",
                        Id = 140,
                        Direction = AxisDirection.X,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                        MinValueOrAxis = 0,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_Y",
                        Id = 141,
                        Direction = AxisDirection.Y,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_Z",
                        Id = 142,
                        Direction = AxisDirection.Z,
                        Nature = AxisNature.Position,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_QX",
                        Id = 143,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_QY",
                        Id = 144,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_QZ",
                        Id = 145,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "RightToeBase_QW",
                        Id = 146,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };

            }
        }

        public float GetAxis(int iAxisId)
        {
            _skeletonData = _natNetDriver.GetSkeletonData(_natNetId);

            float value = 0;

            if (_skeletonData != null)
            { if (_skeletonData.nRigidBodies != 21)
            {
                Debug.WriteLine("{0} Warning: This NatNet skeleton device is build for a 21 bone basline marker set. There are {1} bones present in the current dataset.", Id, _skeletonData.nRigidBodies);
            }

            int divRemainder;
            int div = System.Math.DivRem(iAxisId, 7, out divRemainder);

                if (_skeletonData.RigidBodies[div] != null)
                {
                    RigidBodyData rbData = _skeletonData.RigidBodies[div];

                    switch (divRemainder)
                    {
                        case 0:
                            value = rbData.x;
                            break;
                        case 1:
                            value = rbData.y;
                            break;
                        case 2:
                            value = rbData.z;
                            break;
                        case 3:
                            value = rbData.qx;
                            break;
                        case 4:
                            value = rbData.qy;
                            break;
                        case 5:
                            value = rbData.qz;
                            break;
                        case 6:
                            value = rbData.qw;
                            break;
                        default:
                            value = 0;
                            break;
                    }
                }

                if (_lastValues[iAxisId] != value)
                {
                    _lastValues[iAxisId] = value;

                    AxisDescription axisdesc = new AxisDescription();

                    foreach (var axisImpDescription in AxisImpDesc)
                    {
                        if (axisImpDescription.AxisDesc.Id == iAxisId)
                        {
                            axisdesc = axisImpDescription.AxisDesc;
                        }
                    }

                    AxisValueChanged?.Invoke(this, new AxisValueChangedArgs() {Axis = axisdesc, Value = value});
                }
            }
            return value;
    }

        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;
        public int ButtonCount => 0;
        public IEnumerable<ButtonImpDescription> ButtonImpDesc
        {
            get
            {
                yield break;
            }
        }
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException($"This device supports no pollable buttons.");
        }

        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;

        public void SetDriverReference(NatNetDriverImp natNetTrackerDriverImp, int natNetId)
        {
            _natNetDriver = natNetTrackerDriverImp;
            _natNetId = natNetId;
        }
    }
}
