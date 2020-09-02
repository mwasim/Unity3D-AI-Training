public class GoToCubicleAction : GAction
{
    public override bool PrePerform()
    {
        // Get a free cubicle
        target = inventory.FindItemWithTag("Cubicle");

        // Check that we did indeed get a cubicle
        if (target == null)
            // No cubicle so return false
            return false;

        // All good
        return true;
    }

    public override bool PostPerform()
    {
        // Add a new state "TreatingPatient"
        GWorld.Instance.World.ModifyState("TreatingPatient", 1);

        // Give back the cubicle
        GWorld.Instance.AddCubicle(target);

        // Remove the cubicle from the list
        inventory.RemoveItem(target);

        // Give the cubicle back to the world
        GWorld.Instance.World.ModifyState("FreeCubicle", 1);

        return true;
    }
}
