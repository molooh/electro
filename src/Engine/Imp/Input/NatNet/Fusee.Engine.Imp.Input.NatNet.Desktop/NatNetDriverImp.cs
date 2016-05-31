using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using NatNetML;
using RigidBody = NatNetML.RigidBody;

namespace Fusee.Engine.Imp.Input.NatNet.Desktop
{
    /// <summary>
    /// Input driver implementation for NatNet devices.
    /// </summary>
    /// <seealso cref="Fusee.Engine.Common.IInputDriverImp" />
    public class NatNetDriverImp : IInputDriverImp
    {
        private NatNetClientML _natNetClient;
        private List<NatNetRigidBodyDeviceImp> _natNetRigidBodyDevices;
        private List<NatNetSkeletonDeviceImp> _natNetSkeletonDevices;

        private List<RigidBody> _rigidbodieDescriptors;
        private List<Skeleton> _skeletonDescriptors;
        
        private FrameOfMocapData _frameOfMocapData = new FrameOfMocapData();
        private ServerDescription _serverDescription = new ServerDescription();

        private long _lastUpdateFrame;

        public bool Connected { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NatNetDriverImp"/> class.
        /// </summary>
        /// <param name="localAddress">The local ip address.</param>
        /// <param name="serverAddress">The servers ip address.</param>
        /// <param name="_connectionType">Type of the connection type. 0 = multicast, 1 = unicast.</param>
        public NatNetDriverImp(string localAddress, string serverAddress, int _connectionType = 0)
        {
            if (_natNetClient != null)
                _natNetClient.Uninitialize();

            _natNetClient = new NatNetClientML(_connectionType);
            _natNetClient.Initialize(localAddress, serverAddress);
            _natNetRigidBodyDevices = new List<NatNetRigidBodyDeviceImp>();
            _natNetSkeletonDevices = new List<NatNetSkeletonDeviceImp>();

            _rigidbodieDescriptors = new List<RigidBody>();
            _skeletonDescriptors = new List<Skeleton>();

            if (_natNetClient.GetServerDescription(_serverDescription) == 0)
            {
                Debug.WriteLine("NatNet server is present and running: {0} {1}.{2}.{3}.{4}, NatNet: {5}.{6}.{7}.{8}",
                    _serverDescription.HostApp, _serverDescription.HostAppVersion[0],
                    _serverDescription.HostAppVersion[1], _serverDescription.HostAppVersion[2],
                    _serverDescription.HostAppVersion[3], _serverDescription.NatNetVersion[0],
                    _serverDescription.NatNetVersion[1], _serverDescription.NatNetVersion[2],
                    _serverDescription.NatNetVersion[3]);

                UpdateDataDescriptors();
            }
            else
            {
                Debug.WriteLine("Error: couldn't contact NatNet Server");
            }

            Connected = _serverDescription.HostPresent;
        }

        public void Dispose()
        {
            _natNetClient.Uninitialize();
            _natNetClient.Dispose();
        }

        /// <summary>
        /// Devices supported by this driver: NatNetRigidBody and NatNetSkeleton.
        /// </summary>
        public IEnumerable<IInputDeviceImp> Devices
        {
            get
            {
                foreach (var device in _natNetRigidBodyDevices)
                    yield return device;
                foreach (var device in _natNetSkeletonDevices)
                    yield return device;
            }
        }

        /// <summary>
        /// Returns a (hopefully) unique ID for this driver. Uniqueness is granted by using the 
        /// full class name (including namespace).
        /// </summary>
        public string DriverId => GetType().FullName;

        /// <summary>
        /// Returns a human readable description of this driver.
        /// </summary>
        public string DriverDesc => "Driver providing access to NatNet devices.";

        /// <summary>
        /// Not supported on this driver. NatNet devices are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<NewDeviceImpConnectedArgs> NewDeviceConnected;
        /// <summary>
        /// Not supported on this driver. NatNet devices are considered to be connected all the time.
        /// You can register handlers but they will never get called.
        /// </summary>
        public event EventHandler<DeviceImpDisconnectedArgs> DeviceDisconnected;


        /// <summary>
        /// Adds a NatNetRigidBodyDevice device to be handled by this driver.
        /// </summary>
        /// <param name="natNetDevice">A NatNetRigidBodyDeviceImp device.</param>
        public void AddDevice(NatNetRigidBodyDeviceImp natNetDevice)
        {
            var natNetId = GetRigidbodyId(natNetDevice.Name);
            _natNetRigidBodyDevices.Add(natNetDevice);
            natNetDevice.SetDriverReference(this, natNetId);

            Core.Input.Instance.RegisterInputDeviceType(imp => imp.Category == DeviceCategory.NatNetTracker, imp => new NatNetRigidbody(imp));
        }


        /// <summary>
        /// Adds a NatNetSkeleton device to be handled by this driver.
        /// </summary>
        /// <param name="natNetDevice">A NatNetSkeletonDeviceImp device.</param>
        public void AddDevice(NatNetSkeletonDeviceImp natNetDevice)
        {
            var natNetId = GetSkeletonId(natNetDevice.Name);
            _natNetSkeletonDevices.Add(natNetDevice);
            natNetDevice.SetDriverReference(this, natNetId);

            Core.Input.Instance.RegisterInputDeviceType(imp => imp.Category == DeviceCategory.Skeleton, imp => new NatNetSkeleton(imp));
        }


        private int GetRigidbodyId(string name)
        {
            if (Connected)
            {
                foreach (var rigidbody in _rigidbodieDescriptors)
                {
                    if (rigidbody.Name.Equals(name))
                    {
                        return rigidbody.ID;
                    }
                }
            }

            return -1;
        }

        private int GetSkeletonId(string name)
        {
            if (Connected)
            {
                foreach (var skeleton in _skeletonDescriptors)
                {
                    if (skeleton.Name.Equals(name))
                    {
                        return skeleton.ID;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the RigidBodyData for s specific id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal RigidBodyData GetRigidbodyData(int id)
        {
            if (Connected && id >= 0)
            {
                UpdateData();
                if (_frameOfMocapData.nRigidBodies > 0)
                {
                    foreach (var rigidBodyData in _frameOfMocapData.RigidBodies)
                    {
                        if (rigidBodyData.ID == id)
                        {
                            return rigidBodyData;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the SkeletonData for s specific id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        internal SkeletonData GetSkeletonData(int id)
        {
            if (Connected)
            {
                UpdateData();

                foreach (var skeletonData in _frameOfMocapData.Skeletons)
                {
                    if (skeletonData.ID == id)
                    {
                        return skeletonData;
                    }
                }
            }
            return null;
        }

        private void UpdateData()
        {
            if (Time.Frames != _lastUpdateFrame)
            {
                _frameOfMocapData = _natNetClient.GetLastFrameOfData();
                _lastUpdateFrame = Time.Frames;
            }
        }

        private void UpdateDataDescriptors()
        {
            _rigidbodieDescriptors.Clear();
            _skeletonDescriptors.Clear();

            List<DataDescriptor> dataDescriptors = new List<DataDescriptor>();
            if (_natNetClient.GetDataDescriptions(out dataDescriptors))
            {
                foreach (DataDescriptor dataDescriptor in dataDescriptors)
                {
                    if (dataDescriptor.type == (int)DataDescriptorType.eRigidbodyData)
                    {
                        _rigidbodieDescriptors.Add((RigidBody) dataDescriptor);
                    }
                    else if (dataDescriptor.type == (int)DataDescriptorType.eSkeletonData)
                    {
                        var skeleton = (Skeleton) dataDescriptor;
                        _skeletonDescriptors.Add(skeleton);
                    }
                }
            }
        }
    }
}
