using FluentValidation.Results;

using Settlers.Toolbox.ViewModels.Interfaces;

namespace Settlers.Toolbox.ViewModels.Validator.Interfaces
{
    public interface IMainViewModelValidator
    {
        ValidationResult Validate(IMainViewModel context);
    }
}