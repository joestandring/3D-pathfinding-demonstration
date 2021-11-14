# 3D Pathfinding Demonstration
A test environment for comparing 3D pathdinding algorythms in a voxelized environment. Allows for comparing the 4 most used algorihtms (A*, Dijkstra, Breadth-First-Search, Greedy Best-First-Search) in a standardised environment and exporting test results.

THIS PROJECT IS A WORK IN PROGRESS. MANY CORE FEATURES ARE NOT COMPLETE.

![example](https://raw.githubusercontent.com/joestandring/3D-pathfinding-demonstration/main/example.png)

This tool was initially developed as a test environment for research into the efficiency of various pathfinding algorithms in a voxelized 3D space, and therefore the original "levels" used to test the algorithms have been included in the "research-build" branch. This branch will not be developed, however is kept to allow reproduction of test results.

## Features
- Implementation of the 4 most widely used pathfinding algorithms
  - A*
  - Dijkstra
  - Breadth-First-Search
  - Greedy Best-First-Search
- Exporting results to .csv files
- Full camera control
### Planned features
- Level editor
- On-the-fly level and variable configuration

## Installation
Currently, the build will not run correctly outside of the Unity editor. This means that the project needs to be imported into Unity and run via play mode.
1. Download the latest .unitypackage in releases
2. Open the package in Unity
## Useage
As the level editor is being worked on, changes to the environment need to be made in-code. This will be updated.
1. Press the "play" button in the Unity editor
2. Control the camera by following the inputs shown on the UI
3. Press space to run the current algorithm. This will freeze control until the algorithm has finished
4. Results will be shown in the results panel and exported into the Results directory as a .csv file with the current timestamp
5. The arrow keys can be used to step through a visualisation of the pathfinding algorithm
