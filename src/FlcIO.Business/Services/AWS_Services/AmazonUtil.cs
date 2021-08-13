using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using FlcIO.Business.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlcIO.Business.Services.AWS_Services
{
	public class AmazonUtil
	{
		#region Variables

		private BasicAWSCredentials _credential;
		private RegionEndpoint _region;
		private AmazonSQSClient _client;
		private List<FlcMessage> _messages;
		private string _queueUrl;

		#endregion

		#region Constructors

		public AmazonUtil()
		{
			_credential = AwsCredentials();
			_client = new AmazonSQSClient(_credential, _region);
			_messages = new List<FlcMessage>();
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

		public async Task<IEnumerable<FlcMessage>> AwsReceiveMessage()
		{
			var request = new ReceiveMessageRequest
			{
				QueueUrl = _queueUrl
			};

			try
			{
				var response = await _client.ReceiveMessageAsync(request);
				foreach (var mensagem in response.Messages)
				{
					_messages.Add(JsonConvert.DeserializeObject<FlcMessage>(mensagem.Body));
					//await _client.DeleteMessageAsync(_queueUrl, mensagem.ReceiptHandle);
				}
			}
			catch (Exception ex)
			{
				_messages.Add(new FlcMessage($"Erro: {ex.Message}"));
				return _messages;
			}
			return _messages;
		}

		#endregion
	}
}
