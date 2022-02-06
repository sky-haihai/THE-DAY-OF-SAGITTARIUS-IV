using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Spread")]
public class SpreadFormation : StrategyBase {
    [Range(0,5f)]public float distMin = 2f;
    [Range(0,10f)]public float distMax = 10f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        Random.InitState(localId);
        var dist = Random.Range(distMin, distMax);

        var rad = Mathf.PI * 2f / count;
        rad *= localId;

        var x = Mathf.Cos(rad) * dist;
        var y = Mathf.Sin(rad) * dist;
        var local = new Vector3(x, 0, y);
        var global = motherMatrix.MultiplyPoint3x4(local);

        return global;
    }
}