using FlcIO.Business.Models;
using FlcIO.Business.Services.AWS_Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlcIO.Business.Services
{
	public static class MessengerService
	{
        private static Thread threadMain;
        private static CancellationTokenSource source;
        private static CancellationToken _token;
        private static AmazonUtil _amazonUtil = new AmazonUtil();
        public static int _executionCount;
        private static Int32 Segundo = 1000; 

        public static int ExecutionCount
        {
			get { return _executionCount; }
			set { _executionCount = value; }
		}
 
        public static Task SendMessage(string message)
        {
            try
            {
                threadMain = new Thread(new ThreadStart(() => 
                {
                    while (Executa(message)); 
                }));
                threadMain.Start();
            }
            catch (Exception e) when (!string.IsNullOrEmpty(e.Message))
            {
                //Fazer o tratamento
            }

            return Task.CompletedTask;
        }

        public static Task StopMessage()
		{
            threadMain.Interrupt();
            _executionCount = 0;
            return Task.CompletedTask;
        }

        private static bool Executa(string message)
        {
            source = new CancellationTokenSource();
            _token = source.Token;
            TaskFactory factory = new TaskFactory(_token);
            List<Task> tarefa = new List<Task>();

			try
			{
				tarefa.Add(factory.StartNew(() => {
                    Interlocked.Increment(ref _executionCount);
                    //Task.WaitAny(_amazonUtil.AwsSendMessage(new FlcMessage(message)));
                }, _token));

				tarefa.RemoveAll(t => t.Status != TaskStatus.Running);
				Thread.Sleep(Segundo * 5);
			}
			catch (Exception)
			{
                return false;
			}

            return true;
        }

        public static async Task<IEnumerable<FlcMessage>> ReceiveMessage()
        {
            return await _amazonUtil.AwsReceiveMessage();
        }
    }
}
