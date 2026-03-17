using System.Collections.Generic;
using UnityEngine;


namespace Track.Scripts
{
    public class StartPieceBehaviour : MonoBehaviour
    {
        public Transform playerTransform;

        public float despawnDistance = 20f;
        private float _despawnX;

        private Dictionary<GameObject, TrackPieceBehaviour> _pool;

        private float GetDespawnX()
        {
            return playerTransform.position.x + despawnDistance;
        }

        private void Start()
        {
            _despawnX = GetDespawnX();
        }

        private void Update()
        {
            if (playerTransform.position.x > _despawnX) gameObject.SetActive(false);
        }
    }
}