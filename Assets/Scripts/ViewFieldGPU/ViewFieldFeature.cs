using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XiheFramework;

public class ViewFieldFeature : ScriptableRendererFeature {
    class ViewFieldPass : ScriptableRenderPass {
        private RenderTargetHandle m_TempRt;

        private RenderTexture m_BattleCamRt;
        private RenderTexture m_ViewFieldMaskRt;

        private Material m_BlitMaterial;
        private ComputeShader m_ComputeShader;

        private ShipComputeData[] m_ComputeData;
        private ComputeBuffer m_ShipBuffer;

        public ViewFieldPass(Material blitMaterial, RenderTexture camRt, RenderTexture viewFieldMaskRt, ComputeShader viewFieldShader) {
            m_BlitMaterial = blitMaterial;
            m_BattleCamRt = camRt;
            m_ComputeShader = viewFieldShader;
            m_ViewFieldMaskRt = viewFieldMaskRt;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
            //setup compute buffer
            m_ComputeData = GameManager.GetModule<ShipModule>().UpdateComputeData();

            if (m_ComputeData == null) {
                return;
            }

            var size = sizeof(float) * 4; //position + radius
            m_ShipBuffer = new ComputeBuffer(m_ComputeData.Length, size);

            //get temp rt
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.enableRandomWrite = true;
            cmd.GetTemporaryRT(m_TempRt.id, desc);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            if (m_ComputeData == null) {
                return;
            }

            if (renderingData.cameraData.isSceneViewCamera) {
                return;
            }

            Debug.Log("exe " + (m_ShipBuffer == null));

            CommandBuffer cmd = CommandBufferPool.Get();

            cmd.SetComputeBufferParam(m_ComputeShader, 0, "Ships", m_ShipBuffer);
            cmd.SetComputeBufferData(m_ShipBuffer, m_ComputeData);

            // cmd.SetComputeMatrixParam(m_ComputeShader, "_CameraToWorld", renderingData.cameraData.camera.cameraToWorldMatrix);
            // cmd.SetComputeMatrixParam(m_ComputeShader, "_CameraInverseProjection", renderingData.cameraData.camera.projectionMatrix.inverse);
            // cmd.SetComputeTextureParam(m_ComputeShader, 0, "Result", m_TempRt.Identifier());

            cmd.SetComputeTextureParam(m_ComputeShader, 0, "Result", m_TempRt.Identifier());
            int threadGroupsX = Mathf.CeilToInt(m_ViewFieldMaskRt.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(m_ViewFieldMaskRt.height / 8.0f);
            cmd.DispatchCompute(m_ComputeShader, 0, threadGroupsX, threadGroupsY, 1);

            //cmd.Blit(battleCamRt, m_TempRt.Identifier(), m_BlitMaterial);
            cmd.Blit(m_TempRt.Identifier(), m_ViewFieldMaskRt, m_BlitMaterial);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }


        public override void OnCameraCleanup(CommandBuffer cmd) {
            if (m_ComputeData == null) {
                return;
            }

            cmd.ReleaseTemporaryRT(m_TempRt.id);
            m_ShipBuffer.Dispose();
        }
    }

    ViewFieldPass m_ScriptablePass;

    public ComputeShader viewFieldShader;
    public RenderTexture battleCamRt;
    public RenderTexture maskRt;
    public Material blitMaterial;
    public RenderPassEvent renderPassEvent;
    public bool enable = false;

    /// <inheritdoc/>
    public override void Create() {
        m_ScriptablePass = new ViewFieldPass(blitMaterial, battleCamRt, maskRt, viewFieldShader);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = renderPassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (!enable) {
            return;
        }

        if (!Application.isPlaying) {
            return;
        }

        renderer.EnqueuePass(m_ScriptablePass);
    }
}