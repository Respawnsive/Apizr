﻿using System;
using System.Linq.Expressions;
using Apizr.Caching;
using Apizr.Configuring.Common;
using Apizr.Configuring.Request;
using Apizr.Mapping;

namespace Apizr
{
    internal static class InternalExtensions
    {
        #region RequestOptionsBuilder

        internal static Action<IApizrRequestOptionsBuilder> WithOriginalExpression(
            this Action<IApizrRequestOptionsBuilder> optionsBuilder, Expression originalExpression)
        {
            if (optionsBuilder == null)
                optionsBuilder = options => options.WithOriginalExpression(originalExpression);
            else
                optionsBuilder += options => options.WithOriginalExpression(originalExpression);

            return optionsBuilder;
        }

        #endregion

        #region CacheHandler

        #region Static

        private static Func<ICacheHandler> _cacheHandlerFactory;

        internal static void SetCacheHandlerInternalFactory(this IApizrGlobalCommonOptionsBuilderBase builder,
            Func<ICacheHandler> cacheHandlerFactory)
        {
            _cacheHandlerFactory = cacheHandlerFactory;
        }

        internal static Func<ICacheHandler> GetCacheHandlerInternalFactory(this IApizrCommonOptionsBase builder) =>
            _cacheHandlerFactory;

        #endregion

        #region Extended

        private static Type _cacheHandlerType;

        internal static void SetCacheHandlerType<TCacheHandler>(this IApizrExtendedCommonOptionsBuilderBase builder) where TCacheHandler : ICacheHandler
        {
            _cacheHandlerType = typeof(TCacheHandler);
        }

        internal static Type GetCacheHanderType(this IApizrCommonOptionsBase builder) =>
            _cacheHandlerType;

        #endregion

        #endregion

        #region MappingHandler

        #region Static

        private static Func<IMappingHandler> _mappingHandlerFactory;

        internal static void SetMappingHandlerInternalFactory(this IApizrCommonOptionsBuilderBase builder,
            Func<IMappingHandler> mappingHandlerFactory)
        {
            _mappingHandlerFactory = mappingHandlerFactory;
        }

        internal static Func<IMappingHandler> GetMappingHandlerInternalFactory(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerFactory;

        #endregion
        
        #region Extended

        private static Type _mappingHandlerType;

        internal static void SetMappingHandlerType<TMappingHandler>(this IApizrExtendedCommonOptionsBuilderBase builder) where TMappingHandler : IMappingHandler
        {
            _mappingHandlerType = typeof(TMappingHandler);
        }

        internal static Type GetMappingHanderType(this IApizrCommonOptionsBase builder) =>
            _mappingHandlerType;

        #endregion

        #endregion
    }
}
