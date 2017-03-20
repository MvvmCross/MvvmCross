// MvxComposeEmailTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.System;

namespace MvvmCross.Plugins.Email.WindowsCommon
{
    public class MvxComposeEmailTask
      : IMvxComposeEmailTaskEx
  {
    public bool CanSendAttachments => true;

    public bool CanSendEmail => true;

    public void ComposeEmail(string to, string cc = null, string subject = null, string body = null, bool isHtml = false, string dialogTitle = null)
    {
      var toArray = to == null ? null : new[] { to };
      var ccArray = cc == null ? null : new[] { cc };
      ComposeEmail(
          toArray,
          ccArray,
          subject,
          body,
          isHtml,
          null,
          dialogTitle);
    }

    public async void ComposeEmail(IEnumerable<string> to, IEnumerable<string> cc = null, string subject = null, string body = null, bool isHtml = false, IEnumerable<MvvmCross.Plugins.Email.EmailAttachment> attachments = null, string dialogTitle = null)
    {
        //TODO: It is better to have this function as async Task so to avoid exception swallowing
      EmailMessage email = new EmailMessage();

      if (to != null)
        foreach (var item in to)
        {
          email.To.Add(new EmailRecipient(item));
        }

      if (cc != null)
        foreach (var item in cc)
        {
          email.CC.Add(new EmailRecipient(item));
        }

      email.Subject = subject ?? "";
      email.Body = body ?? "";

      if (attachments != null)
        foreach (var item in attachments)
        {
          email.Attachments.Add(new Windows.ApplicationModel.Email.EmailAttachment(item.FileName, await GetTextFile(item))
          );
        }


      await EmailManager.ShowComposeNewEmailAsync(email);

    }


    private static async Task<StorageFile> GetTextFile(MvvmCross.Plugins.Email.EmailAttachment attachement)
    {
      var localFolder = ApplicationData.Current.LocalFolder;
      var file = await localFolder.CreateFileAsync(attachement.FileName, CreationCollisionOption.ReplaceExisting);

      using (var reader = new StreamReader(attachement.Content))
      {
        await FileIO.WriteTextAsync(file, await reader.ReadToEndAsync());
      }
      return file;
    }
  }
}