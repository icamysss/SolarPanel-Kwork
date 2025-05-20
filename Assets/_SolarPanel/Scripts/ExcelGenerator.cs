using System.IO;
using UnityEngine;
using OfficeOpenXml;
#if !UNITY_WEBGL && !UNITY_ANDROID && !UNITY_IOS
using SFB;
#endif

namespace _SolarPanel.Scripts
{
    public class ExcelGenerator : MonoBehaviour
    {
        [SerializeField] private string defaultFileName = "SolarReport.xlsx";
        [SerializeField] private string defaultFolder = "SolarReports";

        public void GenerateAndSaveExcel()
        {
            try
            {
                var excelData = GenerateExcelData();
                SaveExcelFile(excelData);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Ошибка генерации Excel: {ex.Message}");
            }
        }

        private byte[] GenerateExcelData()
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Отчёт");
                AddHeaders(worksheet);
                FillData(worksheet);
                FormatColumns(worksheet);
                return excelPackage.GetAsByteArray();
            }
        }

        private void AddHeaders(ExcelWorksheet ws)
        {
            ws.Cells[1, 1].Value = "Характеристика";
            ws.Cells[1, 2].Value = "Значение";
        }

        private void FillData(ExcelWorksheet ws)
        {
            var data = DataManager.Instance;
            int row = 2;

            AddRow(ws, ref row, "Город", data.SelectedCity?.Name ?? "-");
            AddRow(ws, ref row, "Длина дома", $"{data.HouseParam.HouseLength} м");
            AddRow(ws, ref row, "Ширина дома", $"{data.HouseParam.HouseWidth} м");
            AddRow(ws, ref row, "Тип кровли", data.HouseParam.Roof.RoofType.ToString());
            AddRow(ws, ref row, "Угол наклона кровли", $"{data.HouseParam.Roof.Angle}°");
            AddRow(ws, ref row, "Энергопотребление", $"{data.DailyConsumption} кВт·ч/день");
            AddRow(ws, ref row, "Солнечная панель", data.SelectedPanel?.PanelName ?? "-");
            
            if (data.SelectedPanel != null)
            {
                AddRow(ws, ref row, "Мощность панели", $"{data.SelectedPanel.NominalPower} кВт");
                AddRow(ws, ref row, "Длина панели", $"{data.SelectedPanel.Dimensions.x} м");
                AddRow(ws, ref row, "Ширина панели", $"{data.SelectedPanel.Dimensions.z} м");
                AddRow(ws, ref row, "Высота панели", $"{data.SelectedPanel.Dimensions.y} м");
            }
            
            AddRow(ws, ref row, "Уровень инсоляции", $"{data.GetCityAverageInsolation()} кВт·ч/м²/день");
            AddRow(ws, ref row, "Оптимальный угол наклона", $"{data.GetAverageOptimalAngle()}°");
            AddRow(ws, ref row, "Расчетный угол наклона", $"{data.HouseParam.Roof.Angle}°");
            AddRow(ws, ref row, "Требуемая мощность панелей", $"{data.RequiredPower} кВт");
            AddRow(ws, ref row, "Необходимое количество панелей", data.GetPanelCount());
        }

        private void AddRow(ExcelWorksheet ws, ref int row, string parameter, object value)
        {
            ws.Cells[row, 1].Value = parameter;
            ws.Cells[row, 2].Value = value;
            row++;
        }

        private void FormatColumns(ExcelWorksheet ws)
        {
            ws.Column(1).Width = 35;
            ws.Column(2).Width = 25;
            ws.Cells[1, 1, 1, 2].Style.Font.Bold = true;
        }

        private void SaveExcelFile(byte[] data)
        {
            Debug.Log("Standalone path: " + Path.Combine(Application.persistentDataPath, defaultFolder));
            var path = StandaloneFileBrowser.SaveFilePanel(
                "Сохранить отчёт",
                Path.Combine(Application.persistentDataPath, defaultFolder),
                defaultFileName,
                "xlsx");

            if (!string.IsNullOrEmpty(path))
            {
                SaveToFile(path, data);
            }
        }

        private void SaveToFile(string path, byte[] data)
        {
            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllBytes(path, data);
                Debug.Log($"Файл успешно сохранён: {path}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}