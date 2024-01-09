namespace Pathfinding.Runtime
{
    public readonly struct NodeIdentity
    {
        public readonly int Context;

        public NodeIdentity(int context)
        {
            Context = context;
        }
    }
}