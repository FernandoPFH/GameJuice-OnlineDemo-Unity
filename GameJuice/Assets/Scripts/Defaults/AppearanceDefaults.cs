using UnityEngine;

[CreateAssetMenu(fileName = "Appearance_DefaultsSO", menuName = "Defaults/Appearance")]
public class AppearanceDefaults : DefaultsSO<AppearanceDefaults>
{
    public Color SpriteColor = Color.white;
    public Color CameraBackgroundColor = Color.darkGray;
    public Sprite BlockSprite;
}
