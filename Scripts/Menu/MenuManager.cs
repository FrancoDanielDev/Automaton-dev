using System.Collections;
using UnityEngine;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private float _resetGameDelay = 2.5f;
    [Space]
    [SerializeField] private SliderValueSelector _coin;
    [SerializeField] private SliderValueSelector _muscularity;
    [SerializeField] private SliderValueSelector _stealth;
    [SerializeField] private EnumValueSelector _weapon;
    [SerializeField] private ToggleValueSelector _key;
    [SerializeField] private ToggleValueSelector _door;
    [Space]
    [SerializeField] private Human _human;
    [SerializeField] private GameObject _menu;

    public void StartGame()
    {
        WorldState wS = new WorldState()
        {
            coins = (int)_coin.value,
            muscularity = _muscularity.value,
            stealth = _stealth.value,
            weapon = _weapon.value,
            hasKey = _key.value,
            doorOpen = _door.value
        };

        _menu.SetActive(false);
        _human.StartGamePlan(wS);
    }

    public void RestartGame()
    {
        StartCoroutine(Process());
        IEnumerator Process()
        {
            yield return new WaitForSeconds(_resetGameDelay);           
            _menu.SetActive(true);
            _human.ResetHuman();

            IReset[] resetObjects = FindObjectsOfType<MonoBehaviour>().OfType<IReset>().ToArray();
            foreach (var obj in resetObjects) obj.Reset();
        }
    }
}
