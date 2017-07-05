using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;

public class Aspect_Menu_Item_Text_Controller : MonoBehaviour, IInputHandler
{
    bool open = false;
    GameObject _localroot;
	// Use this for initialization
	void Start () {
        _localroot = GetComponentInParent<Object_Controller>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTap()
    {
        if (open)
        {
            OnClose();
        }
        else
        {
            _localroot.SendMessage("GetAspectWindow", gameObject.GetComponent<Text>().text);
        }
        open = !open;
    }
    void OnOpen()
    {
        OnTap();
    }
    void OnClose()
    {
        _localroot.SendMessage("CloseAspectWindow", gameObject.GetComponent<Text>().text);
    }
    void OnCloseAll()
    {
        OnClose();
    }

    public void OnInputUp(InputEventData eventData)
    {

    }

    public void OnInputDown(InputEventData eventData)
    {
        OnTap();
    }
}
