using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ball : Singleton<Ball>
{
    [SerializeField] private float blockHitVelocityMultiplier = 1.5f;
    [SerializeField] private float maxVelocity = 30f;

    public static Action OnSpawn;
    public static Action<string> OnHit;

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
                OnBlockHit(other.GetComponent<Block>(), collision.relativeVelocity);
                break;
            case "Wall":
                OnWallHit(other.GetComponent<Wall>());
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

        OnHit?.Invoke(other.tag);

        FixBallIsStuck();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        rigidbody.linearVelocity = Vector2.zero;
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        OnSpawn?.Invoke();
    }

    private void OnBlockHit(Block block, Vector3 ballVelocity)
        => block.Hit(ballVelocity);

    private void OnWallHit(Wall wall)
        => wall.Hit();

    private void OnDeathWallHit(DeathWall deathWall)
    {
        Reset();

        deathWall.Hit();
    }

    private void OnBarHit(Bar bar)
    {
        if (rigidbody.linearVelocity.sqrMagnitude < maxVelocity * maxVelocity)
            rigidbody.linearVelocity *= blockHitVelocityMultiplier;

        bar.Hit();
    }

    private void FixBallIsStuck()
    {
        float angleX = Vector2.Angle(rigidbody.linearVelocity, Vector2.right);
        float angleY = Vector2.Angle(rigidbody.linearVelocity, Vector2.up);

        if ((Mathf.Abs(angleX - 90f) <= 5f || Mathf.Abs(angleX - 90f) <= 5f) || (Mathf.Abs(angleX - 90f) <= 5f || Mathf.Abs(angleX - 90f) <= 5f))
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
