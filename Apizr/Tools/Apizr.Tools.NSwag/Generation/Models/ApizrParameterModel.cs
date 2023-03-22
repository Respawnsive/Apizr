using NJsonSchema.CodeGeneration;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp.Models;

namespace Apizr.Tools.NSwag.Generation.Models
{
    public class ApizrParameterModel : CSharpParameterModel
    {
        public ApizrParameterModel(string parameterName, 
            string variableName,
            string variableIdentifier,
            string typeName,
            OpenApiParameter parameter, 
            IList<OpenApiParameter> allParameters, 
            CodeGeneratorSettingsBase settings,
            IClientGenerator generator, 
            TypeResolverBase typeResolver) : base(parameterName, variableName, variableIdentifier, typeName,
            parameter, allParameters, settings, generator, typeResolver)
        {
        }

        /// <summary>
        /// IsSortLast
        /// </summary>
        public bool IsSortLast { get; set; }
        
        /// <summary>
        /// KindName
        /// </summary>
        public string KindName => base.Kind.ToString();
    }
}