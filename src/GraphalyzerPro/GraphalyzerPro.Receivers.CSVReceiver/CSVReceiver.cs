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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using GraphalyzerPro.Common.Interfaces;
using GraphalyzerPro.Common;
using System.Windows;
using Microsoft.Win32;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GraphalyzerPro.Tests")]

namespace GraphalyzerPro.Receivers
{
	/// <summary>
	/// Provides a common interface for all receiver implementations.
	/// </summary>
	public class CSVReceiver : Receiver
	{
		/// <summary>
		/// The header of a CSV-file in the correct format.
		/// </summary>
		internal const string TABLE_HEADER="Timestamp;Gap;Duration;PID;Thread;Type;Domain;Application;Component;Module;Code;Text;Meta-Informationen";
		/// <summary>
		/// Column seperators of a CSV-file in the correct format.
		/// </summary>
		internal static readonly char[] SEPARATORS=new char[] { ';' };
		/// <summary>
		/// Culture containing the datetime format of a CSV-file in the correct format.
		/// </summary>
		internal static readonly CultureInfo DATETIME_FORMAT_CULTURE=CultureInfo.CreateSpecificCulture("de");

		/// <summary>
		/// <see cref="CSVReceiver"/> constructor.
		/// </summary>
		/// <param name="name">The name of the <see cref="CSVReceiver"/>.</param>
		public CSVReceiver(string name) : base(name)
		{
		}

		/// <summary>
		/// Reads a line from the passed <see cref="StreamReader"/> and returns the corresponding <see cref="IDiagnoseOutputEntry"/>.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <returns>The created IDiagnoseOutputEntry.</returns>
		/// <exception cref="FormatException">The file format is incorrect.</exception>
		internal IDiagnoseOutputEntry ReadLine(StreamReader reader)
		{
			IDiagnoseOutputEntry result;
			string[] cols=reader.ReadLine().Split(SEPARATORS);
			if(cols.Length==13)
			{
				long gap;
				long duration;
				int processId;
				int threadNumber;
				try
				{
					gap=long.Parse(cols[1], NumberStyles.None);
					duration=long.Parse(cols[2], NumberStyles.None);
					processId=int.Parse(cols[3], NumberStyles.None);
					threadNumber=int.Parse(cols[4], NumberStyles.None);
				}
				catch
				{
					throw new FormatException();
				}
				result=(IDiagnoseOutputEntry)(new DiagnoseOutputEntry(
					DateTime.Parse(cols[0], DATETIME_FORMAT_CULTURE),
					gap,
					duration,
					processId,
					threadNumber,
					cols[5].ToCharArray()[cols[5].Length-1],
					cols[6],
					cols[7],
					cols[8],
					cols[9],
					cols[10],
					cols[11],
					cols[12]
					));
			}
			else
			{
				throw new FormatException();
			}
			return result;
		}

		/// <summary>
		/// Reads the passed <see cref="StreamReader"/> and returns the corresponding <see cref="List<IDiagnoseOutputEntry>"/> .
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <returns>The created <see cref="List<IDiagnoseOutputEntry>"/>.</returns>
		/// <exception cref="FormatException">The file format is incorrect.</exception>
		internal List<IDiagnoseOutputEntry> ReadFile(StreamReader reader)
		{
			List<IDiagnoseOutputEntry> entries;
			if((!reader.EndOfStream)&&(reader.ReadLine()==TABLE_HEADER))
			{
				entries=new List<IDiagnoseOutputEntry>();
				while(!reader.EndOfStream)
				{
					entries.Add(ReadLine(reader));
				}
			}
			else
			{
				throw new FormatException();
			}
			return entries;
		}

		/// <summary>
		/// Reads the file selected in the <see cref="OpenFileDialog"/> and returns the corresponding <see cref="List<IDiagnoseOutputEntry>"/>.
		/// </summary>
		/// <param name="reader">The <see cref="StreamReader"/> to read from.</param>
		/// <returns>The created IDiagnoseOutputEntry.</returns>
		/// <exception cref="InvalidOperationException">No files were selected in the dialog.</exception>
		internal List<IDiagnoseOutputEntry> OpenFile(OpenFileDialog dialog)
		{
			List<IDiagnoseOutputEntry> entries;
			StreamReader reader=new StreamReader(dialog.OpenFile());
			try
			{
				entries=ReadFile(reader);
				reader.Close();
			}
			catch
			{
				entries=null;
				MessageBox.Show("Leider konnte die angegebene Datei '"+dialog.FileName+"' aufgrund eines fehlerhaften Formats nicht geöffnet werden.", "Fehlerhaftes Format", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return entries;
		}

		/// <summary>
		/// Initializes the <see cref="IReceiver"/>.
		/// </summary>
		/// <param name="informationEngine">The <see cref="IInformationEngine"/> which handels the requests of the <see cref="IReceiver"/>.
		/// </param>
		/// <exception cref="OperationCanceledException">Thrown if the initialization is canceled.</exception>
		public override void Initialize(IInformationEngine informationEngine)
		{
			base.Initialize(informationEngine);
			List<IDiagnoseOutputEntry> entries=null;
			OpenFileDialog dialog=new OpenFileDialog();
			dialog.Title="Öffnen";
			dialog.Filter="CSV-Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
			bool? result;
			bool notcanceled;
			while((notcanceled=((result=dialog.ShowDialog()).HasValue)&&(result.Value))&&(entries==null))
			{
				try
				{
					entries=OpenFile(dialog);
					foreach(IDiagnoseOutputEntry entry in entries)
					{
						InformationEngine.ProcessNewDiagnoseOutputEntry(entry);
					}
				}
				catch
				{
					MessageBox.Show("Leider konnte die angegebene Datei '"+dialog.FileName+"' aufgrund eines E/A-Fehlers nicht geöffnet werden.", "E/A-Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			if(!notcanceled)
			{
				throw new OperationCanceledException();
			}
		}

		/// <summary>
		/// The <see cref="IReceiver"/> cleanup everything to be destructed without errors.
		/// </summary>
		public override void Deactivate()
		{
		}
	}
}

