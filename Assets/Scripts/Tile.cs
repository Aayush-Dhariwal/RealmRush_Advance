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
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
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
        if (isPlaceable)
        {
            bool isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);                //Returns true if we have managed to place a tower or 
                                                                                                     //Return false if we didn't have enough money in bank 
            isPlaceable = !isPlaced;                                                              
        }
    }
}
