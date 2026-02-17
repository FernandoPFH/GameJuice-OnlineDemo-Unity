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
        InitValue<Color>(ref lastBlockColor, blockColor);
        InitValue<Color>(ref lastWallColor, wallColor);
        InitValue<Color>(ref lastBarColor, barColor);
        InitValue<Color>(ref lastBallColor, ballColor);
        InitValue<Color>(ref lastBackgroundColor, backgroundColor);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged<Color>(ref lastBlockColor, blockColor, OnBlockColorChanged);
        CheckValueChanged<Color>(ref lastWallColor, wallColor, OnWallColorChanged);
        CheckValueChanged<Color>(ref lastBarColor, barColor, OnBarColorChanged);
        CheckValueChanged<Color>(ref lastBallColor, ballColor, OnBallColorChanged);
        CheckValueChanged<Color>(ref lastBackgroundColor, backgroundColor, OnBackgroundColorChanged);
    }
#endif

    private void OnBlockColorChanged(Color color)
    {

        blockColor = color;
        if (isEnabled)
            SetBlockColor();
    }

    private void OnWallColorChanged(Color color)
    {

        wallColor = color;
        if (isEnabled)
            SetWallColor();
    }

    private void OnBarColorChanged(Color color)
    {

        barColor = color;
        if (isEnabled)
            SetBarColor();
    }

    private void OnBallColorChanged(Color color)
    {

        ballColor = color;
        if (isEnabled)
            SetBallColor();
    }

    private void OnBackgroundColorChanged(Color color)
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
       => Bar.Instance.GetComponent<SpriteRenderer>().color = barColor;

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
       => Bar.Instance.GetComponent<SpriteRenderer>().color = AppearanceDefaults.Instance.SpriteColor;

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
