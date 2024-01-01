using UnityEngine;

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        // Required references.
        [SerializeField] private Camera mainCamera;
        [SerializeField] private NodeMapController nodeMapController;
        [SerializeField] private GameObject currentNodeHighlight;
        [SerializeField] private GameObject[] neighborNodeHighlights;

        // Raycast settings.
        [SerializeField, Min(0.1f)] private float interactionDistance;
        [SerializeField] private LayerMask interactableLayers;

        private void Start()
        {
            currentNodeHighlight.transform.localScale = Node.Size;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers)) return;
            currentNodeHighlight.transform.position = raycastHit.point;

            nodeMapController.WorldPointToNode(raycastHit.point, out var node);

            if (!currentNodeHighlight.activeSelf) currentNodeHighlight.SetActive(true);

            // ReSharper disable once Unity.InefficientPropertyAccess
            currentNodeHighlight.transform.position = node.PositionInWorldCoordinates;

            // Also highlight the neighbor nodes.
            HighlightNeighbors(node);
        }

        private void HighlightNeighbors(Node node)
        {
            for (byte i = 0; i < node.Neighbors.Length; i++)
            {
                if (node.Neighbors[i].Equals(NodeIdentity.InvalidIdentity))
                {
                    neighborNodeHighlights[i].gameObject.SetActive(false);
                    continue;
                }

                neighborNodeHighlights[i].gameObject.SetActive(true);

                nodeMapController.NodeFromIdentity(node.Neighbors[i], out var neighborNode);
                neighborNodeHighlights[i].transform.position = neighborNode.PositionInWorldCoordinates;
            }
        }
    }
}