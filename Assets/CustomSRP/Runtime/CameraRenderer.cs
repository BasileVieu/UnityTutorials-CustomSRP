using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private ScriptableRenderContext m_context;

    private Camera m_camera;

    private const string m_bufferName = "Render Camera";

    private CommandBuffer m_buffer = new CommandBuffer
    {
        name = m_bufferName
    };

    private CullingResults m_cullingResults;

    private static ShaderTagId s_unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        m_context = context;
        m_camera = camera;

        PrepareBuffer();
        
        PrepareForSceneWindow();

        if (!Cull())
        {
            return;
        }

        Setup();
        
        DrawVisibleGeometry();

        DrawUnsupportedShaders();

        DrawGizmos();

        Submit();
    }

    private bool Cull()
    {
        if (m_camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            m_cullingResults = m_context.Cull(ref p);
            
            return true;
        }

        return false;
    }

    private void Setup()
    {
        m_context.SetupCameraProperties(m_camera);

        CameraClearFlags flags = m_camera.clearFlags;
        
        m_buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth,
            flags <= CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? m_camera.backgroundColor.linear : Color.clear);
        
        m_buffer.BeginSample(SampleName);
        
        ExecuteBuffer();
    }

    private void Submit()
    {
        m_buffer.EndSample(SampleName);

        ExecuteBuffer();
        
        m_context.Submit();
    }

    private void ExecuteBuffer()
    {
        m_context.ExecuteCommandBuffer(m_buffer);

        m_buffer.Clear();
    }

    private void DrawVisibleGeometry()
    {
        SortingSettings sortingSettings = new SortingSettings(m_camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        
        DrawingSettings drawingSettings = new DrawingSettings(s_unlitShaderTagId, sortingSettings);
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
        
        m_context.DrawSkybox(m_camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
    }
}