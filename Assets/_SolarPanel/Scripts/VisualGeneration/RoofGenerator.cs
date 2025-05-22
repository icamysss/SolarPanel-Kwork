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
        private readonly HouseParam _houseParam;
        private readonly Transform _parent;
        private Texture2D _texture;

        public RoofGenerator(HouseParam houseParam, Transform parent = null, Texture2D texture = null)
        {
            this._houseParam = houseParam;
            this._parent = parent;
            _texture = texture;
        }


        public GameObject GenerateRoof()
        {
            if (_houseParam.Roof.RoofType == RoofType.Односкатная) return GenerateSingleRoof();
            if (_houseParam.Roof.RoofType == RoofType.Двухскатная) return GenerateDoubleRoof();
            return null;
        }

        private GameObject GenerateSingleRoof()
        {
            if (_houseParam.Roof.Angle == 0f)
            {
                // Создаем плоскую крышу
                var roofMesh = ShapeGenerator.GeneratePlane(
                    PivotLocation.Center,
                    _houseParam.HouseLength, //+ Constants.ROOF_OVERHANG * 2,
                    _houseParam.HouseWidth, // + Constants.ROOF_OVERHANG * 2,
                    1, 1,
                    Axis.Up
                    );

                roofMesh.transform.position = new Vector3(
                    0f,
                    _houseParam.HouseHeight + 0.01f,
                    0f
                    );

                return FinalizeRoof(roofMesh);
            }

            // Рассчитываем параметры
            var triangleBaseLength = _houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2 ;
            var roofLength = _houseParam.HouseLength + Constants.ROOF_OVERHANG * 2;
            var triangleHeight = triangleBaseLength * Mathf.Tan(_houseParam.Roof.Angle * Mathf.Deg2Rad);
           
            // Создаем меш треугольника по центру
            var triangle = CreateTriangle(triangleBaseLength, triangleHeight);
            var triangle2 = CreateTriangle(triangleBaseLength, triangleHeight, true);
            
            // Экструдируем
            Extrude(triangle, triangle.faces, ExtrudeMethod.IndividualFaces, roofLength / 2);
            Extrude(triangle2, triangle2.faces, ExtrudeMethod.IndividualFaces, roofLength / 2);
            
            
            // ставим на место
            triangle.transform.position = new Vector3(0f, _houseParam.HouseHeight, 0f);
            triangle2.transform.position = new Vector3(0f, _houseParam.HouseHeight, 0f);
            
            var roof = new GameObject("Roof");
            FinalizeRoof(triangle).transform.SetParent(roof.transform);
            FinalizeRoof(triangle2).transform.SetParent(roof.transform);
            roof.transform.SetParent(_parent);
            return roof;
        }
        
        private GameObject GenerateDoubleRoof()
        {
            if (_houseParam.Roof.Angle == 0f)
            {
                // Создаем плоскую крышу аналогично односкатной
                return GenerateSingleRoof();
            }
            else
            {
                // Рассчитываем параметры треугольной призмы
                var baseWidth = _houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2;
                var fullLength = _houseParam.HouseLength + Constants.ROOF_OVERHANG * 2;
                var height = (baseWidth / 2) * Mathf.Tan(_houseParam.Roof.Angle * Mathf.Deg2Rad);

                // Создаем треугольник основания

                var triangle = ShapeGenerator.GeneratePrism(PivotLocation.Center,
                    new Vector3(baseWidth, height, fullLength));
              
                // Позиционируем и дублируем для симметрии
                var roof = FinalizeRoof(triangle);
                roof.transform.localPosition = new Vector3(0, _houseParam.HouseHeight + height / 2, 0);

                roof.transform.SetParent(_parent);
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
            roofMaterial.mainTexture = _texture;
            roofMaterial.mainTextureOffset = new Vector2(0.5f, 0.5f);
            roofMaterial.mainTextureScale = new Vector2(-.5f, -.5f);
           
            //roofMaterial.color = Constants.ROOF_COLOR;
            mesh.GetComponent<MeshRenderer>().material = roofMaterial;
            

            // Назначаем родителя
            if (_parent != null)
            {
                mesh.transform.SetParent(_parent, false);
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