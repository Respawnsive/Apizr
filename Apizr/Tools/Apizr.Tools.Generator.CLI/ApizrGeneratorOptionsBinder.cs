using System.CommandLine;
using System.CommandLine.Binding;

namespace Apizr.Tools.Generator.CLI;

public class ApizrGeneratorOptionsBinder : BinderBase<ApizrGeneratorOptions>
{
    private readonly Option<string> _outputPathOption;
    private readonly Option<string> _nsOption;
    private readonly Option<ApizrRegistrationType> _registrationTypeOption;
    private readonly Option<bool> _withPriorityOption;
    private readonly Option<bool> _withContextOption;
    private readonly Option<bool> _withTokenOption;
    private readonly Option<bool> _withRetryOption;
    private readonly Option<bool> _withLogsOption;
    private readonly Option<CacheProviderType> _withCacheProviderOption;
    private readonly Option<bool> _withMediationOption;
    private readonly Option<bool> _withOptionalMediationOption;

    public ApizrGeneratorOptionsBinder(Option<string> outputPathOption,
        Option<string> nsOption,
        Option<ApizrRegistrationType> registrationTypeOption, 
        Option<bool> withPriorityOption,
        Option<bool> withContextOption, 
        Option<bool> withTokenOption, 
        Option<bool> withRetryOption,
        Option<bool> withLogsOption, 
        Option<CacheProviderType> withCacheProviderOption,
        Option<bool> withMediationOption, 
        Option<bool> withOptionalMediationOption)
    {
        _outputPathOption = outputPathOption;
        _nsOption = nsOption;
        _registrationTypeOption = registrationTypeOption;
        _withPriorityOption = withPriorityOption;
        _withContextOption = withContextOption;
        _withTokenOption = withTokenOption;
        _withRetryOption = withRetryOption;
        _withLogsOption = withLogsOption;
        _withCacheProviderOption = withCacheProviderOption;
        _withMediationOption = withMediationOption;
        _withOptionalMediationOption = withOptionalMediationOption;
    }

    protected override ApizrGeneratorOptions GetBoundValue(BindingContext bindingContext) =>
        new(bindingContext.ParseResult.GetValueForOption(_outputPathOption)!, 
            bindingContext.ParseResult.GetValueForOption(_nsOption)!,
            bindingContext.ParseResult.GetValueForOption(_registrationTypeOption),
            bindingContext.ParseResult.GetValueForOption(_withPriorityOption),
            bindingContext.ParseResult.GetValueForOption(_withContextOption),
            bindingContext.ParseResult.GetValueForOption(_withTokenOption),
            bindingContext.ParseResult.GetValueForOption(_withRetryOption),
            bindingContext.ParseResult.GetValueForOption(_withLogsOption),
            bindingContext.ParseResult.GetValueForOption(_withCacheProviderOption),
            bindingContext.ParseResult.GetValueForOption(_withMediationOption),
            bindingContext.ParseResult.GetValueForOption(_withOptionalMediationOption));
}