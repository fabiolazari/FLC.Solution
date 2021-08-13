using System;

namespace FlcIO.Business.Models
{
	public abstract class Entity
	{
		private Guid _id;

		public Guid Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public Entity()
		{
			_id = Guid.NewGuid();
		}
	}
}
