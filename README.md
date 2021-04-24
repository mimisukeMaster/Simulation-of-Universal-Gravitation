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
|:---|------:|:---|
|1.| **English/Japanese Button**|パラメータの表示言語を日本語⇔英語に切り替えるボタン|
|2.|**Add all Rigi... scene Button**|下の`GravityTargets_obj`listへSceneから全てのRigidbodyを追加するボタン(詳しくは[下方](#aarfts)へ↓)|
|3.|**GravityTarget_obj list**|このScriptの影響を受けるRigidbody(-ComponentのついたGameObject)を入れるlist|
|4.|**advanced Setting Title**|このScriptは、実際の万有引力定数(6.67408*10^-11 [m^3 kg^−1 s^−2])を初めは使用して計算しているため、Rigidbody.massが小さいと視覚的に分かりづらくなります。そこで、このタイトル以下からは、それを改善するため万有引力定数を意図的に小さくする設定を行えます。|
|5.|**VisualSimulation Box**|advanced Settingを有効化するか指定できます(有効化しない場合これより下は操作できません)。|
|6.|**Exponent Parameter**|上のBoxを有効化した際に、どの程度視覚化するか(万有引力定数をどれほど大きくするか)を指定するもの|
> 補足
  - 3: **GravityTargets_obj list**について、この文字の上にカーソルをおくと*Tooltip*が表示されます。参考にしてください。
  - 5: **VisualSimulation Box** について、これにチェックを入れると*info*が表示され、説明文が現れます。同じく参考にしてください。

> <h3>内部処理の概要</h3>
万有引力の計算は以下の公式を用いて行っています:

![\begin{align*}
F= \G\frac{Mm}{r^2}
\end{align*}
](https://render.githubusercontent.com/render/math?math=%5Chuge+%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0AF%3D+%5CG%5Cfrac%7BMm%7D%7Br%5E2%7D%0A%5Cend%7Balign%2A%7D%0A)

**F** は万有引力の大きさ、**M**,**m** は2物体のそれぞれの質量、**r** は物体間の距離、**G** は万有引力定数。

万有引力計算は以下の部分:
``` csharp
for (int i = 0; i <= GravityTargets_obj.Count -1 ;)
        {
            for(int n = 0; n <= GravityTargets_obj.Count -1 ;)
            {
                if (i != n)
                {
                    Vector3 direction = (GravityTargets_obj[i].transform.position - GravityTargets_obj[n].transform.position);

                    double distance = (double)direction.magnitude;
                    distance *= distance;

                    double gravity = coefficient * GravityTargets_obj[i].mass * GravityTargets_obj[n].mass / distance;

                    float gravityf = (float)gravity; 
                    GravityTargets_obj[i].AddForce(-gravityf * direction.normalized, ForceMode.Force);
                }
                n++;
            }
            i++;
        }
```
互いに引き合う(両者に力が加わる)ように`for`文のなかに`for`文をいれています。そのため、処理が多少重くなります。
`Script`内の変数はそれぞれ以下に対応:
|公式の変数,値|Script内の変数,値|
|:---:|:---|
|***F***|`gravityf`(double->float変換後)|
|***M***|`GravityTargets_obj[i].mass`(順不同)|
|***m***|`GravityTargets_obj[n].mass`(順不同)|
|***r***|`distance`(二乗後)|
|***G***|`coefficient`|
``` csharp
coefficient * GravityTargets_obj[i].mass * GravityTargets_obj[n].mass / distance;
```
で、計算しています。


> <h3>Editor拡張の使用</h3>
このScriptはEditor拡張を使用しています。自作ですので、inspectorのサイズを変更するとボタンの位置が崩れたり文字が隠れてしまうことがありますがご了承ください。


#### <h2 id="simplescrscene">SimpleScriptSceneについて(メインシーン)</h2>

##### SimpleScriptSceneを正しく`Play`させるための前準備 ――少し手を加えるだけです:open_hands:


> <h3>なぜ手を加える必要がある？</h3>

何もいじらずそのまま`Play`すると、Scene内のObjectらはすべてそのまま落下してしまいます。
これは、[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)の変数リストが空っぽになっていて、これにScene内のObjectらを加えていないためです。

> <h3>修正方法</h3>
1. `SimpleScriptScene`内のHierarchyから`UniversalGravitationDirector`Objectを選択し、inspectorに[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)スクリプトを表示させます。
2. スクリプト上方の`Add all Rigidbodies from the scene`(日本語に切り替えたなら`シーン内の全Rigidbodyを追加`)をクリックします。
   - <h4 id="aarfts">このボタンは、今現在開いているSceneの中からRigidbodyコンポーネントを持つすべてのGameObjectを`GravityTarget_obj`listに一括に追加するボタンです。</h4>重複がある場合は追加されないようになっているので既にいくつかlistに追加されていても2個(2 Elements)以上同じRigidbodyが入ることはありません。


#### <h2 id="demoscene">DemoSceneについて(シミュレーションシーン)</h2>
> <h3>概要</h3>
DemoSceneでは[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)を使用するとともに、[PlanetLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/PlanetLauncher.cs),[CometLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/CometLauncher.cs)で周りの惑星に初速度を与えています。それにより、中央の`Sun_psedo`の周りを周回させています。
全ての天体が[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs).GravityTargets_Obj listに入っています。

各天体について
- **`Sun_psedo`**

`Sun_psedo`は実際の太陽ほど質量を大きくできないため、周りに天体があると多少移動していってしまいます。そのためRigidbodyを`IsKinematic`にしています。

- **`earth_pseudo, mars_pseudo, venus_pseudo, jupyter_pseudo, ganimede_pseudo`**

これらは[`UniversalGravitationController`](/Simulation_of_Universal_Gravitation/Assets/Scripts/_MainScripts/UnivarsalGravitationController.cs)に加え[PlanetLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/PlanetLauncher.cs)をアタッチさせています。`ganimede_pseudo`は`jupyter_pseudo`を周回の対象として計算しています。

- **`comet_pseudo`**

彗星のような楕円軌道をシミュレートしたものです。公転面を傾けています。初速度は[CometLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/CometLauncher.cs)で計算しています。
> <h3 id="firstunivelo">PlanetLauncher.csの初速導出方法</h3>
初速は第一宇宙速度の考えを基にしています。定義の中の、*地球*と*飛行物体*をそれぞれ*`Sun_pseudo`(太陽)*と*自身の惑星*として、太陽の周りを円軌道で周回するように導出しています。
第一宇宙速度の公式は

![\begin{align*}
v_{1}= \sqrt{\frac{GM}{r}}
\end{align*}](https://render.githubusercontent.com/render/math?math=%5CLarge+%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0Av_%7B1%7D%3D+%5Csqrt%7B%5Cfrac%7BGM%7D%7Br%7D%7D%0A%5Cend%7Balign%2A%7D)

![\begin{align*}
v_{1}
\end{align*}
](https://render.githubusercontent.com/render/math?math=%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0Av_%7B1%7D%0A%5Cend%7Balign%2A%7D%0A)は第一宇宙速度、**G**は万有引力定数、**M**は太陽の質量、**r**は太陽と惑星の距離。
なので、この公式に[PlanetLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/PlanetLauncher.cs)内の変数を対応させています。
|公式の変数|Script内の変数|
|:---:|:---|
|***v1***|`float initvelocityZ`|
|***G***|`var G`|
|***M***|`var M`|
|***r***|`var r`|
``` csharp
initVelocityZ = System.Math.Sqrt(G * M / r);
```
で、計算しています。
> <h3>CometLauncher.csの初速導出方法</h3>
計算は途中まで[PlanetLauncher.cs](Simulation_of_Universal_Gravitation/Assets/Scripts/DemoScripts/PlanetLauncher.cs)と同じですが、楕円軌道を描いてほしいので離心率![\begin{align*}
e<1
\end{align*}](https://render.githubusercontent.com/render/math?math=%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0Ae%3C1%0A%5Cend%7Balign%2A%7D)の範囲でなるべく大きめの値をとっています。
先ほどと同様にして、第二宇宙速度の*地球*と*飛行物体*をそれぞれ*`Sun_pseudo`(太陽)*と*自身の惑星*とし、公式は

![\begin{align*}
v_{2}= \sqrt{\frac{2GM}{r}}
\end{align*}](https://render.githubusercontent.com/render/math?math=%5CLarge+%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0Av_%7B2%7D%3D+%5Csqrt%7B%5Cfrac%7B2GM%7D%7Br%7D%7D%0A%5Cend%7Balign%2A%7D)

これは第一宇宙速度の![\begin{align*}
\sqrt{2}
\end{align*}
](https://render.githubusercontent.com/render/math?math=%5Cdisplaystyle+%5Cbegin%7Balign%2A%7D%0A%5Csqrt%7B2%7D%0A%5Cend%7Balign%2A%7D%0A)倍なので計算結果を`* System.Math.Sqrt(2)`した値を上限としています。



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
