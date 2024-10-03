using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Http;
using TAM.VMS.Domain;
using ExcelDataReader;

namespace TAM.VMS.Web.Helper
{
    public class ExcelHelper
    {
        public static DataTable ReadExcelFile(IFormFile file)
        {
            string FileSaveWithPath = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles\\Temp " + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx");
            DataTable DT = new DataTable();
            int HeaderRowIndex = 0;
            
            using (var fs = new FileStream(FileSaveWithPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            using (var stream = File.Open(FileSaveWithPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {   
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = reader => new ExcelDataTableConfiguration
                        {
                            // Set the UseHeaderRow option to true, to skip for first row (header row)
                            UseHeaderRow = true,
                            ReadHeaderRow = (reader) =>
                            {
                                bool empty = true;
                                HeaderRowIndex = 0;

                                while (empty)
                                {
                                    for (var i = 0; i < reader.FieldCount && empty; i++)
                                        empty = string.IsNullOrWhiteSpace(reader.GetString(i));

                                    if (empty)
                                    {
                                        empty = reader.Read(); // Only continue if more content is available
                                        HeaderRowIndex++; // Keep track of the first row position.
                                    }
                                }
                            }
                        }
                    });
                    DT = result.Tables[0];
                }
            }

            File.Delete(FileSaveWithPath);
            return DT;
        }

        private static bool IsRowEmpty(IRow row, int startColumn, int endColumn)
        {
            bool isEmpty = true;

            for (int i = startColumn - 1; i < endColumn; i++)
            {
                //var testCell = row.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                var cell = row.GetCell(i, MissingCellPolicy.RETURN_NULL_AND_BLANK);
                if (cell != null)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            if (!string.IsNullOrEmpty(cell.NumericCellValue.ToString()))
                            {
                                isEmpty = false;
                                break;
                            }
                            break;

                        case CellType.String:
                            if (!string.IsNullOrEmpty(cell.StringCellValue))
                            {
                                isEmpty = false;
                                break;
                            }
                            break;
                    }
                }
            }

            return isEmpty;
        }

        public static DataTable ReadExcelFile(IFormFile file, int startColumn, int endColumn)
        {
            string FileSaveWithPath = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles\\Temp " + Guid.NewGuid().ToString() + ".xlsx");

            using (var fs = new FileStream(FileSaveWithPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            XSSFWorkbook wb;
            XSSFSheet sh;
            String Sheet_name;

            using (var fs = new FileStream(FileSaveWithPath, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(fs);

                Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
            }
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT.Columns.Clear();

            // get sheet
            sh = (XSSFSheet)wb.GetSheet(Sheet_name);

            int i = 0;
            while (sh.GetRow(i) != null)
            {
                // write row value, except row 0 which is header

                var row = sh.GetRow(i);
                if (i != 0)
                {
                    DataRow dr = DT.NewRow();

                    if (row != null && !IsRowEmpty(row, startColumn, endColumn))
                    {
                        for (int j = startColumn - 1; j < endColumn; j++)
                        {
                            var cell = row.GetCell(j, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK);

                            if (cell != null)
                            {
                                // TODO: you can add more cell types capatibility, e. g. formula
                                switch (cell.CellType)
                                {
                                    case NPOI.SS.UserModel.CellType.Numeric:
                                        dr[j] = cell.NumericCellValue;
                                        //dataGridView1[j, i].Value = sh.GetRow(i).GetCell(j).NumericCellValue;

                                        break;
                                    case NPOI.SS.UserModel.CellType.String:
                                        //dr[j] = cell.StringCellValue.ToUpper();
                                        dr[j] = cell.StringCellValue;

                                        break;
                                }
                            }
                        }

                        DT.Rows.Add(dr);
                    }
                }
                else
                {
                    if (row != null)
                    {
                        // add neccessary columns
                        if (DT.Columns.Count < row.Cells.Count)
                        {
                            for (int j = startColumn - 1; j < endColumn; j++)
                            {
                                var cell = row.GetCell(j, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK);
                                if (cell != null)
                                {
                                    DT.Columns.Add(cell.StringCellValue, typeof(string));
                                }
                            }
                        }
                    }
                }
                i++;
            }

            File.Delete(FileSaveWithPath);
            return DT;
        }

        public static DataTable ReadExcelFileSheets(int startColumn, int endColumn, int sheetCount, string FileSaveWithPath)
        {

            XSSFWorkbook wb;
            XSSFSheet sh;
            String Sheet_name;

            using (var fs = new FileStream(FileSaveWithPath, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(fs);

                Sheet_name = wb.GetSheetAt(sheetCount).SheetName;  //get first sheet name
            }
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT.Columns.Clear();

            // get sheet
            sh = (XSSFSheet)wb.GetSheet(Sheet_name);

            int i = 0;
            while (sh.GetRow(i) != null)
            {
                // write row value, except row 0 which is header

                var row = sh.GetRow(i);
                if (i != 0)
                {
                    DataRow dr = DT.NewRow();

                    if (row != null && !IsRowEmpty(row, startColumn, endColumn))
                    {
                        for (int j = startColumn - 1; j < endColumn; j++)
                        {
                            var cell = row.GetCell(j, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK);

                            if (cell != null)
                            {
                                // TODO: you can add more cell types capatibility, e. g. formula
                                switch (cell.CellType)
                                {
                                    case NPOI.SS.UserModel.CellType.Numeric:
                                        dr[j] = cell.NumericCellValue;
                                        //dataGridView1[j, i].Value = sh.GetRow(i).GetCell(j).NumericCellValue;

                                        break;
                                    case NPOI.SS.UserModel.CellType.String:
                                        dr[j] = cell.StringCellValue.ToUpper();

                                        break;
                                }
                            }
                        }

                        DT.Rows.Add(dr);
                    }
                }
                else
                {
                    if (row != null)
                    {
                        // add neccessary columns
                        if (DT.Columns.Count < row.Cells.Count)
                        {
                            for (int j = startColumn - 1; j < endColumn; j++)
                            {
                                var cell = row.GetCell(j, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK);
                                if (cell != null)
                                {
                                    DT.Columns.Add(cell.StringCellValue, typeof(string));
                                }
                            }
                        }
                    }
                }
                i++;
            }

            return DT;
        }

        public static string ReadExcelFileSize(IFormFile files)
        {
            string res = string.Empty;

            if (files.Length > 15728640)
            {
                res = "File Size Cant be Larger than 15Mb";
            }

            return res;
        }

        public static string CopyExcelFilesTargetAdjustment(IFormFile file)
        {
            string result = "";
            using (DbHelper db = new DbHelper())
            {
                string exportTaskLoc = "C:\\TargetAdjustmentUpload\\";
                Config configExportTaskLoc = db.ConfigRepository.FindAll().Where(c => c.ConfigKey == "UploadTargetAdjustmentOriginalFilesDir").FirstOrDefault();


                if (configExportTaskLoc != null)
                    exportTaskLoc = configExportTaskLoc.ConfigValue;

                string filename = Guid.NewGuid().ToString()+ ".xlsx";
                string filePath = exportTaskLoc;
                //string FileSaveWithPath = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles\\Temp " + Guid.NewGuid().ToString() + ".xlsx");
                string FileSaveWithPath = filePath + filename;

                DirectoryInfo dirInfo = new DirectoryInfo(filePath);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                FileInfo excelFile = new FileInfo(FileSaveWithPath);

                using (var fs = new FileStream(FileSaveWithPath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                result = FileSaveWithPath;

            }
            return result;
        }

        public static string CopyExcelFilesForUploadTask(IFormFile file)
        {
            string result = "";
            using (DbHelper db = new DbHelper())
            {
                string exportTaskLoc = "C:\\UploadTaskFiles\\";
                Config configExportTaskLoc = db.ConfigRepository.FindAll().Where(c => c.ConfigKey == "UploadTaskFiles").FirstOrDefault();


                if (configExportTaskLoc != null)
                    exportTaskLoc = configExportTaskLoc.ConfigValue;

                string filename = Guid.NewGuid().ToString() + ".xlsx";
                string filePath = exportTaskLoc;
                //string FileSaveWithPath = Path.Combine(Directory.GetCurrentDirectory(), "TempFiles\\Temp " + Guid.NewGuid().ToString() + ".xlsx");
                string FileSaveWithPath = filePath + filename;

                DirectoryInfo dirInfo = new DirectoryInfo(filePath);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                FileInfo excelFile = new FileInfo(FileSaveWithPath);

                using (var fs = new FileStream(FileSaveWithPath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                result = FileSaveWithPath;

            }
            return result;
        }

    }
}