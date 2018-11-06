using System;
using System.Threading.Tasks;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Types;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class StatCommand : Command
    {
        public override string Name => "Stat";
        public override string Description => "Sets a specified stat to a specified value.";

        public StatCommand()
        {
            Aliases.Add("set");
        }

        protected override Task Execute(CommandContext ctx)
        {
            try
            {
                var stat = Enum.Parse(typeof(ModifyStatType), ctx.Args.Dequeue(), true);
                var value = ctx.Args.Dequeue();

                ctx.User.ModifyStats(s =>
                {
                    switch (stat)
                    {
                        case ModifyStatType.Skin:
                            s.Skin = Convert.ToByte(value);
                            break;
                        case ModifyStatType.Face:
                            s.Face = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.Hair:
                            s.Hair = Convert.ToInt32(value);
                            break;
                        default:
                        case ModifyStatType.Pet:
                        case ModifyStatType.Pet2:
                        case ModifyStatType.Pet3:
                            // TODO: do something
                            break;
                        case ModifyStatType.Level:
                            s.Level = Convert.ToByte(value);
                            break;
                        case ModifyStatType.Job:
                            s.Job = (Job) Convert.ToInt16(value);
                            break;
                        case ModifyStatType.STR:
                            s.STR = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.DEX:
                            s.DEX = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.INT:
                            s.INT = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.LUK:
                            s.LUK = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.HP:
                            s.HP = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.MaxHP:
                            s.MaxHP = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.MP:
                            s.MP = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.MaxMP:
                            s.MaxMP = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.AP:
                            s.AP = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.SP:
                            s.SP = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.EXP:
                            s.EXP = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.POP:
                            s.POP = Convert.ToInt16(value);
                            break;
                        case ModifyStatType.Money:
                            s.Money = Convert.ToInt32(value);
                            break;
                        case ModifyStatType.TempEXP:
                            s.TempEXP = Convert.ToInt32(value);
                            break;
                    }

                    ctx.User.Message($"Successfully set {stat} to {value}.");
                });
            }
            catch (ArgumentException)
            {
                // TODO: do something
            }

            return Task.CompletedTask;
        }
    }
}