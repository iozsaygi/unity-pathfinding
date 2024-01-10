using UnityEngine;

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform interactionHighlight;
        [SerializeField] private PathfindingSceneConnection pathfindingSceneConnection;

        private void Start()
        {
            interactionHighlight.transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
        }

        private void Update()
        {
            var mousePositionInScreenCoordinates = Input.mousePosition;
            mousePositionInScreenCoordinates.z = mainCamera.transform.position.y;

            var mousePositionInWorldCoordinates = mainCamera.ScreenToWorldPoint(mousePositionInScreenCoordinates);

            // Update the highlight's position to mouse position.
            pathfindingSceneConnection.Grid.WorldPointToNode(mousePositionInWorldCoordinates, out var node);
            interactionHighlight.position = node.Position;
        }
    }
}