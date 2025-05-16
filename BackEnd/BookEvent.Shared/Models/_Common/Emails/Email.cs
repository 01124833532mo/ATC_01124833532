namespace BookEvent.Shared.Models._Common.Emails
{
    public class Email
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = false;

    }
}
