public class KillOrc : IAction
{
    public string Name()
    {
        return HumanAction.KillOrc.ToString();
    }

    public float Cost()
    {
        return 2f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.muscularity >= 7.5f && gS.worldState.weapon == Weapon.Sword ||
               gS.worldState.weapon == Weapon.Knife;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.hasKey = true;
        return wS;
    }
}
