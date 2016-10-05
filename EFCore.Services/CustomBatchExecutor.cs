using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EFCore.Services
{
    public class CustomBatchExecutor : BatchExecutor
    {
        ILogger _logger;

        public CustomBatchExecutor(ILogger<CustomBatchExecutor> logger)
        {
            _logger = logger;
        }
        public override int Execute(IEnumerable<ModificationCommandBatch> commandBatches, IRelationalConnection connection)
        {
            // Get list of batches and their target tables
            foreach (var batch in commandBatches)
            {
                int numOfCommands = batch.ModificationCommands.Count();
                string table = batch.ModificationCommands.First().TableName;
                _logger.LogDebug($"batching {numOfCommands} items into table {table}");
            }
            return base.Execute(commandBatches, connection);
        }
    }
}
