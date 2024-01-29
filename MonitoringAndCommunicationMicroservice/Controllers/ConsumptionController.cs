using Microsoft.AspNetCore.Mvc;
using MonitoringAndCommunicationMicroservice.DataTransferObject;
using MonitoringAndCommunicationMicroservice.Services;

namespace MonitoringAndCommunicationMicroservice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConsumptionController : ControllerBase
	{
		private readonly ConsumptionService _service;

		public ConsumptionController(ConsumptionService service)
		{
			_service = service;
		}

		[HttpPost]
		[Route("getChartData")]
		public IActionResult GetChartData([FromBody] GetChartDataDTO requestBody)
		{
			double[] m = _service.GetChartData(requestBody);

			return Ok(new { response = "success", consumptions = m });
		}
	}
}
