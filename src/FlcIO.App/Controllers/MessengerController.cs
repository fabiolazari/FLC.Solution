using FlcIO.App.ViewModels;
using FlcIO.Business.Services;
using FlcIO.Business.Services.AWS_Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlcIO.App.Controllers
{
	public class MessengerController : Controller
	{
		private MessengerService _message;

		public MessengerController()
		{
			_message = new MessengerService();
		}

		public ActionResult Index()
		{

			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Send(string inputMensagem)
		{

			ViewData["Mensagem"] = inputMensagem;
			ViewBag.mensagem = inputMensagem;
			//_message.SendMessage(inputMensagem);

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
