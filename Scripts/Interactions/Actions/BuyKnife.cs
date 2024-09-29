public class BuyKnife : IAction
{
    public string Name()
    {
        return HumanAction.BuyKnife.ToString();
    }

    public float Cost()
    {
        return 5f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.coins >= 2 &&
               gS.worldState.weapon != Weapon.Knife;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.coins -= 2;
        wS.weapon = Weapon.Knife;
        return wS;
    }
}
