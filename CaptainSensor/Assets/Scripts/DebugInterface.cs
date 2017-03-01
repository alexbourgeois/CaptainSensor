using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugInterface : MonoBehaviour
{

    private Canvas _debugCanvas;

	// Use this for initialization
	void Start ()
	{
	    _debugCanvas = GetComponent < Canvas>();
    }

    void CreateText(string title, string value)
    {


    }

	// Update is called once per frame
	void Update () {
		
	}
}
