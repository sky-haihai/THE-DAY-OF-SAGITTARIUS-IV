using System;
using UnityEngine;
using UnityEngine.Rendering;
using XiheFramework;

[Serializable]
public abstract class ShipBase : MonoBehaviour {
    [SerializeField] protected ShipBase target;

    [SerializeField] public ShipRuntimeData runtimeData;
    [SerializeField] public ShipData shipData;

    //attack line
    public AttackLine attackLine;

    public abstract int ClubId { get; }

    protected virtual void Start() {
        GameManager.GetModule<ShipModule>().Register(this);

        InitShipRuntimeData();
    }

    private void InitShipRuntimeData() {
        string n = "";
        if (string.IsNullOrEmpty(shipData.shipName)) {
            n = GetHashCode().ToString();
        }
        else {
            n = shipData.shipName;
        }

        runtimeData = new ShipRuntimeData(n, shipData.initialHp, shipData.offense, shipData.defense, shipData.moveSpeed);
    }

    protected virtual void Update() {
        if (runtimeData.isDead) {
            return;
        }

        if (ComputeIsLinedUp()) {
            Attack();
        }
        else {
            attackLine.ClearLine();
        }

        //die
        if (runtimeData.hp <= 0f && !runtimeData.isDead) {
            Die();
            runtimeData.isDead = true;
        }
    }

    private void Attack() {
        //draw
        var damage = GameManager.GetModule<ShipModule>().ApplyAttack(this, target, out float multiplier);
        // Debug.Log("Damage " + damage);
        // damage = Mathf.Floor(damage / 2f) * 2f;//0-2 2-4 4-6 (eg.3.5 -> 1.75 -> 1.0 -> 2.0
        attackLine.scrollSpeed = multiplier;
        attackLine.DrawLine(transform.position + Vector3.up, target.transform.position + Vector3.up);

        // var args = runtimeData.shipName + " 全砲、発射！";
        // var args = gameObject.name + " 全砲、発射！";
        // Game.Event.Invoke("OnUpdateGlobalMessage", this, args);
        // Debug.Log(args);
    }

    protected virtual void Die() {
        //unregister from ship list
        GameManager.GetModule<ShipModule>().UnRegister(this);

        //TODO:play a sprite animation
        gameObject.SetActive(false);

        var args = gameObject.name + " に栄光あれーー！";
        Game.Event.Invoke("OnUpdateGlobalMessage", this, args);
        Debug.Log(args);
    }

    public void ReceiveDamage(float damage) {
        runtimeData.hp -= damage;
    }

    private bool ComputeIsLinedUp() {
        if (!target) {
            return false;
        }

        var offset = target.transform.position - transform.position;
        if (offset.magnitude > shipData.attackRadius) {
            return false;
        }

        if (Vector3.Angle(transform.forward, offset) > shipData.tolerantAngle / 2) {
            return false;
        }

        return true;
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.white;
        GizmosUtil.DrawCircle(transform.position, shipData.viewRadius + 0.01f, 25);
    }
}