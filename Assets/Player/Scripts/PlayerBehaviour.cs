using System;
using UnityEngine;
using UnityEngine.InputSystem;

using utils;

namespace Player.Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private InputAction _moveAction;

        public Rigidbody2D playerRigidbody;

        public float laneHeight = 1;
        public int laneCount = 3;

        public float minPlayerSpeed = 1;
        public float maxPlayerSpeed = 10;
        public float playerAcceleration = 5;
        private float _currentPlayerSpeed;

        public float minPlayerY = 0.5f;
        private float _maxPlayerY;
        public float laneSpeed = 10;
        private float _currentPlayerY;

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
            GameData.TotalDistance.Increase(Math.Abs(playerRigidbody.position.x - _oldX));
            _oldX = playerRigidbody.position.x;

            _currentPlayerSpeed = Mathf.MoveTowards(
                _currentPlayerSpeed,
                maxPlayerSpeed,
                playerAcceleration * Time.deltaTime
            );

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

            var newY = _currentPlayerY + moveValue.y * laneHeight;

            if (newY >= minPlayerY && newY < _maxPlayerY)
                _currentPlayerY = newY;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.GetComponent<IInteractive>()?.OnCollision(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IInteractive>()?.OnTrigger(gameObject);
        }
    }
}
