using System.Collections.Generic;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;

namespace Fusee.Engine.Imp.Input.NatNet
{
    /// <summary>
    /// Special case of an <see cref="InputDevice"/>.
    /// Defines convenience methods to access the position and orientation.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Core.InputDevice" />
    public class NatNetSkeleton : InputDevice
    {
        /// <summary>
        /// Gets the position and orientation of the hips.
        /// </summary>
        /// <value>
        /// The hips.
        /// </value>
        public SkeletonRigidBodyData Hips => new SkeletonRigidBodyData(new float3(GetAxis(0), GetAxis(1), _coordinateSystemCompensation * GetAxis(2)), new Quaternion(GetAxis(3), GetAxis(4), _coordinateSystemCompensation * GetAxis(5), GetAxis(6)));
        /// <summary>
        /// Gets the position and orientation of the spine.
        /// </summary>
        /// <value>
        /// The spine.
        /// </value>
        public SkeletonRigidBodyData Spine => new SkeletonRigidBodyData(new float3(GetAxis(7), GetAxis(8), _coordinateSystemCompensation * GetAxis(9)), new Quaternion(GetAxis(10), GetAxis(11), _coordinateSystemCompensation * GetAxis(12), GetAxis(23)));
        /// <summary>
        /// Gets the position and orientation of the spine1.
        /// </summary>
        /// <value>
        /// The spine1.
        /// </value>
        public SkeletonRigidBodyData Spine1 => new SkeletonRigidBodyData(new float3(GetAxis(14), GetAxis(15), _coordinateSystemCompensation * GetAxis(16)), new Quaternion(GetAxis(17), GetAxis(18), _coordinateSystemCompensation * GetAxis(19), GetAxis(20)));
        /// <summary>
        /// Gets the position and orientation of the neck.
        /// </summary>
        /// <value>
        /// The neck.
        /// </value>
        public SkeletonRigidBodyData Neck => new SkeletonRigidBodyData(new float3(GetAxis(21), GetAxis(22), _coordinateSystemCompensation * GetAxis(23)), new Quaternion(GetAxis(24), GetAxis(25), _coordinateSystemCompensation * GetAxis(26), GetAxis(27)));
        /// <summary>
        /// Gets the position and orientation of the head.
        /// </summary>
        /// <value>
        /// The head.
        /// </value>
        public SkeletonRigidBodyData Head => new SkeletonRigidBodyData(new float3(GetAxis(28), GetAxis(29), _coordinateSystemCompensation * GetAxis(30)), new Quaternion(GetAxis(31), GetAxis(32), _coordinateSystemCompensation * GetAxis(33), GetAxis(34)));
        /// <summary>
        /// Gets the position and orientation of the left shoulder.
        /// </summary>
        /// <value>
        /// The left shoulder.
        /// </value>
        public SkeletonRigidBodyData LeftShoulder => new SkeletonRigidBodyData(new float3(GetAxis(35), GetAxis(36), _coordinateSystemCompensation * GetAxis(37)), new Quaternion(GetAxis(38), GetAxis(39), _coordinateSystemCompensation * GetAxis(40), GetAxis(41)));
        /// <summary>
        /// Gets the position and orientation of the left arm.
        /// </summary>
        /// <value>
        /// The left arm.
        /// </value>
        public SkeletonRigidBodyData LeftArm => new SkeletonRigidBodyData(new float3(GetAxis(42), GetAxis(43), _coordinateSystemCompensation * GetAxis(44)), new Quaternion(GetAxis(45), GetAxis(46), _coordinateSystemCompensation * GetAxis(47), GetAxis(48)));
        /// <summary>
        /// Gets the position and orientation of the left fore arm.
        /// </summary>
        /// <value>
        /// The left fore arm.
        /// </value>
        public SkeletonRigidBodyData LeftForeArm => new SkeletonRigidBodyData(new float3(GetAxis(49), GetAxis(50), _coordinateSystemCompensation * GetAxis(51)), new Quaternion(GetAxis(52), GetAxis(53), _coordinateSystemCompensation * GetAxis(54), GetAxis(55)));
        /// <summary>
        /// Gets the position and orientation of the left hand.
        /// </summary>
        /// <value>
        /// The left hand.
        /// </value>
        public SkeletonRigidBodyData LeftHand => new SkeletonRigidBodyData(new float3(GetAxis(56), GetAxis(57), _coordinateSystemCompensation * GetAxis(58)), new Quaternion(GetAxis(59), GetAxis(60), _coordinateSystemCompensation * GetAxis(61), GetAxis(62)));
        /// <summary>
        /// Gets the position and orientation of the right shoulder.
        /// </summary>
        /// <value>
        /// The right shoulder.
        /// </value>
        public SkeletonRigidBodyData RightShoulder => new SkeletonRigidBodyData(new float3(GetAxis(63), GetAxis(64), _coordinateSystemCompensation * GetAxis(65)), new Quaternion(GetAxis(66), GetAxis(67), _coordinateSystemCompensation * GetAxis(68), GetAxis(69)));
        /// <summary>
        /// Gets the position and orientation of the right arm.
        /// </summary>
        /// <value>
        /// The right arm.
        /// </value>
        public SkeletonRigidBodyData RightArm => new SkeletonRigidBodyData(new float3(GetAxis(70), GetAxis(71), _coordinateSystemCompensation * GetAxis(72)), new Quaternion(GetAxis(73), GetAxis(74), _coordinateSystemCompensation * GetAxis(75), GetAxis(76)));
        /// <summary>
        /// Gets the position and orientation of the right fore arm.
        /// </summary>
        /// <value>
        /// The right fore arm.
        /// </value>
        public SkeletonRigidBodyData RightForeArm => new SkeletonRigidBodyData(new float3(GetAxis(77), GetAxis(78), _coordinateSystemCompensation * GetAxis(79)), new Quaternion(GetAxis(80), GetAxis(81), _coordinateSystemCompensation * GetAxis(82), GetAxis(83)));
        /// <summary>
        /// Gets the position and orientation of the right hand.
        /// </summary>
        /// <value>
        /// The right hand.
        /// </value>
        public SkeletonRigidBodyData RightHand => new SkeletonRigidBodyData(new float3(GetAxis(84), GetAxis(85), _coordinateSystemCompensation * GetAxis(86)), new Quaternion(GetAxis(87), GetAxis(88), _coordinateSystemCompensation * GetAxis(89), GetAxis(90)));
        /// <summary>
        /// Gets the position and orientation of the left upper leg.
        /// </summary>
        /// <value>
        /// The left up leg.
        /// </value>
        public SkeletonRigidBodyData LeftUpLeg => new SkeletonRigidBodyData(new float3(GetAxis(91), GetAxis(92), _coordinateSystemCompensation * GetAxis(93)), new Quaternion(GetAxis(94), GetAxis(95), _coordinateSystemCompensation * GetAxis(96), GetAxis(97)));
        /// <summary>
        /// Gets the position and orientation of the left leg.
        /// </summary>
        /// <value>
        /// The left leg.
        /// </value>
        public SkeletonRigidBodyData LeftLeg => new SkeletonRigidBodyData(new float3(GetAxis(98), GetAxis(99), _coordinateSystemCompensation * GetAxis(100)), new Quaternion(GetAxis(101), GetAxis(102), _coordinateSystemCompensation * GetAxis(103), GetAxis(104)));
        /// <summary>
        /// Gets the position and orientation of the left foot.
        /// </summary>
        /// <value>
        /// The left foot.
        /// </value>
        public SkeletonRigidBodyData LeftFoot => new SkeletonRigidBodyData(new float3(GetAxis(105), GetAxis(106), _coordinateSystemCompensation * GetAxis(107)), new Quaternion(GetAxis(108), GetAxis(109), _coordinateSystemCompensation * GetAxis(110), GetAxis(111)));
        /// <summary>
        /// Gets the position and orientation of the left toe base.
        /// </summary>
        /// <value>
        /// The left toe base.
        /// </value>
        public SkeletonRigidBodyData LeftToeBase => new SkeletonRigidBodyData(new float3(GetAxis(112), GetAxis(113), _coordinateSystemCompensation * GetAxis(114)), new Quaternion(GetAxis(115), GetAxis(116), _coordinateSystemCompensation * GetAxis(117), GetAxis(118)));
        /// <summary>
        /// Gets the position and orientation of the right up leg.
        /// </summary>
        /// <value>
        /// The right up leg.
        /// </value>
        public SkeletonRigidBodyData RightUpLeg => new SkeletonRigidBodyData(new float3(GetAxis(119), GetAxis(120), _coordinateSystemCompensation * GetAxis(121)), new Quaternion(GetAxis(122), GetAxis(123), _coordinateSystemCompensation * GetAxis(124), GetAxis(125)));
        /// <summary>
        /// Gets the position and orientation of the right leg.
        /// </summary>
        /// <value>
        /// The right leg.
        /// </value>
        public SkeletonRigidBodyData RightLeg => new SkeletonRigidBodyData(new float3(GetAxis(126), GetAxis(127), _coordinateSystemCompensation * GetAxis(128)), new Quaternion(GetAxis(129), GetAxis(130), _coordinateSystemCompensation * GetAxis(131), GetAxis(132)));
        /// <summary>
        /// Gets the position and orientation of the right foot.
        /// </summary>
        /// <value>
        /// The right foot.
        /// </value>
        public SkeletonRigidBodyData RightFoot => new SkeletonRigidBodyData(new float3(GetAxis(133), GetAxis(134), _coordinateSystemCompensation * GetAxis(135)), new Quaternion(GetAxis(136), GetAxis(137), _coordinateSystemCompensation * GetAxis(138), GetAxis(139)));
        /// <summary>
        /// Gets the position and orientation of the right toe base.
        /// </summary>
        /// <value>
        /// The right toe base.
        /// </value>
        public SkeletonRigidBodyData RightToeBase => new SkeletonRigidBodyData(new float3(GetAxis(140), GetAxis(141), _coordinateSystemCompensation * GetAxis(142)), new Quaternion(GetAxis(143), GetAxis(144), _coordinateSystemCompensation * GetAxis(145), GetAxis(146)));

        /// <summary>
        /// Allows to enumerate through all the bones.
        /// </summary>
        /// <value>
        /// The bones.
        /// </value>
        public IEnumerable<SkeletonRigidBodyData> Bones
        {
            get
            {
                yield return Hips;
                yield return Spine;
                yield return Spine1;
                yield return Neck;
                yield return Head;
                yield return LeftShoulder;
                yield return LeftArm;
                yield return LeftForeArm;
                yield return LeftHand;
                yield return RightShoulder;
                yield return RightArm;
                yield return RightForeArm;
                yield return RightHand;
                yield return LeftUpLeg;
                yield return LeftLeg;
                yield return LeftFoot;
                yield return LeftToeBase;
                yield return RightUpLeg;
                yield return RightLeg;
                yield return RightFoot;
                yield return RightToeBase;
            }
        }

        /// <summary>
        /// Defines if the device originates from a left- or righthanded coordinatesystem.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public CoordinatesystemOrientation Orientation
        {
            get
            {
                if (_coordinateSystemCompensation == 1)
                    return CoordinatesystemOrientation.LeftHanded;
                return CoordinatesystemOrientation.RightHanded;
            }
            set
            {
                if (value == CoordinatesystemOrientation.LeftHanded)
                    _coordinateSystemCompensation = 1;
                else
                    _coordinateSystemCompensation = -1;
            }
        }

        private int _coordinateSystemCompensation;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatNetSkeleton"/> class.
        /// </summary>
        /// <param name="inpDeviceImp">The platform dependent connector to the underlying device.</param>
        public NatNetSkeleton(IInputDeviceImp inpDeviceImp) : base(inpDeviceImp)
        {
            Orientation = CoordinatesystemOrientation.RightHanded;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct SkeletonRigidBodyData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonRigidBodyData"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="quaternionRotation">The quaternion rotation.</param>
        public SkeletonRigidBodyData(float3 position, Quaternion quaternionRotation)
        {
            Position = position;
            RotationQuaternion = quaternionRotation;
        }
        /// <summary>
        /// Gets the position relative to the source's origin in millimeters.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public float3 Position { get; set; }
        /// <summary>
        /// Gets the current rotation as quaternion.
        /// </summary>
        /// <value>
        /// The current rotation quaternion.
        /// </value>
        public Quaternion RotationQuaternion { get; set; }
        /// <summary>
        /// Gets the rotation matrix.
        /// </summary>
        /// <value>
        /// The rotation matrix.
        /// </value>
        public float4x4 RotationMatrix => Quaternion.QuaternionToMatrix(RotationQuaternion);
        /// <summary>
        /// Gets the rotation in euler angles.
        /// </summary>
        /// <value>
        /// The rotation euler.
        /// </value>
        public float3 RotationEuler => Quaternion.QuaternionToEuler(RotationQuaternion);
    }
}
