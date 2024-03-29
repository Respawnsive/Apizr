﻿using System;
using System.Collections.Concurrent;
using Apizr.Mediation.Requesting.Sending;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Mediation.Configuring.Registry
{
    /// <summary>
    /// Registry options available for extended registrations with mediation
    /// </summary>
    public class ApizrMediationRegistry : ApizrMediationRegistryBase, IApizrMediationConcurrentRegistry
    {
        private IServiceProvider _serviceProvider;

        private ConcurrentDictionary<Type, Func<IApizrMediatorBase>> ThrowIfNotConcurrentImplementation()
        {
            if (ConcurrentRegistry is ConcurrentDictionary<Type, Func<IApizrMediatorBase>> concurrentRegistry)
            {
                return concurrentRegistry;
            }

            throw new InvalidOperationException($"This {nameof(ApizrMediationRegistry)} is not configured for concurrent operations.");
        }

        internal IApizrMediationRegistry GetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }

        /// <inheritdoc />
        public void AddOrUpdateManager(Type managerType)
        {
            var registry = ThrowIfNotConcurrentImplementation();
            Func<IApizrMediatorBase> mediatorFactory = () => _serviceProvider.GetRequiredService(managerType) as IApizrMediatorBase;
            registry.AddOrUpdate(managerType, k => mediatorFactory, (k, e) => mediatorFactory);
        }
    }
}
