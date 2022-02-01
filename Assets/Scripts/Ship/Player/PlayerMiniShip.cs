using UnityEngine;
using XiheFramework;

public class PlayerMiniShip : ShipBase {
    public Transform stencilSphere;

    // public Renderer stencilMaskRenderer;
    public Renderer stencilObjectRenderer;

    private PlayerMotherShip m_MotherShip;
    private int localId = -1; //id inside the fleet

    private Vector3 m_Destination;

    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    protected override void Start() {
        base.Start();

        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);

        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);

        Game.Event.Subscribe("OnSetFormation", OnSetFormation);
    }

    protected override void Update() {
        base.Update();

        TryGoToDestination();

        // if (target) {
        //     TryLockTarget(target.transform.position);
        // }
    }

    public void SetMotherShip(PlayerMotherShip motherShip) {
        this.m_MotherShip = motherShip;
    }


    public PlayerMotherShip GetMotherShip() {
        return m_MotherShip;
    }

    private void OnSetFormation(object sender, object e) {
        var s = sender as PlayerMotherShip;
        if (!s) {
            return;
        }

        if (s == m_MotherShip) {
            var ne = (IFormationStrategy) e;
            //set destination
            var motherTransform = m_MotherShip.transform;
            var trs = Matrix4x4.TRS(motherTransform.position, motherTransform.rotation, motherTransform.localScale);
            m_Destination = ne.GetDestination(trs, localId, m_MotherShip.GetMiniShipCount());
        }
    }

    private void TryGoToDestination() {
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

    // private void TryLockTarget(Vector3 worldPosition) {
    //     var delta = worldPosition - transform.position;
    //
    //     var angleSigned = Vector3.SignedAngle(transform.forward, delta, Vector3.up);
    //
    //     if (Mathf.Approximately(angleSigned, 0f)) {
    //         return;
    //     }
    //
    //     transform.Rotate(Vector3.up, angleSigned / Mathf.Abs(angleSigned) * Time.deltaTime * shipData.rotateSpeed);
    // }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = UnityEngine.Color.yellow;
        GizmosUtil.DrawCircle(transform.position, shipData.attackRadius, 25);
    }
}