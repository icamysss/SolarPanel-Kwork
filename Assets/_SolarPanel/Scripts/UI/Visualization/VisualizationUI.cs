using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SolarPanel.Scripts.UI.Visualization
{
    public class VisualizationUI : CanvasGroupMenu
    {
        [SerializeField] private TextMeshProUGUI optimalAngle;
        [SerializeField] private TextMeshProUGUI installAngle;
       
        [Header("Report Button")]
        [SerializeField] private Button reportButton;
        [SerializeField] private Color failReport = Color.red;
        [SerializeField] private Color goodReport = Color.green;
        [SerializeField] private float timeToChangeColor = 5f;
        
        private ExcelGenerator exсelGenerator;
        private Color defaultColor; 
        

        private void Start()
        {
            defaultColor = reportButton.image.color;
            exсelGenerator = FindFirstObjectByType<ExcelGenerator>();
          
            if (exсelGenerator == null)
            {
                Debug.LogWarning("exelGenerator not found");
            }
            else
            {
                if (reportButton == null) return;
                reportButton.onClick.AddListener(OnReport);
            }
        }

        private void OnReport()
        {
            if (exсelGenerator.GenerateAndSaveExcel())
            {
                Debug.Log("Excel generated");
                StartCoroutine(ChangeColor(goodReport));
            }
            else
            {
                Debug.Log("Excel not generated");
                StartCoroutine(ChangeColor(failReport));
            }
        }

        private IEnumerator ChangeColor(Color c)
        {
            reportButton.image.color = c;
            reportButton.interactable = false;
            
            yield return new WaitForSeconds(timeToChangeColor);
            
            reportButton.interactable = true;
            reportButton.image.color = defaultColor;
            yield return null;
        }
        
        
        public void UpdateText()
        {
            optimalAngle.text = $"Отимальный угол :  {GetOptimalAngle()} º";
            installAngle.text = $"Угол установки      :  {GetInstallAngle()} º";
        }

        private string GetInstallAngle()
        {
            var optimalAngle = DataManager.Instance.GetAverageOptimalAngle();
            var roofAngle = DataManager.Instance.HouseParam.Roof.Angle;
            
            return Mathf.Abs(roofAngle - optimalAngle).ToString("0.00");
        }

        private string GetOptimalAngle()
        {
           return DataManager.Instance.GetAverageOptimalAngle().ToString("0.00");
        }
    }
}