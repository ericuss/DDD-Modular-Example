using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder b);
    }

    public interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}
