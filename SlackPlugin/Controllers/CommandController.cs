using SlackPlugin.Messages;
using System;
using System.Linq;
using System.Web.Http;

namespace SlackPlugin.Controllers
{
    public partial class SlackController : ApiController
    {
        [HttpPost]
        [Route("api/slack/command")]
        public IHttpActionResult Command(TriggerWord obj) => ProcessCommand(obj);

        [HttpPost]
        [Route("api/slack/slash")]
        public IHttpActionResult Slash(TriggerWord obj) => ProcessCommand(obj);

        public IHttpActionResult ProcessCommand(IncomingMessage obj)
        {
            if (obj != null && obj.IsValid())
            {
                SlackPlugin.Log.Log($"Slack user {obj.user_name} issued: {obj.MessageText}");
                using (var sender = new SlackSender(obj.user_name))
                {
                    try
                    {
                        //Check slack user
                        if (!String.IsNullOrWhiteSpace(SlackPlugin.Config.ExecAccessName) && SlackPlugin.Config.ExecAccessName.Split(',').Contains(obj.user_name))
                        {
                            if (!OTA.Commands.CommandManager.Parser.ParseAndProcess(sender, obj.MessageText))
                            {
                                return Ok(new
                                {
                                    text = sender.Text + "\nInvalid command."
                                });
                            }
                            else return Ok(new
                            {
                                text = sender.Text + "\nCommand executed."
                            });
                        }
                        else return Ok(new
                        {
                            text = sender.Text + "\nRequest denied."
                        });
                    }
                    catch (OTA.Misc.ExitException)
                    {
                    }
                    catch (Exception e)
                    {
                        SlackPlugin.Log.Log(e, "Slack command failed: " + obj.MessageText);
                        return Ok(new
                        {
                            text = sender.Text + $"\nException from {obj.MessageText}: {e.ToString()}"
                        });
                    }
                }
            }

            return Ok(new
            {
                text = "Nothing happened: " + obj.ToString()
            });
        }
    }
}
