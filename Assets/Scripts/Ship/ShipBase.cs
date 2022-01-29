using System;
using UnityEngine;
using UnityEngine.Rendering;
using XiheFramework;

[Serializable]
public abstract class ShipBase : MonoBehaviour {
    [SerializeField] protected ShipBase target;

    public ShipData shipData = new ShipData();


    public abstract int ClubId { get; }

    //public abstract void TryLockTarget(Vector3 worldPosition);

    protected virtual void Start() {
        GameManager.GetModule<ShipModule>().Register(this);
    }

    protected virtual void Update() {
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawWireSphere(transform.position, shipData.viewRadius);
        GizmosUtil.DrawCircle(transform.position, shipData.viewRadius, 25);
        
        
        //Gizmos.DrawSphere(transform.position, shipData.viewRadius);
    }
}