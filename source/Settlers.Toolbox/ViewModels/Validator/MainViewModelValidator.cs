using System.IO;
using System.Text.RegularExpressions;

using FluentValidation;
using FluentValidation.Results;

using Settlers.Toolbox.ViewModels.Interfaces;
using Settlers.Toolbox.ViewModels.Validator.Interfaces;

namespace Settlers.Toolbox.ViewModels.Validator
{
    public class MainViewModelValidator : AbstractValidator<IMainViewModel>, IMainViewModelValidator
    {
        private const string REGEX_PATTERN_NUMBER = "^[0-9]+$";

        public MainViewModelValidator()
        {
            // Validation rules for any control
            RuleFor(vm => vm.SettlersExePath)
                .Must(value => !string.IsNullOrEmpty(value)).WithMessage("Please enter a path.")
                .Must(value => value?.EndsWith("S4.exe") ?? false).WithMessage("File path must end with 'S4.exe'.")
                .Must(File.Exists).WithMessage("None file with the given path does exist.");

            RuleFor(vm => vm.CustomWidth)
                .Must(IsPlainNumber).WithMessage("Only numbers from 0-9 are valid.")
                .Must(IsUShortNumber).WithMessage($"Number must be in range of {ushort.MinValue}-{ushort.MaxValue}.");

            RuleFor(vm => vm.CustomHeight)
                .Must(IsPlainNumber).WithMessage("Only numbers from 0-9 are valid.")
                .Must(IsUShortNumber).WithMessage($"Number must be in range of {ushort.MinValue}-{ushort.MaxValue}.");
        }

        public new ValidationResult Validate(IMainViewModel context)
        {
            return base.Validate(context);
        }

        private bool IsPlainNumber(string value)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, REGEX_PATTERN_NUMBER);
        }

        private bool IsUShortNumber(string value)
        {
            return ushort.TryParse(value, out _);
        }
    }
}