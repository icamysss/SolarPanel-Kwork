using System.Collections.Generic;
using _SolarPanel.Scripts.Data;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class RoofGenerator
    {
        private readonly HouseParam houseParam;
        private readonly Transform parent;

        public RoofGenerator(HouseParam houseParam, Transform parent = null)
        {
            this.houseParam = houseParam;
            this.parent = parent;
        }


        public GameObject GenerateRoof()
        {
            if (houseParam.Roof.RoofType == RoofType.Односкатная) return GenerateSingleRoof();
            if (houseParam.Roof.RoofType == RoofType.Двухскатная) return GenerateDoubleRoof();
            return null;
        }

        private GameObject GenerateSingleRoof()
        {
            if (houseParam.Roof.Angle == 0f)
            {
                // Создаем плоскую крышу
                var roofMesh = ShapeGenerator.GeneratePlane(
                    PivotLocation.Center,
                    houseParam.HouseLength, //+ Constants.ROOF_OVERHANG * 2,
                    houseParam.HouseWidth, // + Constants.ROOF_OVERHANG * 2,
                    1, 1,
                    Axis.Up
                    );

                roofMesh.transform.position = new Vector3(
                    0f,
                    houseParam.HouseHeight + 0.01f,
                    0f
                    );

                return FinalizeRoof(roofMesh);
            }

            // Рассчитываем параметры
            var triangleBaseLength = houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2 ;
            var roofLength = houseParam.HouseLength + Constants.ROOF_OVERHANG * 2;
            var triangleHeight = (triangleBaseLength / 2) * Mathf.Tan(houseParam.Roof.Angle * Mathf.Deg2Rad);
            
            // Создаем прямоугольный треугольник и экструдируем
            var triangle = CreateRightTriangle(triangleBaseLength, triangleHeight);
            ExtrudeWithFrontons(triangle, roofLength);

            // Позиционируем
            triangle.transform.position = new Vector3(
                0f,
                houseParam.HouseHeight + 0.01f,
                -roofLength / 2f
                );

            return FinalizeRoof(triangle);
        }
        
        private GameObject GenerateDoubleRoof()
        {
            if (houseParam.Roof.Angle == 0f)
            {
                // Создаем плоскую крышу аналогично односкатной
                return GenerateSingleRoof();
            }
            else
            {
                // Рассчитываем параметры треугольной призмы
                var baseWidth = houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2;
                var fullLength = houseParam.HouseLength + Constants.ROOF_OVERHANG * 2;
                var height = (baseWidth / 2) * Mathf.Tan(houseParam.Roof.Angle * Mathf.Deg2Rad);

                // Создаем треугольник основания

                var triangle = ShapeGenerator.GeneratePrism(PivotLocation.Center,
                    new Vector3(baseWidth, height, fullLength));
              
                // Позиционируем и дублируем для симметрии
                var roof = FinalizeRoof(triangle);
                roof.transform.localPosition = new Vector3(0, houseParam.HouseHeight + height / 2, 0);

                return roof;
            }
        }

        private ProBuilderMesh CreateRightTriangle(float baseLenght, float height)
        {
            Vector3[] vertices =
            {
                new(-baseLenght / 2, 0f, 0f), // Левый нижний
                new(baseLenght / 2, 0f, 0f), // Правый нижний
                new(-baseLenght / 2, height, 0f) // Левый верхний
            };

            var face = new Face(new[] { 0, 1, 2 });
            var mesh = ProBuilderMesh.Create(vertices, new[] { face });


            // Автоматическая генерация UV
            mesh.unwrapParameters = new UnwrapParameters()
            {
                angleError = 0.1f,
                areaError = 0.1f
            };

            mesh.ToMesh();
            mesh.Refresh();
            return mesh;
        }

        private void ExtrudeWithFrontons(ProBuilderMesh mesh, float length)
        {
            // Используем метод экструзии без ExtrudeOptions
            mesh.Extrude(
                new List<Face> { mesh.faces[0] },
                ExtrudeMethod.FaceNormal,
                length
                );

            CloseFronton(mesh, true);
            CloseFronton(mesh, false);

            // Генерируем UV через стандартный метод
           // mesh.GenerateUV();
            mesh.Refresh();
            mesh.ToMesh();
        }

        private void CloseFronton(ProBuilderMesh mesh, bool isFront)
        {
            Bounds bounds = mesh.GetComponent<MeshFilter>().sharedMesh.bounds;
            float targetZ = isFront ? bounds.min.z : bounds.max.z;

            List<int> frontonVertices = new List<int>();

            // Получаем вершины через индексы
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector3 pos = mesh.positions[i];
                if (Mathf.Abs(pos.z - targetZ) < 0.001f)
                {
                    frontonVertices.Add(i);
                }
            }

            if (frontonVertices.Count == 3)
            {
                // Правильный порядок вершин для нормалей
                Face frontonFace = new Face(new int[] {
                    frontonVertices[0],
                    frontonVertices[2],
                    frontonVertices[1]
                });
                mesh.faces.Add(frontonFace);
            }

            mesh.ToMesh();
            mesh.Refresh();
            mesh.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();
        }
        
        private GameObject FinalizeRoof(ProBuilderMesh mesh)
        {
            // Применяем материал
            Material roofMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            roofMaterial.color = Constants.ROOF_COLOR;
            mesh.GetComponent<MeshRenderer>().material = roofMaterial;

            // Назначаем родителя
            if (parent != null)
            {
                mesh.transform.SetParent(parent, false);
            }

            // Конвертируем в обычный GameObject
            GameObject roofObject = mesh.gameObject;
            roofObject.name = "Roof";
            Object.DestroyImmediate(mesh);

            return roofObject;
        }

        private void Extrude(ProBuilderMesh mesh, IEnumerable<Face> faces, ExtrudeMethod method, float distance)
        {
            mesh.Extrude(faces, method, distance);
            mesh.ToMesh();
            mesh.Refresh();
        }
    }
}