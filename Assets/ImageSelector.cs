﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageSelector : MonoBehaviour {
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void getAvailableImages(GameObject BigPapa)
    {
        UnityEngine.Object[] ob = Resources.LoadAll("Images/");
        float c = 0;
        foreach (UnityEngine.Object o in ob)
        {
            GameObject g = new GameObject(o.name);
            g.transform.parent = transform;
            g.transform.localScale = new Vector3(.01f, .01f, .01f);
            g.transform.localPosition=new Vector3(c,0,0);
            g.AddComponent<RawImage>();
            g.AddComponent<ImageSelectorImage>();
            g.AddComponent<BoxCollider>();
            g.GetComponent<BoxCollider>().size = new Vector3(g.GetComponent<RectTransform>().sizeDelta.x, g.GetComponent<RectTransform>().sizeDelta.y, 1);
            try
            {
                g.GetComponent<RawImage>().texture = o as Texture2D;
            }
            catch (Exception)
            {
                Texture t = new Texture();
                throw new Exception("Could not convert " + o.GetType().ToString() + " to " + t.GetType().ToString());
            }
            g.SendMessage("SetTarget", BigPapa);
            c += g.GetComponent<RectTransform>().sizeDelta.x;
        }
    }
    void SelectionComplete(GameObject g)
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject != g) { Destroy(t.gameObject); }
        }
    }
}
