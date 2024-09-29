public interface IAction
{
    string Name();

    float Cost();

    bool Precondition(GoapState gS);

    WorldState Effect(WorldState gS);
}
