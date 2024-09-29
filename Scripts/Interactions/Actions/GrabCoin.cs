public class GrabCoin : IAction
{
    public string Name()
    {
        return HumanAction.GrabCoin.ToString();
    }

    public float Cost()
    {
        return 1f;
    }

    public bool Precondition(GoapState gS)
    {
        return gS.worldState.coins < 100;
    }

    public WorldState Effect(WorldState wS)
    {
        wS.coins++;
        return wS;
    }
}
