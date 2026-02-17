using UnityEngine;

public class DefaultsHandler : MonoBehaviour
{
    private void Awake()
        => Resources.LoadAll<ScriptableObject>("Defaults");
}
