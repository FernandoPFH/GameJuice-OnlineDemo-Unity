using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Bar : Singleton<Bar>
{
    [SerializeField] private float moveIntensity = 1f;

    public static Action OnHit;

    InputAction moveAction;

    void Start()
        => moveAction = InputSystem.actions.FindAction("Move");

    public void Reset()
        => transform.position = new(0f, transform.position.y, transform.position.z);

    public void Hit()
        => OnHit?.Invoke();

    private void Update()
        => GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveAction.ReadValue<Vector2>().x * moveIntensity, ForceMode2D.Impulse);
}
