using Newtonsoft.Json;
using System;
using System.Linq;

namespace SlackPlugin.Messages
{
    public class TriggerWord : IncomingMessage
    {
        public string timestamp { get; set; }

        public string text { get; set; }

        public string trigger_word { get; set; }

        private string _messageText;
        [JsonIgnore]
        public override string MessageText
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

        public override bool IsValid()
        {
            return SlackPlugin.Config.WebHookToken.Split(',').Contains(token) && !String.IsNullOrWhiteSpace(MessageText) && !String.IsNullOrWhiteSpace(user_name);
        }

        public override string ToString()
        {
            return $"MT: {MessageText}, U: {user_name}, WT: {(token ?? String.Empty).Length}, T: {text}";
        }
    }
}
