# Unity Pathfinding

Contains the implementation of the [A*](https://en.wikipedia.org/wiki/A*_search_algorithm) pathfinding algorithm for
top-down 3D environments.

Uses [Manhattan distance](https://xlinux.nist.gov/dads/HTML/manhattanDistance.html) to calculate heuristics and doesn't provide support for diagonal movement. However, it is easy to
provide diagonal movement support since we just need to calculate four more extra neighbors for each node.

## License

[MIT License](https://github.com/iozsaygi/unity-pathfinding/blob/main/LICENSE)