using OTA;
using OTA.Command;
using SlackPlugin.Controllers;
using System;
using System.Text;

namespace SlackPlugin
{
    public class SlackSender : ConsoleSender, IDisposable
    {
        private string _senderName;
        private StringBuilder _builder;

        public string Text
        {
            get { return _builder.ToString(); }
        }

        public SlackSender(string username)
        {
            _senderName = username;
            _builder = new StringBuilder();
        }

        public override string SenderName => _senderName;

        public override void SendMessage(string message, int sender = 255, byte R = 255, byte G = 255, byte B = 255)
        {
            //SlackPlugin.Instance.SendMessage(message);
            if (_builder != null)
            {
                _builder.AppendLine(message);
            }
        }

        public void Dispose()
        {
            if (_senderName != null) _senderName = null;
            if (_builder != null)
            {
                _builder.Clear();
                _builder = null;
            }
        }
    }
}
