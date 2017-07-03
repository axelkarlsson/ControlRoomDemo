using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Controller : MonoBehaviour {
    AspectMenuController AMC;
	// Use this for initialization
	void Start () {
        
        AMC = GameObject.Find("Aspect_Menu").GetComponent<AspectMenuController>();
        //gameObject.transform.GetComponentInChildren<Text>().font = AMC._font;
        //gameObject.transform.GetComponentInChildren<Text>() = AMC._AspectName;
    }
	
	// Update is called once per frame
	void Update () {
    }
    void UpdateSize(float[] xy)
    {
        Debug.Log("Size of Canvas has Changed");
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(xy[0], xy[1]);
        gameObject.transform.Find("Aspect_Menu_Item_Text").GetComponent<RectTransform>().sizeDelta = new Vector2(xy[0], xy[1]);
    }
}
