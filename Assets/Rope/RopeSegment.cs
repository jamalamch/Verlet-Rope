using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RopeSegment
{
    public Vector2 posNow;
    public Vector2 posOld;

    public RopeSegment(Vector2 pos)
    {
        this.posNow = pos;
        this.posOld = pos;
    }
}
