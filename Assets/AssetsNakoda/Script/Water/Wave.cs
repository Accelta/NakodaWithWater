using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaveWireframe : MonoBehaviour
{
    public float waveSpeed = 1.0f;    // Speed of the wave animation
    public float waveHeight = 0.5f;   // Height of the wave
    public float waveFrequency = 1.0f; // Frequency of the wave

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        // Get the mesh from the MeshFilter component
        mesh = GetComponent<MeshFilter>().mesh;

        // Store the original vertices of the mesh
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        // Animate the vertices over time
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 originalVertex = originalVertices[i];
            displacedVertices[i] = originalVertex;

            // Apply sine wave based on both x and z coordinates
            float waveX = Mathf.Sin(Time.time * waveSpeed + originalVertex.x * waveFrequency);
            float waveZ = Mathf.Sin(Time.time * waveSpeed + originalVertex.z * waveFrequency);

            // Combine both waves for an "X" pattern effect
            displacedVertices[i].y += (waveX + waveZ) * waveHeight * 0.5f;
        }

        // Apply the modified vertices to the mesh
        mesh.vertices = displacedVertices;

        // Recalculate mesh bounds and normals for correct rendering
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Optional: Draw the wireframe
        DrawWireframe();
    }

    // Function to render the wireframe
    void DrawWireframe()
    {
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 v0 = transform.TransformPoint(displacedVertices[mesh.triangles[i]]);
            Vector3 v1 = transform.TransformPoint(displacedVertices[mesh.triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(displacedVertices[mesh.triangles[i + 2]]);

            Debug.DrawLine(v0, v1, Color.cyan);
            Debug.DrawLine(v1, v2, Color.cyan);
            Debug.DrawLine(v2, v0, Color.cyan);
        }
    }
}
