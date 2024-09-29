public class OpenDoor : IAction
{
    public string Name()
    {
        return HumanAction.OpenDoor.ToString();
    }

    public float Cost()
    {
        return 2f;
    }

    public bool Precondition(GoapState gS)
    {
        return !gS.worldState.doorOpen &&
                gS.worldState.hasKey;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.doorOpen = true;
        wS.hasKey = false;
        return wS;
    }
}
