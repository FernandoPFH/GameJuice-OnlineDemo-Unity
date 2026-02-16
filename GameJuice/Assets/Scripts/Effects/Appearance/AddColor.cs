using UnityEngine;
using System;
using System.Collections.Generic;

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

    protected override List<(object, object, Action<object>)> valuesToCheck => new() {
            (lastBlockColor, blockColor, (obj) => OnBlockColorChanged((Color)obj)),
            (lastWallColor, wallColor, (obj) => OnWallColorChanged((Color)obj)),
            (lastBarColor, barColor, (obj) => OnBarColorChanged((Color)obj)),
            (lastBallColor, ballColor, (obj) => OnBallColorChanged((Color)obj)),
            (lastCameraBackgroundColor, cameraBackgroundColor, (obj) => OnCameraBackgroundColorChanged((Color)obj)),
    };
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
