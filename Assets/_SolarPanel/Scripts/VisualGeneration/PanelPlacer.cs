using System;
using System.Collections.Generic;
using _SolarPanel.Scripts.Data;
using UnityEngine;
using UnityEngine.ProBuilder;
using Object = UnityEngine.Object;

/*
 *  все расчеты выполняются относительно центра 0, 0, 0 - Так что если дом не центрирован
 * относительно начала координат необходимо ввнести изменения
 */

namespace _SolarPanel.Scripts.VisualGeneration
{ 
    
    public class PanelPlacer
    {
       private readonly HouseParam _houseParam;
        private readonly SolarPanel _panel;
        private readonly int _panelCount;// кол - во панелей
        private readonly Transform _parent;
        
        private readonly Vector3 _startPoint;  // центр кровли
        private readonly Vector3 _panelSize;  // размер панели в метрах
        private readonly float _optimalAngle; // Оптимальный угол относительно земли
        private readonly Vector2 _availableRoofArea;  // зона в которой можно разместить панели

        private Texture2D _panelTexture;
        public PanelPlacer(Transform parent, out Vector3 startPoint, Texture2D panelTexture)
        {
            if (parent != null) _parent = parent;
            
            _houseParam = DataManager.Instance.HouseParam;
            _panel = DataManager.Instance.SelectedPanel;
            _panelCount = DataManager.Instance.GetPanelCount();
            _panelSize = new Vector3(_panel.Dimensions.x / 1000f, _panel.Dimensions.y / 1000f, _panel.Dimensions.z / 1000f);
            _optimalAngle = DataManager.Instance.GetAverageOptimalAngle();
            _startPoint = GetStartPosition();
            startPoint = _startPoint;
            _availableRoofArea = GetUsefulRoofSize();
            _panelTexture  = panelTexture;
        }

        public GameObject Place()
        {
            if (!CheckSquarePanel()) return null;
    
            // расчитываем количество панелей в ряду и сколько рядом необходимо
            var panelsPerRow = Mathf.FloorToInt((_availableRoofArea.y + Constants.PANELS_SPACING) /
                                                (_panelSize.z + Constants.PANELS_SPACING));
            var rowsCount = Mathf.CeilToInt((float)_panelCount / panelsPerRow);
            Debug.Log($"Необходимо рядов: {rowsCount}");
    
            // Список хранения рядов 
            var rows = new List<Transform>();
            // Создаем родителей для рядов с симметричным расположением
            var rowSpacing = _panelSize.x + Constants.PANELS_SPACING;
            // делаем проекцию rowSpacing на ось X
            rowSpacing *= Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad);
            
            var isEvenRows = rowsCount % 2 == 0;
            for (var i = 0; i < rowsCount; i++)
            {
                var row = new GameObject($"row_{i}");
        
                // Расчёт смещения для симметрии
                float offset;
                if (isEvenRows)
                    offset = (i - rowsCount/2 + 0.5f) * rowSpacing;
                else
                    offset = (i - (rowsCount-1)/2) * rowSpacing;

                var rowPosition = new Vector3(
                    _startPoint.x + offset, 
                    _startPoint.y, 
                    0
                    );

                row.transform.position = rowPosition;
                rows.Add(row.transform);
            }

            var pCount = _panelCount;
            foreach (var row in rows)
            {
                // высота для каждого ряда, выраженная через позицию по x
                row.transform.position = new Vector3(
                    row.transform.position.x,
                    CalculateRowHeight(row.transform.position.x) , 
                    row.transform.position.z);
                // Определяем сколько панелей будет в этом ряду
                int panelsInThisRow = Mathf.Min(panelsPerRow, pCount);
        
                // Центрирование относительно фактического количества панелей
                for (var panelIndex = 0; panelIndex < panelsInThisRow; panelIndex++)
                {
                    // Расчет позиции с центрированием
                    float zPos = (panelIndex - (panelsInThisRow - 1) * 0.5f) * (_panelSize.z + Constants.PANELS_SPACING);
                    var panel = CreatePanel();
                    panel.transform.SetParent(row.transform);
                    panel.transform.localPosition = new Vector3(0, 0, zPos);
                }
                pCount -= panelsInThisRow;
            }
            
            // Все в контейнер и возвращаем
            var panelsRoot = new GameObject("SolarPanels");
            panelsRoot.transform.SetParent(_parent);
            foreach (var row in rows)
            {
                // устанавливаем угол для каждого ряда
                row.transform.localRotation = CalculateRowAngle();
                row.SetParent(panelsRoot.transform);
            }
            return panelsRoot;
        }
        
        /// <param name="xPos"> Позиция по x</param>
        /// <returns>высоту по координате Y</returns>
        private float CalculateRowHeight(float xPos)
        {
            var panelNormal = 0f;
            var panelHeight = 0f;

            if (_optimalAngle >= _houseParam.Roof.Angle)
            {
                // Высота подъема панели от кровли (перпендикуляр от панели на крышу)
                panelNormal = _panelSize.x / 2 *
                              Mathf.Sin((_optimalAngle - _houseParam.Roof.Angle) * Mathf.Deg2Rad)
                              + Constants.PANELS_SPACE_FROM_ROOF;
            }
            else
            {
                panelNormal = _panelSize.x / 2 *
                              Mathf.Sin((_houseParam.Roof.Angle - _optimalAngle) * Mathf.Deg2Rad)
                              + Constants.PANELS_SPACE_FROM_ROOF;
            }

            // Расстояние от панели до кровли в точке xPos
            panelHeight = panelNormal / Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad);
            // Высота кровли в точке xPos
            var roofHeightAtPos = (GetRoofWidth() / 2 * Mathf.Sin(_houseParam.Roof.Angle * Mathf.Deg2Rad))
                                  - (xPos - _startPoint.x) * Mathf.Tan(_houseParam.Roof.Angle * Mathf.Deg2Rad);


            return panelHeight + roofHeightAtPos + _houseParam.HouseHeight;
        }

        private Quaternion CalculateRowAngle()
        {
            return Quaternion.Euler(0, 0, - _optimalAngle);
        }
        
        /// <returns>false - если панели не влезут на кровлю</returns>
        private bool CheckSquarePanel()
        {
            Debug.Log($"Длина панели: {_panelSize.z}, Ширина панели: {_panelSize.x}");
            // Расчёт количества панелей в ряду и рядов
            var panelsPerRow = Mathf.FloorToInt((_availableRoofArea.y + Constants.PANELS_SPACING) /
                                                (_panelSize.x + Constants.PANELS_SPACING));
            var rows = Mathf.FloorToInt((_availableRoofArea.x + Constants.PANELS_SPACING) /
                                        (_panelSize.z + Constants.PANELS_SPACING));

            var maxPossible = panelsPerRow * rows;
            if (maxPossible < _panelCount)
            {
                Debug.Log($"Недостаточно места. Максимум: {maxPossible}, требуется: {_panelCount}");
                return false;
            }

            Debug.Log($"Размещение возможно. Максимум: {maxPossible}, требуется: {_panelCount}");
            return true;
        }

        /// <returns>Vector2 где x - ширина(гипотенуза), y - длина (длина кровли)</returns>
        private Vector2 GetUsefulRoofSize()
        {
            var result = new Vector2(0, 0)
            {
                // полезная длина кровли для размещения
                y = (_houseParam.HouseLength + Constants.ROOF_OVERHANG * 2) - Constants.PANELS_SPACING_FROM_END_ROOF * 2,
                x = GetRoofWidth() - Constants.PANELS_SPACING_FROM_END_ROOF * 2,
            };


            Debug.Log($"Полезный размер кровли: длина {result.y}, ширина {result.x}");
            return result;
        }

        /// <returns>Ширину кровли (размер по X) </returns>
        private float GetRoofWidth()
        {
            var result = 0f;

            switch (_houseParam.Roof.RoofType)
            {
                case RoofType.Односкатная:
                    var singleBaseLength = _houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2;
                    result = singleBaseLength / Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad);
                    break;
                case RoofType.Двухскатная:
                    var doubleBaseLength = (_houseParam.HouseWidth / 2) + Constants.ROOF_OVERHANG;
                    result = doubleBaseLength / Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log($"Ширина кровли: {result}");
            return result;
        }

        /// <returns>Точка центра плоскости кровли</returns>
        private Vector3 GetStartPosition()
        {
            var result = new Vector3(0, 0, 0);

            var hypotenuse = GetRoofWidth() / 2;
            result.y = hypotenuse * Mathf.Sin(_houseParam.Roof.Angle * Mathf.Deg2Rad) + _houseParam.HouseHeight;
            var halfRoofSize = _houseParam.HouseWidth / 2 + Constants.ROOF_OVERHANG;
            
            var singleX = halfRoofSize - hypotenuse * Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad);
            var xCentr = _houseParam.Roof.RoofType == RoofType.Односкатная
                ? singleX
                : halfRoofSize - (hypotenuse * Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad));

            result.x = xCentr;
            // дебаг 
            var debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debugSphere.transform.localScale = Vector3.one * 0.2f;
            debugSphere.transform.position = result;
            debugSphere.GetComponent<Renderer>().material.color = Color.red;
            Object.Destroy(debugSphere, 5f); // Автоматическое удаление через 5 секунд
            // дебаг
            Debug.Log($"Стартовая позиция: {result}");
            return result;
        }

        /// <returns>Объект солнечной панели, с правильным пивотом сразу</returns>
        private GameObject CreatePanel()
        {
             // Создаем основной объект панели
            GameObject panel = new GameObject(_panel.PanelName);
            
            // Добавляем компоненты меша
            MeshFilter meshFilter = panel.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = panel.AddComponent<MeshRenderer>();

            // Создаем и настраиваем меш
            Mesh mesh = new Mesh();
            mesh.name = "PanelMesh";

            // Размеры панели из параметров
            Vector3 size = _panelSize;
            
            // 1. Вершины (24 точки - 6 граней × 4 вершины)
            Vector3[] vertices = new Vector3[24];
            
            // Передняя грань (Z+)
            vertices[0] = new Vector3(-size.x/2, -size.y/2,  size.z/2);
            vertices[1] = new Vector3( size.x/2, -size.y/2,  size.z/2);
            vertices[2] = new Vector3( size.x/2,  size.y/2,  size.z/2);
            vertices[3] = new Vector3(-size.x/2,  size.y/2,  size.z/2);

            // Задняя грань (Z-)
            vertices[4] = new Vector3(-size.x/2, -size.y/2, -size.z/2);
            vertices[5] = new Vector3( size.x/2, -size.y/2, -size.z/2);
            vertices[6] = new Vector3( size.x/2,  size.y/2, -size.z/2);
            vertices[7] = new Vector3(-size.x/2,  size.y/2, -size.z/2);

            // Боковые грани
            // Левая (X-)
            vertices[8] = vertices[4]; vertices[9] = vertices[0];
            vertices[10] = vertices[3]; vertices[11] = vertices[7];

            // Правая (X+)
            vertices[12] = vertices[1]; vertices[13] = vertices[5];
            vertices[14] = vertices[6]; vertices[15] = vertices[2];

            // Верхняя (Y+)
            vertices[16] = vertices[3]; vertices[17] = vertices[2];
            vertices[18] = vertices[6]; vertices[19] = vertices[7];

            // Нижняя (Y-)
            vertices[20] = vertices[0]; vertices[21] = vertices[1];
            vertices[22] = vertices[5]; vertices[23] = vertices[4];

            // 2. Треугольники (12 треугольников)
            int[] triangles = new int[36];
            int triIndex = 0;
            
            void AddFace(int startVertex) {
                triangles[triIndex++] = startVertex;
                triangles[triIndex++] = startVertex + 1;
                triangles[triIndex++] = startVertex + 2;
                triangles[triIndex++] = startVertex;
                triangles[triIndex++] = startVertex + 2;
                triangles[triIndex++] = startVertex + 3;
            }

            for(int i = 0; i < 6; i++) {
                AddFace(i * 4);
            }

            // 3. UV-координаты
            Vector2[] uv = new Vector2[24];
            float textureScale = 1f; // Масштаб текстуры

            for(int i = 0; i < 6; i++) {
                // Верхняя грань (индексы 4-й грани)
                if(i == 4) {
                    // UV для верхней грани (вершины 16-19)
                    uv[16] = new Vector2(0, 0);
                    uv[17] = new Vector2(size.x * textureScale, 0);
                    uv[18] = new Vector2(size.x * textureScale, size.z * textureScale);
                    uv[19] = new Vector2(0, size.z * textureScale);
                }
                else {
                    // Простые UV для остальных граней
                    int startIndex = i * 4;
                    uv[startIndex] = new Vector2(0, 0);
                    uv[startIndex + 1] = new Vector2(1, 0);
                    uv[startIndex + 2] = new Vector2(1, 1);
                    uv[startIndex + 3] = new Vector2(0, 1);
                }
            }
            

            // 4. Настройка меша
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;

            // 5. Настройка материала
            var panelMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            panelMat.mainTexture = _panelTexture;
            panelMat.mainTextureScale = Vector2.one; // Тайлинг 1x1
            panelMat.mainTexture.wrapMode = TextureWrapMode.Repeat;
            
            meshRenderer.material = panelMat;

            // Смещаем пивот в нижнюю грань
            panel.transform.position = new Vector3(0, size.y/2, 0);
            
            return panel;
        }
    }
}