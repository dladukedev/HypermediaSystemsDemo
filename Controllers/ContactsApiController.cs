using HypermediaSystemsDemo.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace HypermediaSystemsDemo.Controllers;

public class ContactsApiController(
    ContactRepository contactRepo
  ) : Controller
{
  private readonly ContactRepository _contactRepo = contactRepo;

  [HttpGet]
  [Route("api/v1/contacts")]
  public IActionResult GetContacts()
  {
    var contacts = _contactRepo.BrowseContacts();

    return Json(contacts);
  }

  [HttpGet]
  [Route("api/v1/contacts/{id}")]
  public IActionResult GetContact(int id)
  {
    var contact = _contactRepo.ReadContact(id);

    if (contact is null)
    {
      return NotFound();
    }

    return Json(contact);
  }

  [HttpPut]
  [Route("api/v1/contacts/{id}")]
  public IActionResult PutContact(int id, Contact contact)
  {
    var updatingContact = _contactRepo.ReadContact(id);

    if (updatingContact is null) return NotFound();

    var validationResult = ContactValidator.Validate(contact);

    if (!validationResult.IsValid)
    {
      return BadRequest();
    }

    var updatedContact = _contactRepo.EditContact(contact);

    return Json(updatedContact);
  }

  [HttpPost]
  [Route("api/v1/contacts")]
  public IActionResult PostContact(Contact contact)
  {
    var validationResult = ContactValidator.Validate(contact);

    if (!validationResult.IsValid)
    {
      return BadRequest();
    }

    var newContact = _contactRepo.AddContact(contact);

    return Json(newContact);
  }

  [HttpDelete]
  [Route("api/v1/contacts/{id}")]
  public IActionResult DeleteContact(int id)
  {
    var deletedContact = _contactRepo.DeleteContact(id);

    if (deletedContact is null) return NotFound();

    return Json(deletedContact);
  }
}
