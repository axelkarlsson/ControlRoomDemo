using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface that should be implemented by all classes that can be used as nodes in Pathfinding
public class IPathNode {

    public bool visited;
    public bool inActivePath;
    public float costSoFar;
    public List<IPathNode> neighbours;
    public PathNode cameFrom;
    public IPathNode node;
}
