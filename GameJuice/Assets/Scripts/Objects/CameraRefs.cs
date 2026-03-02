using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraRefs : MonoBehaviour
{
    public static Dictionary<string, CinemachineCamera> Cameras = new();

    private void Awake()
        => Cameras[name] = GetComponent<CinemachineCamera>();

    private void Start()
        => gameObject.SetActive(false);
}
