using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : Singleton<Background>
{
    private SpriteRenderer renderer;

    private void Start()
        => renderer = GetComponent<SpriteRenderer>();

    public void SetColor(Color color)
        => renderer.color = color;
}
