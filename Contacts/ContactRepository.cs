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
  List<Contact> BrowseContacts(string? query = null, int? page = null);
  Contact? ReadContact(int id);
  Contact? EditContact(Contact update);
  Contact? AddContact(Contact create);
  Contact? DeleteContact(int id);
}

public class ContactRepositoryImpl : ContactRepository
{
  private List<Contact> contacts = [
    new Contact() { Id = 1, FirstName = "James", LastName = "Anderson", Email = "james.anderson@email.com", PhoneNumber = "5551234567" },
    new Contact() { Id = 2, FirstName = "Sarah", LastName = "Johnson", Email = "sarah.j@gmail.com" },
    new Contact() { Id = 3, FirstName = "Michael", LastName = "Chen", PhoneNumber = "5559876543" },
    new Contact() { Id = 4, FirstName = "Emily", LastName = "Davis", Email = "emily.davis@company.com", PhoneNumber = "5552345678" },
    new Contact() { Id = 5, FirstName = "David", LastName = "Wilson", Email = "d.wilson@outlook.com" },
    new Contact() { Id = 6, FirstName = "Jessica", LastName = "Brown", PhoneNumber = "5553456789" },
    new Contact() { Id = 7, FirstName = "Robert", LastName = "Miller", Email = "rob.miller@work.net", PhoneNumber = "5554567890" },
    new Contact() { Id = 8, FirstName = "Ashley", LastName = "Garcia", Email = "ashley.garcia@personal.com" },
    new Contact() { Id = 9, FirstName = "Christopher", LastName = "Martinez", PhoneNumber = "5555678901" },
    new Contact() { Id = 10, FirstName = "Amanda", LastName = "Rodriguez", Email = "amanda.r@service.org", PhoneNumber = "5556789012" },
    new Contact() { Id = 11, FirstName = "Matthew", LastName = "Thompson", Email = "matt.thompson@domain.com" },
    new Contact() { Id = 12, FirstName = "Lauren", LastName = "White", PhoneNumber = "5557890123" },
    new Contact() { Id = 13, FirstName = "Daniel", LastName = "Harris", Email = "daniel.harris@business.co", PhoneNumber = "5558901234" },
    new Contact() { Id = 14, FirstName = "Megan", LastName = "Clark", Email = "megan.clark@mail.com" },
    new Contact() { Id = 15, FirstName = "Andrew", LastName = "Lewis", PhoneNumber = "5559012345" },
    new Contact() { Id = 16, FirstName = "Nicole", LastName = "Walker", Email = "nicole.walker@site.net", PhoneNumber = "5550123456" },
    new Contact() { Id = 17, FirstName = "Kevin", LastName = "Hall", Email = "kevin.hall@provider.com" },
    new Contact() { Id = 18, FirstName = "Stephanie", LastName = "Allen", PhoneNumber = "5551237890" },
    new Contact() { Id = 19, FirstName = "Brian", LastName = "Young", Email = "brian.young@tech.io", PhoneNumber = "5552348901" },
    new Contact() { Id = 20, FirstName = "Rachel", LastName = "King", Email = "rachel.king@connect.org" },
    new Contact() { Id = 21, FirstName = "Justin", LastName = "Wright", PhoneNumber = "5553459012" },
    new Contact() { Id = 22, FirstName = "Samantha", LastName = "Lopez", Email = "sam.lopez@network.com", PhoneNumber = "5554560123" },
    new Contact() { Id = 23, FirstName = "Tyler", LastName = "Hill", Email = "tyler.hill@digital.net" },
    new Contact() { Id = 24, FirstName = "Kayla", LastName = "Green", PhoneNumber = "5555671234" },
    new Contact() { Id = 25, FirstName = "Nathan", LastName = "Adams", Email = "nathan.adams@system.co", PhoneNumber = "5556782345" },
  ];

  private int GenerateNextId()
  {
    var maxId = contacts.MaxBy(contact => contact.Id)?.Id ?? 0;
    return maxId + 1;
  }

  public List<Contact> BrowseContacts(string? query = null, int? page = null)
  {
    var result = contacts;
    if (!string.IsNullOrEmpty(query))
    {
      result = result.Where(contact =>
         contact.FirstName.Contains(query)
         || contact.LastName.Contains(query)
         || (contact.PhoneNumber?.Contains(query) ?? false)
         || (contact.Email?.Contains(query) ?? false)
      ).ToList();
    }

    if (page != null)
    {
      var start = (page.Value - 1) * 10;

      var count = result.Count - start - 10 > 0 ? 10 : result.Count - start;

      result = result.GetRange(start, count);
    }

    return result;
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
