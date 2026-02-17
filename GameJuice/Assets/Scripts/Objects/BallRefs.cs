using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BallRefs : Singleton<BallRefs>
{
    public SpriteRenderer Renderer;
    public Light2D SourceLight;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer = GetComponent<SpriteRenderer>();
        SourceLight = transform.Find("Light").GetComponent<Light2D>();
    }
#endif
}
