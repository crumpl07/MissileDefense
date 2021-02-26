﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLine : MonoBehaviour {


    /// <summary>
    /// Draws line from object origin towards direction.
    /// </summary>
    /// <param name="direction">Direction in Vector3</param>
    public void DrawLine(Vector3 direction)
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, direction);
    }
}
