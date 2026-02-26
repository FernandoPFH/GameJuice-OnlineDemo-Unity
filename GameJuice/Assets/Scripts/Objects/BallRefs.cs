using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallRefs : Singleton<BallRefs>
{
    public SpriteRenderer Renderer;
    public Light2D SourceLight;
    public TrailRenderer Trail;
    public AudioSource Audio;
    public GameObject FireTrail;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        SourceLight = transform.Find("Light").GetComponent<Light2D>();
        Trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        Audio = transform.Find("Audio").GetComponent<AudioSource>();
        FireTrail = transform.Find("FireTrail").gameObject;
    }
#endif
}
