using ContractorsApp.Models;
using FluentValidation;

namespace ContractorsApp.Validation
{
    public class ContractorFluentValidation : AbstractValidator<Contractor>
    {
        public ContractorFluentValidation()
        {
            // required, max length 255
            RuleFor(c => c.CompanyName)
                .NotEmpty().WithMessage("Nazwa firmy nie może być pusta.")
                .MaximumLength(255).WithMessage("Nazwa firmy nie może przekraczać 255 znaków.");

            // required, max length 15, optional format validation
            RuleFor(c => c.TaxNumber)
                .NotEmpty().WithMessage("NIP nie może być pusty.")
                .MaximumLength(15).WithMessage("NIP nie może przekraczać 15 znaków.")
                .Matches(@"^\d{10}$").WithMessage("NIP musi składać się z dokładnie 10 cyfr."); // Polish NIP

            // length 9 - 14, optional format validation
            RuleFor(c => c.REGON)
                .MaximumLength(14).WithMessage("REGON nie może przekraczać 14 znaków.")
                .Matches(@"^\d{9,14}$").WithMessage("REGON musi składać się z 9 lub 14 cyfr."); // Polish REGON

            // only one main address
            RuleFor(c => c.Addresses.Count(a => a.IsMainAddress))
                .Equal(1).WithMessage("Dokładnie jeden adres musi być oznaczony jako główny.");

            // Validate each address
            RuleForEach(c => c.Addresses).SetValidator(new AddressFluentValidation());
        }
    }

    public class AddressFluentValidation : AbstractValidator<Address>
    {
        public AddressFluentValidation()
        {
            //max length 255
            RuleFor(a => a.Street)
                .MaximumLength(255).WithMessage("Ulica nie może przekraczać 255 znaków.");

            // max length 255
            RuleFor(a => a.City)
                .MaximumLength(255).WithMessage("Miasto nie może przekraczać 255 znaków.");

            //format validation
            RuleFor(a => a.PostalCode)
                .Matches(@"^\d{2}-\d{3}$").WithMessage("Kod pocztowy musi być w formacie XX-XXX.")
                .When(a => !string.IsNullOrEmpty(a.PostalCode));
        }
    }
}

