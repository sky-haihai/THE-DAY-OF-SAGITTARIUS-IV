using UnityEngine;
using XiheFramework;

public class MotherShip : ShipBase {
    private Vector3 m_Destination;
    private Quaternion m_TargetRotaion;
    private Vector4 m_Bound;

    public bool autoLock;

    protected override void Start() {
        base.Start();
        
        m_Bound = Game.Blackboard.GetData<Vector4>("bound");

        if (shipData.clubId == 0) {
            Game.Event.Subscribe("OnAutoLock", OnAutoLock);
        }
    }

    protected override void Update() {
        base.Update();

        if (shipData.clubId == 0) {
            HandleInput();
        }

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

        m_Destination = transform.position + transform.forward * (input.y * Time.deltaTime * shipData.moveSpeed);

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

        transform.position = Vector3.Lerp(transform.position, m_Destination, 50f);
    }
}