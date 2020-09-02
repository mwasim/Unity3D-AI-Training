using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Shell - This script is using custom coding, and disabled in the scene
 */
public class Shell : MonoBehaviour
{
    public GameObject explosion;

    /*
     ACCELERATION:
       Acceleration is the RATE OF CHANGE in VELOCITY
       We know,
       Force = mass * acceleration,
   So, Acceleration = Force / mass

    Let's use this equation here
     */
    float mass = 10f; //10 kg
    float force = 200f;
    float acceleration;
    float speedZ;

    //moving downward direction
    float speedY;

    //gravity of falling object
    float gravity = -9.8f;
    float gAccel;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.Translate(0, 0, Time.deltaTime); //make sure shell moves at constant velocity as Time.delta time will be consistent

        //Let's apply acceleration
        // Acceleration = Force / mass
        acceleration = force / mass;
        speedZ += acceleration * Time.deltaTime; //rate of change in the velocity

        //show compounding affect of addding the acceleration over and ove to the shell/bullet
        //transform.Translate(0, 0, speedZ); //velocity vector

        //Over time we also need to decelerate (or slow down the acceleration)
        /*
            We know mass remains the same, however, we can change the force e.g. (decreasing the force will decelerate)
         */
        //force -= 0.5f; //decrease force over time but it doesn't produce the result as expected

        //eventually the force should become ZERO, we can achieve this by applying speedY also

        //acceleration of falling object with gravity
        gAccel = gravity / mass;

        //Speed on the Y axis
        speedY += gAccel * Time.deltaTime;
        
        transform.Translate(0, speedY, speedZ);

        force = 0;
    }
}
