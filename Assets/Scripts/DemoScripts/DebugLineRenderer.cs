using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineRenderer : MonoBehaviour
{
    LineRenderer line; // LineRendererコンポーネントを受ける変数
    int count; // 線の頂点の数

    void Start () 
    {
        line = GetComponent<LineRenderer>(); // LineRendererコンポーネントを取得
	}

    void FixedUpdate() // updateでもいいけど，fixedのほうが今回都合がいい
    {
        count += 1; // 頂点数を１つ増やす
        line.positionCount = count; // 頂点数の更新
        line.SetPosition(count - 1, transform.position); // オブジェクトの位置情報をセット
    }
}
