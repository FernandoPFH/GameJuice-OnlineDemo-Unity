using UnityEngine;

public class DefaultsHandler : MonoBehaviour
{
    private void OnEnable()
        => Resources.LoadAll<ScriptableObject>("Defaults");
}
