using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.UI.Keyboard;
using System;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour, IHoldHandler, IInputClickHandler
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        


    }
    public void OnHoldStarted(HoldEventData eventData)
    {
        if (GetComponentInParent<PathFinder>() != null && GetComponentInParent<PathFinder>().editMode)
        {
            //Show the keyboard prefab included in holotoolkit and subscribe to relevant events
            Keyboard.Instance.Close();
            Keyboard.Instance.PresentKeyboard();
            Keyboard.Instance.RepositionKeyboard(Camera.main.transform.position + Camera.main.transform.forward * 2);
            Keyboard.Instance.onTextSubmitted += this.Keyboard_onTextSubmitted;
        }
    }

    private void Keyboard_onTextSubmitted(object sender, EventArgs e)
    {
        string tmp;
        var keyboard = sender as Keyboard;
        tmp = keyboard.m_InputField.text;
        GetComponent<Text>().text = tmp;
        GetComponent<BoxCollider>().size = new Vector3(Mathf.Min(2,GetComponent<Text>().preferredWidth), Mathf.Min(2,GetComponent<Text>().preferredHeight),0.3f);
        if (transform.parent.name == "Top_Canvas")
        {
            GetComponent<BoxCollider>().center = new Vector3(0, Mathf.Min(2,GetComponent<Text>().preferredHeight) / 2 - 1, 0);
        }
        else if (transform.parent.name == "Side_Canvas")
        {

            GetComponent<BoxCollider>().center = new Vector3(Mathf.Min(2,GetComponent<Text>().preferredWidth) / 2 -1, 0, 0);
        }
        else if (transform.parent.name == "Bottom_Canvas")
        {

            GetComponent<BoxCollider>().center = new Vector3(0, -Mathf.Min(2,GetComponent<Text>().preferredHeight) / 2 + 1, 0);
        }
        Keyboard.Instance.onTextSubmitted -= this.Keyboard_onTextSubmitted;

    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        //NoTUsed
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        //Notused
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {

#if NETFX_CORE
                        LaunchThing();
#endif
    }

#if NETFX_CORE
    private async void LaunchThing()
    {
        bool success = await Windows.System.Launcher.LaunchUriAsync(new Uri("holoabb-aspect://" + GetComponent<Text>().text));
    }
#endif
    public void BBoxEnabled(bool state)
    {
        if (GetComponent<Text>().text == string.Empty)
        {
            GetComponent<BoxCollider>().enabled = state;
        }
    }
}
