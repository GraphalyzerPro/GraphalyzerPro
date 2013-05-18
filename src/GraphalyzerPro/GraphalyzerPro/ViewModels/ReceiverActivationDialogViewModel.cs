/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Configuration;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace GraphalyzerPro.ViewModels
{
    public class ReceiverActivationDialogViewModel : ReactiveObject, IReceiverActivationDialogViewModel
    {
        private ReactiveCollection<IReceiver> _allReceiver;
        private IReceiver _selectedReceiver;

        public ReceiverActivationDialogViewModel()
        {
            ApplyCommand =
                new ReactiveCommand(this.ObservableForProperty(x => x.SelectedReceiver, x => x != null).StartWith(false));

            AllReceiver = new ReactiveCollection<IReceiver>();

            CreateInstanceOfReceiverImplementations(
                ConfigurationAccessor.GetAllAssemblyFileNamesBySectionName(ConfigurationAccessor.ReceiversSectionName));
        }

        public IReactiveCommand ApplyCommand { get; private set; }

        public ReactiveCollection<IReceiver> AllReceiver
        {
            get { return _allReceiver; }
            private set { this.RaiseAndSetIfChanged(value); }
        }

        public IReceiver SelectedReceiver
        {
            get { return _selectedReceiver; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        private void CreateInstanceOfReceiverImplementations(IEnumerable<string> allReceiverAssemblyFileNames)
        {
            foreach (var receiverAssembly in allReceiverAssemblyFileNames)
            {
                var assembly = Assembly.LoadFrom(receiverAssembly);

                var allIReceiverTypesOfTheAssembly =
                    assembly.GetTypes().Where(x => typeof (IReceiver).IsAssignableFrom(x)).ToList();

                foreach (var type in allIReceiverTypesOfTheAssembly)
                {
                    AllReceiver.Add(Activator.CreateInstance(type) as IReceiver);
                }
            }
        }
    }
}