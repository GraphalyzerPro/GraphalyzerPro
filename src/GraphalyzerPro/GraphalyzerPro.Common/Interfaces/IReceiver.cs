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

namespace GraphalyzerPro.Common.Interfaces
{
    /// <summary>
    /// Provides a common interface for all receiver implementations.
    /// </summary>
    public interface IReceiver
    {
        /// <summary>
        /// The name of the <see cref="IReceiver"/>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initializes the <see cref="IReceiver"/>
        /// </summary>
        /// <param name="informationEngine">
        /// The <see cref="IInformationEngine"/> which handels the requests of the <see cref="IReceiver"/>.
        /// </param>
        void Initialize(IInformationEngine informationEngine);

        /// <summary>
        /// The <see cref="IReceiver"/> cleanup everything to be destructed without errors.
        /// </summary>
        void Deactivate();
    }
}
