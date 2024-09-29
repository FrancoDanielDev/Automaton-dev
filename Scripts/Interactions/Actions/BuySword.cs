public class BuySword : IAction
{
    public string Name()
    {
        return HumanAction.BuySword.ToString();
    }

    public float Cost()
    {
        return 2f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.coins >= 2 &&
               gS.worldState.weapon != Weapon.Sword;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.coins -= 2;
        wS.weapon = Weapon.Sword;
        return wS;
    }
}
