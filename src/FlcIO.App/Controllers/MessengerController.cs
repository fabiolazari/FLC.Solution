using AutoMapper;
using FlcIO.App.ViewModels;
using FlcIO.Business.Interfaces;
using FlcIO.Business.Models;
using FlcIO.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlcIO.App.Controllers
{
	public class MessengerController : Controller
	{
		private readonly IFlcMessageRepository _messsageRepository;
		private readonly IMapper _mapper;
		private bool _stop;
		private bool _back;

		public MessengerController(IFlcMessageRepository messsageRepository, IMapper mapper)
		{
			_messsageRepository = messsageRepository;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			MessengerService.ExecutionCount = 0;
			MessengerService.Messages.Clear();

			_stop = false;
			_back = false;

			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Send(string inputMensagem, bool flexCheckReceive = false)
		{
			if (MessengerService.ExecutionCount == 0)
				MessengerService.SendMessage(inputMensagem);

			_stop = false;
			_back = true;

			return SendPage();
		}

		public IActionResult Stop()
		{
			MessengerService.StopMessage();

			_stop = true;
			_back = false;

			return SendPage();
		}

		public IActionResult Receive()
		{
			MessengerService.ReceiveMessage();
			var messages = _mapper.Map<IEnumerable<FlcMessageViewModel>>(MessengerService.Messages);

			messages.ToList().ForEach(message =>
			{
				var check = _messsageRepository.GetById(message.Id).Result;
				if (check == null)
					_messsageRepository.Add(_mapper.Map<FlcMessage>(message));
			});
			
			return PartialView("_ReceivedMessages", messages);
		}

		public IActionResult UpdateCounter()
		{
			ViewData["Counter"] = MessengerService.ExecutionCount;
			return PartialView("_CounterSend");
		}

		public IActionResult SendPage()
		{
			ViewData["stopButtonStatus"] = (_stop) ? "disabled" : "";
			ViewData["backButtonStatus"] = (_back) ? "disabled" : "";
			ViewData["Counter"] = MessengerService.ExecutionCount;

			return View("Send");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
