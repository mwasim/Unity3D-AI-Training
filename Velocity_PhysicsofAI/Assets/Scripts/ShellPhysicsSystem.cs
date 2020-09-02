using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ShellPhysicsSystem - This script is using Physics Engline, and enabled in the scene on the Shell
 */
public class ShellPhysicsSystem : MonoBehaviour
{
    public GameObject explosion;
    Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*
            Since, the shell has a velocity (with direction)
            So, we can use it to set the direction of the shell
         */
        transform.forward = rb.velocity;   
    }
}
