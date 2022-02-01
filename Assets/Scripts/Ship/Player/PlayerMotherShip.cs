using UnityEngine;
using XiheFramework;

public class PlayerMotherShip : ShipBase {
    private Vector3 m_Destination;
    private Vector4 m_Bound;

    private float m_ThrustDestination; //thrust destination

    private int miniShipCount;

    private Formations m_CurrentFormation;

    public PlayerMiniShip miniShipTemplate;

    public Transform stencilSphere;
    public Renderer stencilObjectRenderer;

    public bool autoLock;

    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    protected override void Start() {
        base.Start();

        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        Game.Event.Subscribe("OnAutoLock", OnAutoLock);

        InitStencilMeshScale();
        InitShipColor();
    }

    void InitStencilMeshScale() {
        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);
    }

    void InitShipColor() {
        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);
    }

    protected override void Update() {
        base.Update();

        HandlePlayerInput();

        UpdateTarget();

        if (target != null && autoLock) {
            TryLockTarget(target.transform.position);
        }
    }

    public int GetMiniShipCount() {
        return miniShipCount;
    }

    public void SeparateMiniShipFromMother() {
        var root = GameObject.FindWithTag("PlayerShipRoot");
        var go = Instantiate(miniShipTemplate, transform.position, transform.rotation, root.transform);
    }

    public void SetFormation() {
        //TODO: implement dynamic id for mini ships
        Game.Event.Invoke("OnSetFormation", this, m_CurrentFormation);
    }

    private void OnAutoLock(object sender, object e) {
        Debug.Log("autoLock switched");

        var ne = (bool) e;

        autoLock = ne;
    }

    private void TryLockTarget(Vector3 worldPosition) {
        var delta = worldPosition - transform.position;

        var angleSigned = Vector3.SignedAngle(transform.forward, delta, Vector3.up);

        if (Mathf.Approximately(angleSigned, 0f)) {
            return;
        }

        transform.Rotate(Vector3.up, angleSigned / Mathf.Abs(angleSigned) * Time.deltaTime * shipData.rotateSpeed);
    }

    private void HandlePlayerInput() {
        var input = Game.Input.GetWASDInput();

        if (!autoLock) {
            transform.Rotate(Vector3.up, input.x * shipData.rotateSpeed * Time.deltaTime);
        }

        if (Game.Input.GetKeyDown(KeyActionTypes.MoveForward)) {
            m_ThrustDestination += 1f;
        }

        if (Game.Input.GetKeyDown(KeyActionTypes.MoveBackward)) {
            m_ThrustDestination -= 1f;
        }

        m_Destination = transform.position + transform.forward * (runtimeData.thrustLevel * Time.deltaTime * shipData.moveSpeed);

        m_ThrustDestination = Mathf.Clamp(m_ThrustDestination, shipData.thrustLevelLimit.x, shipData.thrustLevelLimit.y);
        runtimeData.thrustLevel = Mathf.Lerp(runtimeData.thrustLevel, m_ThrustDestination, 1 / 10f);

        if (m_Destination.x < m_Bound.x) {
            m_Destination.x = m_Bound.x;
        }

        if (m_Destination.z < m_Bound.y) {
            m_Destination.z = m_Bound.y;
        }

        if (m_Destination.x > m_Bound.z) {
            m_Destination.x = m_Bound.z;
        }

        if (m_Destination.z > m_Bound.w) {
            m_Destination.z = m_Bound.w;
        }

        transform.position = Vector3.Lerp(transform.position, m_Destination, 1 / 10f);
    }

    // private float m_UpdateTargetTimer = 0f;

    private void UpdateTarget() {
        // if (m_UpdateTargetTimer < 0.5f / Time.deltaTime) {
        //     m_UpdateTargetTimer += Time.deltaTime;
        //     return;
        // }
        //
        // m_UpdateTargetTimer -= 0.5f / Time.deltaTime;

        target = GameManager.GetModule<ShipModule>().GuessBestTarget(this, shipData.viewRadius);
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