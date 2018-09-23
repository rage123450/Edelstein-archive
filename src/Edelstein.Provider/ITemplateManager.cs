namespace Edelstein.Provider
{
    public interface ITemplateManager<out T>
    {
        T Get(int templateId);
    }
}