public class OpenChest : IAction
{
    public string Name()
    {
        return HumanAction.OpenChest.ToString();
    }

    public float Cost()
    {
        return 1f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.hasKey &&
               gS.worldState.doorOpen;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.coins = 999;
        wS.hasKey = false;
        return wS;
    }
}
