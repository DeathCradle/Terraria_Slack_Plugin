using System;
using System.Linq;

namespace SlackPlugin.Messages
{
    public class SlashCommand : IncomingMessage
    {
        public string text { get; set; }

        public string command { get; set; }

        public string response_url { get; set; }

        public override string MessageText => text;

        public override bool IsValid()
        {
            return SlackPlugin.Config.WebHookToken.Split(',').Contains(token) && !String.IsNullOrWhiteSpace(command) && !String.IsNullOrWhiteSpace(user_name);
        }

        public override string ToString()
        {
            return $"C: {command}, U: {user_name}, WT: {(token ?? String.Empty).Length}, T: {text}";
        }
    }
}
