using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static RaycastHit ShootRaycastDownFromAbove(Vector3 origin)
    {
        RaycastHit hit;
        Physics.Raycast(origin, -Vector3.up, out hit, Mathf.Infinity);
        return hit;
    }

    public static float GetYFromAbove(Vector3 origin)
    {
        return ShootRaycastDownFromAbove(origin).point.y;
    }
}
