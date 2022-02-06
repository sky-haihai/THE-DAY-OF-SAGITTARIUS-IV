using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Line")]
public class LineFormation : StrategyBase {
    [Range(0, 10f)] public float scale = 3f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        var motherPos = motherMatrix.MultiplyPoint3x4(Vector3.zero);
        if (localId == -1) {
            Debug.Log("ERROR local id = -1");
            return motherPos;
        }

        Vector3 result = motherPos;
        var d = count / 2 - localId;
        if (localId < count / 2) {
            result = new Vector3(-d * scale, 0, 0);
            result = motherMatrix.MultiplyPoint3x4(result);
        }

        if (localId >= count / 2) {
            result = new Vector3((-d + 1) * scale, 0, 0);
            result = motherMatrix.MultiplyPoint3x4(result);
        }

        return result;
    }
}