using System;
using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class SuperSpeedCommand : Command
    {
        public override string Name => "SuperSpeed";
        public override string Description => "Be like flash and go to another timeline";

        protected override Task Execute(CommandContext ctx)
        {
            return ctx.Args.Count > 0
                ? ctx.User.ResetForcedStats()
                : ctx.User.ModifyForcedStats(s =>
                {
                    s.Jump = byte.MaxValue;
                    s.Speed = byte.MaxValue;
                    s.SpeedMax = byte.MaxValue;
                });
        }
    }
}