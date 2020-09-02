using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShell : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0, 0, speed * Time.deltaTime); //moves in Z direction with constant velocity

        //We'd like the object to move diagonally between y and z axix, so we can simply apply speed to both axis
        //Because rate of change in y and z axis values are the same
        //transform.Translate(0, speed * Time.deltaTime, speed * Time.deltaTime);

        //applying half the rate along y axis
        transform.Translate(0, (speed * Time.deltaTime) / 2.0f, speed * Time.deltaTime);
    }
}
