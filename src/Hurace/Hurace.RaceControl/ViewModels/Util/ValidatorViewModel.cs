using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Hurace.RaceControl.ViewModels.Util
{
    public abstract class ValidatorViewModel<TContext,TValidator> : NotifyPropertyChanged where TValidator : AbstractValidator<TContext>, new()
    {
        private string _validationMessage;
        private bool _validatorIsValid;
        private readonly TValidator _validator = new TValidator();
        
        public bool ValidatorEnabled { get; set; }

        public bool ValidatorIsValid
        {
            get => _validatorIsValid;
            set => Set(ref _validatorIsValid, value);
        }

        public string ValidationMessage
        {
            get => _validationMessage;
            set => Set(ref _validationMessage, value);
        }
        
        protected void RegisterValidator(TContext context,params string[] excludedFields)
        {
            var exclusionList = excludedFields.ToList();
            exclusionList.Add(nameof(ValidatorIsValid));
            exclusionList.Add(nameof(ValidationMessage));
            
            PropertyChanged += (sender, args) =>
            {
                if (exclusionList.Contains(args.PropertyName) && ValidatorEnabled) return;
                var res = _validator.Validate(context);
                ValidationMessage = res.Errors.FirstOrDefault()?.ErrorMessage;
                ValidatorIsValid = res.IsValid;
            };
        }
    }
}