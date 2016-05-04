using SlackPlugin.Messages;
using System;
using System.Linq;
using System.Web.Http;

namespace SlackPlugin.Controllers
{
    public partial class SlashController : ApiController
    {
        [HttpPost]
        [Route("api/slack/slash")]
        public IHttpActionResult Command(SlashCommand obj)
        {
            if (obj != null && obj.IsValid())
            {
                SlackPlugin.Log.Log($"Slack user {obj.user_name} issued: {obj.text}");
                var sender = new SlackSender(obj.user_name);

                try
                {
                    //Check slack user
                    if (!String.IsNullOrWhiteSpace(SlackPlugin.Config.ExecAccessName) && SlackPlugin.Config.ExecAccessName.Split(',').Contains(obj.user_name))
                    {
                        if (!OTA.Commands.CommandManager.Parser.ParseAndProcess(sender, obj.text))
                        {
                            return Ok(new
                            {
                                text = "Invalid command."
                            });
                        }
                        else return Ok(new
                        {
                            text = "Command executed."
                        });
                    }
                    else return Ok(new
                    {
                        text = "Request denied."
                    });
                }
                catch (OTA.Misc.ExitException)
                {
                }
                catch (Exception e)
                {
                    SlackPlugin.Log.Log(e, "Slack command failed: " + obj.text);
                    return Ok(new
                    {
                        text = $"Exception from {obj.text}: {e.ToString()}"
                    });
                }
            }

            return Ok(new
            {
                text = "Nothing happened: " + obj.ToString()
            });
        }
    }
}
