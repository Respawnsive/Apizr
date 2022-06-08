using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.Generator.Models
{
    public class ApizrRegistrationTemplateModel : CSharpTemplateModelBase
    {
        private readonly OpenApiDocument _document;
        private readonly ApizrGeneratorSettings _settings;
        public ApizrRegistrationTemplateModel(string controllerName, IEnumerable<string> services, OpenApiDocument document, ApizrGeneratorSettings settings) : base(controllerName, settings)
        {
            _document = document;
            _settings = settings;

            BaseUrl = document.BaseUrl;
            Class = controllerName;
            BaseClass = _settings.ControllerBaseClass?.Replace("{controller}", controllerName);
            NameSpace = _settings.CSharpGeneratorSettings.Namespace;
            Services = services;
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

        /// <summary>Gets or sets the operations.</summary>
        public IEnumerable<string> Services { get; set; }

        public string LastService => Services.LastOrDefault();

        public bool WithRetry => _settings.WithRetry;

        public string RegistrationType => _settings.RegistrationType.ToString();

    }
}
