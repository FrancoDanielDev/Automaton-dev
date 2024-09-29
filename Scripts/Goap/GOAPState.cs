public class GoapState
{
    public WorldState worldState;

    public GOAPAction generatingAction = null;
    public int step = 0;

    #region CONSTRUCTOR

    public GoapState(GOAPAction gen = null)
    {
        generatingAction = gen;
        worldState = new WorldState() { };
    }

    public GoapState(GoapState source, GOAPAction gen = null)
    {
        worldState = source.worldState.Clone();
        generatingAction = gen;
    }

    #endregion
}
