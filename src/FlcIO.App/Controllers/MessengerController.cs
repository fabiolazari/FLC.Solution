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
using System.Threading;

namespace FlcIO.App.Controllers
{
	public class MessengerController : Controller
	{
		private readonly IFlcMessageRepository _messsageRepository;
		private readonly IMapper _mapper;
		private bool _send;
		private bool _stop;
		private bool _back;
		private bool _receive;

		public MessengerController(IFlcMessageRepository messsageRepository, IMapper mapper)
		{
			_messsageRepository = messsageRepository;
			_mapper = mapper;
		}

		public ActionResult Index()
		{
			MessengerService.ExecutionCount = 0;
			MessengerService.Messages.Clear();

			_send = false;
			_stop = false;
			_back = false;
			_receive = false;

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

			_send = false;
			_stop = false;
			_back = true;
			_receive = false;

			return Atualizar();
		}

		public IActionResult Stop()
		{
			MessengerService.StopMessage();

			_send = true;
			_stop = true;
			_back = false;
			_receive = false;

			return Atualizar();
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

			ViewData["Counter"] = MessengerService.ExecutionCount;
			return View("Send", messages);
		}

		public IActionResult Atualizar()
		{
			ViewData["sendButtonStatus"] = (_send) ? "disabled" : "";
			ViewData["stopButtonStatus"] = (_stop) ? "disabled" : "";
			ViewData["backButtonStatus"] = (_back) ? "disabled" : "";
			ViewData["receiveButtonStatus"] = (_receive) ? "disabled" : "";
			ViewData["Counter"] = MessengerService.ExecutionCount;

			return View("Send", _mapper.Map<IEnumerable<FlcMessageViewModel>>(MessengerService.Messages));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
