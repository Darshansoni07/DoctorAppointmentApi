using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IUnitOfWork uow;
        public ReportController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet("getReportFile/{fileName}")]
        public async Task<IActionResult> GetReportFileByName(string fileName)
        {
            try
            {
                var report = await uow.ReportRepository.GetReportFile(fileName);
                if (report == null)
                {
                    return NotFound();
                }
                return File(report, "application/pdf", $"{fileName}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getReportData")]
        public async Task<IActionResult> GetReportData(int patientId, int metaId, int pageIndex=0, int pageSize =5)
        {
            var report = await uow.ReportRepository.GetReportAnalysis(patientId, metaId, pageIndex, pageSize);
            return Ok(report);
        }

    }
}
