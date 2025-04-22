using _SolarPanel.Scripts.Data;
using UnityEngine;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class RoofGenerator
    {
        public void GenerateRoof(GameObject house, float length, float width, RoofType roofType) {
            // Основные параметры дома
            var houseScale = house.transform.localScale;
            var roofOverhang = 0.5f; // Выступ крыши

            // Свесы по бокам (4 куба)
            CreateRoofPart(house, new Vector3(0, houseScale.y / 2, 0), new Vector3(houseScale.x + 2 * roofOverhang, 0.1f, roofOverhang));
            CreateRoofPart(house, new Vector3(0, houseScale.y / 2, 0), new Vector3(houseScale.x + 2 * roofOverhang, 0.1f, roofOverhang), Quaternion.Euler(0, 180, 0));
            CreateRoofPart(house, new Vector3(0, houseScale.y / 2, 0), new Vector3(roofOverhang, 0.1f, houseScale.z + 2 * roofOverhang));
            CreateRoofPart(house, new Vector3(0, houseScale.y / 2, 0), new Vector3(roofOverhang, 0.1f, houseScale.z + 2 * roofOverhang), Quaternion.Euler(0, 180, 0));
        }

        private void CreateRoofPart(GameObject parent, Vector3 position, Vector3 scale, Quaternion rotation = default) {
            GameObject roofPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roofPart.transform.parent = parent.transform;
            roofPart.transform.localPosition = position;
            roofPart.transform.localScale = scale;
            roofPart.transform.rotation = rotation;
            roofPart.name = "RoofPart";
        }
    }
}