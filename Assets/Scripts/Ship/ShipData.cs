using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShipData {
    // public int clubId;
    public Color shipColor;

    public float viewRadius = 1f;

    public float offense;
    public float defense;
    public float initialHp;

    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;
}