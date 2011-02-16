
namespace OpenWealth
{
    /// <summary>
    /// Плагины и т.п. реализующие данный интерфейс могут заявить о себе большую информацию
    /// Предполагается, что гдето в интерфейсе это будет выводиться
    /// </summary>
    public interface IDescription
    {
        string Name { get; }
        string Description { get; }
        string URL { get; }
    }
}
