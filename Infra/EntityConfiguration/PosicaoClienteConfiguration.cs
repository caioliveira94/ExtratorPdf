using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityConfiguration
{
    class PosicaoClienteConfiguration : IEntityTypeConfiguration<PosicaoCliente>
    {
        public void Configure(EntityTypeBuilder<PosicaoCliente> builder)
        {
            builder.HasKey(b => b.Id);

            //builder.HasOne<PosicaoCliente>(b => b.ExtratoCliente)
            //    .WithOne(b => b.PosicaoCliente)
            //    .HasForeignKey(b => b.)
        }
    }
}
