using UnityEngine;
using XiheFramework;

public class AIMotherShip : ShipBase {
    private Vector3 m_Destination;
    private Quaternion m_TargetRotaion;
    private Vector4 m_Bound;
    private bool m_IsStandby; //is reached destination and waiting 

    private StateMachine m_StateMachine;

    private int miniShipCount;
    private IFormationStrategy m_CurrentFormation;
    
    private static readonly int Color = Shader.PropertyToID("_Color");

    public Renderer meshRenderer;
    public bool autoLock;

    public override int ClubId => 2;

    protected override void Start() {
        base.Start();

        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        meshRenderer.material.SetColor(Color, shipData.shipColor);

        GameManager.GetModule<ShipModule>().RegisterAI(this);
    }

    protected override void Update() {
        base.Update();

        HandleAIDecision(); //handle ai decision

        UpdateTarget();

        m_IsStandby = TryGoToDestination();

        if (target != null && m_IsStandby) {
            if (autoLock) {
                TryLockTarget(target.transform.position);
            }
        }
    }

    public bool HasTarget() {
        return target != null;
    }

    public bool IsStandBy() {
        return m_IsStandby;
    }

    public Vector3 GetTargetPosition() {
        return target.transform.position;
    }

    public void SetDestination(Vector3 destination) {
        m_Destination = destination;
    }

    public int GetMiniShipCount() {
        return miniShipCount;
    }

    public void SetFormation(IFormationStrategy shipFormation) {
        Game.Event.Invoke("OnSetFormation", this, shipFormation);
    }

    private void OnAutoLock(object sender, object e) {
        Debug.Log("autoLock switched");

        var ne = (bool) e;

        autoLock = ne;
    }

    /// <summary>
    /// Try Go To Destination
    /// </summary>
    /// <returns>true if destination is reached false otherwise</returns>
    private bool TryGoToDestination() {
        var delta = m_Destination - transform.position;

        if (delta.magnitude <= 0.01f) {
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
        const float pow = 5;
        var level = -max * Mathf.Pow(rad / Mathf.PI, 1 / pow) + max;
        level = Mathf.Clamp(level, min, max);
        runtimeData.thrustLevel = level;

        var dest = transform.position + transform.forward * (runtimeData.thrustLevel * Time.deltaTime * shipData.moveSpeed);
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

    private void UpdateTarget() {
        target = GameManager.GetModule<ShipModule>().GuessBestTarget(this);
    }

    void HandleAIDecision() {
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        Gizmos.color = UnityEngine.Color.red;
        GizmosUtil.DrawCircle(transform.position, shipData.attackRadius, 25);
    }
}