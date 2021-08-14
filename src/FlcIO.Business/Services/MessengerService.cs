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
        #region Variables

        private static Thread threadMain;
        private static CancellationTokenSource source;
        private static CancellationToken _token;
        private static AmazonUtil _amazonUtil = new AmazonUtil();
        private static List<FlcMessage> _messages = new List<FlcMessage>();
        private static int _executionCount;
        private static Int32 Segundo = 1000;

		#endregion

		#region Properties

        public static List<FlcMessage> Messages
        {
			get { return _messages; }
			set { _messages = value; }
		}

        public static int ExecutionCount
        {
            get { return _executionCount; }
            set { _executionCount = value; }
        }

        #endregion

        #region Private methods

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
                    Task.WaitAny(_amazonUtil.AwsSendMessage(new FlcMessage(message)));
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

        #endregion

        #region Public methods

        public static Task SendMessage(string message)
        {
            threadMain = new Thread(new ThreadStart(() => 
            {
                while (Executa(message)); 
            }));
            threadMain.Start();

            return Task.CompletedTask;
        }

        public static Task StopMessage()
		{
            threadMain.Interrupt();
            return Task.CompletedTask;
        }

        public static async void ReceiveMessage()
        {
            var receiveMessage = await _amazonUtil.AwsReceiveMessage();
            if (receiveMessage != null)
                _messages.Add(receiveMessage);
        }

        #endregion
    }
}
