using UnityEngine;

namespace _SolarPanel.Scripts.VisualGeneration
{
    public class HouseGenerator
    {
        public GameObject GenerateHouse(float length, float width, float height)
        {
            var house = GameObject.CreatePrimitive(PrimitiveType.Cube);
            house.transform.localScale = new Vector3(length, height, width);
            house.transform.position = Vector3.zero;
            house.name = "House";
            return house;
        }
    }
}