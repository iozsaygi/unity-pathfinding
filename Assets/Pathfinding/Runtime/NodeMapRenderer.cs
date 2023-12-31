using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class NodeMapRenderer : MonoBehaviour
    {
        [SerializeField] private NodeMapController nodeMapController;
        [SerializeField] private LineRenderer lineRendererPrefab;
        [SerializeField, Min(0.0f)] private float heightRate;

        private void Start()
        {
            GenerateHorizontalLines();
            GenerateVerticalLines();
        }

        private void GenerateHorizontalLines()
        {
            for (var i = 0; i < nodeMapController.HorizontalNodeCount + 1; i++)
            {
                var lineRendererPrefabInstance = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
                lineRendererPrefabInstance.transform.SetParent(transform, true);

                var firstPosition = new Vector3(nodeMapController.TopLeftCornerInWorldCoordinates.x, heightRate,
                    nodeMapController.TopLeftCornerInWorldCoordinates.z - i * Node.Size.z);

                var secondPosition = new Vector3(nodeMapController.TopRightCornerInWorldCoordinates.x, heightRate,
                    nodeMapController.TopRightCornerInWorldCoordinates.z - i * Node.Size.z);

                var lineRenderer = lineRendererPrefabInstance.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, firstPosition);
                lineRenderer.SetPosition(1, secondPosition);
            }
        }

        private void GenerateVerticalLines()
        {
            for (var i = 0; i < nodeMapController.VerticalNodeCount + 1; i++)
            {
                var lineRendererPrefabInstance = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
                lineRendererPrefabInstance.transform.SetParent(transform, true);

                var firstPosition = new Vector3(nodeMapController.TopLeftCornerInWorldCoordinates.x + i * Node.Size.x,
                    heightRate,
                    nodeMapController.TopLeftCornerInWorldCoordinates.z);

                var secondPosition = new Vector3(nodeMapController.TopLeftCornerInWorldCoordinates.x + i * Node.Size.x,
                    heightRate, nodeMapController.BottomLeftCornerInWorldCoordinates.z);

                var lineRenderer = lineRendererPrefabInstance.GetComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, firstPosition);
                lineRenderer.SetPosition(1, secondPosition);
            }
        }
    }
}