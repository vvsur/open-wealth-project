using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, реализующий сущность "Счет"
    /// </summary>
    interface IAccount
    {
        /// <summary>
        /// Идентификатор счета
        /// </summary>
        string Name { get; }
    }
}
