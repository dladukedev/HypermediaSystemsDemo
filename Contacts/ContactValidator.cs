using System.Text.RegularExpressions;

namespace HypermediaSystemsDemo.Contacts;

public class ValidationResult
{
  public Dictionary<string, List<string>> Errors { get; set; } = [];
  public bool IsValid => Errors.Count == 0;
}

public class ContactValidator
{
  public static ValidationResult Validate(Contact contact)
  {
    var validationResult = new Dictionary<string, List<string>>() {
      { "FirstName", ValidateFirstName(contact.FirstName) },
      { "LastName", ValidateLastName(contact.LastName) },
      { "Phone", ValidatePhone(contact.PhoneNumber) },
      { "Email", ValidateEmail(contact.Email) },
    };

    var errors = validationResult.Where(pair => pair.Value.Count != 0).ToDictionary();

    return new ValidationResult() { Errors = errors };
  }

  private static List<string> ValidateFirstName(string? firstName)
  {
    var Errors = new List<string>();

    if (string.IsNullOrEmpty(firstName))
    {
      Errors.Add("First Name is Required");
    }

    return Errors;
  }

  private static List<string> ValidateLastName(string? lastName)
  {
    var Errors = new List<string>();

    if (string.IsNullOrEmpty(lastName))
    {
      Errors.Add("Last Name is Required");
    }

    return Errors;
  }

  private static List<string> ValidatePhone(string? phone)
  {
    if (string.IsNullOrEmpty(phone))
    {
      return [];
    }

    var Errors = new List<string>();

    if (!Regex.IsMatch(phone, @"^\d*$"))
    {
      Errors.Add("Phone Number must be all digits");
    }

    if (phone.Length != 10 && (phone.First() != '1' && phone.Length == 11))
    {
      Errors.Add("Phone Number Must be length 10 or 11");
    }

    return Errors;
  }

  public static List<string> ValidateEmail(string? email)
  {
    if (string.IsNullOrEmpty(email))
    {
      return [];
    }

    var Errors = new List<string>();

    if (!Regex.IsMatch(email, @".*@.*\.."))
    {
      Errors.Add("Email is in Incorrect Format");
    }

    return Errors;
  }
}
