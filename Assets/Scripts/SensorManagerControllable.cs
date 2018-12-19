using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorManagerControllable : Controllable
{

    [Header("Network settings")]
    [OSCProperty]
    public string TargetIp;
    [OSCProperty]
    public int TargetPort;

    [Header("Accelerometer")]
    [OSCProperty]
    public bool sendAccelerometer;
    [OSCProperty(isInteractible = false)]
    public string accelerometerValue;
    [OSCProperty]
    public string accelerometerOSCAddress;

    [Header("Gyroscope")]
    [OSCProperty]
    public bool sendGyroscope;
    [OSCProperty(isInteractible = false)]
    public string gyroscopeValue;
    [OSCProperty]
    public string gyroscopeOSCAddress;

    [Header("Touch")]
    [OSCProperty]
    public bool sendTouch;
    [OSCProperty(isInteractible = false)]
    public int touchCount;
    [OSCProperty]
    public string touchOSCAddress;
}
