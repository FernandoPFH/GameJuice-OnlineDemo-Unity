using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : Singleton<Background>
{
    public AudioSource AudioSource;

    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        AudioSource = GetComponent<AudioSource>();
    }

    public Color GetColor()
        => renderer.color;

    public void SetColor(Color color)
        => renderer.color = color;
}
