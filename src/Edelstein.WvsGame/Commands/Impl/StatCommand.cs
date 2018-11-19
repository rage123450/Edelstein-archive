using System;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Common.Packets.Stats;
using Edelstein.Database.Entities.Types;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class StatCommand : Command<StatCommandOption>
    {
        public override string Name => "Stat";
        public override string Description => "Sets a stat to a specified value.";

        public StatCommand()
        {
            Aliases.Add("Set");
        }

        public override async Task Execute(FieldUser user, StatCommandOption option)
        {
            await user.ModifyStats(s =>
            {
                switch (option.Type)
                {
                    case ModifyStatType.Skin:
                        s.Skin = Convert.ToByte(option.Value);
                        break;
                    case ModifyStatType.Face:
                        s.Face = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.Hair:
                        s.Hair = Convert.ToInt32(option.Value);
                        break;
                    default:
                    case ModifyStatType.Pet:
                    case ModifyStatType.Pet2:
                    case ModifyStatType.Pet3:
                        user.Message($"Failed to set {option.Type} as {option.Value}.");
                        break;
                    case ModifyStatType.Level:
                        s.Level = Convert.ToByte(option.Value);
                        break;
                    case ModifyStatType.Job:
                        s.Job = (Job) Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.STR:
                        s.STR = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.DEX:
                        s.DEX = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.INT:
                        s.INT = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.LUK:
                        s.LUK = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.HP:
                        s.HP = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.MaxHP:
                        s.MaxHP = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.MP:
                        s.MP = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.MaxMP:
                        s.MaxMP = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.AP:
                        s.AP = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.SP:
                        s.SP = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.EXP:
                        s.EXP = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.POP:
                        s.POP = Convert.ToInt16(option.Value);
                        break;
                    case ModifyStatType.Money:
                        s.Money = Convert.ToInt32(option.Value);
                        break;
                    case ModifyStatType.TempEXP:
                        s.TempEXP = Convert.ToInt32(option.Value);
                        break;
                }
            });

            await user.Message($"Successfully set {option.Type} to {option.Value}.");
        }
    }

    public class StatCommandOption : CommandOption
    {
        [Value(0, MetaName = "type", Required = true, HelpText = "The stat type.")]
        public ModifyStatType Type { get; set; }

        [Value(1, MetaName = "value", Required = true, HelpText = "The stat value.")]
        public int Value { get; set; }
    }
}