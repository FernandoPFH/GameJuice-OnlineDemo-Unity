using UnityEngine;
using UnityEngine.InputSystem;

public class Bar : Singleton<Bar>
{
    [SerializeField] private float moveIntensity = 1f;

    InputAction moveAction;

    void Start()
        => moveAction = InputSystem.actions.FindAction("Move");

    private void Update()
        => GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveAction.ReadValue<Vector2>().x * moveIntensity, ForceMode2D.Impulse);
}
