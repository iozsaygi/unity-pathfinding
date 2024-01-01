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

            // ReSharper disable once Unity.InefficientPropertyAccess
            currentNodeHighlight.transform.position = node.PositionInWorldCoordinates;
        }
    }
}