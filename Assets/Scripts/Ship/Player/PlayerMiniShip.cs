using UnityEngine;
using XiheFramework;

public class PlayerMiniShip : ShipBase {
    public Transform stencilSphere;

    // public Renderer stencilMaskRenderer;
    public Renderer stencilObjectRenderer;

    private PlayerMotherShip m_MotherShip;
    private int localId = -1; //id inside the fleet
    private IFormationStrategy m_Strategy;
    private Vector3 m_Destination;
    private bool m_IsStandby; //is reached destination and waiting 
    private bool m_IsRetrieving = false; //is going back to mother ship

    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    protected override void Start() {
        base.Start();

        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);

        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);
        stencilObjectRenderer.material.renderQueue += 1;

        Game.Event.Subscribe("OnSetFormation", OnSetFormation);
        Game.Event.Subscribe("OnRetrievePlayerScout", OnRetrievePlayerScout);
        Game.Event.Subscribe("OnPlayerMiniShipDestroyed", OnPlayerMiniShipDestroyed);
        Game.Event.Subscribe("OnPlayerDie", OnPlayerDie);
    }

    protected override void Update() {
        base.Update();

        if (m_MotherShip == null) {
            return;
        }

        var reached = TryGoToDestination();
        UpdateTarget();

        if (m_IsRetrieving) {
            m_Destination = m_MotherShip.transform.position;
        }
        else {
            UpdateDestination();
        }

        if (m_IsRetrieving && reached) {
            CombineToMother();
        }

        if (target && reached) {
            TryLockTarget(target.transform.position);
        }
    }

    private void OnPlayerDie(object sender, object e) {
        var m = sender as PlayerMotherShip;
        if (m == m_MotherShip) {
            runtimeData.hp = 0;
        }
    }

    void CombineToMother() {
        m_MotherShip.runtimeData.hp += this.runtimeData.hp;
        m_MotherShip.runtimeData.offense += runtimeData.offense;
        m_MotherShip.runtimeData.defense += runtimeData.defense;

        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void OnRetrievePlayerScout(object sender, object e) {
        var ne = e as PlayerMiniShip;
        if (ne != this) {
            return;
        }

        m_IsRetrieving = true;

        m_Destination = m_MotherShip.transform.position;

        GameManager.GetModule<ShipModule>().UnRegister(this);
        Game.Event.Invoke("OnPlayerMiniShipDestroyed", m_MotherShip, localId);
    }

    private void OnPlayerMiniShipDestroyed(object sender, object e) {
        var ns = sender as PlayerMotherShip;
        if (ns != m_MotherShip) {
            return;
        }

        var ne = (int) e;
        if (ne == localId) {
            return;
        }

        if (localId > ne) {
            localId -= 1;
        }
    }

    private void UpdateTarget() {
        target = GameManager.GetModule<ShipModule>().GuessBestTarget(this);
    }

    protected override void Die() {
        base.Die();

        Game.Event.Invoke("OnPlayerMiniShipDestroyed", m_MotherShip, localId);
    }

    private void UpdateDestination() {
        //set destination
        var motherTransform = m_MotherShip.transform;
        var trs = Matrix4x4.TRS(motherTransform.position, motherTransform.rotation, motherTransform.localScale);
        m_Destination = m_Strategy.GetDestination(trs, localId, m_MotherShip.GetMiniShipCount());
    }

    public void Setup(PlayerMotherShip motherShip, int id) {
        m_MotherShip = motherShip;
        localId = id;
        m_Strategy = motherShip.GetStrategy();
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
            m_Strategy = ne;
        }
    }

    /// <summary>
    /// Try Go To Destination
    /// </summary>
    /// <returns>true if destination is reached false otherwise</returns>
    private bool TryGoToDestination() {
        var delta = m_Destination - transform.position;

        if (delta.magnitude <= 0.05f) {
            return true;
        }

        var angleSigned = Vector3.SignedAngle(transform.forward, delta, Vector3.up);

        //rotate
        if (!Mathf.Approximately(angleSigned, 0f)) {
            transform.Rotate(Vector3.up, angleSigned / Mathf.Abs(angleSigned) * Time.deltaTime * shipData.rotateSpeed);
        }

        //y=-m\left(\frac{x}{\pi}\right)^{\frac{1}{a}}+m
        //TODO: thrust strategy might be changed later
        var rad = Mathf.Deg2Rad * Mathf.Abs(angleSigned);
        var max = shipData.thrustLevelLimit.y;
        var min = shipData.thrustLevelLimit.x;
        const float pow = 4;
        var level = -max * Mathf.Pow(rad / Mathf.PI, 1 / pow) + max;
        level = Mathf.Floor(level);
        level = Mathf.Clamp(level, min, max);
        runtimeData.thrustLevel = level;

        var dest = transform.position + transform.forward * (runtimeData.thrustLevel * Time.deltaTime * shipData.moveSpeed);
        //Debug.Log(dest.ToString());
        transform.position = Vector3.Lerp(transform.position, dest, 1 / 5f);

        return false;
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

        Gizmos.color = UnityEngine.Color.yellow;
        GizmosUtil.DrawCircle(transform.position, shipData.attackRadius, 25);

        if (!Application.isPlaying) {
            var radius = shipData.viewRadius * 2;
            stencilSphere.localScale = new Vector3(radius, radius, radius);
        }
    }
}