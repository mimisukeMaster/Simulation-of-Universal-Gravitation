using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineRenderer : MonoBehaviour
{
    private LineRenderer line;
    private int count;

    void Start () 
    {
        line = GetComponent<LineRenderer>();
	}

    void FixedUpdate()
    {
        count += 1;
        line.positionCount = count;
        line.SetPosition(count - 1, transform.position);
    }
}
