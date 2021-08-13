using System;

namespace FlcIO.Business.Models
{
	public class FlcMessage : Entity
	{
		private Int32 _idRequest;
		private DateTime _timestamp;
		private string _messageDescription;

		public Int32 IdRequest
		{
			get { return _idRequest; }
			set { _idRequest = value; }
		}

		public DateTime Timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		public string MessageDescription
		{
			get { return _messageDescription; }
			set { _messageDescription = value; }
		}

		public FlcMessage()
		{
			_messageDescription = string.Empty;
		}
		
		public FlcMessage(string messageDescricao)
		{
			_idRequest = new Random().Next(100);
			_timestamp = DateTime.Now;
			_messageDescription = messageDescricao;
		}
	}
}
