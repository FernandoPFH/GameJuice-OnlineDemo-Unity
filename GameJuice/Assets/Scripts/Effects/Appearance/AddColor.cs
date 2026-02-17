using UnityEngine;

[CreateAssetMenu(fileName = "AddColor_EffectSO", menuName = "EffectSO/AddColor")]
public class AddColor_EffectSO : EffectSO
{
    [SerializeField] private Color blockColor = Color.grey;
    [SerializeField] private Color wallColor = Color.grey;
    [SerializeField] private Color barColor = Color.grey;
    [SerializeField] private Color ballColor = Color.grey;
    [SerializeField] private Color cameraBackgroundColor = Color.darkGray;

#if UNITY_EDITOR
    private Color lastBlockColor;
    private Color lastWallColor;
    private Color lastBarColor;
    private Color lastBallColor;
    private Color lastCameraBackgroundColor;

    protected override void InitValues()
    {
        InitValue<Color>(ref lastBlockColor, blockColor);
        InitValue<Color>(ref lastWallColor, wallColor);
        InitValue<Color>(ref lastBarColor, barColor);
        InitValue<Color>(ref lastBallColor, ballColor);
        InitValue<Color>(ref lastCameraBackgroundColor, blockColor);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged<Color>(ref lastBlockColor, blockColor, OnBlockColorChanged);
        CheckValueChanged<Color>(ref lastWallColor, wallColor, OnWallColorChanged);
        CheckValueChanged<Color>(ref lastBarColor, barColor, OnBarColorChanged);
        CheckValueChanged<Color>(ref lastBallColor, ballColor, OnBallColorChanged);
        CheckValueChanged<Color>(ref lastCameraBackgroundColor, blockColor, OnCameraBackgroundColorChanged);
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

    private void OnCameraBackgroundColorChanged(Color color)
    {

        cameraBackgroundColor = color;
        if (isEnabled)
            SetCameraBackgroundColor();
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
       => Ball.Instance.GetComponent<SpriteRenderer>().color = ballColor;

    private void SetCameraBackgroundColor()
       => Camera.main.backgroundColor = cameraBackgroundColor;

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
       => Ball.Instance.GetComponent<SpriteRenderer>().color = AppearanceDefaults.Instance.SpriteColor;

    private void ResetCameraBackgroundColor()
       => Camera.main.backgroundColor = AppearanceDefaults.Instance.CameraBackgroundColor;

    public override void OnEnabled()
    {
        SetBlockColor();
        SetWallColor();
        SetBarColor();
        SetBallColor();
        SetCameraBackgroundColor();
    }

    public override void OnDisabled()
    {
        ResetBlockColor();
        ResetWallColor();
        ResetBarColor();
        ResetBallColor();
        ResetCameraBackgroundColor();
    }
}
