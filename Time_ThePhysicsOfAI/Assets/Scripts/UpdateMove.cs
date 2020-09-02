using UnityEngine;

public class UpdateMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Apply the same Translate (moving in Z direction) to all scripts Update, LateUpdate and FixedUpdate
         * And we see the Physics don't work properly, the objects with Update and LateUpdate move faster depending upon the speed of the 
         * System's processor, and FixedUpdate don't keeep up.
         */
        //transform.Translate(0, 0, 0.01f);

        //We can change it to use Tim.deltaTime to use the realworl clock (m/s) instead of move based on the Update frame rate
        transform.Translate(0, 0, Time.deltaTime);

        //We can apply speed by multiplying speed with deltaTime
        //Fo example, applying the double speed, this object will move with double speed
        //transform.Translate(0, 0, 2.0f * Time.deltaTime);
    }
}
