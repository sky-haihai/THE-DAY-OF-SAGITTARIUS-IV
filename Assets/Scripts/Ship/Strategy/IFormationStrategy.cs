using UnityEngine;

public interface IFormationStrategy  {
    Vector3 GetDestination(Matrix4x4 motherMatrix, int localId, int count);
}