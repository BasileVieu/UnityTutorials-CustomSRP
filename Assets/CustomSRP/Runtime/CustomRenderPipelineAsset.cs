using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private bool m_useDynamicBatching;
    [SerializeField] private bool m_useGPUInstancing;
    [SerializeField] private bool m_useSRPBatcher;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(m_useDynamicBatching, m_useGPUInstancing, m_useSRPBatcher);
    }
}