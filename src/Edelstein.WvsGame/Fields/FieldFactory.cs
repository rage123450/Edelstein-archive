using System;
using System.Collections.Generic;
using System.Drawing;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.Provider.NPC;
using Edelstein.WvsGame.Fields.Objects;
using MoreLinq.Extensions;

namespace Edelstein.WvsGame.Fields
{
    public class FieldFactory
    {
        private readonly ITemplateManager<FieldTemplate> _fieldTemplateManager;
        private readonly ITemplateManager<NPCTemplate> _npcTemplateManager;
        private readonly IDictionary<int, Field> _fields;

        public FieldFactory(
            ITemplateManager<FieldTemplate> fieldTemplateManager,
            ITemplateManager<NPCTemplate> npcTemplateManager
        )
        {
            _fieldTemplateManager = fieldTemplateManager;
            _npcTemplateManager = npcTemplateManager;
            _fields = new Dictionary<int, Field>();
        }

        public Field Get(int templateId)
        {
            lock (this)
            {
                if (_fields.ContainsKey(templateId)) return _fields[templateId];
                var field = new Field(templateId, _fieldTemplateManager.Get(templateId));
                _fields[templateId] = field;

                field.Template.Life.ForEach(l =>
                {
                    switch (l.Type)
                    {
                        case FieldLifeType.NPC:
                            var npcTemplate = _npcTemplateManager.Get(l.TemplateID);

                            field.Enter(new FieldNPC(npcTemplate)
                            {
                                X = (short) l.X,
                                Y = (short) l.Y,
                                MoveAction = l.F,
                                Foothold = (short) l.FH,
                                RX0 = l.RX0,
                                RX1 = l.RX1
                            });
                            break;
                        case FieldLifeType.Monster:
                            break;
                    }
                });
                return field;
            }
        }
    }
}