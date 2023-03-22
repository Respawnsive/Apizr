//-----------------------------------------------------------------------
// <copyright file="CSharpControllerOperationModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using Humanizer;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.NSwag.Generation.Models
{
    /// <summary>The CSharp controller operation model.</summary>
    public class ApizrOperationModel : CSharpOperationModel
    {
        private readonly ApizrGeneratorSettings _settings;
        private readonly OpenApiOperation _operation;
        private readonly CSharpGeneratorBase _generator;
        private readonly CSharpTypeResolver _resolver;

        /// <summary>Initializes a new instance of the <see cref="ApizrOperationModel" /> class.</summary>
        /// <param name="operation">The operation.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="resolver">The resolver.</param>
        public ApizrOperationModel(OpenApiOperation operation, ApizrGeneratorSettings settings,
            ApizrGenerator generator, CSharpTypeResolver resolver)
            : base(operation, settings, generator, resolver)
        {
            _operation = operation;
            _generator = generator;
            _resolver = resolver;
            _settings = settings;
        }

        /// <summary>
        /// HeaderInAction
        /// </summary>
        public bool HeaderInAction => _settings.HeaderInAction;

        /// <summary>
        /// NoHeaderParameters
        /// </summary>
        public List<ApizrParameterModel> NoHeaderParameters
        {
            get
            {
                var data = ApizrParameters.Where(x => x?.IsHeader == false).ToList();
                foreach (var item in data.Where(item => data.IndexOf(item) == data.Count - 1))
                {
                    item.IsSortLast = true;
                }

                return data;
            }
        }

        public List<ApizrParameterModel> ApizrHeaderParameters => ApizrParameters.Where(x => x.IsHeader && !string.IsNullOrWhiteSpace(x.Name)).ToList();

        /// <summary>
        /// Parameters
        /// </summary>
        public IList<OpenApiParameter> BaseParameters => base.GetActualParameters();

        /// <summary>
        /// ApizrParameterModels
        /// </summary>
        public List<ApizrParameterModel> ApizrParameters => BaseParameters.Select(parameter =>
                new ApizrParameterModel(parameter.Name, GetParameterVariableName(parameter, _operation.Parameters),
                    GetParameterVariableIdentifier(parameter, _operation.Parameters),
                    ResolveParameterType(parameter), parameter, BaseParameters,
                    _settings.CodeGeneratorSettings,
                    _generator,
                    _resolver))
            .ToList();

        /// <summary>
        /// Format
        /// </summary>
        // ReSharper disable once IdentifierTypo
        public string PascalizeResultType => this.ResultType.Replace("»", "").Replace("«", " Of ").Pascalize();

        public override string ResultType
        {
            get
            {
                if (_settings.UseActionResultType)
                {
                    return SyncResultType == "void" || SyncResultType == "FileResponse"
                        ? "Task"
                        : "Task<" + SyncResultType + ">";
                }

                return base.ResultType;
            }
        }
    }
}