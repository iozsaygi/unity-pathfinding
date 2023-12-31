using UnityEngine;

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField, Min(0.1f)] private float interactionDistance;
        [SerializeField] private LayerMask interactableLayers;


        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out var raycastHit, interactionDistance, interactableLayers);
            if (raycastHit.collider == null) return;
            Debug.Log(raycastHit.point);
        }
    }
}