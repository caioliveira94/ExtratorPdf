using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfiguration
{
    public class ExtratoClienteConfiguration : IEntityTypeConfiguration<ExtratoCliente>
    {
        public void Configure(EntityTypeBuilder<ExtratoCliente> builder)
        {
            //builder.HasKey(b => b.Id);

            //builder.Entity<ExtratoCliente>()
            //.HasMany(c => c.)
            //.WithOne(e => e.Company);
        }
    }
}
