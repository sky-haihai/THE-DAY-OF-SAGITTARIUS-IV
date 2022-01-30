using UnityEngine;
using XiheFramework;

public class AIMiniShip : ShipBase {
    public override int ClubId => 2;

    public Renderer meshRenderer;

    private static readonly int Color = Shader.PropertyToID("_Color");

    private AIMotherShip m_MotherShip;
    private int localId; //id inside the fleet

    private Vector3 m_Destination;
    
    protected override void Start() {
        base.Start();

        meshRenderer.material.SetColor(Color, shipData.shipColor);

        Game.Event.Subscribe("OnSetFormation", OnSetFormation);
    }

    private void OnSetFormation(object sender, object e) {
        var s = sender as AIMotherShip;
        if (!s) {
            return;
        }

        if (s == m_MotherShip) {
            var ne = (ShipFormation) e;
            TryGotoPosition(ne, localId);
        }
    }

    private void TryGotoPosition(ShipFormation formation, int id) {
        
    }

    protected override void Update() {
        base.Update();

        if (target) {
            TryLockTarget(target.transform.position);
        }
    }

    public void SetMotherShip(AIMotherShip motherShip) {
        m_MotherShip = motherShip;
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