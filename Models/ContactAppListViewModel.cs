using HypermediaSystemsDemo.Contacts;

namespace HypermediaSystemsDemo.Models;

public class ContactAppListViewModel
{
  public string? SearchTerm { get; set; }
  public List<Contact> Contacts { get; set; } = [];
  public int Page { get; set; }
}
