public class Door : Item, IReset
{
    protected override void ObjectEnding()
    {
        base.ObjectEnding();
        _visualModel.SetActive(false);
    }

    public void OpenTheDoor()
    {
        _visualModel.SetActive(false);
    }

    public void Reset()
    {
        _visualModel.SetActive(true);
    }
}
