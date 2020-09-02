using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{
    NavMeshAgent agent;
    // Access to the WPManager script
    public GameObject wpManager;
    // Array of waypoints
    List<GameObject> wps;

    // Use this for initialization
    void Start()
    {
        // Get hold of wpManager and Graph scripts
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(wps[4].transform.position);
    }

    public void GoToRuin()
    {
        agent.SetDestination(wps[5].transform.position);
    }

    public void GoBehindHeli()
    {
        agent.SetDestination(wps[9].transform.position);
    }
}