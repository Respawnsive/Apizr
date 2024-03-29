﻿using Apizr.Tools.NSwag.Extensions;
using NSwag;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.NSwag.Generation.Models
{
    public class ApizrRegistrationTemplateModel : CSharpTemplateModelBase
    {
        private readonly OpenApiDocument _document;
        private readonly ApizrGeneratorSettings _settings;
        public ApizrRegistrationTemplateModel(string controllerName, IEnumerable<string> apis, OpenApiDocument document, ApizrGeneratorSettings settings) : base(controllerName, settings)
        {
            _document = document;
            _settings = settings;

            BaseUrl = document.BaseUrl;
            Class = controllerName;
            BaseClass = _settings.ControllerBaseClass?.Replace("{controller}", controllerName);
            NameSpace = _settings.CSharpGeneratorSettings.Namespace;
            Apis = apis;
            Title = document.Info.Title;
        }

        public string BaseUrl { get; }

        /// <summary>Gets or sets the class name.</summary>
        public string Class { get; }

        /// <summary>
        /// NameSpace
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>Gets the base class.</summary>
        public string BaseClass { get; }

        public string Title { get; }

        /// <summary>Gets or sets the operations.</summary>
        public IEnumerable<string> Apis { get; set; }

        public bool HasManyApis => Apis.Count() > 1;

        public string LastApi => Apis.LastOrDefault() ?? string.Empty;

        public bool WithRetry => _settings.WithRetry;

        public bool WithPriority => _settings.WithPriority;

        public bool WithLogs => _settings.WithLogs;

        public string WithCacheProvider => _settings.WithCacheProvider.ToString();

        public string RegistrationType => _settings.RegistrationType.ToString();

        public bool WithMediation => _settings.WithMediation;

        public bool WithOptionalMediation => _settings.WithOptionalMediation;

        public string WithMapping => _settings.WithMapping.ToString();

    }
}
