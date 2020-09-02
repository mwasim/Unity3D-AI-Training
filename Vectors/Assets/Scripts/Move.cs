using UnityEngine;

public class Move : MonoBehaviour
{
    /*
        1. Vector has a). ORIGIN b). DESTINATION c) DIRECTION d) Magnitude / Length

        2. Vector length can be calculated as below
        ||V|| = Sqrt(Squre(X) + SQUARE(Y)) //||V|| = Vector Length

        3. For example, Vector(5,0,4) can have origin (0,0,0) or any other e.g. as set in the scene (-2.58, 0, -2.65)
           And it's direction is from the Origin towards the Destination. In this example, Vector moves from the origin Vector(-2.58, 0, -2.65)
           to the Vector(5,0,4) [In the direction of the destination Vector]        
     */
    public Vector3 goal = new Vector3(5, 0, 4);
    public float speed = 0.1f; //multiply speed with normalized goal to slow down

    void Start()
    {
        //this.transform.Translate(goal);

        //goal = goal * 0.01f; //10 %
    }

    /*
        Any movements you'd like to add to the characters or follow the cameras,
        You should do it when all the Physics has been calculated in the scene,
        Currently, we don't have Physics so it's not an issue

        But you should get into the habit of putting the movement code into the LateUpdate
     */
    private void LateUpdate() 
    {
        //this.transform.Translate(goal);

        //normalize makes the vector standard size 1 in the same direction
        //normalized = Returns the Vector with magnitude = 1
        //this.transform.Translate(goal.normalized * speed);

        //Update calls are normally inconsistent so multiplying with Time.detaTime smooths it.
        this.transform.Translate(goal.normalized * speed * Time.deltaTime);
    }
}
