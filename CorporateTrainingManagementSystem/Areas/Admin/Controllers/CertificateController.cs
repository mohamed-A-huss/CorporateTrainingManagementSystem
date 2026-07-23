
using CorporateTrainingManagementSystem.Documents;
using Microsoft.AspNetCore.Authorization;
using QuestPDF.Fluent;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class CertificateController : Controller
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public async Task<IActionResult> Index(CertificateFilterVM filter,CancellationToken cancellationToken)
        {
            var model =
                await _certificateService.GetAllCertificatesAsync(
                    filter,
                    cancellationToken);

            return View(model);
        }
        public async Task<IActionResult> Download(
        int id,
        CancellationToken cancellationToken)
        {
            var certificate =
                await _certificateService.GetCertificateByIdAsync(id, cancellationToken);

            if (certificate == null)
                return NotFound();

            var document = new CertificateDocument(certificate);

            var pdf = document.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"{certificate.Course.Title}-Certificate.pdf");
        }
    }
}
