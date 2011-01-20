using System;
using System.Windows.Forms;

namespace OpenWealth
{
    /// <summary>
    /// Доступ к главной форме. Главная форма является MDI контейнером.
    /// Получить класс, реализующий данный интерфейс можно используя Core.GetGlobal("Interface", null)
    /// Во время выполнения IPlugin.Init уже должен быть доступен, но вполне может вернуться и null
    /// </summary>
    public interface IInterface
    {
        /// <summary>
        /// Возвращает объект главной MDI формы, предпологается, что будет использоваться так this.MdiParent = IInterface.GetMainForm();
        /// чем позже будите вызывать, тем лучше, т.к., напримем, в WealthLab, главная форма на этапе вызовов IPlugin.Init ещё не известна
        /// </summary>
        Form GetMainForm();
        /// <summary>
        /// Добавить пункт в меню главной формы
        /// </summary>
        /// <param name="textLevel1">Текст первого уровеня, если остальные тексты string.Empty, то на него ставится callback, иначе данный элемент содержит подменю</param>
        /// <param name="textLevel2">Текст, выводимый в подменю textLevel1, если textLevel3!=string.Empty, то является подменю, иначе элементом меню</param>
        /// <param name="textLevel3">Конечный элемент меню, дальнейшая вложенность не возможна</param>
        /// <param name="callback">Что вызывать, когда юзер кликнит данный пункт</param>
        void AddMenuItem(string textLevel1, string textLevel2, string textLevel3, EventHandler callback);
    }
}