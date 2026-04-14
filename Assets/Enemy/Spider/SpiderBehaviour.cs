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
            transform.Translate(speed * Time.fixedDeltaTime * Vector2.left);
            
            if (_mainCam != null && transform.position.x < _mainCam.transform.position.x - despawnDistance)
            {
                Destroy(gameObject);
            }
        }

        public void OnCollision(GameObject other)
        {
            Debug.Log("Player was bitten");
            if (GameData.Hp.GetValue() > 0) GameData.Hp.Decrease(1);
            Destroy(gameObject);
        }
        
        public void OnRayCastHit(GameObject other)
        {
            Debug.Log("Spider killed by raycast!");
            Destroy(gameObject);
        }
    }
}