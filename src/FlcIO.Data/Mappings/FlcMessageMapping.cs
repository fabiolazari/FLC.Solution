using FlcIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlcIO.Data.Mappings
{
	public class FlcMessageMapping : IEntityTypeConfiguration<FlcMessage>
	{
		public void Configure(EntityTypeBuilder<FlcMessage> builder)
		{
			builder.HasKey(men => men.Id);

			builder.Property(men => men.IdRequest)
				.IsRequired();

			builder.Property(men => men.Timestamp)
				.IsRequired();

			builder.Property(men => men.MessageDescription)
				.IsRequired()
				.HasColumnType("varchar(250)");

			builder.ToTable("Messages");
		}
	}
}
