using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ShipData {
    // public int clubId;
    public string shipName;
    public Color shipColor;

    public float viewRadius = 1f;
    public float attackRadius = 1.5f;
    public float tolerantAngle = 15f;

    public float offense;
    public float defense;
    public float initialHp;

    public float moveSpeed = 1f;
    public float rotateSpeed = 1f;

    public Vector2 thrustLevelLimit = new Vector2(-3f, 5f);
}