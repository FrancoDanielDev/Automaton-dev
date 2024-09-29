using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleValueSelector : MonoBehaviour
{
    [HideInInspector] public bool value;

    [SerializeField] private string _false;
    [SerializeField] private string _true;
    [Space]
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MenuManager _menuManager;

    private void Start()
    {
        SendValue();
    }

    public void SendValue()
    {
        value = _slider.value != 0;
        _text.text = !value ? _false : _true;
    }
}
