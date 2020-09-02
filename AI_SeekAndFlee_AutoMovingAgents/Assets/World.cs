using UnityEngine;

public sealed class World
{
    private static GameObject[] hidingSpots = GameObject.FindGameObjectsWithTag("hide");

    private World()
    {

    }

    public GameObject[] HidingSpots => hidingSpots;

    public static World Instance { get; } = new World();
}
