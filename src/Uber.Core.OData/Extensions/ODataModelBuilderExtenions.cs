﻿using Microsoft.AspNet.OData.Builder;
using System;

namespace Uber.Core.OData
{
    public class UberODataModelBuilder
    {
        private readonly string Namespace;
        private readonly ODataModelBuilder Builder;

        internal UberODataModelBuilder(ODataModelBuilder builder, string @namespace)
        {
            Namespace = @namespace;
            Builder = builder;
        }

        public EntitySetConfiguration<TEntity> AddEntitySet<TEntity>()
            where TEntity : class
        {
            var entitySet = Builder.EntitySet<TEntity>(typeof(TEntity).Name);
            entitySet.EntityType.Namespace = Namespace;
            return entitySet;
        }

        public EnumTypeConfiguration<TEnum> AddEnumType<TEnum>()
        {
            var enumType = Builder.EnumType<TEnum>();
            enumType.Namespace = Namespace;
            return enumType;
        }
    }

    public static class ODataModelBuilderExtenions
    {
        

        public static void AddNamespace(this ODataModelBuilder builder, string @namespace, Action<UberODataModelBuilder> action)
        {
            if (string.IsNullOrWhiteSpace(@namespace))
                throw new ArgumentNullException(nameof(@namespace));

            action(new UberODataModelBuilder(builder, @namespace));
        }
    }
}
