# Simulation-of-Universal-Gravitation
***
## Simulation Sctipts of universal gravitation and the solar system with Unity
   - **万有引力を実装した太陽系シミュレーション with Unity**
## > use **Unity-version 2019.4.20(LTS)**

### 目次
> - [概要](#description)
> - [UniversalGravitationController.csスクリプトについて](#ugcontrollercs)
> - [SimpleScriptSceneについて(メインシーン)](#simplescrscene)
> - [DemoSceneについて(シミュレーションシーン)](#demoscene)
> - [](#)
> - [](#)
> - [取り込む際の注意点](#importwarning)
> - [クレジット表記](#credit)


#### <h2 id="description">概要</h3>

**:white_check_mark:このリポジトリは、あくまで万有引力をUnityで実装するという内容が土台になっています。**

**:arrow_right:それを使って、疑似的な太陽系のシミュレーションを行っています。**

構成は以下の通りです:
|Scene name|内容|
|:---|:---|
|[`SimpleScriptScene`](/Simulation_of_Universal_Gravitation/Assets/Scenes/SimpleScriptScene.unity)|ベースとなる[`UniversalGravitationController.cs`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)スクリプトのみを実装したScene|
|[`DemoScene`](/Simulation_of_Universal_Gravitation/Assets/Scenes/Demo/DemoScene.unity)|[`UniversalGravitationController.cs`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)を活用して、太陽系をシミュレーションしているScene|

基盤となるScriptのみを実装した[`SimpleScriptScene`](/Simulation_of_Universal_Gravitation/Assets/Scenes/SimpleScriptScene.unity)は、空間内の物体がひきつけられることのみを確かめているシーンです。このままだと単純すぎるので、[`DemoScene`](/Simulation_of_Universal_Gravitation/Assets/Scenes/Demo/DemoScene.unity)でそのScriptに加え適切な[初速度](#firstunivelo)を与えるScript([PlanetLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/PlanetLauncher.cs),[CometLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/CometLauncher.cs)など)を追加で用いて天体の動きを再現しています。このSceneをいじってみると面白いと思います(詳しくは[下方](#demoscene)へ↓)

#### <h2 id="ugcontrollercs">[UniversalGravitationController.cs]((/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs))スクリプトについて</h2>
> <h3>inspector上での見え方(<b>Editor拡張の使用</b>)</h3>

Unityに取り込んだ初期状態だと、恐らくinspector上では以下の画像のようになると思います:
![UG_ControllerFirstPreview](https://user-images.githubusercontent.com/81568941/115720022-9d664500-a3b7-11eb-82dd-05a7ae87be8d.png)

簡単に説明します
|No.|仮名称|説明|
|:---|---:|:---|
|1.| **English/Japanese Button**|パラメータの表示言語を日本語⇔英語に切り替えるボタンです。|
|2.|**Add all Rigi....scene Button**|下の`GravityTargets_obj`listへSceneから全てのRigidbodyを追加するボタンです(詳しくは[下方](#aarfts)へ↓)|
|3.|**GravityTarget_obj list**|このScriptの影響を受けるRigidbody(-ComponentのついたGameObject)を入れるlist|
|4.|**advanced Setting**|このScriptの処理の内部では、実際の万有引力定数(6.67408*10^-11  m^3 kg^−1 s^−2)を使用しているため、Rigidbody.massが小さいと視覚的に分かりづらくなってしまいます。そこで、このタイトル以下からは、それを改善するため万有引力定数を意図的に小さくする設定を行います。|
|5.|****||
|6.|****||
>  editor拡張、処理概要


#### <h2 id="simplescrscene">SimpleScriptSceneについて(メインシーン)</h2>

##### SimpleScriptSceneを正しく`Play`させるための前準備 ――少し手を加えるだけです:open_hands:


> なぜ手を加える必要がある？

何もいじらずそのまま`Play`すると、Scene内のObjectらはすべてそのまま落下してしまいます。
これは、[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)の変数リストが空っぽになっていて、これにScene内のObjectらを加えていないためです。

> 修正方法
1. `SimpleScriptScene`内のHierarchyから`UniversalGravitationDirector`Objectを選択し、inspectorに[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)スクリプトを表示させます。
2. スクリプト上方の`Add all Rigidbodies from the scene`(日本語に切り替えたなら`シーン内の全Rigidbodyを追加`)をクリックします。
   - <h4 id="aarfts">このボタンは、今現在開いているSceneの中からRigidbodyコンポーネントを持つすべてのGameObjectを`GravityTarget_obj`listに一括に追加するボタンです。</h4>重複がある場合は追加されないようになっているので既にいくつかlistに追加されていても2個(2 Elements)以上同じRigidbodyが入ることはありません。


#### <h2 id="demoscene">DemoSceneについて(シミュレーションシーン)</h2>
> Coming Soon >_<
> *<h4 id="firstunivelo">初速度の与え方等...記述中...</h4>*



#### <h2 id="importwarning">取り込む際の注意点</h2>
Unityに取り込んだ際に、以下の写真のようなエラーまたは警告文:warning:が表示される場合があります。ですが、Consoleタブの`Clear`を押すと消え、そのまま再生できるのであれば問題ありません。再生ボタンを押すと`All compiler errors have to fixed before you can enter playmode!`という表示文でストップされてしまうようであれば、対処する必要があります。恐らくUnity versionが異なっているためかもしれません。

![ErrorWarning](https://user-images.githubusercontent.com/81568941/115403161-21d58e00-a227-11eb-8988-b3644b3ebcba.png)





#### <h2 id="credit">クレジット表記</h2>
   - `DemoScene`内の天体でTextureを外部サイトから引用したものは以下の通りです

- [earth_pseudo](https://www.solarsystemscope.com/textures/)
- [mars_pseudo](https://www.solarsystemscope.com/textures/)
- [venus_pseudo](https://www.solarsystemscope.com/textures/)
- [ganimede_pseudo](http://www.planetaryvisions.com/Texture_map.php?pid=206)
- [jupyter_pseudo](https://www.solarsystemscope.com/textures/)
- [sun_pseudo](https://www.solarsystemscope.com/textures/)
- [comet_pseudo](https://www.solarsystemscope.com/textures/)

`DemoScene`内のSkyboxは以下の*Asset*を使用
- [skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633) : **Skybox Series Free**

- post processing も使用しています
