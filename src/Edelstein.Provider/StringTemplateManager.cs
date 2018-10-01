using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PKG1;

namespace Edelstein.Provider
{
    public abstract class StringTemplateManager
    {
        protected readonly PackageCollection Collection;
        protected readonly IDictionary<int, string> Templates;

        public IReadOnlyDictionary<int, string> All =>
            new ReadOnlyDictionary<int, string>(Templates);

        protected StringTemplateManager(PackageCollection collection)
        {
            Collection = collection;
            Templates = new Dictionary<int, string>();
        }

        public string Get(int templateId)
        {
            lock (this)
            {
                return Templates[templateId];
            }
        }

        public abstract Task LoadAll();
    }
}