using UnityEngine;

public class BallSpawner : Singleton<BallSpawner>
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float velocityIntensity = 1f;
    [Range(0f, 90f)]
    [SerializeField] private float velocityAngleRange = 30f;

    private float velocityAngle = 270f;

    protected override void Awake()
    {
        base.Awake();

        GameStateManager.OnGameStateChange += OnGameStateChange;

        Instantiate(ballPrefab, transform.position, Quaternion.identity).SetActive(false);
    }

    private void OnGameStateChange(GameState state)
    {
        if (state is not GameState.GameLoop)
            return;

        SpawnBall();
    }

    public static void SpawnBall()
        => Instance.spawnBall();

    private void spawnBall()
    {
        Vector2 randomVelocity = getRandomVelocity();

        GameObject ball = Ball.Instance.gameObject;
        ball.SetActive(true);
        ball.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        ball.GetComponent<Rigidbody2D>().linearVelocity = randomVelocity;
    }

    public static Vector2 GetRandomVelocity()
        => Instance.getRandomVelocity();

    private Vector2 getRandomVelocity()
    {
        Vector2 baseVelocity = Vector2.one;

        float angle = velocityAngle + Random.Range(-velocityAngleRange, velocityAngleRange);
        float radAngle = Mathf.Deg2Rad * angle;

        baseVelocity *= new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));

        baseVelocity *= velocityIntensity;

        return baseVelocity;
    }

#if UNITY_EDITOR
    private float gizmosLength = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.DrawLineStrip(GeneratePoints(), true);
    }

    private Vector3[] GeneratePoints()
    {
        float positiveRangeAngle = Mathf.Deg2Rad * (velocityAngle + velocityAngleRange);
        float negativeRangeAngle = Mathf.Deg2Rad * (velocityAngle - velocityAngleRange);

        return new Vector3[3]
        {
            transform.position,
            transform.position + new Vector3(Mathf.Cos(positiveRangeAngle), Mathf.Sin(positiveRangeAngle), 0f) * gizmosLength,
            transform.position + new Vector3(Mathf.Cos(negativeRangeAngle), Mathf.Sin(negativeRangeAngle), 0f) * gizmosLength
        };
    }
#endif
}
