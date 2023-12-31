using UnityEngine;

namespace Pathfinding.Runtime
{
    public readonly struct Node
    {
        // Size of a single node. Will not change at runtime.
        public static readonly Vector3 Size = Vector3.one / 2.0f;

        // Unique identity value for this node.
        public readonly NodeIdentity Identity;

        // The identities of the neighbor nodes to this node.
        public readonly NodeIdentity[] Neighbors;

        // Position of the node in world coordinates.
        public readonly Vector3 PositionInWorldCoordinates;

        public Node(NodeIdentity identity, NodeIdentity[] neighbors, Vector3 positionInWorldCoordinates)
        {
            Identity = identity;
            Neighbors = neighbors;
            PositionInWorldCoordinates = positionInWorldCoordinates;
        }
    }
}