using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.UI;
using System;

public class Aspect_Menu_Item_Text_Controller : MonoBehaviour, IInputHandler
{
    bool open = false;
	// Use this for initialization
	void Start () {
		
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
            transform.root.gameObject.SendMessage("GetAspectWindow", gameObject.GetComponent<Text>().text);
        }
        open = !open;
    }
    void OnOpen()
    {
        OnTap();
    }
    void OnClose()
    {
        transform.root.gameObject.SendMessage("CloseAspectWindow", gameObject.GetComponent<Text>().text);
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
