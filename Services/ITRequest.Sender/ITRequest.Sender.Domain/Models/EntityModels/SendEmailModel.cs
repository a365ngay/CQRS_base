namespace ITRequest.Sender.Domain.Models.EntityModels
{
    public class SendEmailModel
    {
        public IList<string>? ToEmails { get; set; }
        public IList<string>? BccEmails { get; set; }
        public IList<string>? CcEmails { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}
