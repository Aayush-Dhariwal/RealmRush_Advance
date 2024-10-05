using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;         // Instead of GameObject we are using Tower to get reference to our Tower script from Waypoint

    [SerializeField] bool isPlaceable;
    public bool GetIsPlaceable { get { return isPlaceable; } }

    //public bool GetIsPlaceable()           //Another way of implementing above property as method
    //{
    //    return isPlaceable;
    //}

    GridManager gridManager;
    Pathfinder pathfinder;
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (gridManager != null)        //The rest of the code can continue executing after this block, even if gridManager was null. Use this when we don't want an early exit 
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlaceable)
            {
                gridManager.BlockNode(coordinates);
            }
        }          
    }

    private void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates))
        {
            bool isSuccessful= towerPrefab.CreateTower(towerPrefab, transform.position);                //Returns true if we have managed to place a tower or 
                                                                                                     //Return false if we didn't have enough money in bank 
            //Debug.Log("Tower Placed");
            if (isSuccessful)                           
            {
                gridManager.BlockNode(coordinates);        //Block a node when a tower is placed there
                pathfinder.NotifyReceivers();             //Tell pathfinder to notify its receivers that a tower has been placed (to recalculate the path) 
            }
        }
    }
}
