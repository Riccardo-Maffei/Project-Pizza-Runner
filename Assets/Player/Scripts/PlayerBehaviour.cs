using System;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Utils;

namespace Player.Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {
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

        [Header("Shooting Settings")]
        public Transform shotOrigin;
        public float shotRange = 20f;
        public bool blockEarlyShooting = true;
        
        private void Start()
        {
            _currentPlayerSpeed = minPlayerSpeed;
            _currentPlayerY = minPlayerY;
            _maxPlayerY = minPlayerY + laneHeight * laneCount;

            _oldX = playerRigidbody.position.x;
        }

        private void FixedUpdate()
        {
            if (GameData.Hp.GetValue() <= 0) _currentPlayerSpeed = 0;
            else
            {
                // Distance-Tracking for GameData
                GameData.TotalDistance.Increase(Math.Abs(playerRigidbody.position.x - _oldX));
                _oldX = playerRigidbody.position.x;
                
                // Calculate multiplier
                var speedMultiplier = GameData.SpeedMultipliers.Aggregate(1f, (result, next) => result * next);

                // Calculate speed
                _currentPlayerSpeed = Mathf.MoveTowards(
                    _currentPlayerSpeed,
                    maxPlayerSpeed * speedMultiplier,
                    playerAcceleration * Time.fixedDeltaTime
                );
            }

            // Refresh position
            var pos = playerRigidbody.position;
            pos.x += _currentPlayerSpeed * Time.fixedDeltaTime;
            pos.y = Mathf.MoveTowards(pos.y, _currentPlayerY, laneSpeed * Time.fixedDeltaTime);

            playerRigidbody.MovePosition(pos);
        }

        protected void OnMove(InputValue value)
        {
            if (GameData.Hp.GetValue() <= 0) return;
            var moveValue = value.Get<Vector2>();
            
            // Reverse movement axis on wine bottle hit 
            if (GameData.ReversedCommands.GetValue()) moveValue.y *= -1;
            
            var newY = _currentPlayerY + moveValue.y * laneHeight;

            if (newY >= minPlayerY && newY < _maxPlayerY)
                _currentPlayerY = newY;
        }

        protected void OnAttack(InputValue value)
        {
            // Block if player is not over the line and early shots are disabled
            if (!GameData.CrossedFinishLine.GetValue() && blockEarlyShooting) return;
            
            ExecuteRaycastShoot();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.gameObject.GetComponent<IInteractive>()?.OnCollision(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IInteractive>()?.OnTrigger(gameObject);
        }
        
        private void ExecuteRaycastShoot()
        {
            // Cast a ray to the right
            var hit = Physics2D.Raycast(shotOrigin.position, Vector2.right, shotRange);
            
            // Display ray (only in scene view / if gizmos have been turned on)
            Debug.DrawRay(shotOrigin.position, Vector2.right * shotRange, Color.white, 0.5f);

            if (hit) hit.collider.GetComponent<IInteractive>()?.OnRayCastHit(gameObject);
        }
    }
}
