using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ValidateStatCommand : Command
    {
        public override string Name => "Validate";
        public override string Description => "Prints the current basic or secondary stat";

        protected override Task Execute(CommandContext ctx)
        {
            if (ctx.Args.Count > 0)
            {
                var ss = ctx.User.SecondaryStat;

                ctx.User.Message($"PAD = {ss.PAD}, " +
                                 $"PDD = {ss.PDD}, " +
                                 $"MAD = {ss.MAD}, " +
                                 $"MDD = {ss.MDD}, " +
                                 $"ACC = {ss.ACC}, " +
                                 $"EVA = {ss.EVA}, " +
                                 $"Craft = {ss.Craft}, " +
                                 $"Speed = {ss.Speed}, " +
                                 $"Jump = {ss.Jump}");
                ctx.User.Message($"PAD% = {ss.Option.PADr}%, " +
                                 $"PDD% = {ss.Option.PDDr}%, " +
                                 $"MAD% = {ss.Option.MADr}%, " +
                                 $"MDD% = {ss.Option.MDDr}%, " +
                                 $"ACC% = {ss.Option.ACCr}%, " +
                                 $"EVA% = {ss.Option.EVAr}%");
            }
            else
            {
                var bs = ctx.User.BasicStat;
                ctx.User.Message($"STR = {bs.STR}, " +
                                 $"DEX = {bs.DEX}, " +
                                 $"INT = {bs.INT}, " +
                                 $"LUK = {bs.LUK}, " +
                                 $"MaxHP = {bs.MaxHP}, " +
                                 $"MaxMP = {bs.MaxMP}");
                ctx.User.Message($"STR% = {bs.Option.STRr}%, " +
                                 $"DEX% = {bs.Option.DEXr}%, " +
                                 $"INT% = {bs.Option.INTr}%, " +
                                 $"LUK% = {bs.Option.LUKr}%, " +
                                 $"MaxHP% = {bs.Option.MaxHPr}%, " +
                                 $"MaxMP% = {bs.Option.MaxMPr}%");
            }

            return Task.CompletedTask;
        }
    }
}