﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusee.Engine;
using SlimDX.DirectInput;

namespace Fusee.Engine
{
    class InputDeviceImp : IInputDeviceImp
    {
        public List<IInputDeviceImp> _devices;

         // Das GamePad
        private Joystick joystick;
        private JoystickState state;
        private bool[] buttonsPressed;
        private float deadZone;
        
        

        // Status des GamePads
        
        

        public InputDeviceImp(DeviceInstance device)
        {
            DirectInput directInput = new DirectInput();
             state = new JoystickState();
            deadZone = 0.1f;
            // Geräte suchen

            
            buttonsPressed = new bool[100];
            

            // Gamepad erstellen
            joystick = new Joystick(directInput, device.InstanceGuid);
            

            
            // Den Zahlenbereich der Achsen auf -1000 bis 1000 setzen
            foreach (DeviceObjectInstance deviceObject in joystick.GetObjects())
            {
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    joystick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-1000, 1000);

            }

            joystick.Acquire();
        }

        public JoystickState GetState()
        {
            if (joystick.Acquire().IsFailure || joystick.Poll().IsFailure)
            {
                // Wenn das GamePad nicht erreichbar ist, leeren Status zurückgeben.
                state = new JoystickState();
                return state;
            }

            state = joystick.GetCurrentState();

            return state;
        }

        public bool IsButtonDown(int buttonIndex)
        {
            state = GetState();

           
                if (state.IsPressed(buttonIndex))
                {

                    return true;
                }
            return false;
        }

        public bool IsButtonPressed(int buttonIndex)
        {
            state = GetState();


            if (state.IsPressed(buttonIndex) && buttonsPressed[buttonIndex] == false)
                {
                    buttonsPressed[buttonIndex] = true;
                    return true;
                }

            if (state.IsReleased(buttonIndex) && buttonsPressed[buttonIndex])
                {
                    buttonsPressed[buttonIndex] = false;
                }
            return false;
        }

        public float GetZAxis()
        {
            float _tmp = GetState().Z / 1000f;
            if (_tmp > deadZone)
                return _tmp;
            if (_tmp < -deadZone)
                return _tmp;
            return 0;
        }

        public float GetYAxis()
        {
            float _tmp = -GetState().Y / 1000f;
            if (_tmp > deadZone)
                return _tmp;
            if (_tmp < -deadZone)
                return _tmp;
            return 0;
        }

        public float GetXAxis()
        {
          float  _tmp = GetState().X / 1000f;
          if (_tmp > deadZone)
              return _tmp;
          if (_tmp < -deadZone)
              return _tmp;
            return 0;
        }

        public void SetDeadZone (float zone)
        {
            deadZone = zone;
        }

        public void Release()
        {
            if (joystick != null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }
            joystick = null;
        }
    



        public void InitializeDevices()
        {
            DirectInput directInput = new DirectInput();
            var devices = directInput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly);


            foreach (DeviceInstance deviceInstance in devices)
            {
                System.Diagnostics.Debug.WriteLine(deviceInstance.ProductName);
                _devices.Add(new InputDeviceImp(deviceInstance));

            }
        }
    }
}

