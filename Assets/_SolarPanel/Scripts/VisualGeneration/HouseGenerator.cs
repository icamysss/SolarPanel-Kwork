using UnityEngine;
using UnityEngine.ProBuilder;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class HouseGenerator
    {
        private readonly HouseParam houseParam;
        private readonly Transform parent;
        private readonly Texture2D wallTexture;

        public HouseGenerator(HouseParam houseParam, Transform parent = null, Texture2D texture = null)
        {
            this.houseParam = houseParam;
            this.parent = parent;
            this.wallTexture = texture;
        }

        public GameObject GenerateHouse()
        {
            return SimpleGenerate();
        }

        private GameObject SimpleGenerate()
        {
            GameObject houseObject = new GameObject("House");
            houseObject.transform.position = Vector3.zero;

            MeshFilter meshFilter = houseObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = houseObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();

            float length = houseParam.HouseLength;
            float width = houseParam.HouseWidth;
            float height = houseParam.HouseHeight;

            // 24 вершины (6 граней × 4 вершины)
            Vector3[] vertices = new Vector3[24];
            Vector2[] uv = new Vector2[24];
            float textureScale = 0.6f;

            // 1. Передняя грань (Z-)
            vertices[0] = new Vector3(-width/2, 0, -length/2);
            vertices[1] = new Vector3(width/2, 0, -length/2);
            vertices[2] = new Vector3(width/2, height, -length/2);
            vertices[3] = new Vector3(-width/2, height, -length/2);

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(width * textureScale, 0);
            uv[2] = new Vector2(width * textureScale, height * textureScale);
            uv[3] = new Vector2(0, height * textureScale);

            // 2. Задняя грань (Z+)
            vertices[4] = new Vector3(-width/2, 0, length/2);
            vertices[5] = new Vector3(width/2, 0, length/2);
            vertices[6] = new Vector3(width/2, height, length/2);
            vertices[7] = new Vector3(-width/2, height, length/2);

            uv[4] = new Vector2(0, 0);
            uv[5] = new Vector2(width * textureScale, 0);
            uv[6] = new Vector2(width * textureScale, height * textureScale);
            uv[7] = new Vector2(0, height * textureScale);

            // 3. Левая грань (X-)
            vertices[8] = new Vector3(-width/2, 0, -length/2);
            vertices[9] = new Vector3(-width/2, 0, length/2);
            vertices[10] = new Vector3(-width/2, height, length/2);
            vertices[11] = new Vector3(-width/2, height, -length/2);

            uv[8] = new Vector2(0, 0);
            uv[9] = new Vector2(length * textureScale, 0);
            uv[10] = new Vector2(length * textureScale, height * textureScale);
            uv[11] = new Vector2(0, height * textureScale);

            // 4. Правая грань (X+)
            vertices[12] = new Vector3(width/2, 0, -length/2);
            vertices[13] = new Vector3(width/2, 0, length/2);
            vertices[14] = new Vector3(width/2, height, length/2);
            vertices[15] = new Vector3(width/2, height, -length/2);

            uv[12] = new Vector2(0, 0);
            uv[13] = new Vector2(length * textureScale, 0);
            uv[14] = new Vector2(length * textureScale, height * textureScale);
            uv[15] = new Vector2(0, height * textureScale);

            // 5. Верхняя грань (Y+)
            vertices[16] = new Vector3(-width/2, height, -length/2);
            vertices[17] = new Vector3(width/2, height, -length/2);
            vertices[18] = new Vector3(width/2, height, length/2);
            vertices[19] = new Vector3(-width/2, height, length/2);

            uv[16] = new Vector2(0, 0);
            uv[17] = new Vector2(width * textureScale, 0);
            uv[18] = new Vector2(width * textureScale, length * textureScale);
            uv[19] = new Vector2(0, length * textureScale);

            // 6. Нижняя грань (Y-)
            vertices[20] = new Vector3(-width/2, 0, -length/2);
            vertices[21] = new Vector3(width/2, 0, -length/2);
            vertices[22] = new Vector3(width/2, 0, length/2);
            vertices[23] = new Vector3(-width/2, 0, length/2);

            uv[20] = new Vector2(0, 0);
            uv[21] = new Vector2(width * textureScale, 0);
            uv[22] = new Vector2(width * textureScale, length * textureScale);
            uv[23] = new Vector2(0, length * textureScale);

            // Треугольники (6 граней × 6 индексов)
            int[] triangles = {
                // Передняя
                0, 2, 1, 0, 3, 2,
                // Задняя
                4, 5, 6, 4, 6, 7,
                // Левая
                8, 9, 10, 8, 10, 11,
                // Правая
                13, 12, 15, 13, 15, 14,
                // Верхняя
                16, 17, 18, 16, 18, 19,
                // Нижняя
                21, 20, 23, 21, 23, 22
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            meshFilter.mesh = mesh;

            // Настройка материала
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            if(wallTexture != null)
            {
                mat.mainTexture = wallTexture;
                mat.mainTextureScale = Vector2.one;
                mat.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
            else
            {
                mat.color = Constants.HOUSE_COLOR;
            }
            meshRenderer.material = mat;

            houseObject.transform.SetParent(parent);
            return houseObject;
        }
    }
}