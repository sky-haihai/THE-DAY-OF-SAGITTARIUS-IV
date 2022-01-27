using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RayTracingFeature : ScriptableRendererFeature {

    class RayTracingPass : ScriptableRenderPass {
        private RenderTargetIdentifier m_CamRt;
        private RenderTargetHandle m_TempRt;
        private ComputeShader m_Shader;

        // private Matrix4x4 cameraToWorldMatrix;
        // private Matrix4x4 inverseProjectionMatrix;

        public RayTracingPass(ComputeShader rayTracingShader) {
            m_Shader = rayTracingShader;
        }

        public void Setup(RenderTargetIdentifier camRt) {
            m_CamRt = camRt;
            //Debug.Log("Setup");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
            var desc = cameraTextureDescriptor;
            desc.enableRandomWrite = true;
            cmd.GetTemporaryRT(m_TempRt.id, desc);
        }

        // public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
        //     var desc = renderingData.cameraData.cameraTargetDescriptor;
        //     desc.enableRandomWrite = true;
        //     cmd.GetTemporaryRT(m_TempRt.id, desc);
        //
        //     // cameraToWorldMatrix = renderingData.cameraData.camera.cameraToWorldMatrix;
        //     // inverseProjectionMatrix = renderingData.cameraData.camera.projectionMatrix.inverse;
        // }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            if (renderingData.cameraData.isSceneViewCamera) {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get();

            cmd.SetComputeMatrixParam(m_Shader, "_CameraToWorld", renderingData.cameraData.camera.cameraToWorldMatrix);
            cmd.SetComputeMatrixParam(m_Shader, "_CameraInverseProjection", renderingData.cameraData.camera.projectionMatrix.inverse);
            cmd.SetComputeTextureParam(m_Shader, 0, "Result", m_TempRt.Identifier());
            int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
            cmd.DispatchCompute(m_Shader, 0, threadGroupsX, threadGroupsY, 1);

            cmd.Blit(m_TempRt.Identifier(), m_CamRt);
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) {
            cmd.ReleaseTemporaryRT(m_TempRt.id);
        }
    }

    RayTracingPass m_ScriptablePass;

    //public RenderTexture targetRt;
    public ComputeShader rayTracingShader;

    // public Material blitMaterial;
    public RenderPassEvent renderPassEvent;

    // public LayerMask layerMask;
    public bool enable = false;

    /// <inheritdoc/>
    public override void Create() {
        m_ScriptablePass = new RayTracingPass(rayTracingShader);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = renderPassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (!enable) {
            return;
        }

        m_ScriptablePass.Setup(renderer.cameraColorTarget);

        renderer.EnqueuePass(m_ScriptablePass);
    }
}