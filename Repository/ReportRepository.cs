using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using iTextSharp.text.pdf;
using iTextSharp.text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ReportRepository(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Create Report and store in text formate file 
        /// </summary>
        /// <param name="dataDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> CreateReport(AppointmentUpdateDto dataDto)
        {
            try
            {
                string fileName = $"{dataDto.Email}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string webRootPath = webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "textfiles");
                string filePath = Path.Combine(folderPath, fileName);

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    await writer.WriteLineAsync($"Date: {DateTime.Now}");
                    await writer.WriteLineAsync($"Name: {dataDto.First_Name} {dataDto.Last_Name}");
                    await writer.WriteLineAsync($"Gender: {dataDto.Gender}");
                    await writer.WriteLineAsync($"Email: {dataDto.Email}");
                    await writer.WriteLineAsync($"BP: {dataDto.BP}");
                    await writer.WriteLineAsync($"Heart Rate: {dataDto.HeartRate}");
                    await writer.WriteLineAsync($"Sugar: {dataDto.Sugar}");
                    await writer.WriteLineAsync($"Description: {dataDto.Description}");
                    await writer.WriteLineAsync($"Address: {dataDto.Address}");
                    await writer.WriteLineAsync($"Medicine: {dataDto.Medicine}");
                    await writer.WriteLineAsync($"Doctor: {dataDto.Doctor_Name}");
                }
                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create text file: {ex.Message}");
            }
        }

        /// <summary>
        /// Get Report data and convert into key and pair form 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Dictionary<string,string>> GetReportData(string fileName)
        {
            try
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string filePath = Path.Combine(webRootPath, "textfiles", fileName);
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found", fileName);
                }
                string fileContent = await File.ReadAllTextAsync(filePath);
                return ExtractDetailsFromFileContent(fileContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get text file: {ex.Message}");
            }
        }

        /// <summary>
        /// Getting Report File in .text format for downloading purpose 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Stream> GetReportFile(string fileName)
        {
            try
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "textfiles");
                string filePath = Path.Combine(folderPath, fileName);

                if (!File.Exists(filePath))
                {
                    return null;
                }

                string textContent = await File.ReadAllTextAsync(filePath);
                byte[] pdfBytes = ConvertTextToPdf(textContent);
                MemoryStream pdfStream = new MemoryStream(pdfBytes);
                pdfStream.Position = 0;
                return pdfStream;
                //return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get report file: {ex.Message}");
            }
        }

        /// <summary>
        /// This method is Spliting data and convert into dictionory form  
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        private Dictionary<string, string> ExtractDetailsFromFileContent(string fileContent)
        {
            var patientDetails = new Dictionary<string, string>();
            string[] lines = fileContent.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length >= 2)
                {
                    var key = parts[0].Trim();
                    var value = string.Join(":", parts.Skip(1)).Trim();
                    patientDetails[key] = value;
                }
            }
            return patientDetails;
        }

        /// <summary>
        ///  Converting text file to pdf 
        /// </summary>
        /// <param name="textContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] ConvertTextToPdf(string textContent)
        {
            using (MemoryStream pdfStream = new MemoryStream())
            {
                Document document = new Document();
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, pdfStream);
                    document.Open();
                    document.Add(new Paragraph(textContent));
                    document.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to convert text to PDF: {ex.Message}");
                }
                return pdfStream.ToArray();
            }
        }

        public async Task<Object> GetReportAnalysis(int userId, int DocId, int pageIndex, int pageSize)
        {
            var data = await (from appoint in dbContext.DS_Appointment
                              where appoint.UserDetailsUser_Id == userId
                              && appoint.DoctorMetadataDoc_meta_Id == DocId
                              && appoint.Status == "Checked" && appoint.ReportFile != null
                              orderby appoint.UpdatedOn
                              select new AppointmentDetailDto
                              {
                                  Appointment_Id = appoint.Appointment_Id,
                                  Status = appoint.Status,
                                  ApproveStatus = appoint.ApproveStatus,
                                  AppointmentTime = appoint.AppointmentTime,
                                  CreatedOn = appoint.CreatedOn,
                                  SlotId = appoint.SlotId,
                                  UserDetailsUser_Id = appoint.UserDetailsUser_Id,
                                  ReportFile = appoint.ReportFile,
                                  DoctorMetadataDoc_meta_ID = appoint.DoctorMetadataDoc_meta_Id
                              }
                              ).ToListAsync();

            var data1 = await (from appoint in dbContext.DS_Appointment
                              where appoint.UserDetailsUser_Id == userId
                              && appoint.DoctorMetadataDoc_meta_Id == DocId
                              && appoint.Status == "Checked" && appoint.ReportFile != null
                              select new AppointmentDetailDto
                              {
                                  Appointment_Id = appoint.Appointment_Id,
                                  Status = appoint.Status,
                                  ApproveStatus = appoint.ApproveStatus,
                                  AppointmentTime = appoint.AppointmentTime,
                                  CreatedOn = appoint.CreatedOn,
                                  SlotId = appoint.SlotId,
                                  UserDetailsUser_Id = appoint.UserDetailsUser_Id,
                                  ReportFile = appoint.ReportFile,
                                  DoctorMetadataDoc_meta_ID = appoint.DoctorMetadataDoc_meta_Id
                              }
                              ).Skip(pageIndex*pageSize)
                              .Take(pageSize)
                              .ToListAsync();

            var reporDist = new Dictionary<string, List<string>>();
           
            if (data.Count != 0)
            {
                foreach (var item in data)
                {
                    if (item.ReportFile != null && item.ReportFile != "")
                    {
                        var reportdata1 = await GetReportData(item.ReportFile);
                        foreach (var kvp in reportdata1)
                        {
                            if (kvp.Key != "Name" && kvp.Key != "Gender" &&
                                kvp.Key != "Email" && kvp.Key != "Address"
                                && kvp.Key != "Doctor")
                            {
                                if (!reporDist.ContainsKey(kvp.Key))
                                {
                                    reporDist[kvp.Key] = new List<string>();
                                }
                                reporDist[kvp.Key].Add(kvp.Value);
                            }
                        }
                    }
                }
                var response = new
                {
                    StatusCode = 200,
                    TotalAppointmentData = data.Count,
                    AppointmentData = data1,
                    ReportData = reporDist,
                    Message = "Data Successfully Fetched"
                };
                return response;
            }
            else
            {
                var response = new
                {
                    StatusCode = 204,
                    Message = "Not Data Available"
                };
                return response;
            }         

        }
    }
}
