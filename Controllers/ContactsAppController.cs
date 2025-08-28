using HypermediaSystemsDemo.Contacts;
using HypermediaSystemsDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace HypermediaSystemsDemo.Controllers;

public class ContactsAppController(
    ContactRepository contactRepo
  ) : Controller
{
  private readonly ContactRepository _contactRepo = contactRepo;

  [HttpGet]
  [Route("app")]
  public IActionResult Index()
  {
    Response.ContentType = "application/vnd.hyperview+xml";

    return View("../App/Index");
  }

  [HttpGet]
  [Route("app/contacts")]
  public IActionResult GetContacts(string? q, int? page, bool? rows_only)
  {
    var pageValue = page == null ? 1 : page.Value;

    var contacts = _contactRepo.BrowseContacts(q, pageValue);

    var vm = new ContactAppListViewModel()
    {
      SearchTerm = q,
      Contacts = contacts,
      Page = pageValue
    };

    Response.ContentType = "application/vnd.hyperview+xml";

    return rows_only == true
      ? PartialView("../App/ContactList", vm)
      : View("../App/ContactListScreen", vm);
  }

  [HttpGet]
  [Route("app/contacts/{id}")]
  public IActionResult Details(int id)
  {
    var contact = _contactRepo.ReadContact(id);

    if (contact is null)
    {
      return NotFound();
    }

    var vm = new ContactDetailsViewModel() { Contact = contact };

    return View("../App/ContactDetailsScreen", vm);
  }

  [HttpGet]
  [Route("app/contacts/{id}/edit")]
  public IActionResult Edit(int id)
  {
    var updatingContact = _contactRepo.ReadContact(id);

    if (updatingContact is null) return NotFound();

    var vm = new ContactUpsertViewModel() { Contact = updatingContact };

    return View("../App/ContactUpdateScreen", vm);
  }

  [HttpPost]
  [Route("app/contacts/{id}/edit")]
  public IActionResult Edit(int id, Contact contact)
  {
    var updatingContact = _contactRepo.ReadContact(id);

    if (updatingContact is null) return NotFound();

    var validationResult = ContactValidator.Validate(contact);
    Console.WriteLine(validationResult);

    var vm = new ContactUpsertViewModel()
    {
      Errors = validationResult.Errors,
      Contact = contact,
    };

    if (validationResult.IsValid)
    {
      _contactRepo.EditContact(contact);

      TempData["SuccessMessage"] = "Update Contact!";
      TempData["ContactSaveSuccess"] = true;

      contact.Id = id;
    }

    return PartialView("../App/ContactForm", vm);
  }

  [HttpPost]
  [Route("app/contacts/{id}/delete")]
  public IActionResult Delete(int id, bool? root)
  {
    var deletedContact = _contactRepo.DeleteContact(id);

    if (deletedContact is null) return NotFound();

    TempData["SuccessMessage"] = "Contact Removed!";

    var vm = new ContactDeletedViewModel()
    {
      IsFromRoot = root ?? false
    };

    return PartialView("../App/ContactDeleted", vm);
  }

  [HttpGet]
  [Route("app/contacts/new")]
  public IActionResult New()
  {
    var vm = new ContactUpsertViewModel();

    return View("../App/ContactAddScreen", vm);
  }

  [HttpPost]
  [Route("app/contacts/new")]
  public IActionResult New(Contact contact)
  {
    var validationResult = ContactValidator.Validate(contact);

    var vm = new ContactUpsertViewModel()
    {
      Errors = validationResult.Errors,
      Contact = contact,
    };

    if (validationResult.IsValid)
    {
      var newContact = _contactRepo.AddContact(contact);

      TempData["SuccessMessage"] = "Created New Contact!";
      TempData["ContactSaveSuccess"] = true;

      vm.Contact = newContact;
    }

    return PartialView("../App/ContactForm", vm);
  }

}
