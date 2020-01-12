namespace ByteScript.XR
{
    /*
     * Head Representation of the HMD.
     */
    public class XrHead : XrDevice
    {
        protected override void Awake()
        {
            type = DeviceType.Head;
            base.Awake();
        }
    }
}
