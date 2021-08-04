using FlcIO.Business.Interfaces;
using FlcIO.Business.Models;
using FlcIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlcIO.Data.Repository
{
	public class FlcMessageRepository : Repository<FlcMessage>, IFlcMessageRepository
	{
		public FlcMessageRepository(MessengerContext context) : base(context) { }

		public async Task<IEnumerable<FlcMessage>> GetMessageByDate(DateTime initialTimestamp, DateTime finalTimestamp)
		{
			return await Db.Messages.AsNoTracking()
								 .Where(men => men.Timestamp >= initialTimestamp && men.Timestamp >= finalTimestamp)
								 .ToListAsync();
		}

		public async Task<IEnumerable<FlcMessage>> GetMessageByMessagePart(string messagePart)
		{
			return await Db.Messages.AsNoTracking()
								 .Where(men => men.MessageDescription.Contains(messagePart))
								 .ToListAsync();
		}
	}
}
