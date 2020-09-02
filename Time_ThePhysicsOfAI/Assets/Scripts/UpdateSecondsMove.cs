using UnityEngine;

public class UpdateSecondsMove : MonoBehaviour
{
    float timeStartOffset = 0f;
    bool gotStartTime = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
            When time is applied in Physics, it's in meters per second (m/s)
            Time is fundamental to all physics calculations
         */
        if (!gotStartTime) //runs once only
        {
            timeStartOffset = Time.realtimeSinceStartup;
            gotStartTime = true;
        }

        //move the object based on the seconds on th computer clock (seconds in the real world) [It's similar to Time.deltaTime]
        transform.position = new Vector3(transform.position.x, transform.position.y, Time.realtimeSinceStartup - timeStartOffset);
    }
}
