using Microsoft.SqlServer.Server;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Wedge")]
public class WedgeFormation : StrategyBase {
    [Range(0, 5f)] public float horizontal = 1f;
    [Range(0, 5f)] public float vertical = 1f;
    [Range(0, 10f)] public float scale = 5f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        int2 lineId = GetLineId(localId + 1);

        var x = lineId.y * -horizontal + lineId.x * horizontal * 2;
        var y = lineId.y * vertical;

        var local = new Vector3(x, 0, y) * scale;
        var global = motherMatrix.MultiplyPoint3x4(local);
        return global;
    }

    int2 GetLineId(int localId) {
        int delta = 0;
        int sum = 0;
        int y = 0;
        while (localId > sum + delta) {
            sum += ++delta;
            y++;
        }

        return new int2(localId - sum, y);
    }
}