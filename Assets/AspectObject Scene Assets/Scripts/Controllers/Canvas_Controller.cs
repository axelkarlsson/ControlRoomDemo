using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Controller : MonoBehaviour {
	// Use this for initialization
	void Start () {
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
