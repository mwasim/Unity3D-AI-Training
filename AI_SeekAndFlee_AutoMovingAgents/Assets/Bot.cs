using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    private NavMeshAgent agent;
    private Drive driveScript;

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        driveScript = target.GetComponent<Drive>();
    }

    private void Seek(Vector3 destinationLocation)
    {
        //In this case, the destinationLocation is going to be the COP
        agent.SetDestination(destinationLocation);
    }

    private void Flee(Vector3 location)
    {
        /*
         * location => cop's position, transform.position => robber's position
         * We can set the flee position in the opposite direction to the cop
        */
        var fleeVector = location - transform.position; //gives vector in the direction of the location
        var destination = transform.position - fleeVector; //to make it opposite with same magnitude, simply subtract from the current position

        agent.SetDestination(destination);

    }

    /*
        Pursuit is similar to Seek. The difference is, the Persuer predicts the future location of the target and intercepts
     */
    private void Pursue()
    {
        //Predict future location
        var targetCurrentSpeed = driveScript.currentSpeed;

        var targetDirection = target.transform.position - transform.position;
        /*
            For relative angle, translate the target's forward dir relative to the space of the agent
         */
        var relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        var toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDirection));

        //SCENARIO-1: If the target has stopped moving, simply seek and return
        //SCENARIO-2: OR Check if the angle is small (relativeHeading)
        if ((toTarget > 90 && relativeHeading < 20) || targetCurrentSpeed < 0.01f) //we're not comparing with ZERO because sometimes there's is floating point error
        {
            Seek(target.transform.position);
            Debug.Log("JUST SEEKING...");
            return;
        }

        var lookAhead = targetDirection.magnitude / (agent.speed + targetCurrentSpeed);

        //Determin seek location, and seek
        //target.transform.forward has magnitude = 1 (already normalized) -> forward is the direction in which the object is moving
        var seekLocation = target.transform.position + target.transform.forward * lookAhead;
        Seek(seekLocation);
        Debug.Log("PURSUING...");
    }

    /*
        Evade is similar to Pursue. Except it uses the Flee instead of Seek (Opposite of Pursue).
     */
    private void Evade()
    {
        var targetCurrentSpeed = driveScript.currentSpeed;
        var targetDirection = target.transform.position - transform.position;

        var lookAhead = targetDirection.magnitude / (agent.speed + targetCurrentSpeed);

        //Determin flee location, and seek
        //target.transform.forward has magnitude = 1 (already normalized) -> forward is the direction in which the object is moving
        var fleeLocation = target.transform.position + target.transform.forward * lookAhead;

        Flee(fleeLocation);
    }

    Vector3 wanderTarget = Vector3.zero;
    private void Wander()
    {
        //These variables can be adjusted to adjust Wander behavior
        //Ensure the position you're calculating exists on the NavMesh, otherwise it may cause location issues
        var wanderRadius = 10.0f; //adjust the wander circle
        var wanderDistance = 1.0f;
        var wanderJitter = 1.0f;

        var wanderAxisValue = Random.Range(-1.0f, 1.0f) * wanderJitter;
        wanderTarget += new Vector3(wanderAxisValue,
            0f,
            wanderAxisValue);

        wanderTarget.Normalize(); //sets magnitude to 1
        wanderTarget *= wanderRadius;

        var targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        var targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    //Find the best hiding place/spot (can be closest or furtherest)
    private void Hide()
    {
        //find the closest hiding spot
        var distance = Mathf.Infinity;
        var closestSpotToHide = Vector3.zero;

        foreach (var hidingSpot in World.Instance.HidingSpots)
        {
            var hideDirection = hidingSpot.transform.position - target.transform.position;
            var hidePosition = hidingSpot.transform.position + hideDirection.normalized * 10; //position to the hiding spot with little bit of distance (hideDirection.normalized * 5)

            var distanceToHide = Vector3.Distance(transform.position, hidePosition);
            if (distanceToHide < distance)
            {
                closestSpotToHide = hidePosition;
                distance = distanceToHide;

                Debug.Log("Found closes spot at distance: " + distance);
            }
            else
            {
                Debug.Log("Couldn't find the closest spot");
            }
        }

        Seek(closestSpotToHide); //hide behind the hiding spot using Seek
        Debug.Log("Seeking: " + closestSpotToHide);
    }

    /*
        As the ray (from the target/cop) collides with the tree's collider, from the other side of the collider we can determine the hiding spot
     */
    private void CleverHide()
    {
        //find the closest hiding spot
        var distance = Mathf.Infinity;
        var closestSpotToHide = Vector3.zero;
        var chosenDirection = Vector3.zero;
        var chosenHidingSpotGO = World.Instance.HidingSpots[0]; //initialize with the first game object

        foreach (var hidingSpot in World.Instance.HidingSpots)
        {
            var hideDirection = hidingSpot.transform.position - target.transform.position;
            var hidePosition = hidingSpot.transform.position + hideDirection.normalized * 10; //position to the hiding spot with little bit of distance (hideDirection.normalized * 5)

            var distanceToHide = Vector3.Distance(transform.position, hidePosition);
            if (distanceToHide < distance)
            {
                closestSpotToHide = hidePosition;
                chosenDirection = hideDirection;
                chosenHidingSpotGO = hidingSpot;
                distance = distanceToHide;

                Debug.Log("Found closes spot at distance: " + distance);
            }
            else
            {
                Debug.Log("Couldn't find the closest spot");
            }
        }

        //Do the raycast to determine if the agent is behind the collider
        var hideCollider = chosenHidingSpotGO.GetComponent<Collider>();
        var backRay = new Ray(closestSpotToHide, -chosenDirection.normalized);

        RaycastHit info;
        var hitDistance = 250.0f; //ensure this distance to cast ray is more than the distance to to hide used above (e.g. 10) in the hidePosition calculation

        hideCollider.Raycast(backRay, out info, hitDistance);


        //Seek(closestSpotToHide); //hide behind the hiding spot using Seek
        Seek(info.point + chosenDirection.normalized * 2); //add little more distance to the point (e.g. distance between the tree and the spot where the agent will hide

        Debug.Log("Seeking: " + closestSpotToHide);
    }

    private bool CanSeeTarget()
    {
        var rayToTarget = target.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayToTarget, out RaycastHit info))
        {
            if (info.transform.gameObject.CompareTag("cop")) return true;
        }

        return false;
    }

    //Complex behaviors
    /*
        For example, robber (this object) can sneak upon the cop while the cop is looking away
        And when cop is looking towards the robber (e.g. upto 60 degrees angle), then robber can hide from the cop
     */
    private bool TargetCanSeeMe()
    {
        var toAgent = transform.position - target.transform.position;
        var lookingAngle = Vector3.Angle(target.transform.forward, toAgent);

        if (lookingAngle < 60) return true;

        return false;
    }

    private bool coolDown = false;
    private void ResetCoolDown()
    {
        coolDown = false;
    }

    //Challenge - If the distance between cop and the robber is more than 10, then robber will wander, otherwise pursue or cleverly hides
    private bool TargetInRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 10) return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //Testing Seek
        //Seek(target.transform.position);

        //Testing Flee
        //Flee(target.transform.position);

        //Testing Pursuite
        //Pursue();

        //Testing Evade
        //Evade();

        //Testing Wander
        //Wander();

        //Testing Hide
        //Hide();

        //Testing CleverHide
        //hide if can see the target
        //if (CanSeeTarget())
        //{
        //    CleverHide();
        //}


        //Testing Complex behavior
        /*
            For example, robber (this object) can sneak upon the cop while the cop is looking away
            And when cop is looking towards the robber (e.g. upto 60 degrees angle), then robber can hide from the cop
         */
        //if (!coolDown) 
        //{
        //    if (CanSeeTarget() && TargetCanSeeMe())
        //    {
        //        CleverHide();
        //        coolDown = true;
        //        Invoke(nameof(ResetCoolDown), 5.0f); //the robber will pause (or cool down) for few seconds
        //    }
        //    else
        //    {
        //        Pursue();
        //    }
        //}


        //Complex behavior challenge
        // - If the distance between cop and the robber is more than 10, then robber will wander, otherwise pursue or cleverly hides
        if (!coolDown)
        {
            if (!TargetInRange())
            {
                Wander();
            }
            else if (CanSeeTarget() && TargetCanSeeMe())
            {
                CleverHide();
                coolDown = true;
                Invoke(nameof(ResetCoolDown), 5.0f); //the robber will pause (or cool down) for few seconds
            }
            else
            {
                Pursue();
            }
        }        
    }    
}
