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
        RecalculatePath();
        ReturnToStart();
        StartCoroutine(FollowPath());  // Rather than using string reference like in Invoke method we can now use the proper method name
        //InvokeRepeating("PrintWaypointName", 0, 1f);   //Does a similar job to coroutines but not doing what we want
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void RecalculatePath()             //calculate or recalculate a path when a tower placed blocks the current path
    {
        path.Clear();        //If there is already some path then it will clear before adding the below path
        path = pathfinder.GetNewPath();
    }

    void ReturnToStart()                     // Make the enemy start from the startCoordinates
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    IEnumerator FollowPath()               //For enemies to follow the calculated path
    {
        for (int i = 0; i < path.Count; i++)
        {
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
