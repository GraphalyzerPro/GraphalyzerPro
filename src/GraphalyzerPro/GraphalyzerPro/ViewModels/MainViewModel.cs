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
using System.Windows;
using GraphalyzerPro.Common;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Views;

namespace GraphalyzerPro.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
        private readonly InformationEngine _informationEngine;
        private readonly ReactiveCollection<ISessionViewModel> _sessionViewModels;
        private readonly Action _showCloseSessionErrorMessage;
        private readonly Action _showInitializationErrorMessage;

        public MainViewModel()
        {
            _informationEngine = new InformationEngine();
            _sessionViewModels = new ReactiveCollection<ISessionViewModel>();

            _showInitializationErrorMessage = () => MessageBox.Show(
                "Bei der Initialisierung des Receivers ist ein Fehler aufgetreten.\n" +
                "Bitte schließen Sie die Session und starten den Receiver neu.",
                "Fehler");

            _showCloseSessionErrorMessage = () => MessageBox.Show(
                "Beim Schließen der Session trat ein Fehler auf.",
                "Fehler");

            ActivateReceiverCommand = new ReactiveCommand();
            ActivateReceiverCommand.Subscribe(_ => ActivateReceiver());

            InitializeReceiverCommand = new ReactiveAsyncCommand();
            InitializeReceiverCommand.RegisterAsyncAction(x => InitializeReceiver((IReceiver) x));
            InitializeReceiverCommand.ThrownExceptions.Subscribe(
                ex => _showInitializationErrorMessage());

            CloseSessionCommand = new ReactiveCommand();
            CloseSessionCommand.Subscribe(x => CloseSession((ISessionViewModel) x));
            CloseSessionCommand.ThrownExceptions.Subscribe(ex => _showCloseSessionErrorMessage());
        }

        public string Title
        {
            get { return "GraphalyzerPro"; }
        }

        public IReactiveCommand ActivateReceiverCommand { get; private set; }

        public IReactiveCommand CloseSessionCommand { get; private set; }

        public IReactiveAsyncCommand InitializeReceiverCommand { get; private set; }

        public ReactiveCollection<ISessionViewModel> SessionViewModels
        {
            get { return _sessionViewModels; }
            set { this.RaiseAndSetIfChanged(value); }
        }

        private void ActivateReceiver()
        {
            var dialog = new ReceiverActivationDialog();

            if (dialog.ShowDialog() == true)
            {
                var sessionViewModel = new SessionViewModel(dialog.ViewModel.SelectedReceiver);
                SessionViewModels.Add(sessionViewModel);

                InitializeReceiverCommand.Execute(sessionViewModel.Receiver);
            }
        }

        private void InitializeReceiver(IReceiver receiver)
        {
            receiver.Initialize(_informationEngine);
        }

        private void CloseSession(ISessionViewModel sessionViewModel)
        {
            sessionViewModel.Receiver.Deactivate();
            SessionViewModels.Remove(sessionViewModel);
        }
    }
}