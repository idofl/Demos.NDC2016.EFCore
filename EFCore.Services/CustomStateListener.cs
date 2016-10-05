using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Services
{
    class CustomStateListener : IEntityStateListener
    {
        ILogger _logger;
        public CustomStateListener(ILogger<CustomStateListener> logger)
        {
            _logger = logger;
        }
        public void StateChanging(InternalEntityEntry entry, EntityState newState)
        {
            if (newState == EntityState.Added)
            {
                _logger.LogDebug($"Entity of type {entry.EntityType.Name} was added");
            }
        }

        public void StateChanged(InternalEntityEntry entry, EntityState oldState, bool skipInitialFixup, bool fromQuery)
        {

        }
    }
}
