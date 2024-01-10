namespace Pathfinding.Runtime
{
    public readonly struct NodeIdentity
    {
        public static readonly NodeIdentity Invalid = new(-1);

        public readonly int Context;

        public NodeIdentity(int context)
        {
            Context = context;
        }
    }
}