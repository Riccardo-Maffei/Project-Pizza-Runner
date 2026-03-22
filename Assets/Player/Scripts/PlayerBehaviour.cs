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

        // Variable für den Trefferschutz (Cooldown)
        private bool _canTakeDamage = true;

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
            // Distanz-Tracking für GameData
            GameData.TotalDistance.Increase(Math.Abs(playerRigidbody.position.x - _oldX));
            _oldX = playerRigidbody.position.x;

            // Beschleunigung berechnen
            _currentPlayerSpeed = Mathf.MoveTowards(
                _currentPlayerSpeed,
                maxPlayerSpeed,
                playerAcceleration * Time.deltaTime
            );

            // Position aktualisieren
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 1. Prüfen, ob wir gerade Schaden nehmen dürfen (verhindert Doppel-Abzug)
            if (!_canTakeDamage) return;

            // 2. Prüfen, ob das getroffene Objekt ein Hindernis ist
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                _canTakeDamage = false; // Schaden kurzzeitig sperren

                if (GameData.Hp.GetValue() > 0)
                {
                    GameData.Hp.Decrease(1);
                    Debug.Log("Leben abgezogen! Verbleibend: " + GameData.Hp.GetValue());

                    // Nach 0.5 Sekunden wieder Schaden erlauben
                    Invoke(nameof(ResetDamage), 0.5f);
                }

                // 3. Prüfen, ob das Spiel beendet werden muss
                if (GameData.Hp.GetValue() <= 0)
                {
                    EndGame();
                }
            }

            // Bestehende Interaktions-Logik aufrufen
            collision.gameObject.GetComponent<IInteractive>()?.OnCollision(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<IInteractive>()?.OnTrigger(gameObject);
        }

        // Hilfsmethode, um den Trefferschutz aufzuheben
        private void ResetDamage()
        {
            _canTakeDamage = true;
        }

        private void EndGame()
        {
            Debug.Log("GAME OVER - Lost all lives.");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }
    }
}