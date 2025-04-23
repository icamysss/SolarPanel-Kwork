using UnityEngine;
using UnityEngine.ProBuilder;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class HouseGenerator
    {
        private readonly HouseParam houseParam;
        private Transform parent;

        public HouseGenerator(HouseParam houseParam, Transform parent = null)
        {
            this.houseParam = houseParam;
            this.parent = parent;
        }

        /// <param name="length">В МЕТРАХ</param>
        /// <param name="width">В МЕТРАХ</param>
        public GameObject GenerateHouse()
        {
            return GenerateWithProBuilder();
        }

        private GameObject GenerateWithProBuilder()
        {
            // Создаем ProBuilder mesh
            var pbMesh = ShapeGenerator.GenerateCube(
                PivotLocation.Center,
                new Vector3(
                    houseParam.HouseWidth,
                    houseParam.HouseHeight,
                    houseParam.HouseLength
                    )
                );

            // Смещаем позицию чтобы нижняя грань была на Y=0
            pbMesh.transform.position = new Vector3(
                0f,
                houseParam.HouseHeight / 2f,
                0f
                );

            // Применяем изменения
            pbMesh.ToMesh();
            pbMesh.Refresh();

            // Создаем материал
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Constants.HOUSE_COLOR;

            // Назначаем материал
            pbMesh.GetComponent<MeshRenderer>().material = mat;

            // Конвертируем в обычный GameObject
            GameObject houseObject = pbMesh.gameObject;
            houseObject.name = "House";

            // Опционально: удаляем ProBuilder компоненты
            Object.DestroyImmediate(pbMesh);

            houseObject.transform.SetParent(parent);
            return houseObject;
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

            Vector3[] vertices = new Vector3[8];

            // Вершины (переопределены для правильного порядка)
            vertices[0] = new Vector3(-width / 2, 0, -length / 2); // 0
            vertices[1] = new Vector3(width / 2, 0, -length / 2); // 1
            vertices[2] = new Vector3(width / 2, 0, length / 2); // 2
            vertices[3] = new Vector3(-width / 2, 0, length / 2); // 3

            vertices[4] = new Vector3(-width / 2, height, -length / 2); // 4
            vertices[5] = new Vector3(width / 2, height, -length / 2); // 5
            vertices[6] = new Vector3(width / 2, height, length / 2); // 6
            vertices[7] = new Vector3(-width / 2, height, length / 2); // 7

            // Переопределенные треугольники с правильным порядком вершин
            int[] triangles =
            {
                // Bottom
                0, 2, 3,
                0, 1, 2,

                // Front
                3, 2, 6,
                3, 6, 7,

                // Right
                2, 1, 5,
                2, 5, 6,

                // Back
                1, 0, 4,
                1, 4, 5,

                // Left
                0, 3, 7,
                0, 7, 4,

                // Top
                7, 6, 5,
                7, 5, 4
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            // Принудительный пересчет нормалей
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            meshFilter.mesh = mesh;

            // URP материал
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mat.color = Constants.HOUSE_COLOR;
            meshRenderer.material = mat;
            houseObject.transform.SetParent(parent);
            return houseObject;
        }
    }
}