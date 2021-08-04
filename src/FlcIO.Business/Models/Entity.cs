using System;

namespace FlcIO.Business.Models
{
	public abstract class Entity
	{
		private Guid _id;

		public Guid Id
		{
			get { return _id; }
		}

		public Entity()
		{
			_id = new Guid();
		}
	}
}
