using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject shellPrefab;
    public GameObject shellSpawnPos;

    /*
        PROJECTILE MOTION
        Read more here https://en.wikipedia.org/wiki/Projectile_motion
     */
    //we need a target to project to
    public GameObject target; //target is the enemy here
    public GameObject parent; //parent object is the green tank
    float speed = 15; //projectile speed
    float turnSpeed = 2f;

    //Fix the shoot
    bool canShoot = true;

    //Low or Hight angle can be set via the inspect also
    public bool useLowAngle = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Fire()
    {
        if (!canShoot) return;

        /*
            Shell position and rotation will be same as Tank's Turret
         */
        var shell = Instantiate(shellPrefab, shellSpawnPos.transform.position, shellSpawnPos.transform.rotation);

        //set shell's velocity to ensure shell fired has a velocity
        shell.GetComponent<Rigidbody>().velocity = speed * transform.forward;

        //after shot once, set canShoot to false
        canShoot = false;

        //But tank should be able to shoot again after specific interval
        Invoke(nameof(CanShootAgain), 0.2f); //after a fraction of second
    }

    private void CanShootAgain()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Fire();
        //}        

        //Put a slerp to look at the enemy
        var direction = (target.transform.position - parent.transform.position).normalized; //we determine direction by subtracting current position from the target position

        //Find out more here https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
        /*
            Creates a rotation with the specified forward and upwards directions.
         */
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //set the parent's (tank's) rotation in the direction of the target
        //Find more info on Slerp here https://docs.unity3d.com/ScriptReference/Quaternion.Slerp.html
        //Slerp interpolates between quaternions a and b by ratio t. The parameter t is clamped to the range [0, 1].
        /*
            Use this to create a rotation which smoothly interpolates between the first quaternion a to the second quaternion b,
            based on the value of the parameter t. If the value of the parameter is close to 0, the output will be close to a,
            if it is close to 1, the output will be close to b.
         */
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);


        //Let's rotate the Tank's turret
        var angle = RotateTurret();

        //Instead of firing on pressing the space bar, do it automatically after calculating the angle
        if (angle != null && Vector3.Angle(direction, parent.transform.forward) < 10) //fire fi the angle is less than 10 degrees
            Fire();
    }

    //In this method, we'll rotate the Turret
    float? RotateTurret()
    {
        var angle = CalculateAngle(low: useLowAngle);

        //if we've a valid angle, rotate the Turret
        if (angle != null)
        {
            /*
                Here we're using the EulerAngles and not Quaternion
                The EulerAngles allows us to set the rotation around the required axis
                As we're rotating the turret only in the X-axis, so
             */
            const float initialTurretAngle = 360f;
            transform.localEulerAngles = new Vector3(initialTurretAngle - (float)angle, 0f, 0f);
        }

        return angle;
    }


    //Let's calculate the Angle for projectile motion
    float? CalculateAngle(bool low)
    {
        /*
            We're using the equation to determine the Angle required to hit coordinate (x,y)
            More info here https://en.wikipedia.org/wiki/Projectile_motion
         */
        var targetDirection = target.transform.position - transform.position;

        var y = targetDirection.y;
        targetDirection.y = 0f;

        //distance betwen one tank and the othere
        var x = targetDirection.magnitude;
        float gravity = 9.81f; //we don't need negative here because in the equation it's already assumed negative
        float sSqr = Mathf.Pow(speed, 2);
        float underTheSqrRoot = (Mathf.Pow(sSqr, 2) - gravity * (gravity * Mathf.Pow(x, 2) + 2 * y * sSqr));

        if (underTheSqrRoot >= 0f) //if it's negative, it'll be an imaginary number, which we don't want
        {
            var root = Mathf.Sqrt(underTheSqrRoot);

            var highAngle = sSqr + root;
            var lowAngle = sSqr - root;

            if (low) return Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg; //we need it in degrees
            else return Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg;
        }

        //if above code didn't return the value, return null
        return null;
    }
}
