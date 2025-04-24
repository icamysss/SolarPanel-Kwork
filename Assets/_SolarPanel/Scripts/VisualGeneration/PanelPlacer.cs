using System;
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

        public PanelPlacer(Transform parent = null)
        {
            if (parent != null) _parent = parent;
            
            _houseParam = DataManager.Instance.HouseParam;
            _panel = DataManager.Instance.SelectedPanel;
            _panelCount = DataManager.Instance.GetPanelCount();
            _panelSize = new Vector3(_panel.Dimensions.x / 1000f, _panel.Dimensions.y / 1000f, _panel.Dimensions.z / 1000f);
            _optimalAngle = DataManager.Instance.GetAverageOptimalAngle();
            _startPoint = GetStartPosition();
            _availableRoofArea = GetUsefulRoofSize();
        }

        public GameObject Place()
        {
            if (!CheckSquarePanel()) return null;

            
            // определить кол - во рядов 
                // если четное, то центр кровли будет между рядами
                // если нечетно центр кровли будет по центру первого ряда
                
            // создать объект родитель для ряда
                // разместить его относительно центра в зависимости от кол-ва рядов 
                    // если четное кол - во рядов то первый ряд будет выше центра
                    
            // проверить угол установки (угол уст = угол опт - угол кровли) 
                // если отрицательный то проверять зазор по верхней части
                // если положительный то проверять зазор по нижней части панели
          
            // Создать ряд панелей
                // переместить родительский объект на вычисленную высоту (вдоль перпендикуляра к кровле)
                // повернуть родительский объект
                // заспавнить панели симмитрично центру кровли
            
            // Создать остальные ряды
            // на значить всем рядам родителя parent
            // вернуть родителя
            
            return null; 
        }



        /// <returns>false - если панели не влезут на кровлю</returns>
        private bool CheckSquarePanel()
        {
            Debug.Log($"Длина панели: {_panelSize.z}, Ширина панели: {_panelSize.x}");
            // Расчёт количества панелей в ряду и рядов
            var panelsPerRow = Mathf.FloorToInt((_availableRoofArea.x + Constants.PANELS_SPACING) /
                                                (_panelSize.x + Constants.PANELS_SPACING));
            var rows = Mathf.FloorToInt((_availableRoofArea.y + Constants.PANELS_SPACING) /
                                        (_panelSize.y + Constants.PANELS_SPACING));

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
            return result;
        }

        /// <returns>Точка центра плоскости кровли</returns>
        private Vector3 GetStartPosition()
        {
            var result = new Vector3(0, 0, 0);

            var hypotenuse = GetRoofWidth() / 2;
            result.y = hypotenuse * Mathf.Sin(_houseParam.Roof.Angle * Mathf.Deg2Rad) + _houseParam.HouseHeight;

            var single = hypotenuse * Mathf.Cos(_houseParam.Roof.Angle * Mathf.Deg2Rad) - result.y;

            var xCentr = _houseParam.Roof.RoofType == RoofType.Односкатная
                ? single
                : (_houseParam.HouseWidth + Constants.ROOF_OVERHANG * 2) / 4 - single;

            result.x = xCentr;

            Debug.Log($"Стартовая позиция: {result}");
            return result;
        }

        /// <returns>Объект солнечной панели, с правильным пивотом сразу</returns>
        private GameObject CreatePanel()
        {
            // Создаем ProBuilder mesh
            var pbMesh = ShapeGenerator.GenerateCube(PivotLocation.Center, _panelSize);

            // Смещаем позицию чтобы нижняя грань была на Y=0
            pbMesh.transform.position = new Vector3(
                0f,
                _panelSize.y / 2f,
                0f
                );

            // Применяем изменения
            pbMesh.ToMesh();
            pbMesh.Refresh();

            // Создаем материал
            var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = Constants.PANELS_COLOR;

            // Назначаем материал
            pbMesh.GetComponent<MeshRenderer>().material = mat;

            // Конвертируем в обычный GameObject
            var go = pbMesh.gameObject;
            go.name = "Panel";

            // Опционально: удаляем ProBuilder компоненты
            Object.DestroyImmediate(pbMesh);

            var panel = new GameObject(_panel.PanelName);
            go.transform.SetParent(panel.transform);

            return panel;
        }
    }
}