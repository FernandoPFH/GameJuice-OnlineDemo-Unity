using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnExplosionOnHit_EffectSO", menuName = "EffectSO/SpawnExplosionOnHit")]
public class SpawnExplosionOnHit : EffectSO
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float scaleMultiplier = 0.5f;

    private List<ParticleSystem> createdVFXs = new();

    public void OnScaleMultiplierChanged(float mult)
        => scaleMultiplier = mult;

#if UNITY_EDITOR
    protected override void InitValues()
        => createdVFXs.Clear();
#endif

    private void OnDeathWallHit(GameObject wall, Vector3 ballPosition)
    {
        ParticleSystem explosion = createdVFXs.Where(x => !x.isPlaying).FirstOrDefault();

        if (explosion is null)
        {
            explosion = Instantiate(explosionPrefab, ballPosition, Quaternion.identity).GetComponent<ParticleSystem>();
            createdVFXs.Add(explosion);
        }

        explosion.transform.position = ballPosition;
        explosion.transform.localScale = Vector3.one * scaleMultiplier;
        explosion.Play();
    }

    public override void OnEnabled()
        => DeathWall.OnHit += OnDeathWallHit;

    public override void OnDisabled()
        => DeathWall.OnHit -= OnDeathWallHit;
}
