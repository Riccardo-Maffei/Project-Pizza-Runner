using UnityEngine;
using Random = UnityEngine.Random;

using utils;


namespace Track.Scripts
{
    public class TrackPieceBehaviour : MonoBehaviour
    {
        public GameObject[] trackObstacleContainers;
        public GameObject[] trackItemContainers;

        public GameObject finishLinePrefab;

        private enum TrackType
        {
            Clear,
            Single,
            Double,
            Finish
        }

        private readonly TrackType[] _trackTypes =  { TrackType.Clear, TrackType.Single, TrackType.Double };

        public void GenerateObstacles(float distanceFromStart)
        {
            TrackType trackType;

            if (distanceFromStart >= GameData.TrackLength.GetValue() && !GameData.SpawnedFinishLine.GetValue())
            {
                trackType = TrackType.Finish;
                GameData.SpawnedFinishLine.SetValue(true);
            }
            else if (distanceFromStart >= GameData.TrackLength.GetValue()) trackType = TrackType.Clear;
            else trackType = _trackTypes[Random.Range(0, _trackTypes.Length)];

            switch (trackType)
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
                case TrackType.Finish:
                {
                    foreach (var obstacleContainer in trackObstacleContainers)
                    {
                        obstacleContainer.SetActive(false);
                    }

                    foreach (var trackItemContainer in trackItemContainers)
                    {
                        Instantiate(finishLinePrefab, trackItemContainer.transform);
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
