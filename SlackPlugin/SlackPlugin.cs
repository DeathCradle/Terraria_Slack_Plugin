using OTA.Logging;
using OTA.Plugin;
using System;

[assembly: PluginDependency("OTA.Commands")]
[assembly: PluginDependency("TDSM.Core")]

namespace SlackPlugin
{
    [OTAVersion(1, 0)]
    public class SlackPlugin : BasePlugin
    {
        public const String PluginName = "Slack Notification Plugin";

        public static SlackConfig Config { get; private set; } = new SlackConfig();
        public static SlackClient Instance { get; private set; }

        public static readonly LogChannel Log = new LogChannel("Slack", ConsoleColor.Yellow, System.Diagnostics.TraceLevel.Info);

        public SlackPlugin()
        {
            Version = "1";
            Author = "swrhim, DeathCradle";
            Name = PluginName;
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

        [Hook(HookOrder.NORMAL)]
        void OnCommandIssued(ref HookContext ctx, ref OTA.Commands.Events.CommandArgs.CommandIssued args)
        {
            if (ctx.Sender is SlackSender) ctx.SetResult(HookResult.CONTINUE); //Same permissions as ConsoleSender
        }
    }
}
