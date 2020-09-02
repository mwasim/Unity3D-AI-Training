using UnityEngine;

public class LateUpdateMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*
         * Apply the same Translate (moving in Z direction) to all scripts Update, LateUpdate and FixedUpdate
         * And we see the Physics don't work properly, the objects with Update and LateUpdate move faster depending upon the speed of the 
         * System's processor, and FixedUpdate don't keeep up.
         */
        //transform.Translate(0, 0, 0.01f);


        //We can change it to use Tim.deltaTime to use the realworl clock (m/s) instead of move based on the Update frame rate
        transform.Translate(0, 0, Time.deltaTime);
    }
}
