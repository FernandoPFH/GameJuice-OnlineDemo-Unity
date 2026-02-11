using UnityEngine;
using TMPro;

public class LifeUI : Singleton<LifeUI>
{
    [SerializeField] private TextMeshProUGUI textMesh;

    public void UpdateText(int newValue)
        => textMesh.text = newValue.ToString();
}
