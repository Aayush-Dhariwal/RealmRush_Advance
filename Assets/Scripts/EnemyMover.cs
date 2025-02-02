using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1.0f;            // To make sure travelPercent doesn't become negative we are setting a range on speed

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    void OnEnable()                 // Using OnEnable in place of Start as whenever enemy gets active ReturnToStart method will be called and object will be at startPosition(path[0])
    {
        ReturnToStart();
        RecalculatePath(true);
        //StartCoroutine(FollowPath());  // Rather than using string reference like in Invoke method we can now use the proper method name
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void RecalculatePath(bool resetPath)             //calculate or recalculate a path when a tower placed blocks the current path
    {
        Vector2Int coordinates = new Vector2Int();

        if(resetPath )
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);    //To get the current coordinates of the enemy
        }

        StopAllCoroutines(); //Stops the enemy from following the path while it finds a new one
        path.Clear();        //If there is already some path then it will clear before adding the below path
        path = pathfinder.GetNewPath(coordinates);

        StartCoroutine(FollowPath());      //Reason for keeping this in RecalculatePath method is because we also want to stop the routine before we get a new path
    }

    void ReturnToStart()                     // Make the enemy start from the startCoordinates
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    IEnumerator FollowPath()               //For enemies to follow the calculated path
    {
        for (int i = 1; i < path.Count; i++)           //Starting from i = 1 ensures that the enemy starts moving to the next waypoint rather than to the position it's already at
        {                                              //which was happening when we started from i = 0. This also fix the problem of our enemies sitting at the gatehouse when they
                                                       //are first instantiated
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);  // To make the enemy face in the direction of movement

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);           //Enemy movement by linear interpolation
                yield return new WaitForEndOfFrame();
            }
            //yield return new WaitForSeconds(waitTime);    //yield return can mean to give up the control and come back to me in 1 second
        }

        FinishPath();
    }

    private void FinishPath()
    {
        enemy.StealGold();       // When reached endPosition Steal gold from the castle
        gameObject.SetActive(false);
    }
}
