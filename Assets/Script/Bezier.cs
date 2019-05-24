using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{
    //Cubic Bezier Function
    public static Vector3 Cubic(Vector3 start, Vector3 startTangent, Vector3 endTangent, Vector3 end, float t)
    {
        return new Vector3(
            (1 - t) * (1 - t) * (1 - t) * start.x + 3 * t * (1 - t) * (1 - t) * startTangent.x + 3 * t * t * (1 - t) * endTangent.x + t * t * t * end.x
            , 0,
            (1 - t) * (1 - t) * (1 - t) * start.z + 3 * t * (1 - t) * (1 - t) * startTangent.z + 3 * t * t * (1 - t) * endTangent.z + t * t * t * end.z
            );
    }
}
