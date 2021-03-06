﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Metadata
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintEF6Mappings();
            PrintEFCoreMappings();
        }

        private static void PrintEFCoreMappings()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFCore.StarWarsContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Metadata.Core;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            Console.WriteLine("EF Core Mappings");
            using (var context = new EFCore.StarWarsContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated();
                foreach (var entity in context.Model.GetEntityTypes())
                {
                    Console.WriteLine($" {entity.ClrType.Name} => {entity.SqlServer().TableName}");
                    foreach(var property in entity.GetProperties())
                    {
                        Console.WriteLine($"\t {property.Name} ({property.ClrType.Name})");
                    }
                }
            }
        }

        private static void PrintEF6Mappings()
        {
            Console.WriteLine("EF6.x Mappings");
            using (var context = new EF6.StarWarsContext())
            {
                var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

                // Get the part of the model that contains info about the actual CLR types
                var objectItemCollection = 
                    ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

                foreach (var entityType in metadata.GetItems<EntityType>(DataSpace.OSpace))
                {
                    // Get the entity set that uses this entity type
                    var entitySet = metadata
                        .GetItems<EntityContainer>(DataSpace.CSpace)
                        .Single()
                        .EntitySets
                        .Single(s => s.ElementType.Name == entityType.Name);

                    // Find the mapping between conceptual and storage model for this entity set
                    var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                            .Single()
                            .EntitySetMappings
                            .Single(s => s.EntitySet == entitySet);

                    // Find the storage entity set (table) that the entity is mapped
                    var table = mapping
                        .EntityTypeMappings.Single()
                        .Fragments.Single()
                        .StoreEntitySet;

                    var tableName = (string)table.MetadataProperties["Table"].Value ?? table.Name;
                    var clrType = objectItemCollection.GetClrType(entityType);
                    
                    Console.WriteLine($" {clrType.Name} => {tableName}");
                    
                    foreach (var property in entityType.DeclaredProperties)
                    {
                        Console.WriteLine($"\t {property.Name} ({property.TypeName})");
                    }

                }

                Console.WriteLine();
            }
        }
    }
}
