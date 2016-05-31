using System;
using System.Collections.Generic;
using Fusee.Engine.Common;
using NatNetML;

namespace Fusee.Engine.Imp.Input.NatNet.Desktop
{
    /// <summary>
    /// NatNet rigidbody device implementation.
    /// </summary>
    public class NatNetRigidBodyDeviceImp : IInputDeviceImp
    {
        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name { get; }
        private int _natNetId;

        private NatNetDriverImp _natNetDriver;

        private RigidBodyData _rigidBodyData;
        private float[] _lastValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatNetRigidBodyDeviceImp"/> class.
        /// </summary>
        /// <param name="name">The NatNet name.</param>
        public NatNetRigidBodyDeviceImp(string name)
        {
            Name = name;
            _lastValues = new float[AxesCount];
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string Id => GetType().FullName + "_" + Name + "_" + _natNetId;

        /// <summary>
        /// Returns a human readable description of this device.
        /// </summary>
        public string Desc => "NatNet RigidBody device.";

        /// <summary>
        /// Gets the category of this device. Device categories define a minimal common
        /// set of buttons and axes which are identical across all devices sharing the same
        /// category.
        /// </summary>
        /// <value>
        /// The device category.
        /// </value>
        public DeviceCategory Category => DeviceCategory.NatNetTracker;

        /// <summary>
        /// Gets number of axes supported by this device.
        /// </summary>
        /// <value>
        /// The axes count.
        /// </value>
        public int AxesCount => 7;

        /// <summary>
        /// Gets a description of the axis. This value can be used in user setup-dialogs or
        /// to match axes of devices of different categories.
        /// </summary>
        /// <value>
        /// The description of the axis.
        /// </value>
        public IEnumerable<AxisImpDescription> AxisImpDesc
        {
            get
            {
                yield return new AxisImpDescription
                {
                    AxisDesc = new AxisDescription
                    {
                        Name = "X",
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
                        Name = "Y",
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
                        Name = "Z",
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
                        Name = "QX",
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
                        Name = "QY",
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
                        Name = "QZ",
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
                        Name = "QW",
                        Id = 6,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Unbound,
                    },
                    PollAxis = true
                };
            }
        }

        /// <summary>
        /// Gets the value currently present at the given axis if its <see cref="P:Fusee.Engine.Common.IInputDeviceImp.AxisImpDesc" /> identifies it as a to-be-polled axis.
        /// </summary>
        /// <param name="iAxisId">The axis' Id.</param>
        /// <returns>
        /// The value currently set on the axis.
        /// </returns>
        /// <remarks>
        /// See <see cref="T:Fusee.Engine.Common.AxisDescription" /> to get information about how to interpret the
        /// values returned by a given axis.
        /// </remarks>
        public float GetAxis(int iAxisId)
        {
            _rigidBodyData = _natNetDriver.GetRigidbodyData(_natNetId);

            float value = 0;
            if (_rigidBodyData != null)
            {
                switch (iAxisId)
                {
                    case 0:
                        value = _rigidBodyData.x;
                        break;
                    case 1:
                        value = _rigidBodyData.y;
                        break;
                    case 2:
                        value = _rigidBodyData.z;
                        break;
                    case 3:
                        value = _rigidBodyData.qx;
                        break;
                    case 4:
                        value = _rigidBodyData.qy;
                        break;
                    case 5:
                        value = _rigidBodyData.qz;
                        break;
                    case 6:
                        value = _rigidBodyData.qw;
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

                AxisValueChanged?.Invoke(this, new AxisValueChangedArgs() { Axis = axisdesc, Value = value });
            }

            return value;
        }

        /// <summary>
        /// Occurs on value changes of axes exhibited by this device.
        /// Only applies for axes where the <see cref="F:Fusee.Engine.Common.AxisImpDescription.PollAxis" /> is set to false.
        /// </summary>
        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;

        /// <summary>
        /// Gets the number of buttons supported by this device.
        /// </summary>
        /// <value>
        /// The button count.
        /// </value>
        public int ButtonCount => 0;

        /// <summary>
        /// Gets information about of the specified button. This value can be used in user setup-dialogs or
        /// to match buttons of devices of different categories.
        /// </summary>
        /// <value>
        /// Information about the button.
        /// </value>
        public IEnumerable<ButtonImpDescription> ButtonImpDesc
        {
            get
            {
                yield break;
            }
        }

        /// <summary>
        /// Gets the state of the given button if its <see cref="P:Fusee.Engine.Common.IInputDeviceImp.ButtonImpDesc" /> identifies it as a to-be-polled button
        /// </summary>
        /// <param name="iButtonId">The Id of the button.</param>
        /// <returns>
        /// true if the button is currently pressed. false, if the button is currently released.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException($"This device supports no pollable buttons.");
        }

        /// <summary>
        /// Occurs on state changes of buttons exhibited by this device.
        /// Only applies for buttons where the <see cref="F:Fusee.Engine.Common.ButtonImpDescription.PollButton" /> is set to false.
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;

        /// <summary>
        /// Sets the driver reference.
        /// </summary>
        /// <param name="natNetTrackerDriverImp">The NatNet tracker driver imp.</param>
        /// <param name="natNetId">The NatNet identifier.</param>
        public void SetDriverReference(NatNetDriverImp natNetTrackerDriverImp, int natNetId)
        {
            _natNetDriver = natNetTrackerDriverImp;
            _natNetId = natNetId;
        }
    }
}
