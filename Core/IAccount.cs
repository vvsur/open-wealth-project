using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, реализующий сущность "Счет"
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Идентификатор счета
        /// </summary>
        string Name { get; }

        float Ostatok{ get; } // TODO перевести на английский
        float Limit{ get; }      // сумма, на которую могу открывать позиции
        float FreeLimit{ get; }  // сумма, на которую ещё могу открывать позиции
    }
}
