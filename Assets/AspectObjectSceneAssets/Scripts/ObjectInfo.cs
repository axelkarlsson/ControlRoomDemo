using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo {
    public string name;
    public Vector3 Position;
    public Vector3 Scale;
    public Quaternion Rotation;
    public GameObject toLoad;
    public ObjectInfo(string n, Vector3 pos, Vector3 sca, Quaternion rot, GameObject g)
    {
        toLoad = g;
        name = n;
        Position = pos;
        Scale = sca;
        Rotation = rot;
    }

}
