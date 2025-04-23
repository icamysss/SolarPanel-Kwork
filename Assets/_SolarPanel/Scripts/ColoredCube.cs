using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ColoredCube : MonoBehaviour
{
    void Start()
    {
        CreateCube();
    }

    void CreateCube()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Вершины для всех 6 граней (24 вершины)
        Vector3[] vertices = new Vector3[24];
        
        // Передняя грань
        vertices[0] = new Vector3(-0.5f, -0.5f, 0.5f);
        vertices[1] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[2] = new Vector3(0.5f, 0.5f, 0.5f);
        vertices[3] = new Vector3(-0.5f, 0.5f, 0.5f);
        
        // Задняя грань
        vertices[4] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[5] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[6] = new Vector3(-0.5f, 0.5f, -0.5f);
        vertices[7] = new Vector3(0.5f, 0.5f, -0.5f);
        
        // Верхняя грань
        vertices[8] = new Vector3(-0.5f, 0.5f, 0.5f);
        vertices[9] = new Vector3(0.5f, 0.5f, 0.5f);
        vertices[10] = new Vector3(0.5f, 0.5f, -0.5f);
        vertices[11] = new Vector3(-0.5f, 0.5f, -0.5f);
        
        // Нижняя грань
        vertices[12] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[13] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[14] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[15] = new Vector3(-0.5f, -0.5f, 0.5f);
        
        // Правая грань
        vertices[16] = new Vector3(0.5f, -0.5f, 0.5f);
        vertices[17] = new Vector3(0.5f, -0.5f, -0.5f);
        vertices[18] = new Vector3(0.5f, 0.5f, -0.5f);
        vertices[19] = new Vector3(0.5f, 0.5f, 0.5f);
        
        // Левая грань
        vertices[20] = new Vector3(-0.5f, -0.5f, -0.5f);
        vertices[21] = new Vector3(-0.5f, -0.5f, 0.5f);
        vertices[22] = new Vector3(-0.5f, 0.5f, 0.5f);
        vertices[23] = new Vector3(-0.5f, 0.5f, -0.5f);

        // Треугольники (6 граней × 2 треугольника × 3 вершины)
        int[] triangles = new int[36];
        
        for (int i = 0; i < 6; i++)
        {
            int startIndex = i * 4;
            int triIndex = i * 6;
            
            triangles[triIndex] = startIndex;
            triangles[triIndex + 1] = startIndex + 1;
            triangles[triIndex + 2] = startIndex + 2;
            
            triangles[triIndex + 3] = startIndex;
            triangles[triIndex + 4] = startIndex + 2;
            triangles[triIndex + 5] = startIndex + 3;
        }

        // Цвета для вершин (по 4 вершины на грань)
        Color[] colors = new Color[24];
        
        // Передняя (красная)
        FillFaceColor(colors, 0, Color.red);
        // Задняя (синяя)
        FillFaceColor(colors, 4, Color.blue);
        // Верхняя (зеленая)
        FillFaceColor(colors, 8, Color.green);
        // Нижняя (желтая)
        FillFaceColor(colors, 12, Color.yellow);
        // Правая (пурпурная)
        FillFaceColor(colors, 16, Color.magenta);
        // Левая (голубая)
        FillFaceColor(colors, 20, Color.cyan);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void FillFaceColor(Color[] colors, int startIndex, Color color)
    {
        for (int i = startIndex; i < startIndex + 4; i++)
        {
            colors[i] = color;
        }
    }
}