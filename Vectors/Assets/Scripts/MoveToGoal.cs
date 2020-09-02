using UnityEngine;

public class MoveToGoal : MonoBehaviour
{

    public Transform goal;
    public float speed = 2.0f;
    public float accuracy = 0.01f;

    private void Start()
    {
        //Character first turns and looks at where it's about to go using LookAt
        //Give it the goal position, you want it to look at
        //It'll snap around and look in that direction
        //transform.LookAt(goal.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //Putting LookAt on LateUpdate makes the character Look at the goal every time goal position's change
        //For example, within the SceneView (while the game is running), try changing the Goals position by dragging the cube,
        //and the characters will follow it by first looking at it.
        transform.LookAt(goal.position);

        //Determine direction by subtracting current position from target position
        Vector3 direction = goal.position - transform.position;

        //This ray is displayed on within the Scene (not Game view)
        //Useful for debugging
        Debug.DrawRay(transform.position, direction, Color.red);

        //Magnitude is the length of th vector
        if (direction.magnitude > accuracy)
        {
            //The below code line will not work properly
            //transform.Translate(direction.normalized * speed * Time.deltaTime);

            //To fix or make it work you'd like to move the character in the World Space
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
