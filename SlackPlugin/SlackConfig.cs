﻿using TDSM.Core.Config;

namespace SlackPlugin
{
    public class SlackConfig : ComponentConfiguration<SlackConfig>
    {
        [ConfigPrefix("slack-channel")]
        public string Channel { get; set; }

        [ConfigPrefix("slack-webhook-url")]
        public string WebHookUrl { get; set; }

        [ConfigPrefix("slack-username")]
        public string UserName { get; set; }

        [ConfigPrefix("slack-icon-emoji")]
        public string IconEmoji { get; set; }

        [ConfigPrefix("slack-webhook-token")]
        public string WebHookToken { get; set; }

        [ConfigPrefix("slack-chat-prefix")]
        public string ChatPrefix { get; set; }
    }
}