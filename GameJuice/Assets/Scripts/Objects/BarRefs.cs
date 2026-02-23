using UnityEngine;

public class BarRefs : Singleton<BarRefs>
{
    public SpriteRenderer Renderer;
    public Rigidbody2D Rigidbody;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }
#endif
}
