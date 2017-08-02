using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.UI;

public class ImageSelectorImage : MonoBehaviour, IInputClickHandler {
    GameObject BigPapaInTheSky;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        transform.parent.SendMessage("SelectionComplete",gameObject);
        transform.parent = BigPapaInTheSky.transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas");
        transform.parent.Find("ChangePictureButton").SendMessage("ChangeComplete");
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<ImageSelectorImage>());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void SetTarget(GameObject g)
    {
        BigPapaInTheSky = g;
    }
}
