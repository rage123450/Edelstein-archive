using System.Collections.Generic;
using Edelstein.Provider;
using Edelstein.Provider.Fields;

namespace Edelstein.WvsGame.Fields
{
    public class FieldFactory
    {
        private readonly ITemplateManager<FieldTemplate> _templateManager;
        private readonly IDictionary<int, Field> _fields;

        public FieldFactory(ITemplateManager<FieldTemplate> templateManager)
        {
            _templateManager = templateManager;
            _fields = new Dictionary<int, Field>();
        }

        public Field Get(int templateId)
        {
            if (_fields.ContainsKey(templateId)) return _fields[templateId];
            var field = new Field(templateId, _templateManager.Get(templateId));
            _fields[templateId] = field;
            return field;
        }
    }
}