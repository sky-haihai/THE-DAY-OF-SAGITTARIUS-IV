using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Wedge")]
public class WedgeFormation : StrategyBase {
    [Range(0, 5f)] public float horizontal = 1f;
    [Range(0, 5f)] public float hOffset = 0f;
    [Range(0, 10f)] public float scale = 5f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        Vector3 result = Vector3.zero;
        var d = count / 2 - localId;
        if (localId < count / 2) {
            result = new Vector3(-d * scale * horizontal - hOffset, 0, d * scale);
        }

        if (localId >= count / 2) {
            result = new Vector3((-d + 1) * scale * horizontal + hOffset, 0, (-d + 1) * scale);
        }

        result = motherMatrix.MultiplyPoint3x4(result);

        return result;

        var x = Mathf.Lerp(-1f, 1f, localId / (float) count);
        var y = Mathf.Abs(horizontal * x);
        var local = new Vector3(x, 0, y) * scale;
        var global = motherMatrix.MultiplyPoint3x4(local);

        return global;
    }
}