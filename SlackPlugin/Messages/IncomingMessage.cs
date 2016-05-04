namespace SlackPlugin.Messages
{
    public class IncomingMessage
    {
        public string token { get; set; }

        public string team_id { get; set; }

        public string team_domain { get; set; }

        public string channel_id { get; set; }

        public string channel_name { get; set; }

        public string user_id { get; set; }

        public string user_name { get; set; }

        public virtual string MessageText { get { return string.Empty; } }

        public virtual bool IsValid()
        {
            return false;
        }
    }
}
