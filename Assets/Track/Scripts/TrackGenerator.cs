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
        public float trackWidth = 4;

        private float _initialPlayerPosition;

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
            _initialPlayerPosition = playerTransform.position.x;

            for (var i = 0; i < poolSize; i++)
            {
                var piece = Instantiate(trackPrefab, trackContainer);

                var spawnX = trackContainer.position.x + i * trackWidth;
                var distance = spawnX - _initialPlayerPosition;
                var pieceBehaviour = piece.GetComponent<TrackPieceBehaviour>();

                piece.transform.position = new Vector3(spawnX, trackContainer.position.y, 0);
                pieceBehaviour.GenerateObstacles(distance);

                _pool.Add(piece, pieceBehaviour);
            }
        }

        private void Update()
        {
            var despawnX = GetDespawnX();
            foreach (var (piece, pieceBehaviour) in _pool)
            {
                if (piece.transform.position.x > despawnX) continue;

                var spawn = GetRespawnX(piece.transform.position);
                var distance = spawn.x - _initialPlayerPosition;

                piece.transform.position = spawn;
                pieceBehaviour.GenerateObstacles(distance);
            }
        }
    }
}
