using Microsoft.AspNetCore.Mvc;
using HypermediaSystemsDemo.Contacts;
using HypermediaSystemsDemo.Models;
using System.Net;
using System.Net.Mime;

namespace HypermediaSystemsDemo.Controllers;

public class ContactsController(
    ContactArchiver contactArchiver,
    ContactRepository contactRepo
  ) : Controller
{
  private readonly ContactArchiver _contactArchiver = contactArchiver;
  private readonly ContactRepository _contactRepo = contactRepo;

  public IActionResult Index(string? q, int? page)
  {
    var pageValue = page == null ? 1 : page.Value;
    var contacts = _contactRepo.BrowseContacts(q, pageValue);

    var vm = new ContactListViewModel()
    {
      SearchTerm = q,
      Contacts = contacts,
      Page = pageValue,
      Archiver = _contactArchiver
    };

    Request.Headers.TryGetValue("HX-Trigger", out var htmxRequest);

    if (htmxRequest == "search")
    {
      return PartialView("ContactList", vm);
    }

    return View(vm);
  }

  public IActionResult Count()
  {
    var count = _contactRepo.BrowseContacts().Count;

    return Content($" - ({count}) total Contacts");
  }

  public IActionResult New()
  {
    var vm = new ContactUpsertViewModel();

    return View(vm);
  }

  public IActionResult Details(int id)
  {
    var contact = _contactRepo.ReadContact(id);

    if (contact is null)
    {
      return NotFound();
    }

    var vm = new ContactDetailsViewModel() { Contact = contact };

    return View(vm);
  }

  [HttpPost]
  [ActionName("New")]
  public IActionResult New_Post(Contact contact)
  {
    var validationResult = ContactValidator.Validate(contact);

    if (validationResult.IsValid)
    {
      _contactRepo.AddContact(contact);

      TempData["SuccessMessage"] = "Created New Contact!";
      return Redirect("/Contacts");
    }
    else
    {
      var vm = new ContactUpsertViewModel()
      {
        Errors = validationResult.Errors,
        Contact = contact,
      };

      return View(vm);
    }
  }

  public IActionResult Email(string? email)
  {
    var validationResult = ContactValidator.ValidateEmail(email);

    return Content(string.Join(", ", validationResult));
  }

  public IActionResult Edit(int id)
  {
    var updatingContact = _contactRepo.ReadContact(id);

    if (updatingContact is null) return NotFound();

    var vm = new ContactUpsertViewModel() { Contact = updatingContact };

    return View(vm);
  }

  [HttpPost]
  [ActionName("Edit")]
  public IActionResult Edit_Post(int id, Contact contact)
  {
    var updatingContact = _contactRepo.ReadContact(id);

    if (updatingContact is null) return NotFound();

    var validationResult = ContactValidator.Validate(contact);

    if (validationResult.IsValid)
    {
      _contactRepo.EditContact(contact);

      TempData["SuccessMessage"] = "Update Contact!";
      return Redirect("/Contacts");
    }
    else
    {
      var vm = new ContactUpsertViewModel()
      {
        Errors = validationResult.Errors,
        Contact = contact,
      };

      return View(vm);
    }
  }

  [HttpDelete]
  public IActionResult Delete(int id)
  {
    var deletedContact = _contactRepo.DeleteContact(id);

    if (deletedContact is null) return NotFound();

    Request.Headers.TryGetValue("HX-Trigger", out var htmxRequest);

    if (htmxRequest == "delete-btn")
    {
      TempData["SuccessMessage"] = "Contact Removed!";

      Response.Headers.Append("Location", "/Contacts");
      return new StatusCodeResult((int)HttpStatusCode.SeeOther);
    }

    return Content("");
  }

  [HttpDelete]
  public IActionResult BulkDelete(List<int> selected_contact_ids)
  {
    var removedCount = 0;

    foreach (var id in selected_contact_ids)
    {
      var deletedContact = _contactRepo.DeleteContact(id);
      if (deletedContact is not null) { removedCount++; }
    }

    var contacts = _contactRepo.BrowseContacts();

    var vm = new ContactListViewModel()
    {
      SearchTerm = "",
      Contacts = contacts,
      Page = 1,
      Archiver = _contactArchiver
    };

    TempData["SuccessMessage"] = $"{removedCount} Contact(s) Removed!";

    return View("Index", vm);
  }


  public IActionResult Archive()
  {
    return PartialView("Archive", _contactArchiver);
  }

  [HttpPost]
  [ActionName("Archive")]
  public IActionResult Archive_Post()
  {
    _contactArchiver.StartJob();

    return PartialView("Archive", _contactArchiver);
  }

  [Route("Contacts/Archive/File")]
  public IActionResult Archive_File()
  {
    return File("~/contacts.json", MediaTypeNames.Text.Plain, "Contacts.json");
  }

  [HttpDelete]
  [ActionName("Archive")]
  public IActionResult Archive_Delete()
  {
    _contactArchiver.Reset();

    return PartialView("Archive", _contactArchiver);
  }
}
