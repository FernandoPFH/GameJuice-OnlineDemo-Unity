using UnityEngine;

public class EffectUI : MonoBehaviour
{
    public void OpenMenu()
        => gameObject.SetActive(true);

    public void CloseMenu()
        => gameObject.SetActive(false);

    public void EnableAll()
        => SetStates(true);

    public void DisableAll()
        => SetStates(false);

    private void SetStates(bool state)
    {
        foreach (BooleanHandler handler in BooleanHandler.Enablers)
        {
            handler.toggleField.isOn = state;
            handler.OnValueChanged(state);
        }
    }
}
