using UnityEngine;
using Random = UnityEngine.Random;


namespace Track.Scripts
{
    public class TrackPieceBehaviour : MonoBehaviour
    {
        public GameObject[] trackObstacleContainers;

        private enum TrackType
        {
            Clear,
            Single,
            Double
        }

        private readonly TrackType[] _trackTypes =  { TrackType.Clear, TrackType.Single, TrackType.Double };

        private void Start()
        {
            GenerateObstacles();
        }

        public void GenerateObstacles()
        {
            switch (_trackTypes[Random.Range(0, _trackTypes.Length)])
            {
                case TrackType.Single:
                {
                    var targetIndex = Random.Range(0, trackObstacleContainers.Length);

                    for (var obstacleIndex = 0; obstacleIndex < trackObstacleContainers.Length; obstacleIndex++)
                    {
                        trackObstacleContainers[obstacleIndex].SetActive(obstacleIndex == targetIndex);
                    }

                    break;
                }
                case TrackType.Double:
                {
                    var targetIndex = Random.Range(0, trackObstacleContainers.Length);

                    for (var obstacleIndex = 0; obstacleIndex < trackObstacleContainers.Length; obstacleIndex++)
                    {
                        trackObstacleContainers[obstacleIndex].SetActive(obstacleIndex != targetIndex);
                    }

                    break;
                }
                case TrackType.Clear:
                default:
                {
                    foreach (var obstacleContainer in trackObstacleContainers)
                    {
                        obstacleContainer.SetActive(false);
                    }

                    break;
                }
            }
        }
    }
}
