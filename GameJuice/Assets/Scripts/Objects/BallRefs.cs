using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallRefs : Singleton<BallRefs>
{
    public SpriteRenderer Renderer;
    public Light2D SourceLight;
    public Rigidbody2D Rigidbody;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        SourceLight = transform.Find("Light").GetComponent<Light2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }
#endif
}
