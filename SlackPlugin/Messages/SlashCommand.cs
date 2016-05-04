using System;
using System.Linq;

namespace SlackPlugin.Messages
{
    public class SlashCommand
    {
        public string token { get; set; }

        public string team_id { get; set; }

        public string team_domain { get; set; }

        public string channel_id { get; set; }

        public string channel_name { get; set; }

        public string user_id { get; set; }

        public string user_name { get; set; }

        public string command  { get; set; }

        public string text { get; set; }

        public string response_url { get; set; }

        public bool IsValid()
        {
            return SlackPlugin.Config.WebHookToken.Split(',').Contains(token) && !String.IsNullOrWhiteSpace(command) && !String.IsNullOrWhiteSpace(user_name);
        }

        public override string ToString()
        {
            return $"C: {command}, U: {user_name}, WT: {(token ?? String.Empty).Length}, T: {text}";
        }
    }
}
