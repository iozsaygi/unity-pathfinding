using UnityEngine;

// ReSharper disable InconsistentNaming

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
        [SerializeField] private Transform firstPathPointHighlight;
        [SerializeField] private Transform secondPathPointHighlight;
        [SerializeField] private LineRenderer pathPreviewRenderer;

        private Node sourceNode;
        private Node destinationNode;

        private bool resetFirstHighlight = true;

        private void Start()
        {
            interactionHighlight.transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            for (byte i = 0; i < interactionHighlightsForNeighbor.Length; i++)
            {
                interactionHighlightsForNeighbor[i].position = highlightInvalidPosition;
                interactionHighlightsForNeighbor[i].transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            }

            firstPathPointHighlight.transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            secondPathPointHighlight.transform.localScale = pathfindingSceneConnection.Grid.NodeSize;
            firstPathPointHighlight.position = highlightInvalidPosition;
            secondPathPointHighlight.position = highlightInvalidPosition;
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
                for (byte i = 0; i < interactionHighlightsForNeighbor.Length; i++)
                {
                    interactionHighlightsForNeighbor[i].position = highlightInvalidPosition;
                }

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

            // 'LMB' input listener.
            if (Input.GetMouseButtonDown(0))
            {
                // Do not switch highlights if node is registered with blocker object.
                if (pathfindingSceneConnection.Grid.GridGameObjectRegistry.IsNodeRegisteredWithPathfindingBlocker(node))
                {
                    return;
                }

                if (firstPathPointHighlight.position.Equals(highlightInvalidPosition))
                {
                    firstPathPointHighlight.position = node.Position;
                    sourceNode = node;
                }
                else if (secondPathPointHighlight.position.Equals(highlightInvalidPosition))
                {
                    secondPathPointHighlight.position = node.Position;
                    destinationNode = node;
                    UpdatePathPreview();
                }
                else
                {
                    if (resetFirstHighlight)
                    {
                        firstPathPointHighlight.position = node.Position;
                        sourceNode = node;
                        pathPreviewRenderer.positionCount = 0;
                    }
                    else
                    {
                        secondPathPointHighlight.position = node.Position;
                        destinationNode = node;
                        UpdatePathPreview();
                    }

                    resetFirstHighlight = !resetFirstHighlight;
                }
            }

            // 'RMB' input listener.
            // ReSharper disable once InvertIf
            if (Input.GetMouseButtonDown(1))
            {
                pathfindingSceneConnection.Grid.GridGameObjectRegistry.Register(node,
                    pathfindingSceneConnection.PathfindingBlockerObject);

                UpdatePathPreview();
            }
        }

        private void UpdatePathPreview()
        {
            if (sourceNode.Equals(default) || destinationNode.Equals(default))
            {
                return;
            }

            var path = Pathfinder.Execute(sourceNode, destinationNode, pathfindingSceneConnection.Grid);
            pathPreviewRenderer.positionCount = path.Count;

            for (var i = 0; i < path.Count; i++)
            {
                pathPreviewRenderer.SetPosition(i, path[i].Position);
            }
        }
    }
}