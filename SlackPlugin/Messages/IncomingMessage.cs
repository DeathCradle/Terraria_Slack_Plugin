using Newtonsoft.Json;
using System;
using System.Linq;

namespace SlackPlugin.Messages
{
    public class IncomingMessage
    {
        public string token { get; set; }

        public string team_id { get; set; }

        public string team_domain { get; set; }

        public string channel_id { get; set; }

        public string channel_name { get; set; }

        public string timestamp { get; set; }

        public string user_id { get; set; }

        public string user_name { get; set; }

        public string text { get; set; }

        public string trigger_word { get; set; }

        private string _messageText;
        [JsonIgnore]
        public string MessageText
        {
            get
            {
                if (_messageText != null) return _messageText;

                _messageText = text;
                if (!String.IsNullOrEmpty(trigger_word))
                {
                    var ix = _messageText.IndexOf(trigger_word);
                    if (ix > -1)
                    {
                        return _messageText = _messageText.Remove(0, ix + trigger_word.Length).Trim();
                    }
                }

                return _messageText;
            }
        }

        public bool IsValid()
        {
            return SlackPlugin.Config.WebHookToken.Split(',').Contains(token) && !String.IsNullOrWhiteSpace(MessageText) && !String.IsNullOrWhiteSpace(user_name);
        }

        public override string ToString()
        {
            return $"MT: {MessageText}, U: {user_name}, WT: {(token ?? String.Empty).Length}, T: {text}";
        }
    }
}
