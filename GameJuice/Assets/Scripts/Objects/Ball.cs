using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ball : Singleton<Ball>
{
    [SerializeField] private float blockHitVelocityMultiplier = 1.5f;
    [SerializeField] private float maxVelocity = 30f;

    private new Rigidbody2D rigidbody;

    private List<float> timesWhenBallMayBeStuck = new();
    private int minOccurancesToConciderStuck = 5;
    private int minTimeToConciderStuck = 5;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        switch (other.tag)
        {
            case "Block":
                OnBlockHit(other.GetComponent<Block>());
                break;
            case "DeathWall":
                OnDeathWallHit(other.GetComponent<DeathWall>());
                break;
            case "Bar":
                OnBarHit(other.GetComponent<Bar>());
                break;
            default:
                break;
        }

        FixBallIsStuck();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnBlockHit(Block block)
        => block.Hit();

    private void OnDeathWallHit(DeathWall deathWall)
    {
        Reset();

        deathWall.Hit();
    }

    private void OnBarHit(Bar bar)
    {
        if (rigidbody.linearVelocity.sqrMagnitude < maxVelocity * maxVelocity)
            rigidbody.linearVelocity *= blockHitVelocityMultiplier;
    }

    private void FixBallIsStuck()
    {
        Vector2 velocityNormalized = rigidbody.linearVelocity.normalized;

        if (Mathf.Abs(velocityNormalized.x) >= 0.95f || Mathf.Abs(velocityNormalized.y) >= 0.95f)
            timesWhenBallMayBeStuck.Add(Time.time);
        else
            timesWhenBallMayBeStuck.Clear();

        if (timesWhenBallMayBeStuck.Count == 0)
            return;

        if (timesWhenBallMayBeStuck.Count > minOccurancesToConciderStuck || Time.time - timesWhenBallMayBeStuck.First() > minTimeToConciderStuck)
        {
            float currentVelocity = rigidbody.linearVelocity.magnitude;

            Vector2 randomVelocity = BallSpawner.GetRandomVelocity();

            rigidbody.linearVelocity = -randomVelocity.normalized * currentVelocity;

            timesWhenBallMayBeStuck.Clear();
        }
    }
}
