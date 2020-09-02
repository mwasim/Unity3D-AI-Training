using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    /*
     * VELOCITY:
        If an object has moved 14 meters in 2 seconds, then it means it's moved at the rate of 7m/s
        And based on this info, we can also determine how much distance the object will cover in the same direction in 10 seconds => 10 * 7m/s = 70m in 10 seconds
        Velocity is the RATE of CHANGE in TIME.


       ACCELERATION:
       Acceleration is the RATE OF CHANGE in VELOCITY
       We know,
       Force = mass * acceleration,
   So, Acceleration = Force / mass
        
     */

    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
    }
}
