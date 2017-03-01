using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using UnityOSC;

public class SensorManager : MonoBehaviour
{
    //External source
    public GameObject IPAddressSource;
    public GameObject PortSource;

    public GameObject AccelerometerCheckGameObject;
    public GameObject GyroscopeCheckGameObject;
    public GameObject TouchCheckGameObject;

    //OSC 
    private OSCClient _oscSender;
    public string TargetIp = "127.0.0.1";
    public int TargetPort = 8000;

    //Debug
    public GameObject ConsoleHandler;
    public bool IsDebug;
    private ConsoleManager Console;

    //Sensor
    private bool _isTouchSet, _isAccelerometerSet, _isGyroscopeSet;
    
    
    // Use this for initialization
    void Start ()
    {
        _oscSender = new OSCClient(IPAddress.Parse(TargetIp), TargetPort);
        Console = ConsoleHandler.GetComponent<ConsoleManager>();
        IsDebug = true;
    }

    public void Connect()
    {
        _oscSender = new OSCClient(IPAddress.Parse(TargetIp), TargetPort);
        try
        {
            _oscSender.Connect();
            Console.Log("Connected");

            var message = new OSCMessage("/device/screen");
            message.Append(Screen.width);
            message.Append(Screen.height);
            _oscSender.Send(message);

            if (IsDebug)
                Console.Log("Screen : " + Screen.width + "*" + Screen.height);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void Reconnect()
    {
        if (_oscSender != null)
            _oscSender.Close();

        Connect();
    }

    #region **event handler**
    public void OnIpAddressChanged()
    {
        TargetIp = IPAddressSource.GetComponent<Text>().text;
    }
    public void OnAccelerometerChanged()
    {
        _isAccelerometerSet = AccelerometerCheckGameObject.GetComponent<Toggle>().isOn;
    }
    public void OnGyroscopeChanged()
    {
        _isGyroscopeSet = GyroscopeCheckGameObject.GetComponent<Toggle>().isOn;
    }
    public void OnTouchChanged()
    {
        _isTouchSet = TouchCheckGameObject.GetComponent<Toggle>().isOn;
    }
    public void OnPortChanged()
    {
        TargetPort = int.Parse(PortSource.GetComponent<Text>().text);
    }
    public void OnDebugChanged()
    {
        IsDebug = !IsDebug;
    }
    #endregion

    // Update is called once per frame
    void Update ()
    {
        if (_isAccelerometerSet)
        {
            var message = new OSCMessage("/device/accelerometer");
            message.Append(Input.acceleration.x);
            message.Append(Input.acceleration.y);
            message.Append(Input.acceleration.z);
            _oscSender.Send(message);

            if (IsDebug)
                Console.Log("Accelerometer : " + Input.acceleration);
        }

        if (_isGyroscopeSet)
        {
            Input.gyro.enabled = true;
            var message = new OSCMessage("/device/gyroscope");
            message.Append(Input.gyro.rotationRate.x);
            message.Append(Input.gyro.rotationRate.y);
            message.Append(Input.gyro.rotationRate.z);
            _oscSender.Send(message);

            if (IsDebug)
                Console.Log("Gyroscope : " + Input.gyro.rotationRate);
        }

        if (_isTouchSet)
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                var message = new OSCMessage("/device/touch" + i);
                var touch = Input.GetTouch(i);
                message.Append(touch.position.x);
                message.Append(touch.position.y);
                _oscSender.Send(message);

                if(IsDebug)
                    Console.Log("Touch id : " + i + " " + touch.position);
            }
        }
    }
}
