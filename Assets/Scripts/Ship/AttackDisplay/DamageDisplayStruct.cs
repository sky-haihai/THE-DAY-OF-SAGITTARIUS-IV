using UnityEngine;

public struct DamageDisplayStruct {
    public Vector3 from;
    public Vector3 to;

    public DamageDisplayStruct(Vector3 from, Vector3 to) {
        this.from = from;
        this.to = to;
    }
}