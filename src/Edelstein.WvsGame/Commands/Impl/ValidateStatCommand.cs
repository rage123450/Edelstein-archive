using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ValidateStatCommand : Command
    {
        public override string Name => "Validate";
        public override string Description => "Prints the current basic or secondary stat";

        protected override Task Execute(CommandContext ctx)
        {
            var bs = ctx.User.BasicStat;

            ctx.User.Message($"STR = {bs.STR} " +
                             $"DEX = {bs.DEX} " +
                             $"INT = {bs.INT} " +
                             $"LUK = {bs.LUK} " +
                             $"MaxHP = {bs.MaxHP} " +
                             $"MaxMP = {bs.MaxHP}");
            return Task.CompletedTask;
        }
    }
}