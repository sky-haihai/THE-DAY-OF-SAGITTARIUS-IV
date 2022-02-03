using UnityEngine;
using XiheFramework;

public class AIMotherShip : ShipBase {
    private Vector3 m_Destination;
    private Quaternion m_TargetRotaion;
    private Vector4 m_Bound;

    private StateMachine m_StateMachine;

    private int miniShipCount;
    private Formations m_CurrentFormation;

    private StateMachine m_Fsm;

    private static readonly int Color = Shader.PropertyToID("_Color");

    public string startState;
    public Renderer meshRenderer;
    public bool autoLock;

    public override int ClubId => 2;

    protected override void Start() {
        base.Start();

        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        meshRenderer.material.SetColor(Color, shipData.shipColor);

        GameManager.GetModule<ShipModule>().RegisterAI(this);

        InitFsm();
    }

    private void InitFsm() {
        m_Fsm = Game.Fsm.CreateStateMachine("AI " + this.GetHashCode().ToString() + " Behaviour");
        var patrol = new PatrolState(m_Fsm, this);
        var alert = new AlertState(m_Fsm, this);
        var battle = new BattleState(m_Fsm, this);
        m_Fsm.AddState(nameof(PatrolState), patrol);
        m_Fsm.AddState(nameof(AlertState), alert);
        m_Fsm.AddState(nameof(BattleState), battle);
        m_Fsm.SetDefaultState(startState);
        m_Fsm.Start();
    }

    protected override void Update() {
        base.Update();

        HandleAIDecision(); //handle ai decision

        UpdateTarget();

        if (target != null) {
            if (autoLock) {
                TryLockTarget(target.transform.position);
            }
        }
    }

    public bool HasTarget() {
        return target != null;
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