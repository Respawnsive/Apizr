//-----------------------------------------------------------------------
// <copyright file="CSharpControllerTemplateModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NSwag;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.Generator.Models
{
    /// <summary>The CSharp controller template model.</summary>
    public class ApizrTemplateModel : CSharpTemplateModelBase
    {
        private readonly ApizrGeneratorSettings _settings;
        private readonly OpenApiDocument _document;

        /// <summary>Initializes a new instance of the <see cref="ApizrTemplateModel" /> class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="operations">The operations.</param>
        /// <param name="document">The document.</param>
        /// <param name="settings">The settings.</param>
        public ApizrTemplateModel(
            string controllerName,
            IEnumerable<CSharpOperationModel> operations,
            OpenApiDocument document,
            ApizrGeneratorSettings settings)
            : base(controllerName, settings)
        {
            _document = document;
            _settings = settings;

            Class = controllerName;
            Operations = operations;

            BaseClass = _settings.ControllerBaseClass?.Replace("{controller}", controllerName);
            NameSpace = _settings.CSharpGeneratorSettings.Namespace;
        }
        
        /// <summary>Initializes a new instance of the <see cref="ApizrTemplateModel" /> class.</summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="operations">The operations.</param>
        /// <param name="document">The document.</param>
        /// <param name="settings">The settings.</param>
        public ApizrTemplateModel(
            string controllerName,
            IEnumerable<CSharpOperationModel> operations,
            OpenApiDocument document,
            ApizrGeneratorSettings settings,OpenApiTag tag)
            : base(controllerName, settings)
        {
            _document = document;
            _settings = settings;

            Class = controllerName;
            Operations = operations;

            BaseClass = _settings.ControllerBaseClass?.Replace("{controller}", controllerName);
            NameSpace = _settings.CSharpGeneratorSettings.Namespace;
            TagDescription = tag?.Description;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string TagDescription { get; set; }
        
        /// <summary>Gets or sets the class name.</summary>
        public string Class { get; }

        /// <summary>
        /// NameSpace
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>Gets a value indicating whether the controller has a base class.</summary>
        public bool HasBaseClass => !string.IsNullOrEmpty(BaseClass);

        /// <summary>Gets the base class.</summary>
        public string BaseClass { get; }

        /// <summary>Gets or sets the service base URL.</summary>
        public string BaseUrl => _document.BaseUrl;

        /// <summary>Gets or sets a value indicating whether the controller has operations.</summary>
        public bool HasOperations => Operations.Any();

        /// <summary>Gets or sets the operations.</summary>
        public IEnumerable<CSharpOperationModel> Operations { get; set; }

        /// <summary>Gets or sets a value indicating whether the controller has a base path.</summary>
        public bool HasBasePath => !string.IsNullOrEmpty(BasePath);

        /// <summary>Gets or sets the base path.</summary>
        public string BasePath => string.IsNullOrEmpty(_settings.BasePath) ? _document.BasePath?.TrimStart('/') : _settings.BasePath.TrimStart('/');

        /// <summary>Gets a value indicating whether to generate optional parameters.</summary>
        public bool GenerateOptionalParameters => _settings.GenerateOptionalParameters;
        

        /// <summary>Gets a value indicating whether to allow adding cancellation token.</summary>
        public bool UseCancellationToken => _settings.UseCancellationToken;

        /// <summary>Gets a value indicating whether to allow adding model validation attributes</summary>
        public bool GenerateModelValidationAttributes => _settings.GenerateModelValidationAttributes;

        /// <summary>Gets the type of the attribute used to specify a parameter as required.</summary>
        public string RequiredAttributeType => "System.ComponentModel.DataAnnotations.Required";

        /// <summary>Gets the Title.</summary>
        public string Title => _document.Info.Title;

        /// <summary>Gets the Description.</summary>
        public string Description => _document.Info.Description;

        /// <summary>Gets the API version.</summary>
        public string Version => _document.Info.Version;
    }
}
