using Microsoft.Data.Entity.Update;
using Microsoft.Data.Entity.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Storage;

namespace EFCore.Services
{
    public class CustomBatchExecutor : BatchExecutor
    {
        public override int Execute(IEnumerable<ModificationCommandBatch> commandBatches, IRelationalConnection connection)
        {
            // Get list of batches and their target tables
            foreach (var batch in commandBatches)
            {
                int numOfCommands = batch.ModificationCommands.Count();
                string table = batch.ModificationCommands.First().TableName;
                Console.WriteLine($"batching {numOfCommands} items into table {table}");
            }
            return base.Execute(commandBatches, connection);
        }
    }
}
