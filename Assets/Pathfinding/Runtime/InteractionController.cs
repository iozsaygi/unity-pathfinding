using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        // Required references.
        [SerializeField] private Camera mainCamera;
        [SerializeField] private NodeMapController nodeMapController;
        [SerializeField] private GameObject firstNodeHighlight;
        [SerializeField] private GameObject secondNodeHighlight;
        [SerializeField] private GameObject[] neighborNodeHighlights;
        [SerializeField] private LineRenderer lineRenderer;

        // Raycast settings.
        [SerializeField, Min(0.1f)] private float interactionDistance;
        [SerializeField] private LayerMask interactableLayers;

        // Interaction tracking.
        private readonly List<Node> interactedNodes = new();

        private void Start()
        {
            firstNodeHighlight.transform.localScale = Node.Size;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers)) return;

            if (interactedNodes.Count == 2)
            {
                if (interactedNodes.Count != 2) return;


                firstNodeHighlight.gameObject.SetActive(false);
                secondNodeHighlight.gameObject.SetActive(false);

                for (byte i = 0; i < neighborNodeHighlights.Length; i++)
                {
                    neighborNodeHighlights[i].SetActive(false);
                }

                var path = Pathfinder.Execute(interactedNodes[0], interactedNodes[1], nodeMapController);
                lineRenderer.positionCount = path.Count;
                for (var i = 0; i < path.Count; i++) lineRenderer.SetPosition(i, path[i].PositionInWorldCoordinates);

                interactedNodes.Clear();

                return;
            }

            nodeMapController.WorldPointToNode(raycastHit.point, out var node);
            interactedNodes.Add(node);


            switch (interactedNodes.Count)
            {
                // ReSharper disable once Unity.InefficientPropertyAccess
                case 1:
                    firstNodeHighlight.gameObject.SetActive(true);
                    firstNodeHighlight.transform.position = node.PositionInWorldCoordinates;
                    break;
                case 2:
                    secondNodeHighlight.gameObject.SetActive(true);
                    secondNodeHighlight.transform.position = node.PositionInWorldCoordinates;
                    break;
            }

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