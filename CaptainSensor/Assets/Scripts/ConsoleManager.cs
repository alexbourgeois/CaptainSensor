using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    private InputField _console;

	// Use this for initialization
	void Start ()
	{
	    _console = GetComponent<InputField>();
	    _console.text = "[Log]";

	}

    public void Log(string str)
    {
        _console.text = _console.text +" \n" + str;
    }
}
