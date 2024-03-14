# Simulation-of-Universal-Gravitation
<br><p align="left">
    [<img src="https://img.shields.io/github/stars/mimisukeMaster/Simulation-of-Universal-Gravitation">](https://github.com/mimisukeMaster/Simulation-of-Universal-Gravitation/stargazers)
    [<img src="https://img.shields.io/badge/PRs-welcome-orange?&logo=github">](https://github.com/mimisukeMaster/OsakanaFlock/pulls)
    [<img  src="https://img.shields.io/github/license/mimisukeMaster/Simulation-of-Universal-Gravitation">](https://www.apache.org/licenses/)
    <img src="https://img.shields.io/badge/made with-Unity2023.3.x-blue.svg?&logo=unity"><br>
    <img src="https://img.shields.io/github/repo-size/mimisukeMaster/Simulation-of-Universal-Gravitation?color=ff69b4">
    [<img src="https://img.shields.io/static/v1?logo=visualstudiocode&label=&message=Open%20in%20Visual%20Studio%20Code&labelColor=2c2c32&color=007acc&logoColor=007acc">](https://open.vscode.dev/mimisukeMaster/Simulation-of-Universal-Gravitation)
    </p>


## Description
This repo is a sample project that simulates universal gravitation in Unity.

## Environments
### Unity version: Unity2023.3.21 LTS

## Directory in `/Assets`
<pre>
├─Materials
│  │  Comet.mat
│  │  Earth.mat
│  │  Ganimede.mat
│  │  Jupyter.mat
│  │  Mars.mat
│  │  Venus_atmosphere.mat
│  └─ Venus_surface.mat
│
├─Scenes
│  ├─Demo
│  │  ├─Demo_Skybox
│  │  │  └─ CosmicCoolCloud.mat
│  │  │
│  │  ├─Demo_Scene
│  │  │  │  LightingData.asset
│  │  │  └─ RefrectionProbe-0.exr
│  │  │
│  │  ├─Demo_scene_profiles
│  │  │  └─ Post-process Volume Profile.asset
│  │  │
│  │  └─ DemoScene.unity
│  │
│  └── SimpleScriptScene.unity
│
├─Scripts
│  ├─DemoScripts
│  │  │  CometLauncher.cs
│  │  │  DebugLineRenderer.cs
│  │  └─ PlanetLauncher.cs
│  │
│  └─MainScripts
│     └─ UniversalGravitationController.cs
│  
└─Textures
   │  Comet.jpg
   │  Earth.jpg
   │  Earth_normalmap.tif
   │  Extrasolarobject.jpg
   │  Ganimede.png
   │  Jupyter.jpg
   │  Mars.jpg
   │  Sun.jpg
   └─ Venus.jpg

</pre>

## About SimpleScriptScene
This scene simulates only the state of universal gravitation between objects.<br>

If the force is very small due to mass, universal gravitational constant, etc., adjust the value of the parameter `COEFFICIENT` in the [UniversalGravitationController.cs](/Assets/Scripts/MainScripts/UnivarsalGravitationController.cs) from the inspector view accordingly.


## About DemoScene
In addition to simulating universal gravitation, this scene reproduces a pseudo-solar system by giving an object an initial velocity at first cosmic speed.<br>

The initial velocity given to the comet is calculated separately in [CometLauncher](/Assets/Scripts/DemoScripts/CometLauncher.cs), taking an appropriate value between the first cosmic velocity given to the planet and the second cosmic velocity at which it flies away from the star. This can be adjusted with the parameter `COMET_COEFFICIENT` in the [CometLauncher.cs](/Assets/Scripts/DemoScripts/CometLauncher.cs) from the inspector view accordingly.

**ATTENTION:**
- The sun's mass must be very large beyond the upper limit of Rigidbody.mass or it will move under the effect of the planets, so alternatively, the sun's Rigidbody.isKinematic is set to true.


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

### Credits
Imported from an external site or AssetStore:
- https://www.solarsystemscope.com/textures
- https://www.solarsystemscope.com/textures
- https://www.solarsystemscope.com/textures
- http://www.planetaryvisions.com/Texture_map.php?pid=206
- https://www.solarsystemscope.com/textures/
- https://www.solarsystemscope.com/textures/
- https://www.solarsystemscope.com/textures
- https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

---


#### <h2 id="importwarning">取り込む際の注意点</h2>
【注意1】ZIP形式でDownloadした後、それを展開するときに、ファイルをコピーできません:warning:という表示が出てくるかもしれません(以下の写真のように)。

![ExtendError](https://user-images.githubusercontent.com/81568941/115995400-ff23ea80-a615-11eb-9751-64af0bf51907.png)

このようなWindowが出てきたら全てスキップを押してしまって構いません。
※何回か表示される場合があります。以下の動画のように全てスキップしてしまってください。

![SkipHereEdited](https://user-images.githubusercontent.com/81568941/115995299-b53b0480-a615-11eb-9eb7-ddf074377450.gif)

【注意2】Unityに取り込んだ際に、以下の写真のようなエラーまたは警告文:warning:が表示される場合があります。ですが、Consoleタブの`Clear`を押すと消え、そのまま再生できるのであれば問題ありません。再生ボタンを押すと`All compiler errors have to fixed before you can enter playmode!`という表示文でストップされてしまうようであれば、対処する必要があります。恐らくUnity versionが異なっているためかもしれません。

![ErrorWarning](https://user-images.githubusercontent.com/81568941/115403161-21d58e00-a227-11eb-8988-b3644b3ebcba.png)

