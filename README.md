# Unity Pathfinding

Contains the implementation of the [A*](https://en.wikipedia.org/wiki/A*_search_algorithm) pathfinding algorithm for
top-down 3D environments.

Uses [Manhattan distance](https://xlinux.nist.gov/dads/HTML/manhattanDistance.html) to calculate heuristics and doesn't
provide support for diagonal movement. However, it is easy to
provide diagonal movement support since we just need to calculate four more extra neighbors for each node.

## Preview

Captured previews on a ``50x30`` grid map.

[![Video preview](https://img.youtube.com/vi/koZuPMJewcQ?si=Z6Wt44NLBWvpq2uf/0.jpg)](https://www.youtube.com/watch?v=koZuPMJewcQ?si=Z6Wt44NLBWvpq2uf)
![First preview](https://github.com/iozsaygi/unity-pathfinding/blob/main/Media/FirstPreview.gif)
![Second preview](https://github.com/iozsaygi/unity-pathfinding/blob/main/Media/SecondPreview.gif)

## Optimization ideas

- Implementing min-heap to quickly find reliable nodes to search for
- Pre-baking node generation during editor and just executing pathfinding at runtime

## License

[MIT License](https://github.com/iozsaygi/unity-pathfinding/blob/main/LICENSE)