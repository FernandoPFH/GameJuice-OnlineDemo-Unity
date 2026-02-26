using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : Singleton<Background>
{
    private SpriteRenderer renderer;

    private void Start()
        => renderer = GetComponent<SpriteRenderer>();

    public Color GetColor()
        => renderer.color;

    public void SetColor(Color color)
        => renderer.color = color;
}
