using UnityEngine;

[CreateAssetMenu(menuName = "Xihe/Strategy/Bentline")]
public class BentLineFormation : StrategyBase {
    [Range(1f, 10f)] public float radius = 3f;
    [Range(1f, 3f)] public float horizontalScale = 1.3f;
    [Range(-5f, -0f)] public float xMin = -2f;
    [Range(0f, 5f)] public float xMax = 2f; //sqrt(2) / 2
    [Range(0f, 10f)] public float scale = 6f;

    public override Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count) {
        // \left(\frac{x}{m}\right)^{2}+y^{2}=1\left\{-n<x<n\right\}\left\{y>0\right\}

        var x = Mathf.Lerp(xMin, xMax, (localId + 0.5f) / (float) count);
        var y = radius - Mathf.Pow(x / horizontalScale, 2);
        y = Mathf.Sqrt(Mathf.Abs(y));
        Vector3 localPos = new Vector3(x, 0, y);
        localPos *= scale;

        var global = motherMatrix.MultiplyPoint3x4(localPos);

        return global;
    }
}