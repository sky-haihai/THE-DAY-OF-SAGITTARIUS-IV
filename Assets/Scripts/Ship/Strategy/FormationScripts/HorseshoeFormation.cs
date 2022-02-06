using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Horseshoe")]

public class HorseshoeFormation : StrategyBase {
    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        return new Vector3();
    }
}