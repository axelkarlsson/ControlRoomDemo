using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestCall : MonoBehaviour {
    public HttpUtils.RestClient client;

	// Use this for initialization
	void Start () { 
        /*
        client = new HttpUtils.RestClient("http://138.227.236.109:8088/api/v0.1/properties/57461dc4-3171-4b8f-94b8-be6f72347edc/b60fc394-b1c9-4def-80c5-7be809d6763/par.Man?format=json");
        var json = client.MakeRequest();
        Debug.Log(json);
        */
    }
	
	// Update is called once per frame
	void Update () {

	}
}
