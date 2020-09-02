public class GoToWaitingRoomAction : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        // Inject waiting state to world states
        GWorld.Instance.World.ModifyState("Waiting", 1);

        // Patient adds himself to the queue
        GWorld.Instance.AddPatient(this.gameObject);

        // Inject a state into the agents beliefs
        beliefs.ModifyState("atHospital", 1);

        return true;
    }
}
