using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Wedge")]
public class VPatternFormation : StrategyBase {
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
    }
}