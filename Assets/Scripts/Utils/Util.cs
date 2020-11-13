using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static Transform GetTopmostParent(Transform currTransform)
    {
        if (currTransform.parent == null)
            return currTransform;

        return GetTopmostParent(currTransform.parent);
    }
}
