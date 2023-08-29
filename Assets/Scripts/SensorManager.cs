using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class SensorManager : MonoBehaviour
{

    private string _targetIp = "127.0.0.1";
    public string TargetIp
    {
        get
        {
            return _targetIp;
        }
        set
        {
            _targetIp = value;
            Connect();
        }
    }

    private int _targetPort = 8000;
    public int TargetPort
    {
        get
        {
            return _targetPort;
        }
        set
        {
            _targetPort = value;
            Connect();
        }
    }

    public bool sendAccelerometer;
    public bool sendGyroscope;
    public bool sendCompass;
    public bool sendTouch;

    public string accelerometerValue;
    public string gyroscopeValue;
    public string compassValue;
    public int touchCount;

    public string accelerometerOSCAddress = "/device/accelerometer";
    public string gyroscopeOSCAddress = "/device/gyroscope";
    public string compassOSCAddress = "/device/compass";
    public string touchOSCAddress = "/device/touch/";

    private string OSCClientName = "CaptainSensor";

    void Start ()
    {
        Input.compass.enabled = true;
        Input.gyro.enabled = true;

        Connect();
    }

    public void Connect()
    {
        if(OSCMaster.Clients.ContainsKey(OSCClientName))
        {
            OSCMaster.RemoveClient(OSCClientName);
        }
        OSCMaster.CreateClient(OSCClientName, IPAddress.Parse(TargetIp), TargetPort);
        Debug.Log("Connected on " + TargetIp + ":" + TargetPort);

        try
        {
            var message = new OSCMessage("/device/screen");
            message.Append(Screen.width);
            message.Append(Screen.height);

            OSCMaster.SendMessageUsingClient(OSCClientName, message);

            Debug.Log("Screen : " + Screen.width + "*" + Screen.height);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void Update ()
    {
        if (sendAccelerometer)
        {
            var message = new OSCMessage(accelerometerOSCAddress);
            message.Append(Input.acceleration.x);
            message.Append(Input.acceleration.y);
            message.Append(Input.acceleration.z);
            OSCMaster.SendMessageUsingClient(OSCClientName, message);

            accelerometerValue = Input.acceleration.ToString();
        }

        if (sendGyroscope)
        {
            var message = new OSCMessage(gyroscopeOSCAddress);
            message.Append(Input.gyro.attitude.x);
            message.Append(Input.gyro.attitude.y);
            message.Append(Input.gyro.attitude.z);
            message.Append(Input.gyro.attitude.w);
            OSCMaster.SendMessageUsingClient(OSCClientName, message);

            gyroscopeValue = Input.gyro.attitude.ToString();
        }

        if (sendCompass)
        {
            var message = new OSCMessage(compassOSCAddress);
            message.Append(Input.compass.trueHeading);
            message.Append(Input.compass.magneticHeading);
            OSCMaster.SendMessageUsingClient(OSCClientName, message);

            compassValue = Input.compass.trueHeading + " " + Input.compass.magneticHeading;
        }

        if (sendTouch)
        {
            if (!touchOSCAddress.EndsWith("/"))
                touchOSCAddress = touchOSCAddress + "/";

            for (var i = 0; i < Input.touchCount; i++)
            {
                var message = new OSCMessage(touchOSCAddress + i.ToString());
                var touch = Input.GetTouch(i);
                message.Append(touch.position.x);
                message.Append(touch.position.y);
                OSCMaster.SendMessageUsingClient(OSCClientName, message);

                touchCount = Input.touchCount;
            }
        }
    }
}
