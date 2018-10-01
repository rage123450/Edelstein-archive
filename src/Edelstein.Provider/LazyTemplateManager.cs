using System.Collections.Generic;
using System.Collections.ObjectModel;
using PKG1;

namespace Edelstein.Provider
{
    public abstract class LazyTemplateManager<T>: ITemplateManager<T>
    {
        protected readonly PackageCollection Collection;
        private IDictionary<int, T> _templates;

        protected LazyTemplateManager(PackageCollection collection)
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