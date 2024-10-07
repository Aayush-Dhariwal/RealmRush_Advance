using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteAlways]  // Will be executed always even in scene mode
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        
        DisplayCoordinates();
    }
    void Update()
    {
        if(PrefabStageUtility.GetCurrentPrefabStage() != null)       //To disable tile renaming pop-up in prefab mode
        {
            return;
        }
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            //label.enabled = true;
        }
        SetLabelColor();

        ToggleLabels();
    }

    void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }

    void SetLabelColor()   // To change the color of waypoint label depending on if it is isPlaceable or not 
    {
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if (node == null) { return; }

        if(!node.isWalkable)
        {
            label.color = blockedColor;
            //Debug.Log("Grey colored blocked");
        }
        else if(node.isPath)
        {
            label.color = pathColor;
            //Debug.Log("Orange Colored Path");
        }
        else if(node.isExplored)
        {
            label.color = exploredColor;
            //Debug.Log("yellow is explored");
        }
        else
        {
            label.color = defaultColor;
        }
    }

    void DisplayCoordinates()    // To display coordinates as Text on the tiles
    {
        if(gridManager == null) { return; }         //Exiting early if gridManager is null

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);  //coordinates of the parent divided by increment snap value
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = coordinates.x + ", " + coordinates.y;
    }

    void UpdateObjectName()                      //To update the name of the Tile(environment in Hierarchy) acc to where it is placed
    {
        transform.parent.name = coordinates.ToString();
    }
}
