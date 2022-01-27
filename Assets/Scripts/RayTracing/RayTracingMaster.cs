using UnityEngine;

public class RayTracingMaster : MonoBehaviour {
    public ComputeShader rayTracingShader;
    private RenderTexture m_Target;
    private Camera m_Camera;

    private void Awake() {
        m_Camera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        SetShaderParameters();
        Render(destination);
    }

    private void SetShaderParameters() {
        rayTracingShader.SetMatrix("_CameraToWorld", m_Camera.cameraToWorldMatrix);
        rayTracingShader.SetMatrix("_CameraInverseProjection", m_Camera.projectionMatrix.inverse);
    }

    private void Render(RenderTexture destination) {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        rayTracingShader.SetTexture(0, "Result", m_Target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        rayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        // Blit the result texture to the screen
        Graphics.Blit(m_Target, destination);
    }

    private void InitRenderTexture() {
        if (m_Target == null || m_Target.width != Screen.width || m_Target.height != Screen.height) {
            // Release render texture if we already have one
            if (m_Target != null)
                m_Target.Release();
            // Get a render target for Ray Tracing
            m_Target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            m_Target.enableRandomWrite = true;
            m_Target.Create();
        }
    }
}