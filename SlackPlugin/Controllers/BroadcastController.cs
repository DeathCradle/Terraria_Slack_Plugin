using Microsoft.Xna.Framework;
using OTA;
using SlackPlugin.Messages;
using System;
using System.Web.Http;

namespace SlackPlugin.Controllers
{
    [AllowAnonymous]
    public partial class SlackController : ApiController
    {
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
        [Route("api/slack/broadcast")]
        public IHttpActionResult Broadcast(TriggerWord obj)
        {
            if (obj != null && obj.IsValid())
            {
                var count = Tools.NotifyAllPlayers(SlackPlugin.Config.ChatPrefix.Replace("{username}", obj.user_name) + obj.MessageText, Color.Orange, false);
                SlackPlugin.Log.Log(obj.MessageText);


                var suffix = count != 1 ? "s" : String.Empty;
                return Ok(new
                {
                    text = $"Sent to {count} player{suffix}"
                });
            }

            return Ok(new
            {
                text = "Model not valid: " + obj.ToString()
            });
        }
    }
}
