using UnityEngine;
using TMPro;

using FernandoPFH_Essentials_Runtime;

public class LifeUI : Singleton<LifeUI>
{
    [SerializeField] private TextMeshProUGUI textMesh;

    public void UpdateText(int newValue)
        => textMesh.text = newValue.ToString();
}
