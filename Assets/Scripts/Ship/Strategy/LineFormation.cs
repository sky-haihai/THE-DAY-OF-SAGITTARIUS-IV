using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFormation : IFormationStrategy {
    public Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        var motherPos = motherMatrix.MultiplyPoint3x4(Vector3.zero);
        if (localId == -1) {
            Debug.Log("ERROR local id = -1");
            return motherPos;
        }

        const float interval = 0.1f;

        Vector3 result = motherPos;
        var d = count / 2 - localId;
        if (localId < count / 2) {
            result = new Vector3(-d * interval, 0, 0);
            result = motherMatrix.MultiplyPoint3x4(result);
        }

        if (localId >= count / 2) {
            result = new Vector3((-d + 1) * interval, 0, 0);
            result = motherMatrix.MultiplyPoint3x4(result);
        }

        return result;
    }
}