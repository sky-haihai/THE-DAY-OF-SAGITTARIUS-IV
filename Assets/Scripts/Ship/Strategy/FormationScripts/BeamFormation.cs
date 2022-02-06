using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Beam")]
public class BeamFormation : StrategyBase {
    [Range(0, 10f)] public float scale = 3f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        var local = new Vector3(0, 0, localId * scale);

        var global = motherMatrix.MultiplyPoint3x4(local);
        return global;
    }
}