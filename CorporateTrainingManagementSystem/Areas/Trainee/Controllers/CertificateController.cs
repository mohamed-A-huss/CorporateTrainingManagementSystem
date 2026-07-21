using CorporateTrainingManagementSystem.Documents;
using Microsoft.AspNetCore.Authorization;
using QuestPDF.Fluent;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Trainee.Controllers
{
    [Area(SD.TRAINEE_AREA)]
    public class CertificateController : Controller
    {
        private readonly ITraineeCertificateService _certificateService;

        public CertificateController(
            ITraineeCertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            var traineeId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var certificates =
                await _certificateService.GetCertificatesAsync(
                    traineeId,
                    cancellationToken);

            return View(certificates);
        }

        public async Task<IActionResult> Download(
            int id,
            CancellationToken cancellationToken)
        {
            var traineeId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var certificate =
                await _certificateService.GetCertificateAsync(
                    id,
                    traineeId,
                    cancellationToken);

            if (certificate is null)
                return NotFound();

            var document =
                new CertificateDocument(certificate);

            var pdf = document.GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"{certificate.Course.Title}-Certificate.pdf");
        }
        [AllowAnonymous]
        [Route("Verify/{code}")]
        public async Task<IActionResult> Verify(
            string code,
            CancellationToken cancellationToken)
        {
            var vm =
                await _certificateService.VerifyCertificateAsync(
                    code,
                    cancellationToken);

            if (vm is null)
            {
                return View(new VerifyCertificateVM
                {
                    IsValid = false
                });
            }

            return View(vm);
        }
    }
}
