using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node              //This is a pure C# class which we can't add to a gameobject now as it does not inherit from MonoBehaviour
{                              //also we can use public for field variables as we won't have any methods in this class. it is simply a data container

    public Vector2Int coordinates;     // Coordinates our node will occupy in the grid/world
    public bool isWalkable;            // similar to isPlacable 
    public bool isExplored;            // holds the state whether this node has been explored by our pathfinding
    public bool isPath;                // holds the state whether the node is in the path or not
    public Node connectedTo;           // which node is this node connected to

    public Node(Vector2Int coordinates, bool isWalkable)           // A constructor
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
