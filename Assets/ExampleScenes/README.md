# Example Scenes
This folder contains a number of demos for the Unity DualPanto framework.

### Force Field
Contains a force field. If the upper handle enters the area of the force field it will be gently pushed upwards.

### Force Field Labyrinth
This scene contains a small game. The player must reach the bottom of the area without being pulled in by the force fields.

### Haptic Texture
A haptic texture consists of rails in parallel or in a grid. Moving over a texture creates a haptic sensation that can be used to make areas more recognizable. Make sure the haptic texture is set to the same layer as the object it is attached to.

### Introduction
Contains multiple objects that will be introduced at the start of the game.

### Many Obstacles
A scene with a large amount of obstacles that vary in size. This scene uses the `ObstacleSphere`, as it contains more obstacles that can be rendered on the device at once. The sphere will only activate obstacles that are close to the handles.

### Movement
Contains two spheres, each can be controlled by one handle. Pressing `F` will toggle the handles freezing in place.

### MoveToPosition
Contains two cubes, each handle will move to one cube, one handle will be freed afterwards. Press `Space` to move the handle to the cubes again.

### MovingObstacle
Contains an obstacle that moves.

### Obstacles
Contains a player that can be moved with the upper panto handle. The scene contains multiple obstacles, that are registered on the Panto, the player can collide with them. Press `D` to disable all obstacles, press `E` to enable them again.

### PerceptionCone
This scene includes a demo for a navigation technique. The "Perception Cone" is attached to the player. Each "RoomElement" will repeat it's name while in focus of the cone. Walls can be placed to obstruct the cone, if they are on the correct layer.

### Rotation
Contains two objects. The upper one can be rotated by turning the upper handle. The lower objects is rotated via a script and will send it's rotation to the Panto.

### SwitchTo
Contains a cube that is moved via a script. The lower handle will move towards it and then follow it's movement.