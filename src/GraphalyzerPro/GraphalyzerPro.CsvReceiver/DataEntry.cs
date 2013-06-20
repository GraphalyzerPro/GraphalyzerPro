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

using LINQtoCSV;

namespace GraphalyzerPro.CsvReceiver
{
    public class DataEntry
    {
        [CsvColumn(FieldIndex = 1)]
        public string Timestamp { get; set; }

        [CsvColumn(Name = "Gap [�s]", FieldIndex = 2)]
        public string Gap { get; set; }

        [CsvColumn(Name = "Duration [�s]", FieldIndex = 3)]
        public string Duration { get; set; }

        [CsvColumn(Name = "PID", FieldIndex = 4)]
        public string ProcessId { get; set; }

        [CsvColumn(FieldIndex = 5)]
        public string Thread { get; set; }

        [CsvColumn(FieldIndex = 6)]
        public string State { get; set; }

        [CsvColumn(FieldIndex = 7)]
        public string Domain { get; set; }

        [CsvColumn(FieldIndex = 8)]
        public string Application { get; set; }

        [CsvColumn(FieldIndex = 9)]
        public string Component { get; set; }

        [CsvColumn(FieldIndex = 10)]
        public string Module { get; set; }

        [CsvColumn(FieldIndex = 11)]
        public string Code { get; set; }

        [CsvColumn(FieldIndex = 12)]
        public string Object { get; set; }

        [CsvColumn(FieldIndex = 13)]
        public string Parent { get; set; }

        [CsvColumn(FieldIndex = 14)]
        public string Text { get; set; }

        [CsvColumn(FieldIndex = 15)]
        public string Bits { get; set; }

        [CsvColumn(FieldIndex = 16)]
        public string Value0 { get; set; }

        [CsvColumn(FieldIndex = 17)]
        public string Value1 { get; set; }

        [CsvColumn(FieldIndex = 18)]
        public string Value2 { get; set; }

        [CsvColumn(FieldIndex = 19)]
        public string Value3 { get; set; }

        [CsvColumn(FieldIndex = 20)]
        public string TickCount { get; set; }

        [CsvColumn(FieldIndex = 21)]
        public string Operation { get; set; }

        [CsvColumn(FieldIndex = 22)]
        public string HasError { get; set; }

        [CsvColumn(Name = "Pos.", FieldIndex = 23)]
        public string Pos { get; set; }
    }
}