public class StealKey : IAction
{
    public string Name()
    {
        return HumanAction.StealKey.ToString();
    }

    public float Cost()
    {
        return 2f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.weapon == Weapon.BareHands &&
               gS.worldState.stealth >= 0.5f;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.hasKey = true;
        return wS;
    }
}
