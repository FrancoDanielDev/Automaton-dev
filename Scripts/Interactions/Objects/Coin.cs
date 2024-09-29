public class Coin : Item, IReset
{
    protected override void ObjectEnding()
    {
        base.ObjectEnding();
        myGrid.RemoveEntity(this);
        _visualModel.SetActive(false);
    }

    public void Reset()
    {
        _visualModel.SetActive(true);
    }
}
