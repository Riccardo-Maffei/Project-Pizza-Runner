using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerBehaviour : MonoBehaviour
{
    private InputAction _moveAction;
    private Rigidbody2D _playerBody;

    public float gridSize = 1;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _moveAction.started += OnMovementTrigger;

        _playerBody = GetComponent<Rigidbody2D>();
    }

    private void OnMovementTrigger(InputAction.CallbackContext ctx)
    {
        var moveValue = ctx.ReadValue<Vector2>();
        var movementPosition= transform.position + new Vector3(0, moveValue.y * gridSize, 0);

        _playerBody.MovePosition(movementPosition);
    }
}
