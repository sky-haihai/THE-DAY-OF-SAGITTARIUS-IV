using UnityEngine;
using XiheFramework;

public class PlayerMiniShip : ShipBase {
    public Transform stencilSphere;

    // public Renderer stencilMaskRenderer;
    public Renderer stencilObjectRenderer;

    //private static readonly int StencilId = Shader.PropertyToID("_StencilID");
    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    protected override void Start() {
        base.Start();

        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);

        //stencilMaskRenderer.material.SetInt(StencilId, ClubId);
        //fovMaskRenderer.material.renderQueue = (int) RenderQueue.Geometry + shipData.clubId;

        //stencilObjectRenderer.material.SetInt(StencilId, ClubId);
        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);
        // fovObjectRenderer.material.renderQueue = (int) RenderQueue.Geometry - shipData.clubId;
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