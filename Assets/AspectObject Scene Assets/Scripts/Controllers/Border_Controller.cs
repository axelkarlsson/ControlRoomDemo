using System.Collections;
using System.Linq;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class Border_Controller: MonoBehaviour {
    Renderer H_rend;
    AspectMenuController AMC;
	// Use this for initialization
	void Start ()
    {
        H_rend = gameObject.transform.Find("Hightlight").GetComponent<Renderer>();
        if (gameObject.tag == "Aspect_Menu_Item_Border")
        {
            AMC = GameObject.Find("Aspect_Menu").GetComponent<AspectMenuController>();
            gameObject.transform.localScale = new Vector3(AMC.ItemLength * 0.6f, AMC.ItemHeight, Mathf.Min(AMC.ItemLength * 0.6f, AMC.ItemHeight));
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        H_rend.enabled = CheckIfFocused(gameObject);
    }

    private bool CheckIfFocused(GameObject G)
    {
        GameObject g = GameObject.Find("InputManager").GetComponent<GazeManager>().HitObject;

        if (
            g != null
            &&
            g.transform.parent != null
            &&
            g.transform.parent.parent != null
           )
        {
            return (
             g.transform.parent.name        == G.transform.parent.name 
             ||
             g.transform.parent.parent.name == G.transform.parent.name
                   );
        }
        else { return false; }

    }
}
