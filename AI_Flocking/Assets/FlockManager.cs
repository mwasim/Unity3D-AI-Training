using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numberOfFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimit = new Vector3(5, 5, 5);
    public Vector3 goalPosition;

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)] //slider in the inspector
    public float minSpeed = 0.3f;

    [Range(0.0f, 5.0f)]
    public float maxSpeed = 1.0f;

    [Range(0.0f, 10.0f)]
    public float neighbourDistance = 5.0f; //distance from another fish

    [Range(0.0f, 5.0f)]
    public float rotationSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numberOfFish];
        for (int i = 0; i < numberOfFish; i++)
        {            
            allFish[i] = Instantiate(fishPrefab, RandomFlockPosition, Quaternion.identity);

            //set the fish's flockManager
            allFish[i].GetComponent<Flock>().flockManager = this;
        }

        goalPosition = transform.position;
    }  

    // Update is called once per frame
    void Update()
    {        
        if (Random.Range(0, 100) < 10) //only update the goal position when there's is 10 percent chance
        {
            goalPosition = RandomFlockPosition;
        }      
    }

    private Vector3 RandomFlockPosition => transform.position + new Vector3(Random.Range(-swimLimit.x, swimLimit.x),
            Random.Range(-swimLimit.y, swimLimit.y),
            Random.Range(-swimLimit.z, swimLimit.z));
}
