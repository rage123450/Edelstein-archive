using Edelstein.Provider;
using Edelstein.Provider.Fields;

namespace Edelstein.WvsGame.Fields
{
    public class FieldFactory
    {
        private readonly ITemplateManager<FieldTemplate> _templateManager;

        public FieldFactory(ITemplateManager<FieldTemplate> templateManager)
        {
            _templateManager = templateManager;
        }

        public Field Get(int templateId)
        {
            return new Field(_templateManager.Get(templateId));
        }
    }
}