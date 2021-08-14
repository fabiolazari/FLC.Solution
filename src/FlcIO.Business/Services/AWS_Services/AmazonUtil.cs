using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using FlcIO.Business.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlcIO.Business.Services.AWS_Services
{
	public class AmazonUtil
	{
		#region Variables

		private BasicAWSCredentials _credential;
		private RegionEndpoint _region;
		private AmazonSQSClient _client;
		private FlcMessage _message;
		private string _queueUrl;

		#endregion

		#region Constructors

		public AmazonUtil()
		{
			_credential = AwsCredentials();
			_client = new AmazonSQSClient(_credential, _region);
			_queueUrl = AwsCheckQueue().Result;
		}

		#endregion

		#region private Methods

		private BasicAWSCredentials AwsCredentials()
		{
			var sharedFile = new SharedCredentialsFile();//@"C:\aws_service_credentials\credentials"
			CredentialProfile awsProfile;
			if (sharedFile.TryGetProfile("awsprofile", out awsProfile))
			{
				_region = awsProfile.Region;
				return new BasicAWSCredentials(awsProfile.Options.AccessKey, awsProfile.Options.SecretKey);
			}
			return null;
		}

		private async Task<string> AwsCheckQueue()
		{
			var request = new GetQueueUrlRequest
			{
				QueueName = "messenger"
			};

			var response = await _client.GetQueueUrlAsync(request);
			return response.QueueUrl;
		}

		#endregion

		#region public Methods

		public async Task AwsSendMessage(FlcMessage message)
		{
			var jsonMessage = JsonConvert.SerializeObject(message);
			var request = new SendMessageRequest
			{
				QueueUrl = _queueUrl,
				MessageBody = jsonMessage
			};
			await _client.SendMessageAsync(request);
		}

		public async Task<FlcMessage> AwsReceiveMessage()
		{
			_message = null;
			var request = new ReceiveMessageRequest
			{
				QueueUrl = _queueUrl
			};

			try
			{
				var response = await _client.ReceiveMessageAsync(request);
				if (response.Messages.Count > 0)
				{
					var message = response.Messages.FirstOrDefault();
					_message = JsonConvert.DeserializeObject<FlcMessage>(message.Body);
					await _client.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
				}
			}
			catch (Exception ex)
			{
				_message = new FlcMessage($"Erro: {ex.Message}");
				return _message;
			}
			return _message;
		}

		#endregion
	}
}
