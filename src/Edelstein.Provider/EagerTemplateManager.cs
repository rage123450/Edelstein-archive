using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider
{
    public abstract class EagerTemplateManager<T> : ITemplateManager<T>
    {
        protected readonly PackageCollection Collection;
        protected readonly IDictionary<int, T> Templates;

        public IReadOnlyDictionary<int, T> All =>
            new ReadOnlyDictionary<int, T>(Templates);

        protected EagerTemplateManager(PackageCollection collection)
        {
            Collection = collection;
            Templates = new Dictionary<int, T>();
        }

        public T Get(int templateId)
        {
            lock (this)
            {
                return Templates[templateId];
            }
        }

        public abstract Task LoadAll();
    }
}