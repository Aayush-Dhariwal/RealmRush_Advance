using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    [SerializeField] Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();      // To store all the nodes we've got queued from searching through our neighbors but haven't yet looked at to see if they are going to be part of the path or not 
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();    //For a quick lookup reference to see whether a node has already been explored or not

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };   //Direction for search(neighbors),  basically 1 unit in each direction
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid= new Dictionary<Vector2Int, Node>();      //To get reference to Grid in GridManager script

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null )
        {
            grid = gridManager.Grid;                //Accessing Grid (we have a public property for it) from GridManager script 
            startNode = grid[startCoordinates];
            destinationNode = grid[destinationCoordinates];
        }
    }

    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        gridManager.ResetNode();
        BreadthFirstSearch();
        return BuildPath();
    }

    private void ExploreNeighbors()                  // Explore the directions(above vector) and if the grid contains the neighbor coordinates then add it to the list
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + direction;     // neighbor can be found by adding 1 to currentSearchNode in each direction for right, left etc.

            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        foreach(Node neighbor  in neighbors)
        {
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)           //If the node doesn't exist in the reached dictionary and isWalkable is true then
            {
                neighbor.connectedTo = currentSearchNode;                        //Will create the connections on our map and build a Tree for us
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch()
    {
        startNode.isWalkable = true;         //To make sure they are walkable for pathfinding, but not isPlaceable for our Towers
        destinationNode.isWalkable = true;

        frontier.Clear();     //To clear the queue and dictionary for us to start pathfinding for a second time
        reached.Clear();

        bool isRunning = true;     // To help us break out of while loop

        frontier.Enqueue(startNode);   // Enqueue adds to the end of the Queue  (here queue is empty so startNode will be the first element)
        reached.Add(startCoordinates, startNode);

        while ( frontier.Count > 0 && isRunning)         //isRunning helps to break out this while loop when we have found our destination but still we've got things to search
        {
            currentSearchNode = frontier.Dequeue();    // Dequeue returns the front of the queue (to currentSearchNode) and removes it from queue
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if(currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath()                      //Look through the Tree from our destination node back to our starting node and build that path for us
    {                                          // using the connections(connectedTo) we made in ExploreNeighbor method
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;      //we're searching from the destination backwards

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)         //while there are still connected nodes to explore, keep moving back through out tree
        {
            currentNode = currentNode.connectedTo;      //Will take us one step (node) back down our path
            path.Add(currentNode);
            currentNode.isPath = true;
            //Debug.Log("path build");
        }

        path.Reverse();              //To reverse the backtracked path

        return path;
    }

    public bool WillBlockPath(Vector2Int coordinates)       //To check if a tower placed will block the path to destination or not and then create a new path accordingly
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;

            grid[coordinates].isWalkable = false;       //Temporarily setting isWalkable to false at the specified node
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1)    // This means that tower is blocking the path as it was not able to find a new node so we create a new path
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()      //To broadcast the message to every monobehaviour
    {
        BroadcastMessage("RecalculatePath", SendMessageOptions.DontRequireReceiver);     //Will call the RecalculatePath to every MonoBehaviour in this
                                                                                         //gameobject (whichever gameobject this script is attached to)
                                                                                         //2nd parameter makes sure that no error will be returned if there is no receiver
    }
}
