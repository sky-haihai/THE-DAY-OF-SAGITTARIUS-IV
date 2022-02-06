using UnityEngine;
using XiheFramework;

public class PlayerMotherShip : ShipBase {
    private Vector3 m_Destination;
    private Vector4 m_Bound;

    private float m_ThrustDestination; //thrust destination

    private int miniShipCount;

    private IFormationStrategy m_CurrentFormation;

    public PlayerMiniShip miniShipTemplate;

    public Transform stencilSphere;
    public Renderer stencilObjectRenderer;

    public bool autoLock;

    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    #region Override Methods

    protected override void Start() {
        base.Start();

        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        Game.Event.Subscribe("OnAutoLock", OnAutoLock);
        Game.Event.Subscribe("OnSendPlayerScout", OnSendPlayerScout);
        Game.Event.Subscribe("OnFormationUIValueChanged", OnFormationUIValueChanged);

        InitStencilMeshScale();
        InitShipColor();
    }

    protected override void Update() {
        base.Update();

        HandlePlayerInput();

        UpdateTarget();

        if (target != null && autoLock) {
            TryLockTarget(target.transform.position);
        }

        UpdatePlayerRuntimeData();
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

    #endregion

    #region Public Methods

    public IFormationStrategy GetStrategy() {
        return m_CurrentFormation;
    }

    public int GetMiniShipCount() {
        return miniShipCount;
    }

    #endregion

    #region Private Methods

    private void OnFormationUIValueChanged(object sender, object e) {
        var ne = (int) e;
        m_CurrentFormation = GameManager.GetModule<ShipModule>().GetStrategyById(ne);

        UpdateFormationToMiniShips();
    }

    private void OnSendPlayerScout(object sender, object e) {
        SeparateMiniShipFromMother();
    }

    private void InitStencilMeshScale() {
        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);
    }

    private void InitShipColor() {
        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);
    }

    private void UpdatePlayerRuntimeData() {
        var shipLeft = GameManager.GetModule<ShipModule>().GetShipLeftOwnedBy(shipData.shipOwner);
        ShipRuntimeData data = new ShipRuntimeData(runtimeData.shipName, shipLeft,
            Mathf.Round(m_ThrustDestination), runtimeData.offense, runtimeData.defense);
        Game.Blackboard.SetData("PlayerRuntimeData", data, BlackBoardDataType.Runtime);
    }


    private void SeparateMiniShipFromMother() {
        if (runtimeData.hp <= shipData.initialHp / 20f) {
            return;
        }

        var root = GameObject.FindWithTag("PlayerShipRoot");
        var cachedTransform = transform;
        var go = Instantiate(miniShipTemplate, cachedTransform.position, cachedTransform.rotation, root.transform);
        go.Setup(this, miniShipCount++);
        runtimeData.hp -= 750f;
    }

    private void UpdateFormationToMiniShips() {
        Game.Event.Invoke("OnSetFormation", this, m_CurrentFormation);
    }

    private void OnAutoLock(object sender, object e) {
        Debug.Log("autoLock switched");

        var ne = (bool) e;

        autoLock = ne;
    }

    private void TryLockTarget(Vector3 worldPosition) {
        var cachedTransform = transform;
        var delta = worldPosition - cachedTransform.position;

        var angleSigned = Vector3.SignedAngle(cachedTransform.forward, delta, Vector3.up);

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

        var cachedTransform = transform;
        //multiply by 2 because ai ships has different calculation method 
        m_Destination = cachedTransform.position + cachedTransform.forward * (runtimeData.thrustLevel * Time.deltaTime * shipData.moveSpeed * 2);

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

    private void UpdateTarget() {
        target = GameManager.GetModule<ShipModule>().GuessBestTarget(this);
    }

    #endregion
}