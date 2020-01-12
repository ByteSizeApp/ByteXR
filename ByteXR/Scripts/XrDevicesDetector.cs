using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace ByteScript.XR
{
    public delegate void OnXRDeviceConnected(DeviceType deviceType, InputDevice inputDevice);
    public delegate void OnXRDeviceDisconnected(DeviceType deviceType);

    public enum DeviceType
    {
        // For unsupported device detected.
        Unknown,
        // For XrController that has not chosen Left or Right.
        NotSet,
        Head,
        LeftHand,
        RightHand
    }

    /*
     * Manage connection between all xr devices via UnityEngine.XR.InputDevice.
     * Actively look for head/leftHand/rightHand to broadcast to ByteScript.XR.XrDevice
     * for further work.
     * 
     * Usage:
     * 1.) Attach this script to any object in the scene.
     * 2.) Attach XrHead.cs and XrController.cs to GameObject for head and controllers as needed.
     *     Any object with XrHead/XrController will move and rotate according to the real device.
     * 3.) Assign Controller's left and right in UnityEditor.
     * 4.) Add Unity Events in the Editor in the XrController component as needed.
     * 5.) Profit!
     */
    public class XrDevicesDetector : MonoBehaviour
    {
        public event OnXRDeviceConnected onXRDeviceConnected;
        public event OnXRDeviceDisconnected onXRDeviceDisconnected;

        void Start()
        {
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(inputDevices);

            InputDevices.deviceConnected += OnDeviceConnected;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;

            foreach (InputDevice device in inputDevices)
            {
                AssignDeviceAsNeeded(device);
            }
        }

        private void OnDeviceConnected(InputDevice obj)
        {
            AssignDeviceAsNeeded(obj);
        }

        private void OnDeviceDisconnected(InputDevice obj)
        {
            UnassignDeviceAsNeeded(obj);
        }

        private bool ShouldIgnoreInput(InputDevice inputDevice)
        {
            if (inputDevice.role == InputDeviceRole.TrackingReference)
            {
                // This is light house or tracker. We dont care about them.
                return true;
            }

            if (inputDevice.role == InputDeviceRole.HardwareTracker)
            {
                // Not using any generic tracking references for this code.
                // These are controllers that has yet to be assigned. Or the custom trackers.
                return true;
            }

            return false;
        }

        private void AssignDeviceAsNeeded(InputDevice inputDevice)
        {
            if (ShouldIgnoreInput(inputDevice))
            {
                return;
            }

            DeviceType type = GetTypeFromRole(inputDevice.role);
            if (type != DeviceType.Unknown && onXRDeviceConnected != null)
            {
                onXRDeviceConnected(type, inputDevice);
            }
        }

        private void UnassignDeviceAsNeeded(InputDevice inputDevice)
        {
            if (ShouldIgnoreInput(inputDevice))
            {
                return;
            }

            DeviceType type = GetTypeFromRole(inputDevice.role);
            if (type != DeviceType.Unknown && onXRDeviceDisconnected != null)
            {
                onXRDeviceDisconnected(type);
            }
        }

        private DeviceType GetTypeFromRole(InputDeviceRole role)
        {
            switch (role)
            {
                case InputDeviceRole.Generic:
                    // This is the camera.
                    return DeviceType.Head;
                case InputDeviceRole.LeftHanded:
                    return DeviceType.LeftHand;
                case InputDeviceRole.RightHanded:
                    return DeviceType.RightHand;
                default:
                    Debug.LogError("Ignoring unhandled input device: " + role);
                    return DeviceType.Unknown;
            }
        }
    }
}