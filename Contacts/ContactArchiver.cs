namespace HypermediaSystemsDemo.Contacts;


public enum ContactArchiverStatus
{
  Waiting, Running, Complete
}

public interface ContactArchiver
{
  ContactArchiverStatus GetStatus();
  double GetProgress();
  void StartJob();
  string GetArchiveFilePath();
  void Reset();
}

public class ContactArchiverImpl : ContactArchiver
{
  private readonly Random random = new();

  private ContactArchiverStatus status = ContactArchiverStatus.Waiting;
  private double progress = 0.0;

  public string GetArchiveFilePath()
  {
    return "contacts.json";
  }

  public double GetProgress()
  {
    return progress;
  }

  public ContactArchiverStatus GetStatus()
  {
    return status;
  }

  public void StartJob()
  {
    if (status == ContactArchiverStatus.Waiting)
    {
      status = ContactArchiverStatus.Running;
      progress = 0.0;

      Task.Run(RunJobAsync);
    }
  }

  private async Task RunJobAsync()
  {
    for (var i = 0; i < 10; i++)
    {
      var delay = 1_000 * random.NextDouble();
      await Task.Delay((int)delay);

      if (status != ContactArchiverStatus.Running) return;

      progress = (i + 1) / 10.0;
    }

    await Task.Delay(1_000);

    if (status != ContactArchiverStatus.Running) return;
    status = ContactArchiverStatus.Complete;
  }

  public void Reset()
  {
    status = ContactArchiverStatus.Waiting;
  }
}
