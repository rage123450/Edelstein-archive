using System;
using System.Threading.Tasks;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Logging;
using MoonSharp.Interpreter;

namespace Edelstein.WvsGame.Conversations
{
    public class ScriptedConversation<T, S> : Conversation<T, S>
        where T : Speaker
        where S : Speaker
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly string _scriptPath;

        public ScriptedConversation(string scriptPath)
        {
            _scriptPath = scriptPath;
        }

        public override Task Start(ConversationContext context, T target, S self)
        {
            var script = new Script();

            script.Globals["target"] = UserData.Create(target);
            script.Globals["self"] = UserData.Create(self);

            return Task.Run(() =>
                {
                    try
                    {
                        script.DoFile(_scriptPath);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.ToString);
                    }
                },
                context.TokenSource.Token);
        }
    }
}