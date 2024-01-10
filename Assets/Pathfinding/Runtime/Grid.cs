using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Pathfinding.Runtime
{
    public class Grid
    {
        public readonly Node[] Nodes;

        private readonly Vector2 nodeSize;
        private readonly Vector2Int mapSize;
        private readonly float spacingBetweenNodes;

        public Grid(Vector2Int mapSize, Vector2 nodeSize, float spacingBetweenNodes)
        {
            Debug.Assert(!mapSize.Equals(Vector2Int.zero));
            Debug.Assert(!nodeSize.Equals(Vector2Int.zero));
            Debug.Assert(spacingBetweenNodes >= 0.0f);

            Nodes = new Node[mapSize.x * mapSize.y];

            this.mapSize = mapSize;
            this.nodeSize = nodeSize;
            this.spacingBetweenNodes = spacingBetweenNodes;

            Populate();
        }

        private void Populate()
        {
            // Calculate offset for the node.
            var nodePositionOffset =
                new Vector3(nodeSize.x + spacingBetweenNodes, 0.0f, nodeSize.y + spacingBetweenNodes);

            // Figure out the center.
            var gridCenter =
                new Vector3((mapSize.x - 1) * 0.5f * nodePositionOffset.x, 0.0f,
                    (mapSize.y - 1) * 0.5f * nodePositionOffset.z);

            for (var row = 0; row < mapSize.y; row++)
            {
                for (var col = 0; col < mapSize.x; col++)
                {
                    var index = row * mapSize.x + col;
                    var nodeIdentity = new NodeIdentity(index);

                    var nodePosition =
                        new Vector3(col * nodePositionOffset.x, 0.0f, row * nodePositionOffset.z) - gridCenter;

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