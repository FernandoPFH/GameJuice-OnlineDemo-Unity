using UnityEngine;
using Unity.Cinemachine;

[CreateAssetMenu(fileName = "ZoomInLastBlock_EffectSO", menuName = "EffectSO/Finale/ZoomInLastBlock")]
public class ZoomInLastBlock : EffectSO
{
    [SerializeField] private string cameraName;
    [SerializeField] private float Fov = 34f;

    public float FOV => Fov;

#if UNITY_EDITORa
    private float lastFOV;

    protected override void InitValues()
        => InitValue(ref lastFOV, FOV);

    protected override void CheckValuesChanged()
        => CheckValueChanged(ref lastFOV, FOV, OnFOVChanged);
#endif

    public void OnFOVChanged(float fov)
    {
        Fov = fov;
        if (isEnabled && IsTimeToZoom())
            SetupCamera();
    }

    private bool IsTimeToZoom()
    {
        if (!Ball.Instance.BallTrajectoryPredic.NextContacts.TryPeek(out NextContact nContact))
            return false;

        if (Block.Count == 1 && nContact.type == "Block")
            return true;

        return false;
    }

    private void SetupCamera()
    {
        CinemachineCamera camera = CameraRefs.Cameras[cameraName];

        camera.gameObject.SetActive(true);
        camera.Lens.FieldOfView = Fov;
    }

    private void OnBallHit(string otherTag, Vector2 point, Vector2 normal)
    {
        if (IsTimeToZoom())
            SetupCamera();
    }

    public override void OnEnabled()
    {
        Ball.OnHit += OnBallHit;

        if (IsTimeToZoom())
            SetupCamera();
    }

    public override void OnDisabled()
    {
        Ball.OnHit -= OnBallHit;

        CameraRefs.Cameras[cameraName].gameObject.SetActive(false);
    }
}
