using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    private static int s_baseColorId = Shader.PropertyToID("_BaseColor");
    private static int s_cutoffId = Shader.PropertyToID("_Cutoff");
    private static int s_metallicId = Shader.PropertyToID("_Metallic");
    private static int s_smoothnessId = Shader.PropertyToID("_Smoothness");

    private static MaterialPropertyBlock s_block;
    
    [SerializeField] private Color m_baseColor = Color.white;

    [SerializeField, Range(0.0f, 1.0f)] private float m_cutoff = 0.5f;
    [SerializeField, Range(0.0f, 1.0f)] private float m_metallic = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float m_smoothness = 0.5f;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        s_block ??= new MaterialPropertyBlock();

        s_block.SetColor(s_baseColorId, m_baseColor);
        s_block.SetFloat(s_cutoffId, m_cutoff);
        s_block.SetFloat(s_metallicId, m_metallic);
        s_block.SetFloat(s_smoothnessId, m_smoothness);
        
        GetComponent<Renderer>().SetPropertyBlock(s_block);
    }
}