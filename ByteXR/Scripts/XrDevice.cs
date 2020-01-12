using UnityEngine;
using UnityEngine.XR;

namespace ByteScript.XR
{
    /*
     * Base class for any input device. Passthrough the Position and Rotation of the device to the gameobject it attaches to.
     */
    public class XrDevice : MonoBehaviour
    {
        protected DeviceType type;
        protected XrDevicesDetector deviceDetector;
        protected InputDevice? device;

        protected virtual void Awake()
        {
            deviceDetector = FindObjectOfType<XrDevicesDetector>();

            if (deviceDetector == null)
            {
                Debug.LogError("Missing XrDeviceDectctor in the scene");
            }

            deviceDetector.onXRDeviceConnected += OnXRDeviceConnected;
            deviceDetector.onXRDeviceDisconnected += OnXRDeviceDisconnected; ;
        }

        protected virtual void Update()
        {
            if (!device.HasValue)
            {
                return;
            }

            Vector3 position;
            device.Value.TryGetFeatureValue(CommonUsages.devicePosition, out position);
            transform.localPosition = position;

            Quaternion rotation;
            device.Value.TryGetFeatureValue(CommonUsages.deviceRotation, out rotation);
            transform.localRotation = rotation;
        }

        private void OnXRDeviceConnected(DeviceType deviceType, InputDevice inputDevice)
        {
            if (deviceType != type)
            {
                return;
            }

            device = inputDevice;
        }

        private void OnXRDeviceDisconnected(DeviceType deviceType)
        {
            if (deviceType != type)
            {
                return;
            }

            device = null;
        }
    }
}