// шаблон робота для WL, использующего owp.Cap
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using WealthLab;
using WealthLab.Indicators;

namespace WealthLab.Strategies
{
	public class MyStrategy : owp.Cap
	{
		/// <summary>
		/// Вызывается один раз, перед циклом Execute
		/// в бою вызывается один раз, перед запуском робота
		/// </summary>
		/// <returns></returns>
		public override void Init()
		{
			// указываем, с какого бара начинать расчет
			this.firstValidBar = 1;
		}
		/// <summary>
		/// Вызывается на каждом шаге цикла Execute
		/// в бою будет вызываться для каждого нового бара
		/// </summary>
		/// <returns></returns>
		public override void Exec()
		{
			if (IsLastPositionActive)
			{
				//Код выхода из позиции
			}
			else
			{
				//Код входа в позицию
			}			
		}
	}
}