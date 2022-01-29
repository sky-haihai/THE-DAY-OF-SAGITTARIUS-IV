using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XiheFramework;

public class ViewFieldFeature : ScriptableRendererFeature {
    class ViewFieldPass : ScriptableRenderPass {
        private RenderTargetHandle m_TempRt;

        //private RenderTexture m_BattleCamRt;
        private Material m_BlitMaterial;

        private RenderTargetIdentifier m_BattleCamTarget;

        private string m_BattleCamName;

        public ViewFieldPass(Material blitMaterial, string battleCamName) {
            m_BlitMaterial = blitMaterial;
            this.m_BattleCamName = battleCamName;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
            //get temp rt
            //var desc = renderingData.cameraData.cameraTargetDescriptor;
            //desc.enableRandomWrite = true;
            cmd.GetTemporaryRT(m_TempRt.id, renderingData.cameraData.cameraTargetDescriptor);

            m_BattleCamTarget = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            if (renderingData.cameraData.isSceneViewCamera) {
                return;
            }

            if (renderingData.cameraData.camera.name != m_BattleCamName) {
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("BattleCamBlit");

            cmd.Blit(m_BattleCamTarget, m_TempRt.id, m_BlitMaterial);
            cmd.Blit(m_TempRt.id, m_BattleCamTarget);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }


        public override void OnCameraCleanup(CommandBuffer cmd) {
            cmd.ReleaseTemporaryRT(m_TempRt.id);
        }
    }

    ViewFieldPass m_ScriptablePass;

    public string cameraName;

    //public RenderTexture battleCamRt;
    public Material blitMaterial;
    public RenderPassEvent renderPassEvent;
    public bool enable = false;

    /// <inheritdoc/>
    public override void Create() {
        m_ScriptablePass = new ViewFieldPass(blitMaterial, cameraName);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = renderPassEvent;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (!enable) {
            return;
        }

        renderer.EnqueuePass(m_ScriptablePass);
    }
}