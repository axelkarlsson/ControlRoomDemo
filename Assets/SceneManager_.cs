using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_ : MonoBehaviour {
    
    public static bool hasLoaded = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene().name == "Test" &&  !hasLoaded)
        {
            hasLoaded = PlaceDemoObject(new Vector3(0,0,2));
        }
	}
    bool PlaceDemoObject(Vector3 pos)
    {
        GameObject g = Instantiate(Resources.Load("ObjectName") as GameObject, null);
        g.name = "Demo Object";
        g.transform.position = pos;
        g.SetActive(true);
        return (g.activeInHierarchy);
    }
}
