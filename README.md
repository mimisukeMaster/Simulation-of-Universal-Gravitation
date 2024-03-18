# Simulation-of-Universal-Gravitation
<br><p align="left">
    [<img src="https://img.shields.io/github/stars/mimisukeMaster/Simulation-of-Universal-Gravitation">](https://github.com/mimisukeMaster/Simulation-of-Universal-Gravitation/stargazers)
    [<img src="https://img.shields.io/badge/PRs-welcome-orange?&logo=github">](https://github.com/mimisukeMaster/Simulation-of-Universal-Gravitation/pulls)
    [<img  src="https://img.shields.io/github/license/mimisukeMaster/Simulation-of-Universal-Gravitation">](https://www.apache.org/licenses/)
    <img src="https://img.shields.io/badge/made with-Unity2023.3.x-blue.svg?&logo=unity"><br>
    <img src="https://img.shields.io/github/repo-size/mimisukeMaster/Simulation-of-Universal-Gravitation?color=ff69b4">
    [<img src="https://img.shields.io/static/v1?logo=visualstudiocode&label=&message=Open%20in%20Visual%20Studio%20Code&labelColor=2c2c32&color=007acc&logoColor=007acc">](https://open.vscode.dev/mimisukeMaster/Simulation-of-Universal-Gravitation)
    </p>


## Description
This repo is a sample project that simulates universal gravitation in Unity.

The main process is the calculation of the universal gravitation and first cosmic velocity based on physical formulae.

## Environments
### Unity version: Unity2023.3.21 LTS


## About [SimpleScriptScene](/Assets/Scenes/SimpleScriptScene.unity)
This scene simulates only the state of universal gravitation between objects.<br>

If the force is very small due to mass, universal gravitational constant, etc., adjust the value of the parameter `COEFFICIENT` in the [UniversalGravitationController.cs](/Assets/Scripts/MainScripts/UnivarsalGravitationController.cs) from the inspector view accordingly.


## About [DemoScene](/Assets/Scenes/Demo/DemoScene.unity)
In addition to simulating universal gravitation, this scene reproduces a pseudo-solar system by giving an object an initial velocity at first cosmic speed.<br>

The initial velocity given to the comet is calculated separately in [CometLauncher](/Assets/Scripts/DemoScripts/CometLauncher.cs), taking an appropriate value between the first cosmic velocity given to the planet and the second cosmic velocity at which it flies away from the star. This can be adjusted with the parameter `COMET_COEFFICIENT` in the [CometLauncher.cs](/Assets/Scripts/DemoScripts/CometLauncher.cs) from the inspector view accordingly.

**ATTENTION:**
- The sun's mass must be very large beyond the upper limit of Rigidbody.mass or it will move under the effect of the planets, so alternatively, the sun's Rigidbody.isKinematic is set to true.
- The reason why the tracks don't make a nice circle is that the values are slightly different due to calculation errors in the physics engine.
- As this is only a simplified simulation, the planetary rotations, inclination of the orbital planes, mass and distance ratios between the planets, etc. are **not reproduced or correct** in this project.


## LICENSE
This project is under the **[MIT License](LICENSE)**.<br>

Adapted from wikipedia:
> *Permission is hereby granted, free of charge, to deal in this  software on an unrestricted basis. However, the copyright notice and this permission notice must appear on all copies or substantial portions of the software.
The author or copyright holder assumes no responsibility for the software.*

## Author
 みみすけ名人 mimisukeMaster<br>

 [<img src="https://img.shields.io/badge/-X-X.svg?style=flat-square&logo=X&logoColor=white&color=black">](https://twitter.com/mimisukeMaster)
[<img src="https://img.shields.io/badge/-ArtStation-artstation.svg?&style=flat-square&logo=artstation&logoColor=blue&color=gray">](https://www.artstation.com/mimisukemaster)
[<img src="https://img.shields.io/badge/-Youtube-youtube.svg?&style=flat-square&logo=youtube&logoColor=white&color=red">](https://www.youtube.com/channel/UCWnmp8t4GJzcjBxhtgo9rKQ)

## Credits
Imported from an website or AssetStore:
- https://www.solarsystemscope.com/textures
- http://www.planetaryvisions.com/Texture_map.php?pid=206
- https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633
