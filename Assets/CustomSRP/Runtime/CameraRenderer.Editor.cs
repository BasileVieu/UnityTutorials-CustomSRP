using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
	private partial void DrawGizmos();

	private partial void DrawUnsupportedShaders();

	private partial void PrepareForSceneWindow();

	private partial void PrepareBuffer();

#if UNITY_EDITOR
	private static ShaderTagId[] s_legacyShaderTagIds =
	{
		new ShaderTagId("Always"),
		new ShaderTagId("ForwardBase"),
		new ShaderTagId("PrepassBase"),
		new ShaderTagId("Vertex"),
		new ShaderTagId("VertexLMRGBM"),
		new ShaderTagId("VertexLM")
	};

	private static Material s_errorMaterial;

	private string SampleName { get; set; }

	private partial void DrawGizmos()
	{
		if (Handles.ShouldRenderGizmos())
		{
			m_context.DrawGizmos(m_camera, GizmoSubset.PreImageEffects);
			m_context.DrawGizmos(m_camera, GizmoSubset.PostImageEffects);
		}
	}

	private partial void DrawUnsupportedShaders()
	{
		if (s_errorMaterial == null)
		{
			s_errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
		}

		DrawingSettings drawingSettings = new DrawingSettings(s_legacyShaderTagIds[0], new SortingSettings(m_camera))
		{
			overrideMaterial = s_errorMaterial
		};

		for (int i = 1; i < s_legacyShaderTagIds.Length; i++)
		{
			drawingSettings.SetShaderPassName(i, s_legacyShaderTagIds[i]);
		}

		FilteringSettings filteringSettings = FilteringSettings.defaultValue;

		m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
	}

	private partial void PrepareForSceneWindow()
	{
		if (m_camera.cameraType == CameraType.SceneView)
		{
			ScriptableRenderContext.EmitWorldGeometryForSceneView(m_camera);
		}
	}

	private partial void PrepareBuffer()
	{
		Profiler.BeginSample("Editor Only");
		
		m_buffer.name = SampleName = m_camera.name;

		Profiler.EndSample();
	}
#else

	private const string SampleName = m_bufferName;
	
#endif
}