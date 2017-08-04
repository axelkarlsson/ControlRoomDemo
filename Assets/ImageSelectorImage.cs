using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.UI;

public class ImageSelectorImage : MonoBehaviour, IInputClickHandler {
    public GameObject BigPapaInTheSky;
    GameObject Selector;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Selector = transform.parent.gameObject;
        transform.parent = BigPapaInTheSky.transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas");
        transform.localPosition = new Vector3(0, 0, 0.05f);
        transform.localRotation = Quaternion.identity;
        transform.localScale =    new Vector3(1, 1, 1);
        GetComponent<RectTransform>().sizeDelta = new Vector2(1.8f, 1.8f);
        GetComponent<RawImage>().raycastTarget = false;
        BigPapaInTheSky.transform.Find("ChangePictureButton").SendMessage("ChangeComplete");
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<ImageSelectorImage>());
        Selector.SendMessage("SelectionComplete", gameObject);
        transform.SetAsFirstSibling();
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
