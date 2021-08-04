using FlcIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlcIO.Data.Context
{
	public class MessengerContext : DbContext
	{
		public MessengerContext(DbContextOptions options) : base(options) { }

		public DbSet<FlcMessage> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerContext).Assembly);

			foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
				relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

			base.OnModelCreating(modelBuilder);
		}
	}
}
