using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;

public class Object_Text_Controler : MonoBehaviour, IInputHandler {
    public void OnInputDown(InputEventData eventData)
    {
    }

    public void OnInputUp(InputEventData eventData)
    {
        _localroot.SendMessage("OnTap");
    }
    GameObject _localroot;
    // Use this for initialization
    void Start () {
        _localroot = GetComponentInParent<Object_Controller>().gameObject;
        gameObject.GetComponent<Text>().text = _localroot.name;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
