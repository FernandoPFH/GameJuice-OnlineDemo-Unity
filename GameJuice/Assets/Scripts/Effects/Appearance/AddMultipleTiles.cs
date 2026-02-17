using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "AddMultipleTiles_EffectSO", menuName = "EffectSO/AddMultipleTiles")]
public class AddMultipleTiles_EffectSO : EffectSO
{
    [SerializeField]
    private List<SpriteChance> spriteChances;

    [Serializable]
    protected struct SpriteChance
    {
        public Sprite sprite;
        [Range(0, 1)]
        public float chance;
    }

#if UNITY_EDITOR
    private List<SpriteChance> lastSpriteChances;

    protected void InitValue(ref List<SpriteChance> lastValue, List<SpriteChance> currentValue)
        => lastValue = currentValue.ToList();

    protected override void InitValues()
        => InitValue(ref lastSpriteChances, spriteChances);

    protected void CheckValueChanged(ref List<SpriteChance> lastValue, List<SpriteChance> currentValue, Action<List<SpriteChance>> onValueChanged)
    {
        if (lastValue.SequenceEqual(currentValue))
            return;

        InitValue(ref lastValue, currentValue);
        onValueChanged(currentValue);
    }

    protected override void CheckValuesChanged()
        => CheckValueChanged(ref lastSpriteChances, spriteChances, OnTilesChanges);
#endif

    private void OnTilesChanges(List<SpriteChance> spriteChances)
    {
        this.spriteChances = spriteChances;
        if (isEnabled)
            SetRandomTiles();
    }

    private void SetRandomTiles()
    {
        List<SpriteChance> sortedSpriteChances = spriteChances.OrderBy(x => x.chance).ToList();

        foreach (Block block in Block.Instances)
        {
            if (!block.gameObject.activeInHierarchy)
                continue;

            SpriteChance selectedSprite = sortedSpriteChances.FirstOrDefault(x => x.chance >= Random.Range(0f, 1f));
            if (block.TryGetComponent(out SpriteRenderer renderer))
            {
                renderer.sprite = selectedSprite.sprite ? selectedSprite.sprite : AppearanceDefaults.Instance.BlockSprite;
                renderer.size = Vector2.one;
            }
        }
    }

    private void ResetRandomTiles()
    {
        foreach (Block block in Block.Instances)
            if (block.TryGetComponent(out SpriteRenderer renderer))
            {
                renderer.sprite = AppearanceDefaults.Instance.BlockSprite;
                renderer.size = Vector2.one;
            }
    }

    public override void OnEnabled()
        => SetRandomTiles();

    public override void OnDisabled()
        => ResetRandomTiles();
}
