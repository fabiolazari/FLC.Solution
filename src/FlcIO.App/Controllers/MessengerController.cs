using AutoMapper;
using FlcIO.App.ViewModels;
using FlcIO.Business.Interfaces;
using FlcIO.Business.Models;
using FlcIO.Business.Services;
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
		private readonly IFlcMessageRepository _messsageRepository;
		private readonly IMapper _mapper;
		private string _inputMensagem;
		private Int32 _result;
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
		public IActionResult Send(string inputMensagem, bool flexCheckReceive)
		{
			_inputMensagem = inputMensagem;

			if (MessengerService.ExecutionCount == 0)
				MessengerService.SendMessage(_inputMensagem);

			_send = false;
			_stop = false;
			_back = true;
			_receive = false;

			return Send();
		}

		public IActionResult Stop()
		{
			_result = MessengerService.ExecutionCount;
			MessengerService.StopMessage();

			_send = true;
			_stop = true;
			_back = false;
			_receive = true;

			return Send();
		}

		public async Task<IActionResult> Receive()
		{
			var messages = _mapper.Map<IEnumerable<FlcMessageViewModel>>(await MessengerService.ReceiveMessage());

			//Gravar dados na base
			messages.ToList().ForEach(message =>
			{
				_messsageRepository.Add(_mapper.Map<FlcMessage>(message));
			});
			

			//Pega todos os dados da base  depois

			return View("Send", messages);
		}

		public IActionResult Send()
		{
			ViewData["sendButtonStatus"] = (_send) ? "disabled" : "";
			ViewData["stopButtonStatus"] = (_stop) ? "disabled" : "";
			ViewData["backButtonStatus"] = (_back) ? "disabled" : "";
			ViewData["receiveButtonStatus"] = (_receive) ? "disabled" : "";
			ViewData["Mensagem"] = _inputMensagem;
			ViewData["Counter"] = (MessengerService.ExecutionCount == 0) ? _result : MessengerService.ExecutionCount;

			return View("Send", new List<FlcMessageViewModel>());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
