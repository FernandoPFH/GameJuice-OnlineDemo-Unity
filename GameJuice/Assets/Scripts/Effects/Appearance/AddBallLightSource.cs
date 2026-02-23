using UnityEngine;

[CreateAssetMenu(fileName = "AddBallLightSource_EffectSO", menuName = "EffectSO/AddBallLightSource")]
public class AddBallLightSource_EffectSO : EffectSO
{
    private void SetBallLight()
    {
        BallRefs.Instance.SourceLight.enabled = true;

        BallRefs.Instance.SourceLight.color = BallRefs.Instance.Renderer.color;
    }

    private void ResetBallLight()
        => BallRefs.Instance.SourceLight.enabled = false;

    public override void OnEnabled()
        => SetBallLight();

    public override void OnDisabled()
        => ResetBallLight();
}
