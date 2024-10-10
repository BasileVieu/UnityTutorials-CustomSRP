using UnityEngine;

public class MeshBall : MonoBehaviour
{
    private static int s_baseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField] private Mesh m_mesh;

    [SerializeField] private Material m_material;

    private Matrix4x4[] m_matrices = new Matrix4x4[1023];

    private Vector4[] m_baseColors = new Vector4[1023];

    private MaterialPropertyBlock m_block;

    private void Awake()
    {
        for (int i = 0; i < m_matrices.Length; i++)
        {
            m_matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10.0f,
                Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f),
                Vector3.one * Random.Range(0.5f, 1.5f));

            m_baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1.0f));
        }
    }

    private void Update()
    {
        if (m_block == null)
        {
            m_block = new MaterialPropertyBlock();
            m_block.SetVectorArray(s_baseColorId, m_baseColors);
        }
        
        Graphics.DrawMeshInstanced(m_mesh, 0, m_material, m_matrices, 1023, m_block);
    }
}