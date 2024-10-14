using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string m_bufferName = "Lighting";

    private CommandBuffer m_buffer = new CommandBuffer
    {
	    name = m_bufferName
    };

    private const int m_maxDirLightCount = 4;

    private static int s_dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    private static int s_dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors");
    private static int s_dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirections");

    private static Vector4[] s_dirLightColors = new Vector4[m_maxDirLightCount];
    private static Vector4[] s_dirLightDirections = new Vector4[m_maxDirLightCount];

    private CullingResults m_cullingResults;

    public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
    {
	    m_cullingResults = cullingResults;
	    
	    m_buffer.BeginSample(m_bufferName);

	    SetupLights();
	    
	    m_buffer.EndSample(m_bufferName);

	    context.ExecuteCommandBuffer(m_buffer);

	    m_buffer.Clear();
    }

    private void SetupLights()
    {
	    NativeArray<VisibleLight> visibleLights = m_cullingResults.visibleLights;

	    int dirLightCount = 0;

	    for (int i = 0; i < visibleLights.Length; i++)
	    {
		    VisibleLight visibleLight = visibleLights[i];

		    if (visibleLight.lightType == LightType.Directional)
		    {
			    SetupDirectionalLight(dirLightCount++, ref visibleLight);

			    if (dirLightCount >= m_maxDirLightCount)
			    {
				    break;
			    }
		    }
	    }
	    
	    m_buffer.SetGlobalInt(s_dirLightCountId, dirLightCount);
	    m_buffer.SetGlobalVectorArray(s_dirLightColorsId, s_dirLightColors);
	    m_buffer.SetGlobalVectorArray(s_dirLightDirectionsId, s_dirLightDirections);
    }

    private void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
	    s_dirLightColors[index] = visibleLight.finalColor;
	    s_dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
}