using FlcIO.App.ViewModels;
using FlcIO.Business.Services;
using FlcIO.Business.Services.AWS_Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlcIO.App.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private MessengerService _message;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
			_message = new MessengerService();
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Send()
		{
			//Para começar a enviar
			_message.SendMessage("Hello World!");

			return View();
		}

		public IActionResult Stop()
		{
			//Para parar
			_message.StopMessage();

			return View();
		}

		public async Task<IActionResult> Receive()
		{
			//para receber
			AmazonUtil _amazonUtil = new AmazonUtil();
			var retorno = await _amazonUtil.AwsReceiveMessage();

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
