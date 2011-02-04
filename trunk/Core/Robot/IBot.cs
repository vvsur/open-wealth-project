namespace OpenWealth
{
    /// <summary>
    /// Данный интерфейс должен наследоваться всеми роботами системы
    /// </summary>
    public interface IBot : IDescription
    {
        // Вызывается один раз, сразу после конструктора
        void Init(IBotHost BotHost);
        // Start и Stop может вызываться многократно
        // в принципе поменяться между вызовами Stop и Start может всё что угодно, например параметры, очиститься сделки, позиции, заявки, уменьшиться время
        void Start();
        void Stop();
    }
}
