using UnityEngine;

[ExecuteInEditMode]
public class WayPointDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.gameObject.name != "WayPoint") return;

        RenameWayPoints(null);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMesh>().text = transform.parent.gameObject.name;
    }


    private void RenameWayPoints(GameObject overlook)
    {
        var wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        int index = 1;
        foreach (var item in wayPoints)
        {
            if (item != overlook)
            {
                item.name = $"WP {index:000}";
                index++;
            }
        }
    }

    private void OnDestroy()
    {
        RenameWayPoints(gameObject);
    }
}
