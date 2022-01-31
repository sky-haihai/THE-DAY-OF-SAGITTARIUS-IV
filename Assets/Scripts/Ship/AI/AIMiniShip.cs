using UnityEngine;
using XiheFramework;

public class AIMiniShip : ShipBase {
    public override int ClubId => 2;

    public Renderer meshRenderer;

    private static readonly int Color = Shader.PropertyToID("_Color");

    private AIMotherShip m_MotherShip;
    private int localId = -1; //id inside the fleet

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
            var ne = (IFormationStrategy) e;
            //set destination
            var trans = m_MotherShip.transform;
            var trs = Matrix4x4.TRS(trans.position, trans.rotation, trans.localScale);
            m_Destination = ne.GetDestination(trs, localId, m_MotherShip.GetMiniShipCount());
        }
    }

    void TryGoToDestination() {
        var delta = m_Destination - transform.position;

        var angleSigned = Vector3.SignedAngle(transform.forward, delta, Vector3.up);

        //rotate
        if (!Mathf.Approximately(angleSigned, 0f)) {
            transform.Rotate(Vector3.up, angleSigned / Mathf.Abs(angleSigned) * Time.deltaTime * shipData.rotateSpeed);
        }

        //thrust strategy might change later
        runtimeData.thrustLevel = shipData.thrustLevelLimit.y / 10;

        if (Mathf.Abs(angleSigned) < 90) {
            runtimeData.thrustLevel = shipData.thrustLevelLimit.y / 5;
        }

        if (Mathf.Abs(angleSigned) < 60) {
            runtimeData.thrustLevel = shipData.thrustLevelLimit.y / 3;
        }

        if (Mathf.Abs(angleSigned) < 30) {
            runtimeData.thrustLevel = shipData.thrustLevelLimit.y;
        }
    }

    protected override void Update() {
        base.Update();

        //only one thing to do: formation
        TryGoToDestination();
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

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = UnityEngine.Color.red;
        GizmosUtil.DrawCircle(transform.position, shipData.attackRadius, 25);
    }
}