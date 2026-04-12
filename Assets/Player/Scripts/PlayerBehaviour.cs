using System;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Utils;

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
        private InputAction _fireAction; // Neu für die Taste

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
            _fireAction = InputSystem.actions.FindAction("Attack");

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
            
            // Nur wenn wir im Ziel stehen, können wir schießen (Phase 2, Punkt d)
            if (Utils.GameData.CrossedFinishLine.GetValue() && _fireAction != null && _fireAction.WasPressedThisFrame())
            {
                ExecuteRaycastShoot();
            }
        }

        private void OnDestroy()
        {
            if (_moveAction != null)
                _moveAction.started -= OnMovementTrigger;
        }

        private void OnMovementTrigger(InputAction.CallbackContext ctx)
        {
            if (GameData.Hp.GetValue() <= 0) return;
            var moveValue = ctx.ReadValue<Vector2>();
            
            // Reverse movement axis on wine bottle hit 
            if (GameData.ReversedCommands.GetValue()) moveValue.y *= -1;
            
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
        
        private void ExecuteRaycastShoot()
        {
            // Der Strahl geht 20 Einheiten nach rechts
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 20f);
            Debug.DrawRay(transform.position, Vector2.right * 20f, Color.red, 0.5f); // Sichtbar für die Abgabe

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Wir zerstören die Spinne über ihre eigene Methode
                hit.collider.GetComponent<Enemy.Spider.SpiderBehaviour>()?.OnHit();
            }
        }
        
    }
}
