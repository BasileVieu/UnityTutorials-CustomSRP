using UnityEngine;

public class MeshBall : MonoBehaviour
{
    private static int s_baseColorId = Shader.PropertyToID("_BaseColor");
    private static int s_metallicId = Shader.PropertyToID("_Metallic");
    private static int s_smoothnessId = Shader.PropertyToID("_Smoothness");

    [SerializeField] private Mesh m_mesh;

    [SerializeField] private Material m_material;

    private Matrix4x4[] m_matrices = new Matrix4x4[1023];

    private Vector4[] m_baseColors = new Vector4[1023];
    
    private float[] m_metallic = new float[1023];
    private float[] m_smoothness = new float[1023];

    private MaterialPropertyBlock m_block;

    private void Awake()
    {
        for (int i = 0; i < m_matrices.Length; i++)
        {
            m_matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10.0f,
                Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f),
                Vector3.one * Random.Range(0.5f, 1.5f));

            m_baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1.0f));
            m_metallic[i] = Random.value < 0.25f ? 1.0f : 0.0f;
            m_smoothness[i] = Random.Range(0.05f, 0.95f);
        }
    }

    private void Update()
    {
        if (m_block == null)
        {
            m_block = new MaterialPropertyBlock();
            m_block.SetVectorArray(s_baseColorId, m_baseColors);
            m_block.SetFloatArray(s_metallicId, m_metallic);
            m_block.SetFloatArray(s_smoothnessId, m_smoothness);
        }
        
        Graphics.DrawMeshInstanced(m_mesh, 0, m_material, m_matrices, 1023, m_block);
    }
}