using System;
using System.Collections.Generic;
using System.Text;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.Generator.Models
{
    public class ApizrRegistrationTemplateModel : CSharpTemplateModelBase
    {
        private readonly ApizrGeneratorSettings _settings;
        public ApizrRegistrationTemplateModel(string controllerName, IEnumerable<string> services, ApizrGeneratorSettings settings) : base(controllerName, settings)
        {
            _settings = settings;

            Class = controllerName;
            BaseClass = _settings.ControllerBaseClass?.Replace("{controller}", controllerName);
            NameSpace = _settings.CSharpGeneratorSettings.Namespace;
            Services = services;
        }

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
    }
}
