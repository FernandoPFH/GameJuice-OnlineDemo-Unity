using UnityEngine;

[CreateAssetMenu(fileName = "AddColor_EffectSO", menuName = "EffectSO/AddColor")]
public class AddColor_EffectSO : EffectSO
{
    [SerializeField] private Color blockColor = Color.grey;
    [SerializeField] private Color wallColor = Color.grey;
    [SerializeField] private Color barColor = Color.grey;
    [SerializeField] private Color ballColor = Color.grey;
    [SerializeField] private Color backgroundColor = Color.darkGray;

#if UNITY_EDITOR
    private Color lastBlockColor;
    private Color lastWallColor;
    private Color lastBarColor;
    private Color lastBallColor;
    private Color lastBackgroundColor;

    protected override void InitValues()
    {
        InitValue(ref lastBlockColor, blockColor);
        InitValue(ref lastWallColor, wallColor);
        InitValue(ref lastBarColor, barColor);
        InitValue(ref lastBallColor, ballColor);
        InitValue(ref lastBackgroundColor, backgroundColor);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged(ref lastBlockColor, blockColor, OnBlockColorChanged);
        CheckValueChanged(ref lastWallColor, wallColor, OnWallColorChanged);
        CheckValueChanged(ref lastBarColor, barColor, OnBarColorChanged);
        CheckValueChanged(ref lastBallColor, ballColor, OnBallColorChanged);
        CheckValueChanged(ref lastBackgroundColor, backgroundColor, OnBackgroundColorChanged);
    }
#endif

    public void OnBlockColorChanged(Color color)
    {

        blockColor = color;
        if (isEnabled)
            SetBlockColor();
    }

    public void OnWallColorChanged(Color color)
    {

        wallColor = color;
        if (isEnabled)
            SetWallColor();
    }

    public void OnBarColorChanged(Color color)
    {

        barColor = color;
        if (isEnabled)
            SetBarColor();
    }

    public void OnBallColorChanged(Color color)
    {

        ballColor = color;
        if (isEnabled)
            SetBallColor();
    }

    public void OnBackgroundColorChanged(Color color)
    {

        backgroundColor = color;
        if (isEnabled)
            SetBackgroundColor();
    }

    private void SetBlockColor()
    {
        foreach (Block block in Block.Instances)
            block.GetComponent<SpriteRenderer>().color = blockColor;
    }

    private void SetWallColor()
    {
        foreach (Wall wall in Wall.Instances)
            wall.GetComponent<SpriteRenderer>().color = wallColor;
    }

    private void SetBarColor()
       => BarRefs.Instance.Renderer.color = barColor;

    private void SetBallColor()
       => BallRefs.Instance.Renderer.color = ballColor;

    private void SetBackgroundColor()
       => Background.Instance.SetColor(backgroundColor);

    private void ResetBlockColor()
    {
        foreach (Block block in Block.Instances)
            block.GetComponent<SpriteRenderer>().color = AppearanceDefaults.Instance.SpriteColor;
    }

    private void ResetWallColor()
    {
        foreach (Wall wall in Wall.Instances)
            wall.GetComponent<SpriteRenderer>().color = AppearanceDefaults.Instance.SpriteColor;
    }

    private void ResetBarColor()
       => BarRefs.Instance.Renderer.color = AppearanceDefaults.Instance.SpriteColor;

    private void ResetBallColor()
       => BallRefs.Instance.Renderer.color = AppearanceDefaults.Instance.SpriteColor;

    private void ResetBackgroundColor()
       => Background.Instance.SetColor(AppearanceDefaults.Instance.BackgroundColor);

    public override void OnEnabled()
    {
        SetBlockColor();
        SetWallColor();
        SetBarColor();
        SetBallColor();
        SetBackgroundColor();
    }

    public override void OnDisabled()
    {
        ResetBlockColor();
        ResetWallColor();
        ResetBarColor();
        ResetBallColor();
        ResetBackgroundColor();
    }
}
