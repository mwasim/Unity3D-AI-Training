using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    public List<GameObject> waypoints;
    private int currentWayPoint;

    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if the distance between the current object (player) and the target object (obstacle) is short, increment the wayPoint as the object is moving towards the next way point
        if (Vector3.Distance(transform.position, waypoints[currentWayPoint].transform.position) < 3)
        {
            currentWayPoint++;
        }

        //if the current object has reached to the last way point, point it towards the first way point
        if (currentWayPoint >= waypoints.Count - 1)
        {
            currentWayPoint = 0;
        }

        //Rotate the transform so that the forward vector (Z direction) points at the target's current position
        //transform.LookAt(waypoints[currentWayPoint].transform);


        /*
            Although using transform.LookAt is fine yet turns are sharp, and to make it more realistic and smooth,
            we can use the below code which uses Quaternion
         */
        //Get look at way point rotationn (Quaternion are used to represent rotations) [targetPos - currentPos]
        var lookAtWP = Quaternion.LookRotation(waypoints[currentWayPoint].transform.position - transform.position);

        //set current object's rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, rotationSpeed * Time.deltaTime);


        //move the current object with speed (e.g. 10) meters/second (delta time)
        transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
    }
}
