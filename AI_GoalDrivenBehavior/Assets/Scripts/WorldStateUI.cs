using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WorldStateUI : MonoBehaviour
{
    public Text states;
    
    // Update is called once per frame
    void LateUpdate()
    {
        var worldStates = GWorld.Instance.World.States;
        states.text = string.Empty;

        StringBuilder sb = new StringBuilder();

        foreach (var s in worldStates)
        {
            sb.Append($"{s.Key}, {s.Value}\n");
        }

        states.text = sb.ToString();
    }
}
