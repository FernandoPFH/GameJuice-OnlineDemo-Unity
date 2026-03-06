using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "AddBackgroundShapes_EffectSO", menuName = "EffectSO/Appearance/AddBackgroundShapes")]
public class AddBackgroundShapes_EffectSO : EffectSO
{
    [SerializeField] private List<GameObject> shapes;

    [SerializeField] private Color shapesColor = Color.black;

    [SerializeField] private float minSpawnAreaDiameter = 1f;
    [SerializeField] private float maxSpawnAreaDiameter = 3f;

    [SerializeField] private float minSpawnScale = 0.05f;
    [SerializeField] private float maxSpawnScale = 0.2f;

    [SerializeField] private int numOfShapesToSpawn = 5;

    public Color ShapesColor => shapesColor;

    public float MinSpawnAreaDiameter => minSpawnAreaDiameter;
    public float MaxSpawnAreaDiameter => maxSpawnAreaDiameter;

    public float MinSpawnScale => minSpawnScale;
    public float MaxSpawnScale => maxSpawnScale;

    public int NumOfShapesToSpawn => numOfShapesToSpawn;

    private float initialAngle = 0f;

    private List<(GameObject, Vector3)> instanciatedShapes = new();

#if UNITY_EDITOR
    private Color lastShapesColor;

    private float lastMinSpawnAreaDiameter;
    private float lastMaxSpawnAreaDiameter;

    private float lastMinSpawnScale;
    private float lastMaxSpawnScale;

    private int lastNumOfShapesToSpawn;

    protected override void InitValues()
    {
        InitValue(ref lastShapesColor, shapesColor);
        InitValue(ref lastMinSpawnAreaDiameter, minSpawnAreaDiameter);
        InitValue(ref lastMaxSpawnAreaDiameter, maxSpawnAreaDiameter);
        InitValue(ref lastMinSpawnScale, minSpawnScale);
        InitValue(ref lastMaxSpawnScale, maxSpawnScale);
        InitValue(ref lastNumOfShapesToSpawn, numOfShapesToSpawn);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged(ref lastShapesColor, shapesColor, OnShapesColorChanged);
        CheckValueChanged(ref lastMinSpawnAreaDiameter, minSpawnAreaDiameter, OnMinSpawnAreaDiameterChanged);
        CheckValueChanged(ref lastMaxSpawnAreaDiameter, maxSpawnAreaDiameter, OnMaxSpawnAreaDiameterChanged);
        CheckValueChanged(ref lastMinSpawnScale, minSpawnScale, OnMinSpawnScaleChanged);
        CheckValueChanged(ref lastMaxSpawnScale, maxSpawnScale, OnMaxSpawnScaleChanged);
        CheckValueChanged(ref lastNumOfShapesToSpawn, numOfShapesToSpawn, OnNumOfShapesToSpawnChanged);
    }

    public override void OnGizmosDraw()
    {
        Gizmos.color = Color.orange;

        Gizmos.DrawWireSphere(Vector3.zero, minSpawnAreaDiameter);
        Gizmos.DrawWireSphere(Vector3.zero, maxSpawnAreaDiameter);

        float angleOffset = 360f / numOfShapesToSpawn;

        Gizmos.color = Color.blue;
        for (int i = 0; i < numOfShapesToSpawn; i++)
        {
            float radAngle = (initialAngle + angleOffset * i) * Mathf.Deg2Rad;

            Gizmos.DrawLine(
                    Vector3.zero,
                    new Vector3(
                        Mathf.Cos(radAngle) * maxSpawnAreaDiameter,
                        Mathf.Sin(radAngle) * maxSpawnAreaDiameter,
                        0f)
            );
        }
    }
#endif

    public void OnShapesColorChanged(Color color)
    {
        this.shapesColor = color;
        if (isEnabled)
            SpawnShapes();
    }

    public void OnMinSpawnAreaDiameterChanged(float diameter)
    {
        minSpawnAreaDiameter = diameter;
        if (isEnabled)
            SpawnShapes();
    }

    public void OnMaxSpawnAreaDiameterChanged(float diameter)
    {
        maxSpawnAreaDiameter = diameter;
        if (isEnabled)
            SpawnShapes();
    }

    public void OnMinSpawnScaleChanged(float scale)
    {
        minSpawnScale = scale;
        if (isEnabled)
            SpawnShapes();
    }

    public void OnMaxSpawnScaleChanged(float scale)
    {
        maxSpawnScale = scale;
        if (isEnabled)
            SpawnShapes();
    }

    public void OnNumOfShapesToSpawnChanged(int numOfShapes)
    {
        numOfShapesToSpawn = numOfShapes;
        if (isEnabled)
            SpawnShapes();
    }

    private void SpawnShapes()
    {
        if (instanciatedShapes.Count != 0)
            DespawnShapes();

        float angleOffset = 360f / numOfShapesToSpawn;

        for (int i = 0; i < numOfShapesToSpawn; i++)
        {
            float radAngle = (initialAngle + angleOffset * i) * Mathf.Deg2Rad;

            Vector3 initialPos = new Vector3(
                Mathf.Cos(radAngle) * Random.Range(minSpawnAreaDiameter, maxSpawnAreaDiameter),
                Mathf.Sin(radAngle) * Random.Range(minSpawnAreaDiameter, maxSpawnAreaDiameter),
                0f
            );

            SpawnShape(initialPos);
        }
    }

    private void SpawnShape(Vector3 initialPos)
    {
        GameObject shape = Instantiate(
            shapes[Random.Range(0, shapes.Count)],
            initialPos,
            Quaternion.identity
        );

        shape.transform.localScale = Vector3.one * Random.Range(minSpawnScale, maxSpawnScale);

        float initialDist = initialPos.magnitude;
        Vector3 initialDirec = initialPos.normalized;

        Vector3 finalPos = initialDirec * (initialDist + Random.Range(minSpawnAreaDiameter, maxSpawnAreaDiameter)) + Vector3.right * Random.Range(-minSpawnAreaDiameter, minSpawnAreaDiameter) + Vector3.up * Random.Range(-minSpawnAreaDiameter, minSpawnAreaDiameter);

        ConfigShape(shape, finalPos);

        instanciatedShapes.Add((shape, initialPos));
    }

    private void ConfigShape(GameObject shape, Vector3 endPosition)
    {
        shape.GetComponent<SpriteRenderer>().color = shapesColor;

        LeanTween.alpha(shape, shapesColor.a, Random.Range(0.5f, 1f)).setDelay(Random.Range(0.5f, 2f)).setEaseInOutCubic().setOnComplete(
            delegate ()
        {
            Array LTTValues = Enum.GetValues(typeof(LeanTweenType));

            float movementTime = Random.Range(1f, 5f);

            LeanTweenType LTT = (LeanTweenType)LTTValues.GetValue(Random.Range(0, LTTValues.Length));
            LeanTween.rotate(shape, Vector3.forward * 179f, movementTime / 2f).setEase(LTT).setOnComplete(
                delegate ()
                {
                    LeanTween.rotate(shape, Vector3.forward * 359f, movementTime / 2f).setEase(LTT);
                }
                );
            LeanTween.move(shape, endPosition, movementTime).setEase(LTT).setOnComplete(
                delegate ()
                {
                    LeanTween.alpha(shape, 0f, Random.Range(0.5f, 1f)).setEaseInOutCubic().setOnComplete(
                    delegate ()
                    {
                        if (!isEnabled)
                            return;

                        (GameObject, Vector3) shapeInfo = instanciatedShapes.First(x => x.Item1 == shape);
                        instanciatedShapes.Remove(shapeInfo);
                        Destroy(shapeInfo.Item1);
                        SpawnShape(shapeInfo.Item2);
                    });
                });
        });
    }

    private void DespawnShapes()
    {
        foreach ((GameObject shape, Vector3 _) in instanciatedShapes)
        {
            LeanTween.cancelAll(shape);
            Destroy(shape);
        }

        instanciatedShapes.Clear();
    }

    public override void OnEnabled()
        => SpawnShapes();

    public override void OnDisabled()
        => DespawnShapes();
}
