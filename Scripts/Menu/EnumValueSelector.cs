using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnumValueSelector : MonoBehaviour
{
    [HideInInspector] public Weapon value;

    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MenuManager _menuManager;

    private void Start()
    {
        SendValue();
    }

    public void SendValue()
    {
        switch (_slider.value)
        {
            case 0:
                value = Weapon.BareHands;
                _text.text = "X";
                break;

            case 1:
                value = Weapon.Sword;
                _text.text = "Sword";
                break;

            case 2:
                value = Weapon.Knife;
                _text.text = "Knife";
                break;
        }
    }
}
