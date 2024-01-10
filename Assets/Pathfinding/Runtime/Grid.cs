using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    public class Grid
    {
        public readonly Node[] Nodes;
        public readonly Vector2 NodeSize;

        private readonly Vector2Int mapSize;
        private readonly float spacingBetweenNodes;

        private Vector3 center;

        public Grid(Vector2Int mapSize, Vector2 nodeSize, float spacingBetweenNodes)
        {
            Debug.Assert(!mapSize.Equals(Vector2Int.zero));
            Debug.Assert(!nodeSize.Equals(Vector2Int.zero));
            Debug.Assert(spacingBetweenNodes >= 0.0f);

            Nodes = new Node[mapSize.x * mapSize.y];
            NodeSize = nodeSize;

            this.mapSize = mapSize;
            this.spacingBetweenNodes = spacingBetweenNodes;

            Populate();
        }

        public void WorldPointToNode(Vector3 worldPoint, out Node node)
        {
            var positionInGrid =
                new Vector2Int(Mathf.RoundToInt((worldPoint.x + center.x) / (NodeSize.x + spacingBetweenNodes)),
                    Mathf.RoundToInt((worldPoint.z + center.z) / (NodeSize.y + spacingBetweenNodes)));

            if (positionInGrid.x >= 0 && positionInGrid.x < mapSize.x && positionInGrid.y >= 0 &&
                positionInGrid.y < mapSize.y)
            {
                var nodeIndex = positionInGrid.y * mapSize.x + positionInGrid.x;
                node = Nodes[nodeIndex];
            }
            else
            {
                node = new Node(NodeIdentity.Invalid, null, Vector3.zero);
            }
        }

        private void Populate()
        {
            // Calculate offset for the node.
            var nodePositionOffset =
                new Vector3(NodeSize.x + spacingBetweenNodes, 0.0f, NodeSize.y + spacingBetweenNodes);

            // Figure out the center.
            center = new Vector3((mapSize.x - 1) * 0.5f * nodePositionOffset.x, 0.0f,
                (mapSize.y - 1) * 0.5f * nodePositionOffset.z);

            for (var row = 0; row < mapSize.y; row++)
            {
                for (var col = 0; col < mapSize.x; col++)
                {
                    var index = row * mapSize.x + col;
                    var nodeIdentity = new NodeIdentity(index);

                    var nodePosition =
                        new Vector3(col * nodePositionOffset.x, 0.0f, row * nodePositionOffset.z) - center;

                    CalculateNeighbors(nodeIdentity, out var neighbors);

                    Nodes[index] = new Node(nodeIdentity, neighbors, nodePosition);
                }
            }
        }

        private void EnsureNodeIdentityIsValid(NodeIdentity nodeIdentity, out bool isNodeIdentityValid)
        {
            isNodeIdentityValid = nodeIdentity.Context > 0 && nodeIdentity.Context < Nodes.Length;
        }

        private void CalculateNeighbors(NodeIdentity nodeIdentity, out NodeIdentity[] neighbors)
        {
            neighbors = new NodeIdentity[4];

            // Top node.
            {
                var topNodeIdentityAssumption = new NodeIdentity(nodeIdentity.Context + mapSize.x);
                EnsureNodeIdentityIsValid(topNodeIdentityAssumption, out var isTopNodeIdentityAssumptionValid);
                neighbors[0] = isTopNodeIdentityAssumptionValid ? topNodeIdentityAssumption : NodeIdentity.Invalid;
            }

            // Bottom node.
            {
                var bottomNodeIdentityAssumption = new NodeIdentity(nodeIdentity.Context - mapSize.x);
                EnsureNodeIdentityIsValid(bottomNodeIdentityAssumption, out var isBottomNodeIdentityAssumptionValid);
                neighbors[1] = isBottomNodeIdentityAssumptionValid
                    ? bottomNodeIdentityAssumption
                    : NodeIdentity.Invalid;
            }

            // Left node.
            {
                var leftNodeIdentityAssumption = new NodeIdentity(nodeIdentity.Context - 1);
                EnsureNodeIdentityIsValid(leftNodeIdentityAssumption, out var isLeftNodeIdentityAssumptionValid);
                neighbors[2] = isLeftNodeIdentityAssumptionValid ? leftNodeIdentityAssumption : NodeIdentity.Invalid;
            }

            // Right node.
            {
                var rightNodeIdentityAssumption = new NodeIdentity(nodeIdentity.Context + 1);
                EnsureNodeIdentityIsValid(rightNodeIdentityAssumption, out var isRightNodeIdentityAssumptionValid);
                neighbors[3] = isRightNodeIdentityAssumptionValid ? rightNodeIdentityAssumption : NodeIdentity.Invalid;
            }
        }
    }
}