using System.IO;
using UnityEngine;
using OfficeOpenXml; 

namespace _SolarPanel.Scripts
{
    public class ExcelGenerator : MonoBehaviour
    {
        [SerializeField] private string fileName = "GeneratedExcel.xlsx";
        [SerializeField] private string savePath = "Assets/GeneratedExcel/";

        
        public void GenerateExcelFile()
        {
            // Создание нового Excel-пакета
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Добавление листа
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Solar Panels");

                // Заголовки таблицы
                worksheet.Cells[1, 1].Value = "Харрактеристика";
                worksheet.Cells[1, 2].Value = "Значение";
            
                // Данные
                worksheet.Cells[2, 1].Value = "Город";
                worksheet.Cells[2, 2].Value = DataManager.Instance.SelectedCity.Name;
            
                worksheet.Cells[3, 1].Value = "Длина дома";
                worksheet.Cells[3, 2].Value = DataManager.Instance.HouseParam.HouseLength;
                
                worksheet.Cells[4, 1].Value = "Ширина Дома";
                worksheet.Cells[4, 2].Value = DataManager.Instance.HouseParam.HouseWidth;
                
                worksheet.Cells[5, 1].Value = "Город";
                worksheet.Cells[5, 2].Value = "-";
                
                worksheet.Cells[6, 1].Value = "Город";
                worksheet.Cells[6, 2].Value = "-";
                
                worksheet.Cells[7, 1].Value = "Город";
                worksheet.Cells[7, 2].Value = "-";
                
                worksheet.Cells[8, 1].Value = "Город";
                worksheet.Cells[8, 2].Value = "-";
                
                worksheet.Cells[9, 1].Value = "Город";
                worksheet.Cells[9, 2].Value = "-";
                
                worksheet.Cells[10, 1].Value = "Город";
                worksheet.Cells[10, 2].Value = "-";
                
                worksheet.Cells[11, 1].Value = "Город";
                worksheet.Cells[11, 2].Value = "-";
                
                worksheet.Cells[12, 1].Value = "Город";
                worksheet.Cells[12, 2].Value = "-";
                
                worksheet.Cells[13, 1].Value = "Город";
                worksheet.Cells[13, 2].Value = "-";
                
                worksheet.Cells[14, 1].Value = "Город";
                worksheet.Cells[14, 2].Value = "-";
                
                worksheet.Cells[15, 1].Value = "Город";
                worksheet.Cells[15, 2].Value = "-";
                
                worksheet.Cells[16, 1].Value = "Город";
                worksheet.Cells[16, 2].Value = "-";
                
                worksheet.Cells[17, 1].Value = "Город";
                worksheet.Cells[17, 2].Value = "-";
                
                

                // Путь для сохранения
               // savePath = GetFullPath();
                var filePath = Path.Combine(savePath, fileName);
                
                // Сохранение файла
                var excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);

                Debug.Log($"Excel файл создан: {filePath}");
            }
        }

        string GetFullPath()
        {
            // Создаем директорию, если не существует
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            return Path.Combine(savePath, fileName);
        }
    }
}