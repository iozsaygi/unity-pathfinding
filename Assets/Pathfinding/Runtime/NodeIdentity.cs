namespace Pathfinding.Runtime
{
    public readonly struct NodeIdentity
    {
        // The invalid value for node identities.
        public static readonly NodeIdentity InvalidIdentity = new(-1);

        public readonly int Value;

        public NodeIdentity(int value)
        {
            Value = value;
        }
    }
}