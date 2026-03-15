using UnityEngine;
using UnityEngine.InputSystem;


namespace Player.Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private InputAction _moveAction;

        public Transform trackContainer;

        private Vector3 _targetPosition;

        public float gridSize = 1;
        public int gridRows = 3;

        private float _gridStart;

        public float minPlayerSpeed = 1;
        public float maxPlayerSpeed = 10;

        public float playerAcceleration = 1;

        private float _currentPlayerSpeed;

        public string obstacleTag = "Obstacle";


        private void Start()
        {
            _targetPosition = trackContainer.transform.position;
            _gridStart = _targetPosition.y;

            _currentPlayerSpeed = minPlayerSpeed;

            _moveAction = InputSystem.actions.FindAction("Move");
            _moveAction.started += OnMovementTrigger;
        }

        private void Update()
        {
            for (var childIndex = 0; childIndex < trackContainer.childCount; childIndex++)
            {
                var child = trackContainer.GetChild(childIndex);

                _targetPosition.x = child.position.x;
                _targetPosition.x -= _currentPlayerSpeed * Time.deltaTime;

                child.position = _targetPosition;
            }

            if (_currentPlayerSpeed >= maxPlayerSpeed) return;

            var newPlayerSpeed = _currentPlayerSpeed + playerAcceleration * Time.deltaTime;
            _currentPlayerSpeed = newPlayerSpeed <= maxPlayerSpeed ? newPlayerSpeed : maxPlayerSpeed;
        }

        private void OnMovementTrigger(InputAction.CallbackContext ctx)
        {
            var moveValue = ctx.ReadValue<Vector2>();
            var newY = _targetPosition.y - moveValue.y * gridSize;

            var minY = _gridStart - gridSize * (gridRows - 1);
            if (newY <= _gridStart && newY >= minY)
                _targetPosition.y = newY;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(obstacleTag))
            {
                Debug.Log("GAME OVER");
            }
        }
    }
}
