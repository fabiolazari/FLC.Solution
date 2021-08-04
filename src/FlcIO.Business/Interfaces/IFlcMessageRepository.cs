using FlcIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlcIO.Business.Interfaces
{
	public interface IFlcMessageRepository : IRepository<FlcMessage>
	{
		Task<IEnumerable<FlcMessage>> GetMessageByDate(DateTime initialTimestamp, DateTime finalTimestamp);
		Task<IEnumerable<FlcMessage>> GetMessageByMessagePart(string messagePart);
	}
}
