using UnityEngine;


namespace Player.Scripts
{
    public class BackpackBehaviour : MonoBehaviour
    {
        private Rigidbody2D _rb;

        public float torqueMultiplier = 1;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {
            if (Mathf.Abs(_rb.rotation) < 0.5f)
            {
                _rb.angularVelocity = 0;
                return;
            }

            _rb.AddTorque(
                (_rb.rotation < 0 ? torqueMultiplier : -torqueMultiplier) * Time.fixedDeltaTime
            );
        }
    }
}
