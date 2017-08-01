using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageSelector : MonoBehaviour {

    Image i;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    List<Image> getAvailableImages(GameObject Requester)
    {
        List<Image> ret = new List<Image>();
        UnityEngine.Object[] ob = Resources.LoadAll("Images/");
        foreach (UnityEngine.Object o in ob)
        {
            try
            {
                i.sprite = (Sprite)o;
            }
            catch (Exception)
            {

                throw new Exception("Could not convert");
            }
            i.gameObject.AddComponent<ImageSelectorImage>();
            Instantiate(i);
            ret.Add(i);
        }
        return ret;
    }
    void SelectionComplete(GameObject g)
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject != g) { Destroy(t.gameObject); }
        }
    }
}
