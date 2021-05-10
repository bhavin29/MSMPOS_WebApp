using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RocketPOS.Interface.Services;
using RocketPOS.Models;
using RocketPOS.Resources;
using RocketPOS.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketPOS.Controllers.Transaction
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _iInventoryService;
        private readonly IDropDownService _iDropDownService;
        private IStringLocalizer<RocketPOSResources> _sharedLocalizer;
        private readonly ICommonService _iCommonService;
        private readonly LocService _locService;
        public List<InventoryOpenigStockImport> _inventoryOpenigStockImports;
        private readonly IHostingEnvironment _hostingEnvironment;
        public InventoryController(IHostingEnvironment hostingEnvironment, ICommonService iCommonService, IInventoryService inventoryService, IDropDownService idropDownService, IStringLocalizer<RocketPOSResources> sharedLocalizer, LocService locService)
        {
            _hostingEnvironment = hostingEnvironment;
            _iInventoryService = inventoryService;
            _iCommonService = iCommonService;
            _iDropDownService = idropDownService;
            _sharedLocalizer = sharedLocalizer;
            _locService = locService;

            if (_inventoryOpenigStockImports is null)
            {
                _inventoryOpenigStockImports = new List<InventoryOpenigStockImport>();
            }
        }
        public IActionResult Index(int? storeId, int? foodCategoryId,int? itemType)
        {
            _iCommonService.GetPageWiseRoleRigths("Inventory");
            InventoryModel inventoryModel = new InventoryModel();

            if (itemType == 1)
            {
                inventoryModel.FoodCategoryList = _iDropDownService.GetIngredientCategoryList();
            }
            else if (itemType == 2)
            {
                inventoryModel.FoodCategoryList = _iDropDownService.GetAssetCategoryList();
            }
            else
            {
                inventoryModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            }

            inventoryModel.StoreList = _iDropDownService.GetStoreList();
            if (Convert.ToInt32(storeId) > 0)
            {
                inventoryModel.InventoryDetailList = _iInventoryService.GetInventoryDetailList(Convert.ToInt32(storeId), Convert.ToInt32(foodCategoryId), Convert.ToInt32(itemType));
            }
            return View(inventoryModel);
        }

        [HttpGet]
        public ActionResult GetFoodMenuCategoryList()
        {
            InventoryModel inventoryModel = new InventoryModel();
            inventoryModel.FoodCategoryList = _iDropDownService.GetFoodMenuCategoryList();
            return Json(new { inventoryModel.FoodCategoryList });
        }

        [HttpGet]
        public ActionResult GetIngredientCategoryList()
        {
            InventoryModel inventoryModel = new InventoryModel();
            inventoryModel.FoodCategoryList = _iDropDownService.GetIngredientCategoryList();
            return Json(new { inventoryModel.FoodCategoryList });
        }

        [HttpGet]
        public ActionResult GetAssetCategoryList()
        {
            InventoryModel inventoryModel = new InventoryModel();
            inventoryModel.FoodCategoryList = _iDropDownService.GetAssetCategoryList();
            return Json(new { inventoryModel.FoodCategoryList });
        }

        [HttpPost]
        public JsonResult UpdateInventoryDetailList(string inventoryDetail)
        {
            int result = 0;
            List<InventoryDetail> inventoryDetails = new List<InventoryDetail>();
            inventoryDetails = JsonConvert.DeserializeObject<List<InventoryDetail>>(inventoryDetail);
            result = _iInventoryService.UpdateInventoryDetailList(inventoryDetails);
            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult SaveInventoryDetailById(string inventoryDetail)
        {
            int result = 0;
            string res = "";
            List<InventoryDetail> inventoryDetails = new List<InventoryDetail>();
            inventoryDetails = JsonConvert.DeserializeObject<List<InventoryDetail>>(inventoryDetail);
            result = _iInventoryService.UpdateInventoryDetailList(inventoryDetails);

            if (result > 0)
            {
                res = _iInventoryService.StockUpdate(Convert.ToInt32(inventoryDetails[0].StoreId), Convert.ToInt32(inventoryDetails[0].FoodMenuId), Convert.ToInt32(inventoryDetails[0].ItemType));
            }

            return Json(new { result = result });
        }

        [HttpPost]
        public JsonResult StockUpdate(int? storeId, int? foodmenuId, int? itemType)
        {
            string result = "";
            if (Convert.ToInt32(storeId) > 0)
            {
                result = _iInventoryService.StockUpdate(Convert.ToInt32(storeId), Convert.ToInt32(foodmenuId), Convert.ToInt32(itemType));
            }
            return Json(new { result = result });
        }
        public IActionResult Import()
        {
            InventoryOpenigStockImport inventoryOpenigStockImport = new InventoryOpenigStockImport();
            inventoryOpenigStockImport.StoreList = _iDropDownService.GetStoreList();

            return View(inventoryOpenigStockImport);
        }

        public ActionResult ImportData(int storeId,int itemType)
        {
            InventoryOpenigStockImport item = new InventoryOpenigStockImport();
            List<InventoryOpenigStockImport> ItemList = new List<InventoryOpenigStockImport>();
            Random _random = new Random();
            StringBuilder sb = new StringBuilder();

            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string contentRootPath = _hostingEnvironment.ContentRootPath;
 
                string newPath = Path.Combine(webRootPath, folderName);
                string BatchId = DateTime.UtcNow.AddMinutes(LoginInfo.Timeoffset).ToString("MM/dd/yyyy HH:mm").Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + _random.Next(1000).ToString();
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    string fullPath = Path.Combine(newPath, file.FileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Position = 0;    
                        if (sFileExtension == ".xls")
                        {
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                        }
                        IRow headerRow = sheet.GetRow(0); //Get Header Row
                        int cellCount = headerRow.LastCellNum;
                        sb.Append("<p>Process completed successfully</p>");
                        sb.Append("<table class='table table-bordered'><tr>");
                        for (int j = 0; j < cellCount; j++)
                        {
                            NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        sb.Append("</tr>");
                        sb.AppendLine("<tr>");
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (j == 0)
                                    {
                                        item.FoodmenuId = Convert.ToInt32(row.GetCell(j).ToString());
                                        sb.Append("<td class=\"text-right\">" + row.GetCell(j).ToString() + "</td>");
                                    }
                                    else if (j == 3)
                                    {
                                        item.PhysicalStockQty = Convert.ToDecimal(row.GetCell(j).ToString());
                                        sb.Append("<td  class=\"text-right\">" + row.GetCell(j).ToString() + "</td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                    }
                                }
                            }
                            sb.AppendLine("</tr>");
                            ItemList.Add(new InventoryOpenigStockImport { ItemType= itemType, ImportBatch = BatchId, StoreId = storeId, FoodmenuId = item.FoodmenuId, PhysicalStockQty = item.PhysicalStockQty });
                        }
                        sb.Append("</table>");
                        _inventoryOpenigStockImports = ItemList;
                        int Result = _iInventoryService.BulkImport(_inventoryOpenigStockImports);
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
            return this.Content(sb.ToString());
        }

        public ActionResult Download()
        {
            string Files = "D:/UploadExcel/CoreProgramm_ExcelImport.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "employee.xlsx");
        }

        public async Task<IActionResult> Export(int storeId, int categoryId)
        {
            List<InventoryOpenigStockImport> inventoryOpenigStockImports = new List<InventoryOpenigStockImport>();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"MenuItem_OpeningStock.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            try
            {
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("MenuItem_OpeningStock");
                    IRow row = excelSheet.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Menu Item Id");
                    row.CreateCell(1).SetCellValue("Menu Item Category");
                    row.CreateCell(2).SetCellValue("Menu Item");
                    row.CreateCell(3).SetCellValue("Physical Stock");
                //    row.CreateCell(4).SetCellValue("Physical Stock Date");

                    inventoryOpenigStockImports = _iInventoryService.GetInventoryOpeningStockByStore(storeId, categoryId,0);

                    int intRow = 1;
                    foreach (var item in inventoryOpenigStockImports)
                    {
                        row = excelSheet.CreateRow(intRow);
                        row.CreateCell(0).SetCellValue(item.FoodmenuId);
                        row.CreateCell(1).SetCellValue(item.FoodmenuCategoryname);
                        row.CreateCell(2).SetCellValue(item.Foodmenuname);
                        row.CreateCell(3).SetCellValue(item.PhysicalStockQty.ToString());
                        //  row.CreateCell(4).SetCellValue(item.PhysicalDatetime.ToString("dd/mm/yyyy"));
                       // row.CreateCell(4).SetCellValue("");
                        intRow = intRow + 1;
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> ExportIngredient(int storeId, int categoryId)
        {
            List<InventoryOpenigStockImport> inventoryOpenigStockImports = new List<InventoryOpenigStockImport>();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"StockItem_OpeningStock.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            try
            {
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("StockItem_OpeningStock");
                    IRow row = excelSheet.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Stock Item Id");
                    row.CreateCell(1).SetCellValue("Stock Item Category");
                    row.CreateCell(2).SetCellValue("Stock Item");
                    row.CreateCell(3).SetCellValue("Physical Stock");
                    //    row.CreateCell(4).SetCellValue("Physical Stock Date");

                    inventoryOpenigStockImports = _iInventoryService.GetInventoryOpeningStockByStore(storeId, categoryId,1);

                    int intRow = 1;
                    foreach (var item in inventoryOpenigStockImports)
                    {
                        row = excelSheet.CreateRow(intRow);
                        row.CreateCell(0).SetCellValue(item.FoodmenuId);
                        row.CreateCell(1).SetCellValue(item.FoodmenuCategoryname);
                        row.CreateCell(2).SetCellValue(item.Foodmenuname);
                        row.CreateCell(3).SetCellValue(item.PhysicalStockQty.ToString());
                        //  row.CreateCell(4).SetCellValue(item.PhysicalDatetime.ToString("dd/mm/yyyy"));
                        // row.CreateCell(4).SetCellValue("");
                        intRow = intRow + 1;
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> ExportAssetItem(int storeId, int categoryId)
        {
            List<InventoryOpenigStockImport> inventoryOpenigStockImports = new List<InventoryOpenigStockImport>();

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"AssetItem_OpeningStock.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            try
            {
                using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("AssetItem_OpeningStock");
                    IRow row = excelSheet.CreateRow(0);
                    row.CreateCell(0).SetCellValue("Asset Item Id");
                    row.CreateCell(1).SetCellValue("Asset Item Category");
                    row.CreateCell(2).SetCellValue("Asset Item");
                    row.CreateCell(3).SetCellValue("Physical Stock");
                    //    row.CreateCell(4).SetCellValue("Physical Stock Date");

                    inventoryOpenigStockImports = _iInventoryService.GetInventoryOpeningStockByStore(storeId, categoryId, 2);

                    int intRow = 1;
                    foreach (var item in inventoryOpenigStockImports)
                    {
                        row = excelSheet.CreateRow(intRow);
                        row.CreateCell(0).SetCellValue(item.FoodmenuId);
                        row.CreateCell(1).SetCellValue(item.FoodmenuCategoryname);
                        row.CreateCell(2).SetCellValue(item.Foodmenuname);
                        row.CreateCell(3).SetCellValue(item.PhysicalStockQty.ToString());
                        //  row.CreateCell(4).SetCellValue(item.PhysicalDatetime.ToString("dd/mm/yyyy"));
                        // row.CreateCell(4).SetCellValue("");
                        intRow = intRow + 1;
                    }
                    workbook.Write(fs);
                }
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
            }
            catch (Exception ex)
            {
                SystemLogs.Register(ex.Message);
            }
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        public ActionResult ProcessData(InventoryOpenigStockImport inv, int storeId, int categoryId)
        {
            int Result = _iInventoryService.BulkImport(_inventoryOpenigStockImports);
            return View(_inventoryOpenigStockImports);
        }
    }
}
//https://dzone.com/articles/import-and-export-excel-file-in-asp-net-core-31-ra
