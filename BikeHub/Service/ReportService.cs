using BikeHub.Repository.IRepository;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.ReportsDto;
using ClosedXML.Excel;


namespace BikeHub.Service
{
    public class ReportService(IWebHostEnvironment _env,IReportsRepository _reportsRepository) : IReportService
    {
        public async Task<(bool isSuccess, string Msg, string filePath)> BikeServiceJobs(DateTime fromDate, DateTime toDate)
        {
            
            string folderName = "exports";
            string pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fileName = $"BikeServiceJobsReport_{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(pathToSave, fileName);
            try
            {
                var data = await _reportsRepository.BikeServiceJobs(fromDate, toDate);
                if (data == null)
                {
                    return (false, "No data found for the specified date range.", null);
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    foreach (var property in typeof(BikeServiceJobsDto).GetProperties())
                    {
                        worksheet.Cell(1, property.MetadataToken - 1).Value = property.Name;
                        worksheet.Cell(1, property.MetadataToken - 1).Style.Font.Bold = true;
                    }

                    for (int row = 1; row < data.Count(); row++) 
                    { 
                    
                        var item = data.ElementAt(row - 1);
                        worksheet.Cell(row + 1, 1).Value = item.ServiceJobId;
                        worksheet.Cell(row + 1, 2).Value = item.JobCardNumber;
                        worksheet.Cell(row + 1, 3).Value = item.CustomerId;
                        worksheet.Cell(row + 1, 4).Value = item.CustomerName;
                        worksheet.Cell(row + 1, 5).Value = item.ServiceStatus;
                        worksheet.Cell(row + 1, 6).Value = item.CreatedDate?.ToString("yyyy-MM-dd");
                        worksheet.Cell(row + 1, 7).Value = item.ActualDuration;
                        worksheet.Cell(row + 1, 8).Value = item.JobTotal;
                        worksheet.Cell(row + 1, 9).Value = item.LineItems;

                    }


                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(fullPath);
                }
                var fileRelativePath = Path.Combine(commonInfo.excelExportPath, fileName).Replace('\\', '/'); ;
                return (true, "Report generated successfully.", fileRelativePath);
            }
            catch(Exception) 
            {
                throw; 
            }
        }

        public async Task<(bool isSuccess, string Msg, string filePath)> CustomerOrderRevenue(DateTime fromDate, DateTime toDate)
        {
            string folderName = "exports";
            string pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fileName = $"CustomerOrderRevenueReport_{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(pathToSave, fileName);
            try
            {
                var data = await _reportsRepository.CustomerOrderRevenue(fromDate, toDate);
                if (data == null)
                {
                    return (false, "No data found for the specified date range.", null);
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    foreach (var property in typeof(CustomerOrderRevenueDto).GetProperties())
                    {
                        worksheet.Cell(1, property.MetadataToken - 1).Value = property.Name;
                        worksheet.Cell(1, property.MetadataToken - 1).Style.Font.Bold = true;
                    }

                    for (int row = 1; row < data.Count(); row++)
                    {
                        var item = data.ElementAt(row - 1);
                        worksheet.Cell(row + 1, 1).Value = item.CustomerId;
                        worksheet.Cell(row + 1, 2).Value = item.CustomerName;
                        worksheet.Cell(row + 1, 3).Value = item.TotalOrders;
                        worksheet.Cell(row + 1, 4).Value = item.TotalRevenue;
                        worksheet.Cell(row + 1, 5).Value = item.TotalUnits;
                        worksheet.Cell(row + 1, 6).Value = item.AvgUnitPrice;
                    }


                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(fullPath);
                }
                var fileRelativePath = Path.Combine(commonInfo.excelExportPath, fileName).Replace('\\', '/'); ;
                return (true, "Report generated successfully.", fileRelativePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(bool isSuccess, string Msg, string filePath)> Inventory()
        {
            string folderName = "exports";
            string pathToSave = Path.Combine(_env.ContentRootPath,"Content", folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fileName = $"InventoryReport_{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(pathToSave, fileName);
            try
            {
                var data = await _reportsRepository.Inventory();
                if (data == null)
                {
                    return (false, "No data found for the specified date range.", null);
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    int col = 1;
                    foreach (var property in typeof(InventoryDto).GetProperties())
                    {
                        worksheet.Cell(1, col).Value = property.Name;
                        worksheet.Cell(1, col).Style.Font.Bold = true;

                        col++;
                    }

                    for (int row = 1; row <= data.Count(); row++)
                    {
                        
                     var item = data.ElementAt(row - 1);
                        worksheet.Cell(row + 1, 1).Value = item.ProductId;
                        worksheet.Cell(row + 1, 2).Value = item.ProductName;
                        worksheet.Cell(row + 1, 3).Value = item.Quantity;
                        worksheet.Cell(row + 1, 4).Value = item.ActiveStatus;
                        
                    }


                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(fullPath);
                }
                var fileRelativePath = Path.Combine(commonInfo.excelExportPath, fileName).Replace('\\', '/'); ;
                return (true, "Report generated successfully.", fileRelativePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(bool isSuccess, string Msg, string filePath)> MechanicProductivity(DateTime Date)
        {
            string folderName = "exports";
            string pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fileName = $"MechanicProductivityReport_{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(pathToSave, fileName);
            try
            {
                var data = await _reportsRepository.MechanicProductivity(Date);
                if (data == null)
                {
                    return (false, "No data found for the specified date range.", null);
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    foreach (var property in typeof(MechanicProductivityDto).GetProperties())
                    {
                        worksheet.Cell(1, property.MetadataToken - 1).Value = property.Name;
                        worksheet.Cell(1, property.MetadataToken - 1).Style.Font.Bold = true;
                    }

                    for (int row = 1; row < data.Count(); row++)
                    {
                        
                        var item = data.ElementAt(row - 1);
                        worksheet.Cell(row + 1, 1).Value = item.MechanicId;
                        worksheet.Cell(row + 1, 2).Value = item.UserName;
                        worksheet.Cell(row + 1, 3).Value = item.JobsAssigned;
                        worksheet.Cell(row + 1, 4).Value = item.TotalMinutes;
                        worksheet.Cell(row + 1, 5).Value = item.AvgMinutesPerJob;
                        worksheet.Cell(row + 1, 6).Value = item.JobsCompleted;

                    }


                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(fullPath);
                }
                var fileRelativePath = Path.Combine(commonInfo.excelExportPath, fileName).Replace('\\', '/'); ;
                return (true, "Report generated successfully.", fileRelativePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(bool isSuccess, string Msg, string filePath)> TopProductsByRevenue(DateTime fromDate, DateTime toDate)
        {
            string folderName = "exports";
            string pathToSave = Path.Combine(_env.ContentRootPath, folderName);

            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            string fileName = $"TopProductsByRevenueReport_{Guid.NewGuid()}.xlsx";
            string fullPath = Path.Combine(pathToSave, fileName);
            try
            {
                var data = await _reportsRepository.TopProductsByRevenue(fromDate ,toDate);
                if (data == null)
                {
                    return (false, "No data found for the specified date range.", null);
                }
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    foreach (var property in typeof(TopProductsByRevenueDto).GetProperties())
                    {
                        worksheet.Cell(1, property.MetadataToken - 1).Value = property.Name;
                        worksheet.Cell(1, property.MetadataToken - 1).Style.Font.Bold = true;
                    }

                    for (int row = 1; row < data.Count(); row++)
                    { 

                        var item = data.ElementAt(row - 1);
                        worksheet.Cell(row + 1, 1).Value = item.ProductId;
                        worksheet.Cell(row + 1, 2).Value = item.ProductName;
                        worksheet.Cell(row + 1, 3).Value = item.UnitsSold;
                        worksheet.Cell(row + 1, 4).Value = item.Revenue;

                    }


                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(fullPath);
                }
                var fileRelativePath = Path.Combine(commonInfo.excelExportPath, fileName).Replace('\\', '/'); ;
                return (true, "Report generated successfully.", fileRelativePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
