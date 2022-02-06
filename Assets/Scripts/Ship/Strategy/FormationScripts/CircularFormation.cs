using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Circular")]
public class CircularFormation : StrategyBase {
    [Range(0,10f)]public float radius = 7.5f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        var rad = Mathf.PI * 2f / count;
        rad *= localId;

        var x = Mathf.Cos(rad) * radius;
        var y = Mathf.Sin(rad) * radius;
        var local = new Vector3(x, 0, y);
        var global = motherMatrix.MultiplyPoint3x4(local);

        return global;
    }
}