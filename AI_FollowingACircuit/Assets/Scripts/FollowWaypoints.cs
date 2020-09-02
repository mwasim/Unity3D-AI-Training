using System.Collections.Generic;
using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    //List of way points
    public List<GameObject> waypoints;

    //waypoint index
    private int currentWayPoint;

    //player's speed to move ahead
    public float speed = 10.0f;

    //player's rotation speed
    public float rotationSpeed = 10.0f;

    // Limit how far the tracker moves in front of the player
    public float lookAhead = 10.0f;

    //Store a tracker that the player will follow
    GameObject tracker;

    // Start is called before the first frame update
    void Start()
    {
        //create a cylinder for visual purposes
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        //Destroy the cylinder collider so it doesn't cause any issues with physics
        DestroyImmediate(tracker.GetComponent<Collider>());

        //Disable the trackers mesh renderer so you can't see it in the game
        tracker.GetComponent<MeshRenderer>().enabled = false;

        //Rotate and place the tracker
        tracker.transform.position = transform.position;
        transform.transform.rotation = transform.rotation;
    }

    private void ProcessTracker()
    {
        //check the tracker doesn't go too far ahead of the player
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        //Check if the player has reached certain distance from the current way point
        if (Vector3.Distance(tracker.transform.position, waypoints[currentWayPoint].transform.position) < 3.0f)
        {
            //select next way point
            currentWayPoint++;
        }

        //Check we haven't reached the last way point
        if (currentWayPoint >= waypoints.Count - 1)
        {
            //reset to point to the first way point
            currentWayPoint = 0;
        }

        //Aim the tracker at the current way point
        tracker.transform.LookAt(waypoints[currentWayPoint].transform);

        //move the tracker towards the way point
        tracker.transform.Translate(0, 0, (speed + 20.0f) * Time.deltaTime);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //Call the ProcessTracker method to move the tracker
        ProcessTracker();

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


        //move the current object/player with speed (e.g. 10) meters/second (delta time)
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
