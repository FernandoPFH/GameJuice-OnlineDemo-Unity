using UnityEngine;
using UnityEngine.InputSystem;

public class Bar : Singleton<Bar>
{
    [SerializeField] private float moveIntensity = 1f;

    InputAction moveAction;

    void Start()
        => moveAction = InputSystem.actions.FindAction("Move");

    public void Reset()
        => transform.position = new(0f, transform.position.y, transform.position.z);

    private void Update()
        => GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveAction.ReadValue<Vector2>().x * moveIntensity, ForceMode2D.Impulse);
}
