using FlcIO.Business.Models;
using FlcIO.Business.Services.AWS_Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlcIO.Business.Services
{
	public class MessengerService
	{
        private static Thread threadMain;
        private static CancellationTokenSource source;
        private AmazonUtil _amazonUtil;
        private Int32 Segundo = 1000;

        public MessengerService()
        {
            _amazonUtil = new AmazonUtil();
        }

        public void SendMessage(string message)
        {
            try
            {
                threadMain = new Thread(new ThreadStart(() => { while (Executa(message)) ; }));
                threadMain.Start();
            }
            catch (Exception e) when (!string.IsNullOrEmpty(e.Message))
            {
                //Fazer o tratamento
            }
        }

        public void StopMessage()
		{
            threadMain.Interrupt();
        }

        private bool Executa(string message)
        {
            source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            TaskFactory factory = new TaskFactory(token);
            List<Task> tarefa = new List<Task>();
            var retorno = string.Empty;

			try
			{
				tarefa.Add(factory.StartNew(() => {
					Task.WaitAny(_amazonUtil.AwsSendMessage(new FlcMessage(message)));
				}, token));

				tarefa.RemoveAll(t => t.Status != TaskStatus.Running);
				Thread.Sleep(Segundo * 5);
			}
			catch (Exception)
			{
                return false;
			}

            return true;
        }
    }
}
