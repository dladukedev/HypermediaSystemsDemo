using HypermediaSystemsDemo.Contacts;

namespace HypermediaSystemsDemo.Models;

public class ContactUpsertViewModel
{
  public Contact Contact { get; set; } = new Contact();
  public Dictionary<string, List<string>> Errors { get; set; } = [];

  public bool HasError(string key)
  {
    return Errors.GetValueOrDefault(key) != null;
  }

  public string GetErrorString(string key)
  {
    var errors = Errors.GetValueOrDefault(key) ?? [];
    return string.Join(", ", errors);
  }
}
