using System;
using OTA.Plugin;
using System.Web.Http;
using OTA.Logging;

[assembly: PluginDependency("OTA.Commands")]
[assembly: PluginDependency("TDSM.Core")]
namespace SlackPlugin
{
    [OTAVersion(1, 0)]
    public class SlackPlugin : BasePlugin
    {
        public static SlackConfig Config { get; private set; } = new SlackConfig();
        public static SlackClient Instance { get; private set; }

        public SlackPlugin()
        {
            Version = "1";
            Author = "swrhim";
            Name = "Slack Notification Plugin";
            Description = "This plugin will notify a slack channel of users logging in and out";
        }

        protected override void Initialized(object state)
        {
            base.Initialized(state);

            string configFile;
            if (!String.IsNullOrEmpty(configFile = Terraria.Initializers.LaunchInitializer.TryParameter("-config")))
                Config.LoadFromFile(configFile);

            Config.LoadFromArguments();

            Instance = new SlackClient(Config.WebHookUrl);
        }

        protected override void Enabled()
        {
            base.Enabled();
            //instance.SendMessage($"Initialised", _userName, _channel, _iconEmoji);
        }

        [Hook(HookOrder.NORMAL)]
        void OnPlayerJoined(ref HookContext ctx, ref HookArgs.PlayerEnteredGame args)
        {
            Instance.SendMessage($"User: {ctx.Player.name} has joined", Config.UserName, Config.Channel, Config.IconEmoji);
        }

        [Hook(HookOrder.NORMAL)]
        void OnPlayerLeave(ref HookContext ctx, ref HookArgs.PlayerLeftGame args)
        {
            Instance.SendMessage($"User: {ctx.Player.name} has left", Config.UserName, Config.Channel, Config.IconEmoji);
        }

        [Hook(HookOrder.NORMAL)]
        void OnPlayerChat(ref HookContext ctx, ref OTA.Commands.Events.CommandArgs.Chat args)
        {
            Instance.SendMessage($"{ctx.Player.name}> {args.Message}", Config.UserName, Config.Channel, Config.IconEmoji);
        }
    }

    [AllowAnonymous]
    public class SlackController : ApiController
    {
        public static readonly LogChannel Log = new LogChannel("Slack", ConsoleColor.Yellow, System.Diagnostics.TraceLevel.Info);

        /*
            https://api.slack.com/outgoing-webhooks
            token=XXXXXXXXXXXXXXXXXX
            team_id=T0001
            team_domain=example
            channel_id=C2147483705
            channel_name=test
            timestamp=1355517523.000005
            user_id=U2147483697
            user_name=Steve
            text=googlebot: What is the air-speed velocity of an unladen swallow?
            trigger_word=googlebot:
        */
        [HttpPost]
        public IHttpActionResult Post(SlackMessage obj)
        {
            int count = 0;
            if (obj != null && obj.token == SlackPlugin.Config.WebHookToken)
            {
                if (!String.IsNullOrEmpty(obj.text))
                {
                    if (!String.IsNullOrEmpty(obj.trigger_word))
                    {
                        var ix = obj.text.IndexOf(obj.trigger_word);
                        if (ix > -1)
                        {
                            obj.text = obj.text.Remove(0, ix + obj.trigger_word.Length).Trim();
                        }
                    }
                    count = OTA.Tools.NotifyAllPlayers(SlackPlugin.Config.ChatPrefix.Replace("{username}", obj.user_name) + obj.text, Microsoft.Xna.Framework.Color.Orange, false);
                    Log.Log(obj.text);
                }
            }

            var suffix = count != 1 ? "s" : String.Empty;
            return Ok(new
            {
                text = $"Sent to {count} player{suffix}"
            });
        }
    }

    public class SlackMessage
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
    }
}
