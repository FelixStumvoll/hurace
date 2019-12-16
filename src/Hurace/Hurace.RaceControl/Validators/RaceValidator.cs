using System;
using FluentValidation;
using Hurace.RaceControl.Controls;
using Hurace.RaceControl.ViewModels;

namespace Hurace.RaceControl.Validators
{
    public class RaceValidator : AbstractValidator<RaceBaseDataViewModel>
    {
        public RaceValidator()
        {
            const string postfix = "muss definiert werden";

            RuleFor(vm => vm.RaceState.Race.RaceDescription)
                .NotEmpty().WithMessage($"Beschreibung {postfix}")
                .MaximumLength(150).WithMessage("Die Beschreibung darf max. 150 Zeichen lang sein");

            RuleFor(vm => vm.RaceState.Race.RaceDate)
                .NotEmpty().WithMessage($"Renndatum {postfix}")
                .Must(dt => dt > DateTime.Today)
                .When(vm => vm.RaceState.Race.Id == -1)
                .WithMessage("Neue Rennen können nicht in der Vergangenheit liegen");


            RuleFor(vm => vm.SelectedGender)
                .NotEmpty().WithMessage("Geschlecht muss ausgewählt werden");

            RuleFor(vm => vm.SensorCount)
                .NotEmpty().WithMessage($"Sensoren {postfix}")
                .Must(val => val > 0).WithMessage("Es muss mindestens 1 Sensor definiert sein");


            RuleFor(vm => vm.SelectedLocation)
                .NotEmpty().WithMessage("Rennort muss ausgewählt werden");

            RuleFor(vm => vm.SelectedDiscipline)
                .NotEmpty().WithMessage("Disziplin muss ausgewählt werden");
        }
    }
}