using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnDustOnHit_EffectSO", menuName = "EffectSO/SpawnDustOnHit")]
public class SpawnDustOnHit : EffectSO
{
    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private float scaleMultiplier = 0.5f;

    private List<ParticleSystem> createdVFXs = new();

#if UNITY_EDITOR
    protected override void InitValues()
        => createdVFXs.Clear();
#endif

    private void OnBallHit(string otherTag, Vector2 point, Vector2 normal)
    {
        float angle = Vector2.SignedAngle(normal, Vector2.up);

        ParticleSystem dust = createdVFXs.Where(x => !x.isPlaying).FirstOrDefault();

        if (dust is null)
        {
            dust = Instantiate(dustPrefab, point, Quaternion.identity).GetComponent<ParticleSystem>();
            createdVFXs.Add(dust);
        }

        dust.transform.SetPositionAndRotation(point, Quaternion.Euler(Vector3.forward * angle));
        dust.transform.localScale = Vector3.one * scaleMultiplier;
        dust.Play();
    }

    public override void OnEnabled()
        => Ball.OnHit += OnBallHit;

    public override void OnDisabled()
        => Ball.OnHit -= OnBallHit;
}
