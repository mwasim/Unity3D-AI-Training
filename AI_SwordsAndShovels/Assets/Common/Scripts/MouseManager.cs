using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer; // normal mouse pointer
    public Texture2D target; // target mouse pointer
    public Texture2D doorway; // doorway mouse pointer

    public EventVector3 OnClickEnvironment; 

    void Update()
    {
        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
        {
            bool door = false;
            if (hit.collider.gameObject.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }
            else
            {
                Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
            }


            //detect the mouse click
            if (Input.GetMouseButtonDown(0))
            {
                if (door) //if it's door way
                {
                    //we need transform to use door way position
                    var doorway = hit.collider.gameObject.transform;

                    //add forward vector and 10 units to it
                    //It starts with the transform position and adding a vector based on the doorway forward axis, in this way chracter goes right through it (without standing within the doorway)
                    OnClickEnvironment?.Invoke(doorway.position + doorway.forward * 10);
                }
                else //normal behavior when not clicked on the doorway
                {
                    //invoke event and pass hit point
                    OnClickEnvironment?.Invoke(hit.point);
                }                
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
}

[System.Serializable]
public class EventVector3: UnityEvent<Vector3> //ability to send vector3 info
{

}