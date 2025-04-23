using System;
using System.Collections.Generic;
using _SolarPanel.Scripts.Data;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using Object = UnityEngine.Object;

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
            var triangleHeight = triangleBaseLength * Mathf.Tan(houseParam.Roof.Angle * Mathf.Deg2Rad);
           
            // Создаем меш треугольника по центру
            var triangle = CreateTriangle(triangleBaseLength, triangleHeight);
            var triangle2 = CreateTriangle(triangleBaseLength, triangleHeight, true);
            
            // Экструдируем
            Extrude(triangle, triangle.faces, ExtrudeMethod.IndividualFaces, roofLength / 2);
            Extrude(triangle2, triangle2.faces, ExtrudeMethod.IndividualFaces, roofLength / 2);
            
            
            // ставим на место
            triangle.transform.position = new Vector3(0f, houseParam.HouseHeight + 0.01f, 0f);
            triangle2.transform.position = new Vector3(0f, houseParam.HouseHeight + 0.01f, 0f);
            
            var roof = new GameObject("Roof");
            FinalizeRoof(triangle).transform.SetParent(roof.transform);
            FinalizeRoof(triangle2).transform.SetParent(roof.transform);
            return roof;
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

        private ProBuilderMesh CreateTriangle(float baseLenght, float height, bool inverse = false)
        {
            Vector3[] vertices =
            {
                new(-baseLenght / 2, 0f, 0f), // Левый нижний
                new(baseLenght / 2, 0f, 0f), // Правый нижний
                new(-baseLenght / 2, height, 0f) // Левый верхний
            };

            var face = new Face(new[] { 0, 1, 2 });
            if (inverse) face.Reverse();
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
            var roofObject = mesh.gameObject;
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