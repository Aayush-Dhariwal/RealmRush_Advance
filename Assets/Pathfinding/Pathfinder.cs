using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinationCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Dictionary<Vector2Int, Node> reached;    //For a quick lookup reference to see whether a node has already been explored or not

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };   //Direction for search(neighbors),  basically 1 unit in each direction
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null )
        {
            grid = gridManager.Grid;                //Accessing Grid (we have a public property for it) from GridManager script 
        }
        startNode = new Node(startCoordinates, true);
        destinationNode = new Node(destinationCoordinates, true);
    }

    void Start()
    {
        ExploreNeighbors();
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

                //TODO: Remove after testing
                grid[neighborCoords].isExplored = true;
                grid[currentSearchNode.coordinates].isPath = true;
            }
        }
    }
}
