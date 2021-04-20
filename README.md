# Simulation-of-Universal-Gravitation
***
## Simulation Sctipts of universal gravitation and the solar system with Unity
   - **万有引力を実装した太陽系シミュレーション with Unity**
## > use **Unity-version 2019.4.20(LTS)**

### 目次
> - [概要](#description)
> - [SimpleScriptSceneについて(メイン)](#simplescr)
> - [](#)
> - [](#)
> - [](#)
> - [取り込む際の注意点](#importwarning)
> - [クレジット表記](#credit)


#### <h2 id="description">概要</h2>

**:white_check_mark:このリポジトリは、あくまで万有引力をUnityで実装するという内容が土台になっています。**

**:arrow_right:それを使って、疑似的な太陽系のシミュレーションを行っています。**
構成は以下の通りです:
|Scene name|内容|
|:---|:---|
|`SimpleScriptScene`|ベースとなる[`UniversalGravitationController`](Simulation-of-Universal-Gravitation/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)スクリプトのみを実装したScene|
|`DemoScene`|[`UniversalGravitationController`](Simulation-of-Universal-Gravitation/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)を応用して、太陽系をシミュレーションしているScene|

#### <h2 id="simplescr">SimpleScriptSceneについて(メイン)</h2>

##### SimpleScriptSceneを正しく`Play`させるための前準備 ――少し手を加えるだけです:open_hands:


> なぜ手を加える必要がある？

何もいじらずそのまま`Play`すると、Scene内のObjectらはすべてそのまま落下してしまいます。
これは、`UniversalGravitationController`の変数リストが空っぽになっていて、これにScene内のObjectらを加えていないためです。

> 修正方法
1. `SimpleScriptScene`内のHierarchyから`UniversalGravitationDirector`Objectを選択し、inspectorに`UniversalGravitationController`スクリプトを表示させます。
2. スクリプト上方の`Add all Rigidbodies from the scene`(日本語に切り替えたなら`シーン内の全Rigidbodyを追加`)をクリックします。
   - このボタンは、今現在開いているSceneの中からRigidbodyコンポーネントを持つすべてのGameObjectを`GravityTarget_obj`listに一括に追加するボタンです。重複がある場合は追加されないようになっているので既にいくつかlistに追加されていても2個(2 Elements)以上同じRigidbodyが入ることはありません。

#### <h2 id="importwarning">取り込む際の注意点</h2>
Unityに取り込んだ際に、以下の写真のようなエラーまたは警告文:warning:が表示される場合があります。ですが、Consoleタブの`Clear`を押すと消え、そのまま再生できるのであれば問題ありません。再生ボタンを押すと`All compiler errors have to fixed before you can enter playmode!`という表示文でストップされてしまうようであれば、対処する必要があります。恐らくUnity versionが異なっているためかもしれません。
![ErrorWarning](https://user-images.githubusercontent.com/81568941/115403161-21d58e00-a227-11eb-8988-b3644b3ebcba.png)


#### <h2 id="credit">クレジット表記</h2>
   - `DemoScene`内の天体でTextureを外部サイトから引用したものは以下の通りです

[earth_pseudo](https://www.solarsystemscope.com/textures/)

[mars_pseudo](https://www.solarsystemscope.com/textures/)

[venus_pseudo](https://www.solarsystemscope.com/textures/)

[ganimede_pseudo](http://www.planetaryvisions.com/Texture_map.php?pid=206)

[jupyter_pseudo](https://www.solarsystemscope.com/textures/)

[sun_pseudo](https://www.solarsystemscope.com/textures/)

[comet_pseudo](https://www.solarsystemscope.com/textures/)

`DemoScene`内のSkyboxは以下の*Asset*を使用
- [skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633) : **Skybox Series Free**

- post processing も使用しています
