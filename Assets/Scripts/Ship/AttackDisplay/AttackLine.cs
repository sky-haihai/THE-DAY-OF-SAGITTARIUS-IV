using System;
using UnityEngine;

public class AttackLine : MonoBehaviour {
    public LineRenderer lineRenderer;
    public float offset = 0.05f;
    public float scrollSpeed = 1.5f;
    public float reachTargetTime = 0.05f;

    private readonly Vector3[] m_Buffer = new Vector3[2];
    private static readonly int Offset = Shader.PropertyToID("_Offset");

    public void DrawLine(Vector3 from, Vector3 to) {
        var o = -Vector3.Cross(to - from, Vector3.up).normalized * offset;

        m_Buffer[0] = from + o;

        if (m_Buffer[1].Equals(Vector3.zero)) {
            m_Buffer[1] = m_Buffer[0];
        }

        var p = Vector3.Lerp(m_Buffer[1], to + o, Time.deltaTime / reachTargetTime); //1 / (reachTargetTime / Time.deltaTime)
        m_Buffer[1] = p;
        lineRenderer.SetPositions(m_Buffer);
        lineRenderer.material.SetFloat(Offset, -Time.time * scrollSpeed);
    }

    public void ClearLine() {
        m_Buffer[0] = Vector3.zero;
        m_Buffer[1] = Vector3.zero;
        lineRenderer.SetPositions(m_Buffer);
    }
}