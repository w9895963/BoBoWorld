using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectPass : ScriptableRendererFeature {
    class CustomRenderPass : ScriptableRenderPass {

        public RenderTargetIdentifier cameraColorTarget;
        public Material material;
        public RenderTargetHandle renderTargetHandle;

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure (CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) { }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute (ScriptableRenderContext context, ref RenderingData renderingData) {
            CommandBuffer cmb = CommandBufferPool.Get ("_TempBuffer");
            cmb.GetTemporaryRT (renderTargetHandle.id, renderingData.cameraData.cameraTargetDescriptor);


            Blit (cmb, cameraColorTarget, renderTargetHandle.Identifier (), material);
            Blit (cmb, renderTargetHandle.Identifier (), cameraColorTarget);
            context.ExecuteCommandBuffer (cmb);
            CommandBufferPool.Release (cmb);

        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup (CommandBuffer cmd) { }
    }

    CustomRenderPass m_ScriptablePass;
    public Material material = null;


    public override void Create () {
        m_ScriptablePass = new CustomRenderPass ();

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses (ScriptableRenderer renderer, ref RenderingData renderingData) {
        m_ScriptablePass.cameraColorTarget = renderer.cameraColorTarget;
        m_ScriptablePass.material = material;
        m_ScriptablePass.renderTargetHandle.Init ("PostEffect");
        renderer.EnqueuePass (m_ScriptablePass);
    }
}