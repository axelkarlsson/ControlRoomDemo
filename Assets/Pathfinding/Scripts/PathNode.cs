using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class PathNode : MonoBehaviour
{
    public bool visited;
    public bool inActivePath;
    public float costSoFar;
    public List<PathNode> neighbours;
    public PathNode cameFrom;
    public PathFinderNode node;
    public string nodeName;
    private bool cameraNode = false;
  

    public class PathFinderNode : Priority_Queue.FastPriorityQueueNode
    {
        public PathNode parent;
    }


    // Use this for initialization
    private void Awake()
    {
        neighbours = new List<PathNode>();
        node = new PathFinderNode { parent = this};
        ResetNode();

    }


    // Use this for initialization
    void Start()
    {
        if (GetComponentInParent<PathFinder>() != null)
        {
            GetComponentInChildren<MeshRenderer>().enabled = GetComponentInParent<PathFinder>().editMode;
            GetComponentInChildren<MeshCollider>().enabled = GetComponentInParent<PathFinder>().editMode;
        }
        else { cameraNode = true; }
    }


    // Update is called once per frame
    void Update()
    {
        if (!cameraNode && inActivePath && !GetComponentInParent<PathFinder>().editMode)
        {
            transform.Rotate(Vector3.forward, 1f);
        }
    }


    //Rotate this node to look at the next node in the path
    public void Rotate(PathNode target)
    {
        if (!cameraNode)
        {
            GetComponentInChildren<MeshRenderer>().enabled = true;
            transform.LookAt(target.gameObject.transform.position);
        }
    }


    //Rotate the destination node to look at the ground
    public void RotateEndPoint()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        transform.LookAt(transform.position + Vector3.down);
    }


    //Reset the node so a new path can be found
    public void ResetNode()
    {
        visited = false;
        inActivePath = false;
        costSoFar = float.PositiveInfinity;
        if (!cameraNode)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }


    //Adds a neighbour (can traverse to it while finding path
    public void AddNeighbour(PathNode otherNode)
    {
        if (!neighbours.Contains(otherNode))
        {
            neighbours.Add(otherNode);
            otherNode.neighbours.Add(this);
        }
    }


    // Links two nodes together if there are no colliders between them. Returns true if they can be linked and adds the nodes as neihgbours.
    public bool TryLink(PathNode othernode)
    {
        RaycastHit hitInfo;
        Vector3 origin = transform.position;
        Vector3 direction = (othernode.transform.position - transform.position).normalized;
        if (Physics.Raycast(origin, direction , out hitInfo, 20, GazeManager.Instance.RaycastLayerMasks[0]) && hitInfo.transform.gameObject.GetComponentInParent<PathNode>() == othernode)
        {
            AddNeighbour(othernode);
            return true;
        }
        return false;
    }


    //Rechecks what neighbours this node has, useful after moving a node
    public void UpdateNeighbours()
    {
        PathFinder parent = GetComponentInParent<PathFinder>();
        if(parent != null)
        {
            parent.UpdateNode(this);
        }
    }


    //Remove a neighbour
    public void RemoveNeighbour(PathNode otherNode)
    {
        neighbours.Remove(otherNode);
        otherNode.neighbours.Remove(this);
    }



    public float DistanceTo(PathNode otherNode)
    {
        return (transform.position - otherNode.transform.position).magnitude;
    }



    //Turn on or off the meshrenderer and meshcollider for the node
    public void ShowNode(bool active)
    {
        GetComponentInChildren<MeshRenderer>().enabled = active;
        //Layer 0 is default, layer 2 has no collision
        gameObject.layer = active ? 0 : 2;
    }


    //When hold, select this node or deselect it if already selected
    public void NodeSelected()
    {
        PathFinder finder = GetComponentInParent<PathFinder>();

        GameObject oldObject = finder.activeObject;
        finder.activeObject = finder.activeObject == gameObject ? null : gameObject;
        if(oldObject != gameObject && oldObject != null)
        {
            oldObject.GetComponent<PathNode>().Highlight(false);
        }
        Highlight(finder.activeObject == gameObject);
    }


    public void Highlight(bool active)
    {
        //Add highlight/selection to active object
    }

   
    public void OnDestroy()
    {
        foreach (PathNode neighbour in neighbours)
        {
            neighbour.neighbours.Remove(this);
        }
        PathFinder parent = GetComponentInParent<PathFinder>();

        if (parent != null)
        {
            parent.activeObject = parent.activeObject == this ? null : parent.activeObject;
            parent.destinationNodes.Remove(this);
        }
    }
}
