using UnityEngine;

public class Orc : Item, IReset
{
    [SerializeField] private GameObject _key;

    protected override void ObjectEnding()
    {
        if (_interaction.Name() == new StealKey().Name())
        {
            _key.SetActive(false);
            myGrid.RemoveEntity(this);
            return;
        }

        base.ObjectEnding();
        myGrid.RemoveEntity(this);
        _visualModel.SetActive(false);
    }

    public void Reset()
    {
        _visualModel.SetActive(true);
    }
}
