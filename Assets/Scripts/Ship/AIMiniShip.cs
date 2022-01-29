using UnityEngine;

public class AIMiniShip : ShipBase {
    public override int ClubId => 2;

    public Renderer meshRenderer;

    private static readonly int Color = Shader.PropertyToID("_Color");

    protected override void Start() {
        base.Start();

        meshRenderer.material.SetColor(Color, shipData.shipColor);
    }

    protected override void Update() {
        base.Update();

        if (target) {
            TryLockTarget(target.transform.position);
        }
    }

    private void TryLockTarget(Vector3 worldPosition) {
        var delta = worldPosition - transform.position;

        var angleSigned = Vector3.SignedAngle(transform.forward, delta, Vector3.up);

        if (Mathf.Approximately(angleSigned, 0f)) {
            return;
        }

        transform.Rotate(Vector3.up, angleSigned / Mathf.Abs(angleSigned) * Time.deltaTime * shipData.rotateSpeed);
    }
}