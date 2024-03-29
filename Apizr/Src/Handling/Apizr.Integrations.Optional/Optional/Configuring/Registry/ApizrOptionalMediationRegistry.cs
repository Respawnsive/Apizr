﻿using System;
using System.Collections.Concurrent;
using Apizr.Optional.Requesting.Sending;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Optional.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations with optional mediation
    /// </summary>
    public class ApizrOptionalMediationRegistry : ApizrOptionalMediationRegistryBase, IApizrOptionalMediationConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrOptionalMediatorBase>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrOptionalMediationRegistry)} is not configured for concurrent operations.");
        }

        internal IApizrOptionalMediationRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        /// <inheritdoc />
        public void AddOrUpdateManager(Type managerType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrOptionalMediatorBase> mediatorFactory = () => _serviceProvider.GetRequiredService(managerType) as IApizrOptionalMediatorBase;
            registry.AddOrUpdate(managerType, k => mediatorFactory, (k, e) => mediatorFactory);
        }
    }
}
