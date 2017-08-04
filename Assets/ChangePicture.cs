using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ChangePicture : MonoBehaviour,IInputClickHandler {
    public bool isChanging;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (!isChanging)
        {
            GameObject g = new GameObject("PictureSelector");
            g.transform.position = transform.parent.position + new Vector3(0, -0.4f, 0);
            g.AddComponent<Canvas>();
            g.AddComponent<ImageSelector>();
            g.SendMessage("getAvailableImages", transform.parent.gameObject);
            isChanging = false;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void ChangeComplete()
    {
        isChanging = false;
    }
}
