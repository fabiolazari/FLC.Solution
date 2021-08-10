using AutoMapper;
using FlcIO.App.ViewModels;
using FlcIO.Business.Interfaces;
using FlcIO.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlcIO.App.Controllers
{
	public class MessengerController : Controller
	{
		private readonly IFlcMessageRepository _MesssageRepository;
		private readonly IMapper _mapper;
		private string _inputMensagem;
		private Int32 _result;
		private bool _send;
		private bool _stop;
		private bool _back;

		public MessengerController()
		{
		}

		public ActionResult Index()
		{
			_inputMensagem = string.Empty;
			_send = false;
			_stop = false;
			_back = false;

			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Send(string inputMensagem)
		{
			_inputMensagem = inputMensagem;

			if (MessengerService.ExecutionCount == 0)
				MessengerService.SendMessage(_inputMensagem);

			_send = false;
			_stop = false;
			_back = true;

			return Send();
		}

		public IActionResult Stop()
		{
			_result = MessengerService.ExecutionCount;
			//MessengerService.StopMessage();

			_send = true;
			_stop = true;
			_back = false;

			return Send();
		}

		public async Task<IActionResult> Receive()
		{
			FlcMessageViewModel messages = new FlcMessageViewModel();
			messages = _mapper.Map<FlcMessageViewModel>(MessengerService.ReceiveMessage());

			//Gravar dados na base
			//var produtoAtualizacao = _mapper.Map<FlcMessageViewModel>(await _MesssageRepository.ObterProdutoFornecedor(id));

			return View("Send", messages);
		}

		public IActionResult Send()
		{
			ViewData["sendButtonStatus"] = (_send) ? "disabled" : "";
			ViewData["stopButtonStatus"] = (_stop) ? "disabled" : "";
			ViewData["backButtonStatus"] = (_back) ? "disabled" : "";
			ViewData["Mensagem"] = (string.IsNullOrEmpty(_inputMensagem)) ? "xxx" : _inputMensagem;
			ViewData["Counter"] = (MessengerService.ExecutionCount == 0) ? _result : MessengerService.ExecutionCount;

			return View("Send");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
