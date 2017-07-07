using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using UnityEngine.SceneManagement;
using System;

public class HoloItemScript_Menu: MonoBehaviour, IInputHandler
{
    void Start()
    {
        if (!transform.Find("Specific Content").Find("Center Content").gameObject.activeInHierarchy)
        {
            transform.Find("Specific Content").Find("Center Content").gameObject.SetActive(true);
        }
    } 
    void Update()
    {
        if (gameObject == GazeManager.Instance.HitObject)
        { 
            gameObject.transform.Find("Always Present Graphics").Find("Highlight").GetComponent<Renderer>().enabled = true;
        }
        else
        {
            gameObject.transform.Find("Always Present Graphics").Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
    }
    public void OnInputUp(InputEventData eventData)
    {
        //Toggle Aspect Window
        GameObject g = GameObject.Find(name + "_window");
        if (g == null)
        {
            g = Instantiate(Resources.Load("Aspect_Window"), null) as GameObject;
            g.name = name + "_window";
        }
        else
        {
            Destroy(g);
        }
    }
    public void OnInputDown(InputEventData eventData)
    {
    }
}
