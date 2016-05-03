using OTA;
using OTA.Command;

namespace SlackPlugin
{
    public class SlackSender : ConsoleSender
    {
        public static readonly SlackSender Channel = new SlackSender("Channel");

        private string _senderName;

        public SlackSender(string username)
        {
            _senderName = username;
        }

        public override string SenderName => _senderName;

        public override void SendMessage(string message, int sender = 255, byte R = 255, byte G = 255, byte B = 255)
        {
            SlackPlugin.Instance.SendMessage(message);
        }
    }
}
