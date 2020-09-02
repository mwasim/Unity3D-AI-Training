using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// A very simplistic car driving on the x-z plane. 

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    public GameObject fuel;

    private bool autoPilot;
    private float autoSpeed = 0.1f;

    void Start()
    {

    }

    void LateUpdate()
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
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, -rotation);


        //Calculate the distance and print in the debug log
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateDistance();

            //Calculate Dot Product
            CalculateAngleAndFaceFuel();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Toggle auto pilot
            autoPilot = !autoPilot;
        }

        if (autoPilot)
        {
            var distance = CalculateDistance();

            /*
                For example the distance between tank and the fuel is around 32
                When we put 5 here, it means the tank should stop moving as soon as the tank is as close as 5 meters to the fuel
             */
            if (distance > 5)
                StartAutoPilotMode();
        }
    }

    private void StartAutoPilotMode()
    {
        //Rotate the tank to face towards the Fuel
        CalculateAngleAndFaceFuel();

        //tank is moving in the Y direction and facing fuel in the Y direction
        //Vector3.forward is normally used in the 3D environment
        //transform.up (or forward etc.) are already normalized (already of length 1), so we don't need to normalize these
        transform.Translate(transform.up * autoSpeed, Space.World); //make sure we do in the World space, cause how we calculated the angle

        //var distance = CalculateDistance();

        //transform.Translate(Vector3.forward, )
    }

    private void CalculateAngleAndFaceFuel()
    {
        //Facing position of a character is always Z direction (which is Vector3.up)
        var tF = transform.up; //facing position of the tank
        var fD = fuel.transform.position - transform.position; // target position - current position = direction

        //Draw rays for debugging
        //Debug.DrawRay(transform.position, tF, Color.green, 2); //green line is not visible
        Debug.DrawRay(transform.position, tF * 10, Color.green, 2); //green line becomes visible by multiplying tF with 10
        Debug.DrawRay(transform.position, fD, Color.red, 2);

        /*
         * DOT PRODUCT
            We know (normally used for camera of weapons)
            < 90 degrees => v.w > 0 (w object is infront of v object)
            = 90 degrees => v.w = 0 (w object is facing the v object)
            > 90 degrees => v.w < 0 (w object is behind the v object)

            Angle calculation
            Anlge(Theta) = cos-1(v.w/|v||w|) //cost inverse of (dot product of vectors V and W divided by product of lengths of V and W)
         */
        var dot = tF.x * fD.x + tF.y * fD.y; // Dot Product
        var angleInRadians = Mathf.Acos(dot / (tF.magnitude * fD.magnitude));
        var angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        //Debug.Log($"Angle: {angleInRadians}"); //Angle in radians
        Debug.Log($"Angle: {angleInDegrees}"); //Angle in degrees

        var unityAngle = Vector3.Angle(tF, fD);
        Debug.Log($"Angle: {unityAngle}"); //Angle in degrees


        /*
         * CROSS PRODUCT
                We know if
                Cross Product's Z value > 0 (we want to rotate/turn in the Anti Clockwise direction)
                Cross Product's Z value < 0 (we want to rotate/turn in the Clockwise direction)
             */
        //when spacebar is pressed, face the tank exactly towards the fuel
        //We need to rotate in the z axis
        //transform.Rotate(0, 0, angleInDegrees); //This mostly rotates the tank towards fuel but in one quadrant it rotates in the opposite direction

        //To fix the problem, we need the CROSS PRODUCT (Custom approach)
        var clockwise = Cross(tF, fD).z < 0 ? -1 : 1; //determine clockwise or any-clockwise

        //Unity's approach, we used the SignedAngle
        //var unityAngleCrossProduct = Vector3.SignedAngle(tF, fD, Vector3.forward); //Vector3.forward = Z Axix

        //transform.Rotate(0, 0, angleInDegrees * clockwise); //This custom approach also works perfect
        //transform.Rotate(0, 0, unityAngleCrossProduct); //Unity's approach

        //Instead of changing the angle in one go, we can change it incrementally in each Update
        //So, rather than apply the whole angle we're applying the fraction of the angle
        transform.Rotate(0, 0, (angleInDegrees * clockwise) * 0.02f);
    }

    private Vector3 Cross(Vector3 v, Vector3 w)
    {
        var xMult = v.y * w.z - v.z * w.y; //doesn't cross X (Notice)
        var yMult = v.z * w.x - v.x * w.z; //doesn't cross Y
        var zMult = v.x * w.y - v.y * w.x; //doesn't cross Z

        var crossProduct = new Vector3(xMult, yMult, zMult);

        return crossProduct;
    }

    private float CalculateDistance()
    {
        //Calculate using Phythgorean theorem
        //Sqrt(Square(x1 - x2) + Square(y1 - y2)
        Vector3 tP = transform.position; //tank position
        Vector3 fP = fuel.transform.position; //fuel position

        //You can put tp or fP first, it doesn't matter, the order should be same
        //float distance = Mathf.Sqrt(Mathf.Pow(tP.x - fP.x, 2) + Mathf.Pow(tP.y - fP.y, 2));

        //As Unity uses 3 dimensions so, with above code, there can be just a slight difference, to fix that, let's add third dimension also (Z)
        float distance = Mathf.Sqrt(Mathf.Pow(tP.x - fP.x, 2) + Mathf.Pow(tP.y - fP.y, 2) + Mathf.Pow(tP.z - fP.z, 2));

        //Now let's use the Unity's distance calculation method
        float unityDistance = Vector3.Distance(tP, fP);

        //Both distance values are same
        Debug.Log($"Distance: ${distance}");
        Debug.Log($"Unity Distance: ${unityDistance}");

        return distance;
    }
}