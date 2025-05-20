namespace _SolarPanel.Scripts
{
    using UnityEngine;
    using System.IO;
    // using NPOI.HSSF.UserModel; // Для XLS
    // using NPOI.SS.UserModel;

    public class ExcelGenerator : MonoBehaviour
    {
        // void Start()
        // {
        //     // Создаем новую книгу Excel (XLS)
        //     IWorkbook workbook = new HSSFWorkbook();
        //     ISheet sheet = workbook.CreateSheet("Sheet1");
        //
        //     // Заполняем данные
        //     IRow row = sheet.CreateRow(0);
        //     row.CreateCell(0).SetCellValue("Hello");
        //     row.CreateCell(1).SetCellValue("World!");
        //
        //     // Сохраняем файл
        //     string filePath = Path.Combine(Application.persistentDataPath, "test.xls");
        //     using (FileStream fs = new FileStream(filePath, FileMode.Create))
        //     {
        //         workbook.Write(fs);
        //     }
        //
        //     Debug.Log($"Excel файл создан: {filePath}");
        // }
    }
}