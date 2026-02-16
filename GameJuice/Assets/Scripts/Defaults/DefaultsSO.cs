using UnityEngine;

public abstract class DefaultsSO<T> : ScriptableObject where T : DefaultsSO<T>
{
    public static T Instance { get; private set; }

    private void Awake()
        => Instance = this as T;
}
