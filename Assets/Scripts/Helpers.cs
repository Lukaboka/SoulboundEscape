using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static UnityEngine.Vector3 ToIso(this UnityEngine.Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
