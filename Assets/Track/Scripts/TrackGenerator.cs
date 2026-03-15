using System.Collections.Generic;
using UnityEngine;


namespace Track.Scripts
{
    public class TrackGenerator : MonoBehaviour
    {
        public GameObject trackPrefab;
        public Transform trackContainer;

        public Transform playerTransform;

        public int poolSize = 10;
        public float trackWidth = 4f;

        private Dictionary<GameObject, TrackPieceBehaviour> _pool;

        private float GetDespawnX()
        {
            return playerTransform.position.x - trackWidth * (poolSize / 2.0f);
        }

        private Vector3 GetRespawnX(Vector3 piecePosition)
        {
            return new Vector3(piecePosition.x + poolSize * trackWidth, piecePosition.y, piecePosition.z);
        }

        private void Start()
        {
            _pool = new Dictionary<GameObject, TrackPieceBehaviour>();

            for (var i = 0; i < poolSize; i++)
            {
                var spawnX = trackContainer.position.x + i * trackWidth;
                var piece = Instantiate(trackPrefab, trackContainer);

                piece.transform.position = new Vector3(spawnX, trackContainer.position.y, 0);

                _pool.Add(piece, piece.GetComponent<TrackPieceBehaviour>());
            }
        }

        private void Update()
        {
            var despawnX = GetDespawnX();
            foreach (var (piece, pieceBehaviour) in _pool)
            {
                if (piece.transform.position.x > despawnX) continue;

                piece.transform.position = GetRespawnX(piece.transform.position);
                pieceBehaviour.GenerateObstacles();
            }
        }
    }
}
