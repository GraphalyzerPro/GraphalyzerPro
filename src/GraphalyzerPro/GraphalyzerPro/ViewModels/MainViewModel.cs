/*
 * Copyright (c) 2006-2009 by Christoph Menzel, Daniel Birkmaier, 
 * Carl-Clemens Ebinger, Maximilian Madeja, Farruch Kouliev, Stefan Zoettlein
 *
 * This file is part of the GraphalyzerPro rating application.
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

using ReactiveUI;
using System.Windows.Input;
using System.Windows;

namespace GraphalyzerPro.ViewModels
{
    public class MainViewModel : ReactiveObject, IMainViewModel
    {
		public string Title
		{
			get
			{
				return "GraphalyzerPro";
			}
		}

	    public MainViewModel()
	    {
		    if(((App)(Application.Current)).ReceiverTypes.Length>0)
		    {
				ReceiverActivationDialog dialog=new ReceiverActivationDialog();
			    dialog.DataContext=new ReceiverActivationDialogViewModel();
			    bool? result;
			    if(((result=dialog.ShowDialog()).HasValue)&&(result.Value))
			    {

			    }
			    else
			    {
				    Application.Current.Shutdown();
			    }
		    }
		    else
		    {
				MessageBoxShow("Leider konnte kein einziger Empfänger geladen werden. Deshalb wird GraphalyzerPro beendet.", "Keine Empfänger geladen", MessageBoxButton.OK, MessageBoxImage.Error);
		    }
	    }

		/// <summary>
		/// Shows a <see cref="MessageBox"/>-object and can be used for mocking
		/// </summary>
		/// <param name="messageBox">The text of the <see cref="MessageBox"/></param>
		/// <param name="caption">The caption of the <see cref="MessageBox"/></param>
		/// <param name="button">The <see cref="MessageBoxButton"/> of the <see cref="MessageBox"/></param>
		/// <param name="icon">The <see cref="MessageBoxImage"/> of the <see cref="MessageBox"/></param>
		internal void MessageBoxShow(string messageBox, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			MessageBox.Show(messageBox, caption, button, icon);
		}


    }
}
