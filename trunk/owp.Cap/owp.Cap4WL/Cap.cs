using System;
using System.Collections.Generic;
using System.Text;

namespace owp
{
    public abstract class Cap : WealthLab.WealthScript
    {
        protected int firstValidBar = -1;
        protected int bar = -1;
        protected override void Execute()
        {
            Init();
            if (firstValidBar == -1) { PrintDebug("Значение firstValidBar не определено"); }
            else
                for (bar = firstValidBar; bar < Bars.Count; bar++)
                {
                    Exec();
                }
        }
        public abstract void Init(); // выполняется перед циклом в Execute (в бою будет вызываться при старте робота)
        public abstract void Exec(); // выполняется на каждом шаге цикла в Execute (в бою будет вызываться на новый bar)
    }
}
