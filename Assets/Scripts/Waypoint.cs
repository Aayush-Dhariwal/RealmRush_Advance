using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;         // Instead of GameObject we are using Tower to get reference to our Tower script from Waypoint

    [SerializeField] bool isPlaceable;
    public bool GetIsPlaceable { get { return isPlaceable; } }

    //public bool GetIsPlaceable()           //Another way of implementing above property as method
    //{
    //    return isPlaceable;
    //}

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
