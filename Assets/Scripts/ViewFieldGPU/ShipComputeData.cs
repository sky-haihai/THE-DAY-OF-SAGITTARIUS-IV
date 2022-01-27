using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShipComputeData {
    public Vector3 position;
    public float viewRadius;
    
    public ShipComputeData(Vector3 position, float viewRadius) {
        this.position = position;
        this.viewRadius = viewRadius;
    }
}