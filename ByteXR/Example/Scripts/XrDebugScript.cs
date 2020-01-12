using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ByteScript.XR
{
    /*
     * Simple class to test unity events.
     */
    public class XrDebugScript : MonoBehaviour
    {
        public void DebugLogFloat(float value)
        {
            Debug.Log(value);
        }
        public void DebugLogBool(bool value)
        {
            Debug.Log(value);
        }
        public void DebugLogVector2(Vector2 value)
        {
            Debug.Log(value);
        }
    }
}