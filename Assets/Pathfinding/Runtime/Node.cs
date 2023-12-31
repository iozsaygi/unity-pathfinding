namespace Pathfinding.Runtime
{
    public readonly struct Node
    {
        // Unique identity value for this node.
        public readonly NodeIdentity Identity;

        // The identities of the neighbor nodes to this node.
        public readonly NodeIdentity[] Neighbors;

        public Node(NodeIdentity identity, NodeIdentity[] neighbors)
        {
            Identity = identity;
            Neighbors = neighbors;
        }
    }
}