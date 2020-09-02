using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager;

    private float _speed;
    private bool _turning;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomSpeed();
    }

    private void SetRandomSpeed()
    {
        _speed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //determine the bounding box of the manager's cube
        Bounds b = new Bounds(flockManager.transform.position, flockManager.swimLimit * 2);

        //if the fish is outside the bounds of the cube or about to hit something (e.g. sand piller) then start turning around
        /*
            transform.forward * 50 = how long the ray should be (transform.forward = 1 as it's normalized), we can adjust the value 50
         */
        var rayLength = transform.forward * 50;
        var direction = Vector3.zero;
        _turning = false;

        if (!b.Contains(transform.position))
        {
            _turning = true;

            //turn towards the center of the manager's cube
            direction = flockManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, rayLength, out RaycastHit hitInfo))
        {
            //Debug.DrawRay(transform.position, rayLength, Color.red);

            _turning = true;

            //just like in Billiards, the ball is reflected on hitting the target object, we do the reflection here as well
            direction = Vector3.Reflect(transform.forward, hitInfo.normal);
        }

        if (_turning)
        {           
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                flockManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10) //Execute only when there's is 10 percent chance
            {
                SetRandomSpeed();
            }

            if (Random.Range(0, 100) < 20) //Execute only when there's is 20 percent chance
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, Time.deltaTime * _speed);
    }

    private void ApplyRules()
    {
        GameObject[] gameObjects = flockManager.allFish;

        var vCenter = Vector3.zero; //average center
        var vAvoid = Vector3.zero; //average avoidance
        var gSpeed = 0.01f; //global speed of the group
        float nDistance; //neighbour distance
        int groupSize = 0; //count how many of the fish in the flock are group

        foreach (var go in gameObjects) //loop through all the fish in the flock
        {
            if (go != gameObject) //don't calculate data for itself
            {
                nDistance = Vector3.Distance(go.transform.position, transform.position);
                if (nDistance <= flockManager.neighbourDistance)
                {
                    vCenter += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vAvoid = vAvoid + (transform.position - go.transform.position);
                    }

                    var anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock._speed;
                }
            }
        }

        if (groupSize > 0)
        {
            //cause the flocking to occur
            vCenter = vCenter / groupSize + (flockManager.goalPosition - transform.position); //towards the center of the goal position
            _speed = gSpeed / groupSize;

            var direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero) //not facing in the direction
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), flockManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
