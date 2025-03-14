namespace mac;
using FluentValidation;

public class MacTableData
{
  public Guid Id { get; set; }

  public required string MacAddress { get; set; }

  public required DateTime ExpirationDate { get; set; }

  public required string Email { get; set; }
}

public class MacTableDataValidator : AbstractValidator<MacTableData>
{
  public MacTableDataValidator(MacTableDataContext db)
  {
    RuleFor(x => x.MacAddress)
      .NotEmpty()
      .WithMessage("Üres MAC-cím")
      .Matches(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$")
      .WithMessage("Helytelen MAC-cím formátum")
      .Must((instance, address) => NotExist(db, instance))
      .WithMessage(m => $"{m.MacAddress} már létezik");
    RuleFor(x => x.ExpirationDate)
      .NotEmpty()
      .WithMessage("A lejárati dátum nem lehet üres")
      .GreaterThan(DateTime.Now)
      .WithMessage("A lejárati dátum nem lehet múltbeli");
    RuleFor(x => x.Email)
      .NotEmpty()
      .WithMessage("Az email cím nem lehet üres")
      .EmailAddress()
      .WithMessage("Helytelen email cím formátum");
  }

  public bool NotExist(MacTableDataContext db, MacTableData macTbDt)
  {
    return !db.MacTableDatas.Any(m => m.MacAddress == macTbDt.MacAddress && m.Id != macTbDt.Id);
  }
}