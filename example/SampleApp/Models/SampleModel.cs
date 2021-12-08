using System;

using FluentValidation;

namespace SampleApp.Models
{
    public abstract class SampleModelBase
    {
        public string Name { get; set; }
    }

    public class UpdateSampleModel : SampleModelBase
    {
        public Guid Id { get; set; }
    }

    public class CreateSampleModel : SampleModelBase
    {

    }

    public class SampleModel : SampleModelBase
    {
        public Guid Id { get; set; }
    }

    public class CreateSampleModelValidator : AbstractValidator<CreateSampleModel>
    {
        public CreateSampleModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(payload => "Name is required");
        }
    }

    public class UpdateSampleModelValidator : AbstractValidator<UpdateSampleModel>
    {
        public UpdateSampleModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage(payload => "Id is required");
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(payload => "Name is required");
        }
    }
}
