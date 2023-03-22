using System.Reflection;
using NJsonSchema.CodeGeneration;
using NSwag;
using DefaultTemplateFactory = NSwag.CodeGeneration.DefaultTemplateFactory;

namespace Apizr.Tools.NSwag.Generation.Models
{
    public class ApizrTemplateFactory : DefaultTemplateFactory
    {
        /// <summary>Initializes a new instance of the <see cref="ApizrTemplateFactory" /> class.</summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblies">The assemblies.</param>
        public ApizrTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies)
            : base(settings, assemblies)
        {

        }

        /// <summary>Gets the current toolchain version.</summary>
        /// <returns>The toolchain version.</returns>
        protected override string GetToolchainVersion()
        {
            return OpenApiDocument.ToolchainVersion + " (NJsonSchema v" + base.GetToolchainVersion() + ")";
        }

        /// <summary>Tries to load an embedded Liquid template.</summary>
        /// <param name="language">The language.</param>
        /// <param name="template">The template name.</param>
        /// <returns>The template.</returns>
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            if (language != "CSharp")
                throw new NotImplementedException("Apizr generator is compatible with CSharp language only");

            template = template.TrimEnd(new[] {'!'});
            var assembly = GetLiquidAssembly("Apizr.Tools.NSwag");
            var resourceName = $"Apizr.Tools.NSwag.Generation.Templates." + template + ".liquid";

            var resource = assembly.GetManifestResourceStream(resourceName);
            if (resource == null) 
                return base.GetEmbeddedLiquidTemplate(language, template);
            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();

        }
    }
}