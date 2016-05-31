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
    public class NatNetRigidbody : InputDevice
    {

        private readonly int _xVelId;
        private readonly int _yVelId;
        private readonly int _zVelId;

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
        /// Initializes a new instance of the <see cref="NatNetRigidbody"/> class.
        /// </summary>
        /// <param name="inpDeviceImp">The platform dependent connector to the underlying device.</param>
        public NatNetRigidbody(IInputDeviceImp inpDeviceImp) : base(inpDeviceImp)
        {
            _xVelId = RegisterVelocityAxis(0).Id;
            _yVelId = RegisterVelocityAxis(1).Id;
            _zVelId = RegisterVelocityAxis(2).Id;
            
            // NatNet usualy comes from a right handed origin
            Orientation = CoordinatesystemOrientation.RightHanded;
        }

        /// <summary>
        /// Gets the position relative to the source's origin in millimeters.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public float3 Position => new float3(X, Y, Z);

        /// <summary>
        /// The device's position on the x axis
        /// </summary>
        /// <value>
        /// The position relative to the source's origin in millimeters.
        /// </value>
        public float X => GetAxis(0);
        /// <summary>
        /// The device's position on the y axis
        /// </summary>
        /// <value>
        /// The position relative to the source's origin in millimeters.
        /// </value>
        public float Y => GetAxis(1);
        /// <summary>
        /// The device's position on the z axis
        /// </summary>
        /// <value>
        /// The position relative to the source's origin in millimeters.
        /// </value>
        public float Z => _coordinateSystemCompensation * GetAxis(2);

        /// <summary>
        /// Gets the velocity in millimeters per second
        /// </summary>
        /// <value>
        /// The velocity.
        /// </value>
        public float3 Velocity => new float3(XVel, YVel, ZVel);

        /// <summary>
        /// Gets the x velocity
        /// </summary>
        /// <value>
        /// The x velocity
        /// </value>
        public float XVel => GetAxis(_xVelId);
        /// <summary>
        /// Gets the y velocity
        /// </summary>
        /// <value>
        /// The y velocity
        /// </value>
        public float YVel => GetAxis(_yVelId);
        /// <summary>
        /// Gets the z velocity
        /// </summary>
        /// <value>
        /// The z velocity
        /// </value>
        public float ZVel => GetAxis(_zVelId);

        /// <summary>
        /// Gets the current rotation as quaternion.
        /// </summary>
        /// <value>
        /// The current rotation quaternion.
        /// </value>
        public Quaternion RotationQuaternion => new Quaternion(GetAxis(3), GetAxis(4), _coordinateSystemCompensation * GetAxis(5), GetAxis(6));
        /// <summary>
        /// Gets the rotation in euler angles.
        /// </summary>
        /// <value>
        /// The rotation euler.
        /// </value>
        public float3 RotationEuler => Quaternion.QuaternionToEuler(RotationQuaternion);
        /// <summary>
        /// Gets the rotation matrix.
        /// </summary>
        /// <value>
        /// The rotation matrix.
        /// </value>
        public float4x4 RotationMatrix => Quaternion.QuaternionToMatrix(RotationQuaternion);
    }
}
