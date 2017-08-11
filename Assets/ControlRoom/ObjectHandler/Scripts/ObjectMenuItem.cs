using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;

public class ObjectMenuItem : MonoBehaviour, IInputClickHandler {
    public GameObject setFor;
    Color originalColor;
    Text t;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        setFor.name = GetComponent<Text>().text;
        setFor.transform.Find("Specific Content").Find("Center Content").Find("Center_Canvas").Find("Text").GetComponent<Text>().text = GetComponent<Text>().text;
        setFor.transform.Find("Specific Content").Find("Side Content").gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
    public void Awake()
    {
        t = GetComponentInChildren<Text>();
        originalColor = t.color;
    }
    // Use this for initialization
    void Start () {
        
		
	}

    // Update is called once per frame
    private void Update()
    {
        IsHighlighted = GazeManager.Instance.HitObject == this.gameObject;
    }
    private bool IsHighlighted
    {
        set
        {
            t.color = value ? Color.yellow : originalColor;
        }
    }
}
