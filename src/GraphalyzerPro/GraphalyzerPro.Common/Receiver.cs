/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Carl-Clemens Ebinger, Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
 *
 * This file is part of the GraphalyzerPro application.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public
 * License along with this program. If not, see
 * <http://www.gnu.org/licenses/>.
 */

using GraphalyzerPro.Common.Interfaces;

namespace GraphalyzerPro.Common
{
	/// <summary>
	/// Provides a template for all <see cref="IReceiver"/> implementations.
	/// </summary>
	public abstract class Receiver : IReceiver
	{
		/// <summary>
		/// The name.
		/// </summary>
		private string _name;
		/// <summary>
		/// The <see cref="IInformationEngine"/>.
		/// </summary>
		private IInformationEngine _informationEngine;

		public Receiver(string name)
		{
			_name=name;
			_informationEngine=null;
		}

		/// <summary>
		/// The name of the <see cref="IReceiver"/>.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		/// <summary>
		/// The <see cref="IInformationEngine"/> transferred by the <see cref="Initialize"/>-method.
		/// </summary>
		public IInformationEngine InformationEngine
		{
			get
			{
				return _informationEngine;
			}
		}

		/// <summary>
		/// Initializes the <see cref="IReceiver"/>.
		/// </summary>
		/// <param name="informationEngine">
		/// The <see cref="IInformationEngine"/> which handels the requests of the <see cref="IReceiver"/>.
		/// </param>
		public virtual void Initialize(IInformationEngine informationEngine)
		{
			_informationEngine=informationEngine;
		}

		/// <summary>
		/// The <see cref="IReceiver"/> cleanup everything to be destructed without errors.
		/// </summary>
		public virtual void Deactivate()
		{
		}
	}
}

