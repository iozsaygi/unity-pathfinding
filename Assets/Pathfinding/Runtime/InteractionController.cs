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
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GameObject unWalkableMarkerPrefab;

        // Raycast settings.
        [SerializeField, Min(0.1f)] private float interactionDistance;
        [SerializeField] private LayerMask interactableLayers;

        // Interaction tracking.
        private readonly List<Node> interactedNodes = new();

        private readonly NodeStateStorage nodeStateStorage = new();

        private void Start()
        {
            firstNodeHighlight.transform.localScale = Node.Size;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers)) return;

                nodeMapController.WorldPointToNode(raycastHit.point, out var node);
                nodeStateStorage.UpdateNodeState(node, unWalkableMarkerPrefab);
            }

            // ReSharper disable once InvertIf
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers)) return;

                if (interactedNodes.Count == 2)
                {
                    firstNodeHighlight.gameObject.SetActive(false);
                    secondNodeHighlight.gameObject.SetActive(false);

                    var path = Pathfinder.Execute(interactedNodes[0], interactedNodes[1], nodeMapController,
                        nodeStateStorage);

                    interactedNodes.Clear();
                    if (path == null)
                    {
                        lineRenderer.positionCount = 0;
                        return;
                    }

                    lineRenderer.positionCount = path.Count;
                    for (var i = 0; i < path.Count; i++)
                    {
                        lineRenderer.SetPosition(i, path[i].PositionInWorldCoordinates);
                    }

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
            }
        }
    }
}