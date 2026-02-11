using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance;

    private void Awake()
        => Instance = this as T;
}
