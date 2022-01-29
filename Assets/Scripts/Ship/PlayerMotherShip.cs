using UnityEngine;
using XiheFramework;

public class PlayerMotherShip : ShipBase {
    private Vector3 m_Destination;
    private Vector4 m_Bound;
    private float m_ThrustLevel;
    private float m_CurrentThrust;

    public Transform stencilSphere;
    public Renderer stencilMaskRenderer;
    public Renderer stencilObjectRenderer;

    public bool autoLock;

    private static readonly int StencilId = Shader.PropertyToID("_StencilID");
    private static readonly int Color = Shader.PropertyToID("_Color");

    public override int ClubId => 1;

    protected override void Start() {
        base.Start();

        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        Game.Event.Subscribe("OnAutoLock", OnAutoLock);

        var radius = shipData.viewRadius * 2;
        stencilSphere.localScale = new Vector3(radius, radius, radius);

        stencilMaskRenderer.material.SetInt(StencilId, ClubId);
        //fovMaskRenderer.material.renderQueue = (int) RenderQueue.Geometry + shipData.clubId;

        stencilObjectRenderer.material.SetInt(StencilId, ClubId);
        stencilObjectRenderer.material.SetColor(Color, shipData.shipColor);
        // fovObjectRenderer.material.renderQueue = (int) RenderQueue.Geometry - shipData.clubId;
    }

    protected override void Update() {
        base.Update();

        HandleInput();

        if (target != null) {
            if (autoLock) {
                TryLockTarget(target.transform.position);
            }
        }
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

    private void HandleInput() {
        var input = Game.Input.GetWASDInput();

        if (!autoLock) {
            transform.Rotate(Vector3.up, input.x * shipData.rotateSpeed * Time.deltaTime);
        }

        if (Game.Input.GetKeyDown(KeyActionTypes.MoveForward)) {
            m_ThrustLevel += 1f;
        }

        if (Game.Input.GetKeyDown(KeyActionTypes.MoveBackward)) {
            m_ThrustLevel -= 1f;
        }

        m_Destination = transform.position + transform.forward * (m_CurrentThrust * Time.deltaTime * shipData.moveSpeed);

        m_CurrentThrust = Mathf.Lerp(m_CurrentThrust, m_ThrustLevel, 1 / 10f);

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

    private void WatchSurroundings() {
        //sphere cast around
    }
}