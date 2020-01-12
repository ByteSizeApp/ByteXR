using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace ByteScript.XR
{
    [System.Serializable]
    public class OnTriggerChanged : UnityEvent<float> { }
    [System.Serializable]
    public class OnTouchpadClick : UnityEvent<bool> { }
    [System.Serializable]
    public class OnTouchpadTouch : UnityEvent<bool> { }
    [System.Serializable]
    public class OnTouchpadValueChanged : UnityEvent<Vector2> { }
    [System.Serializable]
    public class OnMenuClick : UnityEvent<bool> { }
    [System.Serializable]
    public class OnGripClick : UnityEvent<bool> { }

    /*
     * Listens to button interactions and broadcasts to UnityEvents.
     *
     * This scripts focuses on HTC Vive but the script can be extended especially in XrContoller.cs.
     * More mappings can be found at: https://docs.unity3d.com/Manual/xr_input.html
     */
    public class XrController : XrDevice
    {
        [SerializeField]
        private DeviceType deviceType = DeviceType.NotSet;

        [Header("Events")]
        [SerializeField]
        private OnTriggerChanged onTriggerChanged = null;
        [SerializeField]
        private OnTouchpadTouch onTouchpadTouch = null;
        [SerializeField]
        private OnTouchpadClick onTouchpadClick = null;
        [SerializeField]
        private OnTouchpadValueChanged onTouchpadValueChanged = null;
        [SerializeField]
        private OnMenuClick onMenuClick = null;
        [SerializeField]
        private OnGripClick onGripClick = null;

        private float triggerValue;
        private bool touchpadTouch;
        private bool touchpadClick;
        private Vector2 touchpadValue;
        private bool menuButtonClick;
        private bool gripClick;

        protected override void Awake()
        {
            type = deviceType;
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (!device.HasValue)
            {
                return;
            }

            processTrigger();
            processThumbpadState();
            processMenuButton();
            processGripButton();
        }

        private void processTrigger()
        {
            float triggerValue;
            if (device.Value.TryGetFeatureValue(CommonUsages.trigger, out triggerValue)
                && this.triggerValue != triggerValue)
            {
                this.triggerValue = triggerValue;
                if (onTriggerChanged != null)
                {
                    onTriggerChanged.Invoke(triggerValue);
                }
            }
        }

        private void processThumbpadState()
        {
            bool touchpadTouch;
            if (device.Value.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out touchpadTouch)
                && this.touchpadTouch != touchpadTouch)
            {
                this.touchpadTouch = touchpadTouch;

                if (onTouchpadTouch != null)
                {
                    onTouchpadTouch.Invoke(touchpadTouch);
                }
            }

            bool touchpadClick;
            if (device.Value.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out touchpadClick)
                && this.touchpadClick != touchpadClick)
            {
                this.touchpadClick = touchpadClick;

                if (onTouchpadClick != null)
                {
                    onTouchpadClick.Invoke(touchpadClick);
                }
            }

            Vector2 touchpadValue;
            if (touchpadTouch
                && device.Value.TryGetFeatureValue(CommonUsages.primary2DAxis, out touchpadValue)
                && this.touchpadValue != touchpadValue)
            {
                this.touchpadValue = touchpadValue;
                if (onTouchpadValueChanged != null)
                {
                    onTouchpadValueChanged.Invoke(touchpadValue);
                }
            }
        }

        private void processMenuButton()
        {
            bool menuButtonClick;
            if (device.Value.TryGetFeatureValue(CommonUsages.primaryButton, out menuButtonClick)
                && this.menuButtonClick != menuButtonClick)
            {
                this.menuButtonClick = menuButtonClick;
                if (onMenuClick != null)
                {
                    onMenuClick.Invoke(menuButtonClick);
                }
            }
        }

        private void processGripButton()
        {
            bool gripClick;
            if (device.Value.TryGetFeatureValue(CommonUsages.gripButton, out gripClick)
                && this.gripClick != gripClick)
            {
                this.gripClick = gripClick;
                if (onGripClick != null)
                {
                    onGripClick.Invoke(gripClick);
                }
            }
        }
    }
}