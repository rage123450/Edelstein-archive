using System;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users.Stats;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class BuffCommand : Command
    {
        public override string Name => "Buff";
        public override string Description => "Tests temporary stats.";

        protected override Task Execute(CommandContext ctx)
        {
            ctx.User.ModifyTemporaryStat(s =>
            {
                var date = DateTime.Now;
                s.Set(TemporaryStatType.BlessingArmor, 30001000, 10, date.AddSeconds(30));
                /*s.Set(TemporaryStatType.PDD, 30001000, 10, date.AddSeconds(30));
                s.Set(TemporaryStatType.MAD, 30001000, 10, date.AddSeconds(30));
                s.Set(TemporaryStatType.MDD, 30001000, 10, date.AddSeconds(30));
                s.Set(TemporaryStatType.Speed, 30001000, 100, date.AddSeconds(30));
                s.Set(TemporaryStatType.Jump, 30001000, 100, date.AddSeconds(30));*/
            });
            ctx.User.Message("Successfully set temporary stats.");
            return Task.CompletedTask;
        }
    }
}