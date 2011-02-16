using System;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий провайдера данных
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Хранит ли провайдер исторические данные. Если да, то должен реализовывать метод GetData
        /// </summary>
        bool isHistoryProvider { get; }
        /// <summary>
        /// Поставляет ли провайдер реалтайм данные. Если да, то для новых данных должен вызывать IData.Add, может не реализовы метод GetData (т.е. всегда возвращать из GetData результат false)
        /// </summary>
        bool isRealTimeProvider { get; }

        /// <summary>
        /// Запрос данных. Если провайдер содержит требуемые данные, то он должен вызвать IData.Add, для добавления их в модуль Данные
        /// </summary>
        /// <param name="symbol">Инструмент, для которых запрашиваются данные</param>
        /// <param name="scale">scale необходимых данных</param>
        /// <param name="startDate">С какой даты/времени нужны данные</param>
        /// <param name="endDate">До какой даты/времени нужны данные</param>
        /// <param name="maxBars">Максимально к-во баров. 0 - если не ограниченно</param>
        /// <param name="includePartialBar">Требуется ли передавать неполные бары</param>
        /// <returns>Если выполнил запрос, должен вернуть true, иначе false</returns>
        bool GetData(ISymbol symbol, IScale scale, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar);
    }
}
