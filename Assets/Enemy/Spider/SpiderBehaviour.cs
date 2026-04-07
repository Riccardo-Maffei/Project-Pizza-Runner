using UnityEngine;
using Utils;

namespace Enemy.Spider
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpiderBehaviour : MonoBehaviour, IInteractive
    {
        [Header("Movement Settings")]
        public float speed = 12;
        public float despawnDistance = 15f;

        private Camera _mainCam;
        private Rigidbody2D _rb;

        private void Start()
        {
            _mainCam = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.gravityScale = 0;
        }

        private void FixedUpdate()
        {
            // movement left
            transform.Translate(Vector2.left * speed * Time.fixedDeltaTime);
            
            if (_mainCam != null && transform.position.x < _mainCam.transform.position.x - despawnDistance)
            {
                Destroy(gameObject);
            }
        }
        
        public void OnTrigger(GameObject other)
        {
            // Logic, if the player touches the spider
            if (other.CompareTag("Player")) 
            {
                Debug.Log("Bite! Spinne hat den Spieler erwischt.");
                // Hier Schaden-Logik: other.GetComponent<Health>().Damage(1);
                
                Destroy(gameObject); // Spinne verschwindet nach Biss
            }
        }

        public void OnCollision(GameObject other)
        {
            OnTrigger(other);
        }
        
        public void OnHit()
        {
            Debug.Log("Spider killed by raycast!");
            Destroy(gameObject);
        }
    }
}