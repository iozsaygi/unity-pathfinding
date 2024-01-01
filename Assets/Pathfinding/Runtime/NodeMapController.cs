using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    [DisallowMultipleComponent]
    public class NodeMapController : MonoBehaviour
    {
        // The target plane that we aim to generate nodes on.
        [SerializeField] private MeshFilter meshFilter;

        // Editor debugging properties.
        [SerializeField, Min(0.1f)] private float gizmosSphereRenderRadius;

        // Calculated count values for node generation.
        [field: SerializeField] public int HorizontalNodeCount { get; private set; }
        [field: SerializeField] public int VerticalNodeCount { get; private set; }

        // Caching corner points of plane for debugging.
        [field: SerializeField] public Vector3 TopLeftCornerInWorldCoordinates { get; private set; }
        [field: SerializeField] public Vector3 TopRightCornerInWorldCoordinates { get; private set; }
        [field: SerializeField] public Vector3 BottomLeftCornerInWorldCoordinates { get; private set; }

        // Origin point to start node generation.
        private Vector3 nodeGenerationOrigin;

        // Node array to store generated nodes.
        private Node[] nodes;

        private void Awake()
        {
            // Calculates required values for node generation.
            Warmup();

            GenerateNodes();
        }

        private void OnDrawGizmosSelected()
        {
            // Render nodes.
            if (nodes == null) return;
            Gizmos.color = Color.magenta;
            for (byte i = 0; i < nodes.Length; i++)
            {
                Gizmos.DrawSphere(nodes[i].PositionInWorldCoordinates, gizmosSphereRenderRadius);
            }
        }

        public void WorldPointToNode(Vector3 worldPoint, out Node node)
        {
            var horizontalPercentage = Mathf.Clamp01((worldPoint.x - TopLeftCornerInWorldCoordinates.x) /
                                                     (TopRightCornerInWorldCoordinates.x -
                                                      TopLeftCornerInWorldCoordinates.x));

            var verticalPercentage = Mathf.Clamp01((worldPoint.z - TopLeftCornerInWorldCoordinates.z) /
                                                   (BottomLeftCornerInWorldCoordinates.z -
                                                    TopLeftCornerInWorldCoordinates.z));

            var horizontalIndex = Mathf.FloorToInt(horizontalPercentage * HorizontalNodeCount);
            var verticalIndex = Mathf.FloorToInt(verticalPercentage * VerticalNodeCount);
            var nodeIndex = verticalIndex * HorizontalNodeCount + horizontalIndex;

            if (nodeIndex >= 0 && nodeIndex < nodes.Length)
            {
                node = nodes[nodeIndex];
            }
            else
            {
                node = default;
            }
        }

        public void NodeFromIdentity(NodeIdentity nodeIdentity, out Node node)
        {
            node = nodeIdentity.Equals(NodeIdentity.InvalidIdentity)
                ? node = new Node(NodeIdentity.InvalidIdentity, null, default)
                : nodes[nodeIdentity.Value];
        }

        private void Warmup()
        {
            // The bounds of the plane, will be used to calculate corner points of the plane.
            var meshFilterBounds = meshFilter.sharedMesh.bounds;

            // Cache the top left corner of the plane.
            TopLeftCornerInWorldCoordinates = meshFilter.transform.TransformPoint(meshFilterBounds.min +
                new Vector3(0.0f, 0.0f, meshFilterBounds.size.z));

            // Cache the top right corner of the plane.
            TopRightCornerInWorldCoordinates =
                meshFilter.transform.TransformPoint(meshFilterBounds.min +
                                                    new Vector3(meshFilterBounds.size.x, 0.0f,
                                                        meshFilterBounds.size.z));

            // Cache the bottom left corner of the plane.
            BottomLeftCornerInWorldCoordinates = meshFilter.transform.TransformPoint(meshFilterBounds.min);

            // Calculate how many nodes we need to generate horizontally.
            HorizontalNodeCount =
                Mathf.FloorToInt(Vector3.Distance(TopLeftCornerInWorldCoordinates, TopRightCornerInWorldCoordinates) /
                                 Node.Size.x);

            // Calculate how many nodes we need to generate vertically. (For 'Z' axis)
            VerticalNodeCount =
                Mathf.FloorToInt(Vector3.Distance(TopLeftCornerInWorldCoordinates, BottomLeftCornerInWorldCoordinates) /
                                 Node.Size.y);
        }

        private void GenerateNodes()
        {
            nodes = new Node[HorizontalNodeCount * VerticalNodeCount];

            var generationStep = new Vector3(Node.Size.x / 2.0f, 0.0f, -(Node.Size.y / 2.0f));
            nodeGenerationOrigin = TopLeftCornerInWorldCoordinates + generationStep;

            var iterator = 0;
            var verticalOffset = 0;

            for (var i = 0; i < HorizontalNodeCount * VerticalNodeCount; i++)
            {
                var placement = new Vector3(nodeGenerationOrigin.x + iterator, 0.0f,
                    nodeGenerationOrigin.z - verticalOffset);

                var nodeIdentity = new NodeIdentity(i);
                CalculateNeighborIdentities(nodeIdentity, out var neighbors);
                nodes[i] = new Node(nodeIdentity, neighbors, placement);

                iterator++;
                if (iterator != HorizontalNodeCount) continue;
                verticalOffset += 1;
                iterator = 0;
            }

            Debug.Log($"Generated {HorizontalNodeCount} nodes horizontally and {VerticalNodeCount} nodes vertically");
        }

        private void CalculateNeighborIdentities(NodeIdentity nodeIdentity, out NodeIdentity[] neighbors)
        {
            // Assuming we will not be supporting diagonal movement, we'll only consider top, bottom, left and right nodes as neighbor.
            neighbors = new NodeIdentity[4];

            // Top node identity is current - horizontal node count.
            var topNeighborNodeIdentity = new NodeIdentity(nodeIdentity.Value - HorizontalNodeCount);
            EnsureValidNodeIdentity(ref topNeighborNodeIdentity);
            neighbors[0] = topNeighborNodeIdentity;

            // Bottom node identity is current + horizontal node count.
            var bottomNeighborNodeIdentity = new NodeIdentity(nodeIdentity.Value + HorizontalNodeCount);
            EnsureValidNodeIdentity(ref bottomNeighborNodeIdentity);
            neighbors[1] = bottomNeighborNodeIdentity;

            // Left node identity is current - 1.
            if (nodeIdentity.Value % HorizontalNodeCount == 0)
            {
                neighbors[2] = NodeIdentity.InvalidIdentity;
            }
            else
            {
                var leftNeighborNodeIdentity = new NodeIdentity(nodeIdentity.Value - 1);
                EnsureValidNodeIdentity(ref leftNeighborNodeIdentity);
                neighbors[2] = leftNeighborNodeIdentity;
            }

            // Right node identity is current + 1.
            if (nodeIdentity.Value % HorizontalNodeCount == HorizontalNodeCount - 1)
            {
                neighbors[3] = NodeIdentity.InvalidIdentity;
            }
            else
            {
                var rightNeighborNodeIdentity = new NodeIdentity(nodeIdentity.Value + 1);
                EnsureValidNodeIdentity(ref rightNeighborNodeIdentity);
                neighbors[3] = rightNeighborNodeIdentity;
            }
        }

        private void EnsureValidNodeIdentity(ref NodeIdentity nodeIdentity)
        {
            if (nodeIdentity.Value < 0)
            {
                nodeIdentity = NodeIdentity.InvalidIdentity;
                return;
            }

            if (nodeIdentity.Value >= nodes.Length)
            {
                nodeIdentity = NodeIdentity.InvalidIdentity;
            }
        }
    }
}