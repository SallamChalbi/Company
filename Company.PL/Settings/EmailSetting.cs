namespace Company.PL.Settings
{
    public class EmailSetting
    {
        public string DisplayName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string SmtpClientServer { get; set; }
        public int SmtpClientPort { get; set; }
    }
}
