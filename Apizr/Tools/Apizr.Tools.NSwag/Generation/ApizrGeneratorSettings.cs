﻿using Apizr.Tools.NSwag.Generation.Models;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace Apizr.Tools.NSwag.Generation
{
    /// <summary>
    /// ApizrGeneratorSettings
    /// </summary>
    public class ApizrGeneratorSettings : CSharpGeneratorBaseSettings
    {
        /// <summary>Initializes a new instance of the <see cref="CSharpControllerGeneratorSettings"/> class.</summary>
        public ApizrGeneratorSettings()
        {
            ClassName = "{controller}";
            CSharpGeneratorSettings.ArrayType = "System.Collections.Generic.List";
            CSharpGeneratorSettings.ArrayInstanceType = "System.Collections.Generic.List";
            RouteNamingStrategy = ApizrRouteNamingStrategy.None;
            GenerateModelValidationAttributes = false;
            RegistrationType = ApizrRegistrationType.Both;
        }

        /// <summary>Returns the route name for a controller method.</summary>
        /// <param name="operation">Swagger operation</param>
        /// <returns>Route name.</returns>
        public string GetRouteName(OpenApiOperation operation)
        {
            if (RouteNamingStrategy == ApizrRouteNamingStrategy.OperationId)
            {
                return operation.OperationId;
            }

            return null;
        }

        /// <summary>Gets or sets the full name of the base class.</summary>
        public string ControllerBaseClass { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding priority management </summary>
        public ApizrRegistrationType RegistrationType { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding priority management </summary>
        public bool WithPriority { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding Polly retry management </summary>
        public bool WithRetry { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding Logs </summary>
        public bool WithLogs { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding Request Options </summary>
        public bool WithRequestOptions { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding Cache management </summary>
        public CacheProviderType WithCacheProvider { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding mediation </summary>
        public bool WithMediation { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding optional mediation </summary>
        public bool WithOptionalMediation { get; set; }

        /// <summary>Gets or sets a value indicating whether to allow adding date mapping </summary>
        public DataMapperType WithMapping { get; set; }

        /// <summary>Gets or sets the strategy for naming routes (default: CSharpRouteNamingStrategy.None).</summary>
        public ApizrRouteNamingStrategy RouteNamingStrategy { get; set; }

        /// <summary>Gets or sets a value indicating whether to add model validation attributes.</summary>
        public bool GenerateModelValidationAttributes { get; set; }

        /// <summary>Gets or sets a value indicating whether ASP.Net Core (2.1) ActionResult type is used (default: false).</summary>
        public bool UseActionResultType { get; set; }
        
        /// <summary>
        /// HeaderInAction
        /// </summary>
        public bool HeaderInAction { get; set; }

        /// <summary>Gets or sets the base path on which the API is served, which is relative to the Host.</summary>
        public string BasePath { get; set; }
    }
}