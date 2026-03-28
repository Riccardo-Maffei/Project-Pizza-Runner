using System;
using UnityEngine;
using UnityEngine.InputSystem;

using utils;

namespace Player.Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private InputAction _moveAction;

        [Header("Physics & Components")]
        public Rigidbody2D playerRigidbody;

        [Header("Lane Settings")]
        public float laneHeight = 1;
        public int laneCount = 3;
        public float laneSpeed = 10;
        public float minPlayerY = 0.5f;
        private float _maxPlayerY;
        private float _currentPlayerY;

        [Header("Movement Settings")]
        public float minPlayerSpeed = 1;
        public float maxPlayerSpeed = 10;
        public float playerAcceleration = 5;
        private float _currentPlayerSpeed;
        private float _oldX;

        private void Start()
        {
            _currentPlayerSpeed = minPlayerSpeed;
            _currentPlayerY = minPlayerY;
            _maxPlayerY = minPlayerY + laneHeight * laneCount;

            _moveAction = InputSystem.actions.FindAction("Move");
            _moveAction.started += OnMovementTrigger;

            _oldX = playerRigidbody.position.x;
        }

        private void Update()
        {
            // Distance-Tracking for GameData
            GameData.TotalDistance.Increase(Math.Abs(playerRigidbody.position.x - _oldX));
            _oldX = playerRigidbody.position.x;

            // Calculate speed
            _currentPlayerSpeed = Mathf.MoveTowards(
                _currentPlayerSpeed,
                maxPlayerSpeed,
                playerAcceleration * Time.deltaTime
            );

            // Refresh position
            var pos = playerRigidbody.position;
            pos.x += _currentPlayerSpeed * Time.deltaTime;
            pos.y = Mathf.MoveTowards(pos.y, _currentPlayerY, laneSpeed * Time.deltaTime);

            playerRigidbody.MovePosition(pos);
        }

        private void OnDestroy()
        {
            if (_moveAction != null)
                _moveAction.started -= OnMovementTrigger;
        }

        private void OnMovementTrigger(InputAction.CallbackContext ctx)
        {
            var moveValue = ctx.ReadValue<Vector2>();
            
            if (utils.GameData.ReversedCommands.GetValue())
            {
                // Wir drehen den Wert von 'y' einfach um (hoch wird runter, runter wird hoch)
                moveValue.y *= -1;
            }
            
            var newY = _currentPlayerY + moveValue.y * laneHeight;

            if (newY >= minPlayerY && newY < _maxPlayerY)
                _currentPlayerY = newY;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.gameObject.GetComponent<IInteractive>()?.OnCollision(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IInteractive>()?.OnTrigger(gameObject);
        }
    }
}
