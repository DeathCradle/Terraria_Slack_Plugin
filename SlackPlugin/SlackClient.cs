using Newtonsoft.Json;
using SlackPlugin.Messages;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace SlackPlugin
{
    public class SlackClient
    {
        private Uri _webhookUrl { get; set; }

        public SlackClient(string webhookUrl)
        {
            _webhookUrl = new Uri(webhookUrl);
        }

        public void SendMessage(string text)
        {
            SendMessage(text, SlackPlugin.Config.UserName, SlackPlugin.Config.Channel, SlackPlugin.Config.IconEmoji);
        }

        public void SendMessage(string text, string username, string channel, string icon_emoji)
        {
            var payload = new OutgoingMessage
            {
                Channel = channel,
                UserName = username,
                Text = text,
                IconEmoji = icon_emoji
            };

            string json = JsonConvert.SerializeObject(payload);
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data["payload"] = json;
                var response = client.UploadValues(_webhookUrl, "POST", data);
                string responseText = UTF8Encoding.Default.GetString(response);

                //do we care about response??
            }
        }
    }
}
