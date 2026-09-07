using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CameraRefs : MonoBehaviour
{
    [SerializeField] private bool shouldStartEnabled = false;
    public static Dictionary<string, CinemachineCamera> Cameras = new();

    private void Awake()
        => Cameras[name] = GetComponent<CinemachineCamera>();

    private void Start()
        => gameObject.SetActive(shouldStartEnabled);
}
