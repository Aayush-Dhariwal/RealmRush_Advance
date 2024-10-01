using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] [Range(0f, 5f)]float speed = 1.0f;            // To make sure travelPercent doesn't become negative we are setting a range on speed

    Enemy enemy;
    void OnEnable()                 // Using OnEnable in place of Start as whenever enemy gets active ReturnToStart method will be called and object will be at startPosition(path[0])
    {
        FindPath();
        ReturnToStart();
        //Debug.Log("Start Here");
        StartCoroutine(FollowPath());  // Rather than using string reference like in Invoke method we can now use the proper method name
        //Debug.Log("Finishing start");
        //InvokeRepeating("PrintWaypointName", 0, 1f);   //Does a similar job to coroutines but not doing what we want
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear();        //If there is already some path then it will clear before adding the below waypoints(path)

        //GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Path");         // Maybe possible that this won't add waypoints in order as they are in Hierarchy

        GameObject parent = GameObject.FindGameObjectWithTag("Path");
        foreach (Transform child in parent.transform)
        {
            Tile waypoint = child.GetComponent<Tile>();

            if (waypoint != null )
            {
                path.Add(waypoint);                //Adding waypoints to the List path
            }
            
        }
    }

    void ReturnToStart()                     // Make the enemy start from the first waypoint
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator FollowPath()
    {
        foreach (Tile waypoint in path)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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
