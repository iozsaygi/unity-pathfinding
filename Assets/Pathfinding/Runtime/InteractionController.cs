using UnityEngine;

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform interactionHighlight;
        [SerializeField] private Transform[] interactionHighlightsForNeighbor;
        [SerializeField] private Vector3 highlightInvalidPosition;
        [SerializeField] private PathfindingSceneConnection pathfindingSceneConnection;

        private void Start()
        {
            interactionHighlight.transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            for (byte i = 0; i < interactionHighlightsForNeighbor.Length; i++)
            {
                interactionHighlightsForNeighbor[i].position = highlightInvalidPosition;
                interactionHighlightsForNeighbor[i].transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            }
        }

        private void Update()
        {
            var mousePositionInScreenCoordinates = Input.mousePosition;
            mousePositionInScreenCoordinates.z = mainCamera.transform.position.y;

            var mousePositionInWorldCoordinates = mainCamera.ScreenToWorldPoint(mousePositionInScreenCoordinates);

            // Update the highlight's position to mouse position.
            pathfindingSceneConnection.Grid.WorldPointToNode(mousePositionInWorldCoordinates, out var node);

            // Don't bother with all of this stuff below if the node is invalid.
            if (node.Equals(Node.Invalid))
            {
                interactionHighlight.position = highlightInvalidPosition;
                return;
            }

            interactionHighlight.position = node.Position;
            for (byte i = 0; i < node.Neighbors.Length; i++)
            {
                if (node.Neighbors[i].Equals(NodeIdentity.Invalid))
                {
                    interactionHighlightsForNeighbor[i].position = highlightInvalidPosition;
                    continue;
                }

                var neighborNode = pathfindingSceneConnection.Grid.Nodes[node.Neighbors[i].Context];
                interactionHighlightsForNeighbor[i].position = neighborNode.Position;
            }
        }
    }
}