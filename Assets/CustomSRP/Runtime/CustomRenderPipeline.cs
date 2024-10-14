using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
	private CameraRenderer m_renderer = new CameraRenderer();

	private bool m_useDynamicBatching;
	private bool m_useGPUInstancing;

	public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
	{
		m_useDynamicBatching = useDynamicBatching;
		m_useGPUInstancing = useGPUInstancing;
		
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
		GraphicsSettings.lightsUseLinearIntensity = true;
	}
	
	protected override void Render(ScriptableRenderContext context, Camera[] cameras)
	{
		
	}

	protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
	{
		for (int i = 0; i < cameras.Count; i++)
		{
			m_renderer.Render(context, cameras[i], m_useDynamicBatching, m_useGPUInstancing);
		}
	}
}