namespace Pathfinding.Runtime
{
    /// <summary>
    /// We will be representing each node with 'byte' identity value.
    /// Usage of bytes is safe since we are going to create node map on top of (1.0f, 1.0f, 1.0f) scaled plane.
    /// We already know that '100' nodes will be generated since the distance between plane corners is 10 by 10.
    ///
    /// However, to support bigger maps, usage of 'byte' is not correct. Totally depends on the usage situation.
    /// </summary>
    public readonly struct NodeIdentity
    {
        public readonly byte Value;

        public NodeIdentity(byte value)
        {
            Value = value;
        }
    }
}