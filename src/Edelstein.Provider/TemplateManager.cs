using System.Collections.Generic;
using PKG1;

namespace Edelstein.Provider
{
    public abstract class TemplateManager<T>
    {
        protected readonly PackageCollection Collection;
        private IDictionary<int, T> _templates;

        protected TemplateManager(PackageCollection collection)
        {
            Collection = collection;
            _templates = new Dictionary<int, T>();
        }

        public T Get(int templateId)
        {
            lock (this)
            {
                if (!_templates.ContainsKey(templateId))
                    _templates[templateId] = Load(templateId);
                return _templates[templateId];
            }
        }

        public abstract T Load(int templateId);
    }
}