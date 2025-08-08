using HypermediaSystemsDemo.Models;

namespace HypermediaSystemsDemo.Contacts;

public class Contact
{
  public int Id { get; set; }
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }

  public string FullName => $"{FirstName} {LastName}";
  public string? FormattedPhone
  {
    get
    {
      if (PhoneNumber is null) return null;

      return PhoneNumber.Length == 11
        ? $"({PhoneNumber.Substring(1, 3)}) {PhoneNumber.Substring(4, 3)}-{PhoneNumber.Substring(7, 4)}"
        : $"({PhoneNumber.Substring(0, 3)}) {PhoneNumber.Substring(3, 3)}-{PhoneNumber.Substring(6, 4)}";
    }
  }
}

public interface ContactRepository
{
  List<Contact> BrowseContacts(string? query = null);
  Contact? ReadContact(int id);
  Contact? EditContact(Contact update);
  Contact? AddContact(Contact create);
  Contact? DeleteContact(int id);
}

public class ContactRepositoryImpl : ContactRepository
{
  private List<Contact> contacts = [
    new Contact() { Id = 1, FirstName = "First", LastName = "Last", Email = "me@gmail.com" },
    new Contact() { Id = 2, FirstName = "Other", LastName = "Last", PhoneNumber = "3121231234" },
  ];

  private int GenerateNextId()
  {
    var maxId = contacts.MaxBy(contact => contact.Id)?.Id ?? 0;
    return maxId + 1;
  }

  public List<Contact> BrowseContacts(string? query = null)
  {
    if (string.IsNullOrEmpty(query)) return contacts;

    return contacts.Where(contact =>
       contact.FirstName.Contains(query)
       || contact.LastName.Contains(query)
       || (contact.PhoneNumber?.Contains(query) ?? false)
       || (contact.Email?.Contains(query) ?? false)
    ).ToList();
  }

  public Contact? ReadContact(int id)
  {
    return contacts.Find(contact => contact.Id == id);
  }

  public Contact? EditContact(Contact update)
  {
    contacts = contacts.Select(contact => contact.Id == update.Id ? update : contact).ToList();
    return update;
  }

  public Contact? AddContact(Contact create)
  {
    var nextId = GenerateNextId();
    create.Id = nextId;
    contacts = contacts.Append(create).ToList();
    return create;
  }

  public Contact? DeleteContact(int id)
  {
    var contact = ReadContact(id);

    if (contact == null)
    {
      return null;
    }

    contacts = contacts.Where(contact => contact.Id != id).ToList();

    return contact;
  }
}
