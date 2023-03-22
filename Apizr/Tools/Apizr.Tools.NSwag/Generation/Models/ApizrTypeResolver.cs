using Humanizer;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace Apizr.Tools.NSwag.Generation.Models
{
    public class ApizrTypeResolver : CSharpTypeResolver
    {
        public ApizrTypeResolver(CSharpGeneratorSettings settings) : base(settings)
        {
        }

        public ApizrTypeResolver(CSharpGeneratorSettings settings, JsonSchema exceptionSchema) : base(settings, exceptionSchema)
        {
        }

        public override string GetOrGenerateTypeName(JsonSchema schema, string typeNameHint)
        {
            var data= base.GetOrGenerateTypeName(schema, typeNameHint);
            return data.Replace("»", "").Replace("«", " Of ").Pascalize();
        }
    }
}