using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

using utils;


namespace Track.Scripts
{
    public class TrackPieceBehaviour : MonoBehaviour
    {
        public GameObject[] trackObstacleContainers;
        public GameObject[] trackItemContainers;

        [System.Serializable]
        public struct CollectibleEntry
        {
            public GameObject prefab;
            public float likelihood;
        }

        public float emptinessLikelihood;
        public List<CollectibleEntry> collectibleEntries;

        public GameObject finishLinePrefab;

        private enum TrackType
        {
            Clear,
            Single,
            Double,
            Finish,
            Aftermath
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
            else if (distanceFromStart >= GameData.TrackLength.GetValue()) trackType = TrackType.Aftermath;
            else trackType = _trackTypes[Random.Range(0, _trackTypes.Length)];

            switch (trackType)
            {
                case TrackType.Single:
                {
                    var targetIndex = Random.Range(0, trackObstacleContainers.Length);

                    for (var obstacleIndex = 0; obstacleIndex < trackObstacleContainers.Length; obstacleIndex++)
                    {
                        if (obstacleIndex == targetIndex) trackObstacleContainers[obstacleIndex].SetActive(true);
                        else
                        {
                            trackObstacleContainers[obstacleIndex].SetActive(false);
                            GenerateCollectibles(trackItemContainers[obstacleIndex]);
                        }
                    }

                    break;
                }
                case TrackType.Double:
                {
                    var targetIndex = Random.Range(0, trackObstacleContainers.Length);

                    for (var obstacleIndex = 0; obstacleIndex < trackObstacleContainers.Length; obstacleIndex++)
                    {
                        if (obstacleIndex != targetIndex) trackObstacleContainers[obstacleIndex].SetActive(true);
                        else
                        {
                            trackObstacleContainers[obstacleIndex].SetActive(false);
                            GenerateCollectibles(trackItemContainers[obstacleIndex]);
                        }
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
                case TrackType.Aftermath:
                default:
                {
                    for (var obstacleIndex = 0; obstacleIndex < trackObstacleContainers.Length; obstacleIndex++)
                    {
                        trackObstacleContainers[obstacleIndex].SetActive(false);

                        var collectibleContainer = trackItemContainers[obstacleIndex];

                        if (trackType != TrackType.Aftermath) GenerateCollectibles(collectibleContainer);
                        else ClearChildren(collectibleContainer);
                    }

                    break;
                }
            }
        }

        private float TotalLikelihood()
        {
            return emptinessLikelihood + collectibleEntries.Sum(entry => entry.likelihood);
        }

        private static void ClearChildren(GameObject parent)
        {
            foreach (Transform child in parent.transform)
                Destroy(child.gameObject);
        }

        public void GenerateCollectibles(GameObject itemContainer)
        {
            ClearChildren(itemContainer);

            var roll = Random.Range(0f, TotalLikelihood());

            roll -= emptinessLikelihood;
            if (roll <= 0) return;

            foreach (var entry in collectibleEntries)
            {
                roll -= entry.likelihood;

                if (roll > 0) continue;

                Instantiate(entry.prefab, itemContainer.transform);
                break;
            }
        }
    }
}
