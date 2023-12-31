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
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers)) return;
            currentNodeHighlight.transform.position = raycastHit.point;

            // TODO: Convert world point to node by using node map controller.
        }
    }
}