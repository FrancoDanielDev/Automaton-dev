using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValueSelector : MonoBehaviour
{
    [HideInInspector] public float value;

    [SerializeField] private bool _integer;
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
        value = _slider.value;
        _text.text = value.ToString(_integer ? "F0" : "F1");
    }
}
