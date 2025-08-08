using HypermediaSystemsDemo.Contacts;

namespace HypermediaSystemsDemo.Models;

public class ContactListViewModel
{
  public string? SearchTerm { get; set; }
  public List<Contact> Contacts { get; set; } = [];
}
