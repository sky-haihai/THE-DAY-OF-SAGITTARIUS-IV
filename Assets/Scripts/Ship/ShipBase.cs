using System;
using UnityEngine;
using UnityEngine.Rendering;
using XiheFramework;

[Serializable]
public abstract class ShipBase : MonoBehaviour {
    [SerializeField] protected ShipBase target;
    [SerializeField] protected ShipRuntimeData runtimeData;

    public ShipData shipData = new ShipData();


    public abstract int ClubId { get; }

    //public abstract void TryLockTarget(Vector3 worldPosition);

    protected virtual void Start() {
        GameManager.GetModule<ShipModule>().Register(this);
    }

    protected virtual void Update() {
        if (ComputeIsLinedUp()) {
            Debug.Log("全砲、発射！");

            GameManager.GetModule<ShipModule>().ApplyAttack(this, target);
        }
    }

    public void ReceiveDamage(float damage) {
        runtimeData.hp -= damage;
    }

    bool ComputeIsLinedUp() {
        if (!target) {
            return false;
        }

        var offset = target.transform.position - transform.position;
        if (offset.magnitude > shipData.attackRadius) {
            return false;
        }

        if (Vector3.Angle(transform.forward, offset) > shipData.tolerantAngle) {
            return false;
        }

        return true;
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.white;
        GizmosUtil.DrawCircle(transform.position, shipData.viewRadius, 25);
    }
}