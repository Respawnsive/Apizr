using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apizr.Tools.NSwag.Commands.CodeGeneration;
using Newtonsoft.Json;
using NSwag.Commands;
using NSwag.Commands.CodeGeneration;

namespace Apizr.Tools.NSwag.Commands
{
    /// <summary>The command collection.</summary>
    public class ApizrCodeGeneratorCollection
    {
        /// <summary>Gets or sets the SwaggerToTypeScriptClientCommand.</summary>
        [JsonProperty("OpenApiToApizrClient", NullValueHandling = NullValueHandling.Ignore)]
        public OpenApiToApizrClientCommand OpenApiToApizrClientCommand { get; set; }

        /// <summary>Gets the items.</summary>
        [JsonIgnore]
        public IEnumerable<InputOutputCommandBase> Items => new InputOutputCommandBase[]
        {
            OpenApiToApizrClientCommand
        }.Where(cmd => cmd != null);
    }
}
