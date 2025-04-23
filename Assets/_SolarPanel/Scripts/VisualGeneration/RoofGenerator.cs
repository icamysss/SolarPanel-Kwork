using _SolarPanel.Scripts.Data;
using UnityEngine;

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
            if (houseParam.Roof.RoofType == RoofType.Односкатная)  return GenerateSingleRoof();
            if (houseParam.Roof.RoofType == RoofType.Двухскатная)  return GenerateDoubleRoof();
            return null;
        }

        private GameObject GenerateSingleRoof()
        {
            // при помощи Probuilder api создаем объект 
            
            // если угол кровли ( houseParam.Roof.Angle) равен 0 . создаем плоскость в нулевых координах на высоте houseParam.HouseHeight + 0.01,  
            // в размерах  houseParam.HouseLength + Constants.ROOF_OVERHANG по оси Z,
            // houseParam.HouseWidth + Constants.ROOF_OVERHANG по оси X;
            // окрашиваем плоскость в цвет Constants.ROOF_COLOR
            // назнаем объекту ему parent = parent;
            // возвращаем объект
            
            // если угол кровли больше 0. создаем Треугольную призму.
            // Основание которой это прямоугольный треугольник,
            // угол между гипотенузой и основанием  houseParam.Roof.Angle
            // Длина основания houseParam.HouseWidth + Constants.ROOF_OVERHANG + Constants.ROOF_OVERHANG
            // Основание треугольника находится на высоте houseParam.HouseHeight + 0.01
            // а так же по оси Z в координате (houseParam.HouseLength / 2 ) + Constants.ROOF_OVERHANG
            // второе основание призмы находится зеркально первому, относительно нулевых координат
            // Завершаем создание призмы соединяя основания выше и красим ее в цвет Constants.ROOF_COLOR
            
            // назнаем объекту ему parent = parent;
            // возвращаем объект
            
            return null;
           
        }

        private GameObject GenerateDoubleRoof()
        {
            // при помощи Probuilder api создаем объект 
            
            // если угол кровли ( houseParam.Roof.Angle) равен 0 . создаем плоскость в нулевых координах на высоте houseParam.HouseHeight + 0.01,  
            // в размерах  houseParam.HouseLength + Constants.ROOF_OVERHANG по оси Z,
            // houseParam.HouseWidth + Constants.ROOF_OVERHANG по оси X;
            // окрашиваем плоскость в цвет Constants.ROOF_COLOR
            // назнаем объекту ему parent = parent;
            // возвращаем объект
            
            // если угол кровли больше 0. создаем Треугольную призму.
            // Основание которой это равнобедренный треугольник,
            // угол между ребром и основанием  houseParam.Roof.Angle
            // Длина основания houseParam.HouseWidth + Constants.ROOF_OVERHANG + Constants.ROOF_OVERHANG
            // Основание треугольника находится на высоте houseParam.HouseHeight + 0.01
            // а так же по оси Z в координате (houseParam.HouseLength / 2 ) + Constants.ROOF_OVERHANG
            // второе основание призмы находится зеркально первому, относительно нулевых координат
            // Завершаем создание призмы соединяя основания и красим ее в цвет Constants.ROOF_COLOR
            
            // назнаем объекту ему parent = parent;
            // возвращаем объект
            
            return null;
        }
    }
}