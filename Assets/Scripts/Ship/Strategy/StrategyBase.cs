using System;
using UnityEngine;

[Serializable]
public abstract class StrategyBase : ScriptableObject, IFormationStrategy {
    public abstract Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count);
}