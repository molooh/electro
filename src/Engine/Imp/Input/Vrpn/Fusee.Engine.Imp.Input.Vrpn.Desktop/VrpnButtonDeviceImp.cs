using System;
using System.Collections.Generic;
using Fusee.Engine.Common;
using VrpnClientLib;

namespace Fusee.Engine.Imp.Input.Vrpn.Desktop
{
    /// <summary>
    /// Input driver implementation for button Vrpn devices.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Common.IInputDriverImp" />
    public class VrpnButtonDriverImp : IInputDriverImp
    {
        /// <summary>
        /// Allows for access to the Vrpn client controller.
        /// </summary>
        /// <value>
        /// The client controller.
        /// </value>
        public VrpnClientController ClientController { get; }
        private readonly List<VrpnButtonDeviceImp> _buttons;

        /// <summary>
        /// Initializes a new instance of the <see cref="VrpnButtonDriverImp"/> class.
        /// </summary>
        /// <param name="vrpnDeviceNameList">An array of Vrpn device names.</param>
        public VrpnButtonDriverImp(IEnumerable<string> vrpnDeviceNameList = null)
        {
            ClientController = new VrpnClientController();
            _buttons = new List<VrpnButtonDeviceImp>();

            if (vrpnDeviceNameList != null)
            {
                foreach (var vrpnDeviceName in vrpnDeviceNameList)
                {
                    AddDevice(new VrpnButtonDeviceImp(vrpnDeviceName));
                }
            }
        }

        /// <summary>
        /// Devices supported by this driver: button Vrpn devices.
        /// </summary>
        public IEnumerable<IInputDeviceImp> Devices
        {
            get
            {
                foreach (var device in _buttons)
                    yield return device;
            }
        }

        /// <summary>
        /// Adds a button Vrpn device to be handled by this driver.
        /// </summary>
        /// <param name="vrpnButtonDeviceImp">A VrpnButtonDeviceImp.</param>
        public void AddDevice(VrpnButtonDeviceImp vrpnButtonDeviceImp)
        {
            var vrpnId = ClientController.AddTracker(vrpnButtonDeviceImp.VrpnName);
            _buttons.Add(vrpnButtonDeviceImp);
            vrpnButtonDeviceImp.SetDriverReference(this, vrpnId);
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string DriverId => GetType().FullName;

        /// <summary>
        /// Returns a human readable description of this driver.
        /// </summary>
        public string DriverDesc => "Driver providing access to VRPN button devices.";

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
    /// Vrpn button device implementation.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Common.IInputDeviceImp" />
    public class VrpnButtonDeviceImp : IInputDeviceImp
    {
        private VrpnButtonDriverImp _vrpnButtonDriver;

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string VrpnName { get; }
        private int _vrpnId;
        private readonly int[] _lastValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="VrpnButtonDeviceImp"/> class.
        /// </summary>
        /// <param name="vrpnDeviceName">Name of the Vrpn device.</param>
        public VrpnButtonDeviceImp(string vrpnDeviceName)
        {
            VrpnName= vrpnDeviceName;
            _lastValues = new int[AxesCount];
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
        public string Desc => "VRPN button device.";

        /// <summary>
        /// Gets the category of this device. Device categories define a minimal common
        /// set of buttons and axes which are identical across all devices sharing the same
        /// category.
        /// </summary>
        /// <value>
        /// The device category.
        /// </value>
        public DeviceCategory Category => DeviceCategory.Other;

        /// <summary>
        /// Gets number of axes supported by this device.
        /// </summary>
        /// <value>
        /// The axes count.
        /// </value>
        public int AxesCount => 2;

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
                        Name = "ButtonId",
                        Id = 0,
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
                        Name = "ButtonValue",
                        Id = 1,
                        Direction = AxisDirection.Unknown,
                        Nature = AxisNature.Unknown,
                        Bounded = AxisBoundedType.Constant,
                        MinValueOrAxis = 0,
                        MaxValueOrAxis = 1
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
            int value = _vrpnButtonDriver.ClientController.GetButtonData(_vrpnId)[iAxisId];

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
        /// <exception cref="System.InvalidOperationException">This device supports no pollable buttons. Even tho this is a \'button\' device, button events are handled by the axis of this device.</exception>
        public bool GetButton(int iButtonId)
        {
            throw new InvalidOperationException("This device supports no pollable buttons. Even tho this is a \'button\' device, button events are handled by the axis of this device.");
        }

        /// <summary>
        /// Occurs on state changes of buttons exhibited by this device.
        /// Only applies for buttons where the <see cref="F:Fusee.Engine.Common.ButtonImpDescription.PollButton" /> is set to false.
        /// </summary>
        public event EventHandler<ButtonValueChangedArgs> ButtonValueChanged;

        /// <summary>
        /// Sets the driver reference.
        /// </summary>
        /// <param name="vrpnButtonDriverImp">The Vrpn button driver imp.</param>
        /// <param name="vrpnId">The Vrpn identifier.</param>
        public void SetDriverReference(VrpnButtonDriverImp vrpnButtonDriverImp, int vrpnId)
        {
            _vrpnButtonDriver = vrpnButtonDriverImp;
            _vrpnId = vrpnId;
        }
    }

}
