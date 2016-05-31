using System;
using System.Collections.Generic;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using VrpnClientLib;

namespace Fusee.Engine.Imp.Input.Vrpn.Desktop
{
    /// <summary>
    /// Input driver implementation for tracker Vrpn devices.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Common.IInputDriverImp" />
    public class VrpnTrackerDriverImp : IInputDriverImp
    {
        /// <summary>
        /// Allows for access to the Vrpn client controller.
        /// </summary>
        /// <value>
        /// The client controller.
        /// </value>
        public VrpnClientController ClientController { get; private set; }
        private List<VrpnTrackerDeviceImp> _trackers;

        /// <summary>
        /// Initializes a new instance of the <see cref="VrpnTrackerDriverImp"/> class.
        /// </summary>
        /// <param name="vrpnDeviceNameList">An array of Vrpn device names.</param>
        public VrpnTrackerDriverImp(IEnumerable<string> vrpnDeviceNameList = null)
        {
            Initialize(vrpnDeviceNameList);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VrpnTrackerDriverImp"/> class.
        /// </summary>
        /// <param name="vrpnDeviceName">A Vrpn device name.</param>
        public VrpnTrackerDriverImp(string vrpnDeviceName)
        {
            Initialize(new[] {vrpnDeviceName});
        }

        private void Initialize(IEnumerable<string> vrpnDeviceNameList = null)
        {
            ClientController = new VrpnClientController();
            _trackers = new List<VrpnTrackerDeviceImp>();

            if (vrpnDeviceNameList != null)
            {
                foreach (var vrpnDeviceName in vrpnDeviceNameList)
                {
                    AddDevice(new VrpnTrackerDeviceImp(vrpnDeviceName));
                }
            }
        }

        /// <summary>
        /// Devices supported by this driver: tracker Vrpn devices.
        /// </summary>
        public IEnumerable<IInputDeviceImp> Devices
        {
            get
            {
                foreach (var device in _trackers)
                    yield return device;
            }
        }

        /// <summary>
        /// Adds a tracker Vrpn device to be handled by this driver.
        /// </summary>
        /// <param name="vrpnTrackerDeviceImp">A VrpnTrackerDeviceImp.</param>
        public void AddDevice(VrpnTrackerDeviceImp vrpnTrackerDeviceImp)
        {
            var vrpnId = ClientController.AddTracker(vrpnTrackerDeviceImp.VrpnName);
            _trackers.Add(vrpnTrackerDeviceImp);
            vrpnTrackerDeviceImp.SetDriverReference(this, vrpnId);

            Core.Input.Instance.RegisterInputDeviceType(imp => imp.Category == DeviceCategory.VrpnTracker, imp => new VrpnTrackerDevice(imp));
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string DriverId => GetType().FullName;

        /// <summary>
        /// Returns a human readable description of this driver.
        /// </summary>
        public string DriverDesc => "Driver providing access to VRPN tracker devices.";

        /// <summary>
        /// Not supported on this driver. Vrpn devices are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<NewDeviceImpConnectedArgs> NewDeviceConnected;
        /// <summary>
        /// Not supported on this driver. Vrpn devices are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<DeviceImpDisconnectedArgs> DeviceDisconnected;

        public void Dispose()
        {
        }
    }

    /// <summary>
    /// Vrpn analog device implementation.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Common.IInputDeviceImp" />
    public class VrpnTrackerDeviceImp : IInputDeviceImp
    {
        private VrpnTrackerDriverImp _vrpnTrackerDriver;

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string VrpnName { get; }
        private int _vrpnId;
        private double[] _lastValues;

        private long _lastUpdateFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="VrpnTrackerDeviceImp"/> class.
        /// </summary>
        /// <param name="vrpnDeviceName">Name of the Vrpn device.</param>
        public VrpnTrackerDeviceImp(string vrpnDeviceName)
        {
            VrpnName = vrpnDeviceName;
            _lastValues = new double[AxesCount];
        }

        /// <summary>
        /// Gets an identifier. Implementors take care that this
        /// id is unique across all devices managed by a driver.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => GetType().FullName + "_" + VrpnName;

        /// <summary>
        /// Gets the human readable name of this device. This
        /// parameter can be used in user dialogs to identify devices.
        /// </summary>
        /// <value>
        /// The deivce description.
        /// </value>
        public string Desc => "VRPN tracker device.";

        /// <summary>
        /// Gets the category of this device. Device categories define a minimal common
        /// set of buttons and axes which are identical across all devices sharing the same
        /// category.
        /// </summary>
        /// <value>
        /// The device category.
        /// </value>
        public DeviceCategory Category => DeviceCategory.VrpnTracker;

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
        /// Occurs on value changes of axes exhibited by this device.
        /// Only applies for axes where the <see cref="F:Fusee.Engine.Common.AxisImpDescription.PollAxis" /> is set to false.
        /// </summary>
        public event EventHandler<AxisValueChangedArgs> AxisValueChanged;

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
            float value = (float) _lastValues[iAxisId];

            if (Time.Frames != _lastUpdateFrame)
            {
                _lastUpdateFrame = Time.Frames;
                _lastValues = _vrpnTrackerDriver.ClientController.GetTrackerData(_vrpnId);

                value = (float) _lastValues[iAxisId];

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

            return value;
        }

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
        /// <exception cref="System.InvalidOperationException">This device supports no pollable buttons.</exception>
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException("This device supports no pollable buttons.");
        }

        /// <summary>
        /// Occurs on state changes of buttons exhibited by this device.
        /// Only applies for buttons where the <see cref="F:Fusee.Engine.Common.ButtonImpDescription.PollButton" /> is set to false.
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;

        /// <summary>
        /// Sets the driver reference.
        /// </summary>
        /// <param name="vrpnTrackerDriverImp">The Vrpn tracker driver imp.</param>
        /// <param name="vrpnId">The Vrpn identifier.</param>
        public void SetDriverReference(VrpnTrackerDriverImp vrpnTrackerDriverImp, int vrpnId)
        {
            _vrpnTrackerDriver = vrpnTrackerDriverImp;
            _vrpnId = vrpnId;
        }
    }

}