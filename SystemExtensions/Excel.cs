using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using MSOpenXML = DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Spreadsheet;

namespace System
{
	public sealed class Excel<T> : IExcel
	{
		CultureInfo _en_us_ci = new CultureInfo("en-us");
		private List<ColumnInfo> _columns = new List<ColumnInfo>();
		private List<GroupInfo> _groups = new List<GroupInfo>();
		private string[] _headerText;
		private string[] _footerText;
		private List<T> _dataSource;
		private List<dynamic> _excels;
		private List<dynamic> _innerExcels;
		private dynamic _parent;
		private dynamic Parent
		{
			get
			{
				dynamic result = this._parent;
				for (; result != result._parent; result = result._parent) ;
				return result;
			}
		}
		private bool _summarize = true;
		private bool _freezePanes = true;
		private int _quantidadeDecimais = 2;
		private string _tabName;

		#region Subclasses
		private class GroupInfo
		{
			public GroupInfo(string pHeader, Func<T, object> pValueSelector, Type pType)
			{
				Header = pHeader;
				ValueSelector = pValueSelector;
				Type = pType;
			}

			private GroupInfo()
				: this("", null, null)
			{ }

			private string _header;
			public virtual string Header
			{
				get { return _header; }
				set { _header = value; }
			}

			private Func<T, object> _valueSelector;
			public virtual Func<T, object> ValueSelector
			{
				get { return _valueSelector; }
				set { _valueSelector = value; }
			}

			private Type _type;
			public Type Type
			{
				get { return _type; }
				set { _type = value; }
			}
		}

		private class ColumnInfo
		{
			public ColumnInfo(string pHeader, Func<T, object> pValueSelector, Type pType, ESummaryOperation pSummaryOperation)
				: this(pHeader, "", pValueSelector, pType, pSummaryOperation)
			{ }

			public ColumnInfo(string pHeader, string pGroupHeader, Func<T, object> pValueSelector, Type pType, ESummaryOperation pSummaryOperation)
			{
				Header = pHeader;
				ValueSelector = pValueSelector;
				SummaryOperation = pSummaryOperation;
				GroupHeader = pGroupHeader;
				Type = pType;
			}

			private ColumnInfo()
				: this("", null, null, ESummaryOperation.None)
			{ }

			public ColumnInfo(string pHeader, Func<T, object> pValueSelector, Type pType)
				: this(pHeader, pValueSelector, pType, ESummaryOperation.None)
			{ }

			private string _header;
			public virtual string Header
			{
				get { return _header; }
				set { _header = value; }
			}

			private string _groupHeader;
			public virtual string GroupHeader
			{
				get { return _groupHeader; }
				set { _groupHeader = value; }
			}

			private Func<T, object> _valueSelector;
			public virtual Func<T, object> ValueSelector
			{
				get { return _valueSelector; }
				set { _valueSelector = value; }
			}

			private Type _type;
			public Type Type
			{
				get { return _type; }
				set { _type = value; }
			}

			private ESummaryOperation _summaryOperation;
			public virtual ESummaryOperation SummaryOperation
			{
				get { return _summaryOperation; }
				set { _summaryOperation = value; }
			}
		}
		#endregion

		#region Constructors
		private Excel() { _parent = this; }

		private Excel(IEnumerable<T> pDataSource, object pParent)
			: this(pDataSource, new string[0])
		{
			_parent = pParent;
		}

		private Excel(IEnumerable<T> pDataSource, string[] pHeaderText, object pParent)
			: this(pDataSource, pHeaderText)
		{
			_parent = pParent;
		}

		public Excel(IEnumerable<T> pDataSource)
			: this(pDataSource, new string[0])
		{ }

		public Excel(IEnumerable<T> pDataSource, params string[] pHeaderText)
		{
			_dataSource = pDataSource.ToList();
			_headerText = pHeaderText;
			_footerText = new string[0];
			_excels = new List<object>();
			_innerExcels = new List<object>();
			_parent = this;
		}
		#endregion

		#region Public Methods
		public Excel<T> Column<TProp>(Func<T, TProp> pValueSelector, string pHeader, string pGroupHeader, ESummaryOperation pSummaryOperation)
		{
			_columns.Add(new ColumnInfo(pHeader, pGroupHeader, x => pValueSelector(x), typeof(TProp), pSummaryOperation));
			return this;
		}

		public Excel<T> Column<TProp>(Func<T, TProp> pValueSelector, string pHeader, ESummaryOperation pSummaryOperation)
		{
			_columns.Add(new ColumnInfo(pHeader, x => pValueSelector(x), typeof(TProp), pSummaryOperation));
			return this;
		}

		public Excel<T> Column<TProp>(Func<T, TProp> pValueSelector, string pHeader, string pGroupHeader)
		{
			_columns.Add(new ColumnInfo(pHeader, pGroupHeader, x => pValueSelector(x), typeof(TProp), ESummaryOperation.None));
			return this;
		}

		public Excel<T> Column<TProp>(Func<T, TProp> pValueSelector, string pHeader)
		{
			_columns.Add(new ColumnInfo(pHeader, x => pValueSelector(x), typeof(TProp)));
			return this;
		}

		public Excel<T> Separator()
		{
			_columns.Add(new ColumnInfo("", x => "", typeof(string)));
			return this;
		}

		public Excel<T> Group<TProp>(Func<T, TProp> pValueSelector, string pHeader)
		{
			_groups.Add(new GroupInfo(pHeader, x => pValueSelector(x), typeof(TProp)));
			return this;
		}

		public Excel<T> Summarize(bool pSummarize)
		{
			_summarize = pSummarize;
			return this;
		}

		public Excel<T> FreezePanes(bool pFreezePanes)
		{
			_freezePanes = pFreezePanes;
			return this;
		}

		/// <summary>
		/// Altera a quantidade de casas decimais que aparecerão nos valores do relatório
		/// </summary>
		/// <param name="pQuantidadeDecimais">Quantidade de casas decimais. O valor default é 2, logo qualquer valor menor ou igual a 2 será ignorado.</param>
		/// <returns></returns>
		public Excel<T> QuantidadeDecimais(int pQuantidadeDecimais)
		{
			_quantidadeDecimais = pQuantidadeDecimais;
			return this;
		}

		public Excel<T> HeaderText(params string[] pHeaderText)
		{
			_headerText = pHeaderText;
			return this;
		}

		public Excel<T> FooterText(params string[] pFooterText)
		{
			_footerText = pFooterText;
			return this;
		}

		public Excel<T> TabName(string pTabName)
		{
			_tabName = pTabName;
			return this;
		}

		#endregion

		#region Private Methods

		private static string GetColumnName(int index)
		{
			//index--; //UNCOMMENT THIS TO GET A 1-BASED ALGORITHM
			string result = "";
			for (; index >= 0; index = index / 26 - 1)
				result += Convert.ToChar(index % 26 + 'A');
			return result.Reverse();
		}

		private static uint GetColumnIndex(string column)
		{
			uint index = 0;
			char[] c = column.Reverse().ToCharArray();
			for (int i = 0; i < c.Length; i++)
				index += (uint)(c[i] - 'A' + 1) * (uint)Math.Pow(26, i);
			return index - 1;
		}

		private bool Contains(string interval, string stringValue)
		{
			string[] interval2 = interval.Split(':');
			uint[] index = new uint[]{GetColumnIndex(GetColumnName(interval2[0])), GetRowIndex(interval2[0]),
			GetColumnIndex(GetColumnName(interval2[1])), GetRowIndex(interval2[1])};
			uint[] indexc = new uint[] { GetColumnIndex(GetColumnName(stringValue)), GetRowIndex(stringValue) };
			return indexc[0].Between(index[0], index[2]) && indexc[1].Between(index[1], index[3]);
		}

		#region OpenXML
		private FileInfo ExportXML(string pFileName)
		{
			using (SpreadsheetDocument spreadSheet =
				SpreadsheetDocument.Create(pFileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
			{
				// create the workbook
				WorkbookPart wbp = spreadSheet.AddWorkbookPart();
				wbp.Workbook = new MSOpenXML.Workbook();

				WorkbookStylesPart wbsp = wbp.AddNewPart<WorkbookStylesPart>();
				wbsp.Stylesheet = CreateStylesheet(_quantidadeDecimais);
				wbsp.Stylesheet.Save();

				// create the worksheet
				WorksheetPart wsp = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
				wsp.Worksheet = new MSOpenXML.Worksheet();

				// create sheet data
				MSOpenXML.SheetData ws = wsp.Worksheet.AppendChild(new MSOpenXML.SheetData());

				int row = Export(ws);

				foreach (dynamic item in _innerExcels)
					row = item.Export(ws, ++row);
				// save worksheet
				AutoFit(wsp.Worksheet);
				wsp.Worksheet.Save();
				// create the worksheet to workbook relation
				spreadSheet.WorkbookPart.Workbook.AppendChild(new MSOpenXML.Sheets());
				spreadSheet.WorkbookPart.Workbook.GetFirstChild<MSOpenXML.Sheets>()
					.AppendChild(new MSOpenXML.Sheet()
					{
						Id = spreadSheet.WorkbookPart.GetIdOfPart(wsp),
						SheetId = 1,
						Name = _tabName ?? "Plan1"
					});

				for (int i = 0; i < _excels.Count; i++)
				{
					dynamic item = _excels[i];

					// create the worksheet
					wsp = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
					wsp.Worksheet = new MSOpenXML.Worksheet();

					// create sheet data
					ws = wsp.Worksheet.AppendChild(new MSOpenXML.SheetData());
					row = item.Export(ws);
					foreach (dynamic item2 in item._innerExcels)
						row = item2.Export(ws, ++row);
					// save worksheet
					AutoFit(wsp.Worksheet);
					wsp.Worksheet.Save();
					// create the worksheet to workbook relation
					spreadSheet.WorkbookPart.Workbook.GetFirstChild<MSOpenXML.Sheets>()
						.AppendChild(new MSOpenXML.Sheet()
						{
							Id = spreadSheet.WorkbookPart.GetIdOfPart(wsp),
							SheetId = (uint)i + 2,
							Name = item._tabName ?? "Plan" + (i + 2)
						});
				}

				spreadSheet.WorkbookPart.Workbook.Save();
				return new FileInfo(pFileName);
			}
		}

		private void AutoFit(MSOpenXML.Worksheet worksheet)
		{
			MSOpenXML.MergeCells mergeCells = worksheet.Elements<MSOpenXML.MergeCells>().Count() > 0 ? mergeCells = worksheet.Elements<MSOpenXML.MergeCells>().First() : null;
			List<MergeCell> cells = mergeCells != null ? mergeCells.Elements<MergeCell>().ToList() : new List<MergeCell>();
			Dictionary<string, int> d = new Dictionary<string, int>();
			foreach (var row in worksheet.Descendants<Row>())
			{
				foreach (var cell in row.Elements<Cell>())
				{
					if (cell.CellValue == null) continue;
					if (cells.Exists(x => Contains(x.Reference.Value, cell.CellReference.Value))) continue;
					int s = cell.CellValue.Text.Length;
					if (cell.StyleIndex != null)
					{
						if (cell.StyleIndex.Value == 1) s = 10;
						if (cell.StyleIndex.Value == 3) s = decimal.Parse(cell.CellValue.Text, _en_us_ci.NumberFormat).ToString("n2").Length;
					}
					string c = GetColumnName(cell.CellReference);
					if (d.ContainsKey(c)) d[c] = Math.Max(d[c], s);
					else d[c] = s;
				}
			}
			Columns columns = new Columns();
			foreach (var item in d)
				columns.Append(CreateColumnData(GetColumnIndex(item.Key) + 1, GetColumnIndex(item.Key) + 1, item.Value * 1.2));
			worksheet.InsertBefore(columns, worksheet.Elements<MSOpenXML.SheetData>().First());
		}

		private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
		{
			Column column = new Column();
			column.Min = StartColumnIndex;
			column.Max = EndColumnIndex;
			if (ColumnWidth > 70)
				column.Width = 70;
			else
				column.Width = ColumnWidth;
			column.CustomWidth = true;
			return column;
		}

		private int Export(MSOpenXML.SheetData ws, int row = 1)
		{
			int initrow = row;
			if (ws == null)
				throw new Exception("Worksheet could not be created. Check that your office installation and project references are correct.");

			List<int[]>[] lGroupIndexes = new List<int[]>[_groups.Count + 1];
			for (int i = 0; i < lGroupIndexes.Length; i++)
				lGroupIndexes[i] = new List<int[]>();
			EnumerableComparer lArrayComparer = new EnumerableComparer();

			#region Order
			IOrderedEnumerable<T> lOrder = null;
			if (_groups.Count > 0) lOrder = _dataSource.OrderBy(_groups[0].ValueSelector);
			for (int i = 1; i < _groups.Count; i++) lOrder = lOrder.ThenBy(_groups[i].ValueSelector);
			if (lOrder != null) _dataSource = lOrder.ToList();
			#endregion

			#region Groupping
			List<IGrouping<object[], object[]>> lGroupValues =
				_dataSource.GroupBy<T, object[], object[]>(
					item => _groups.Select(x => x.ValueSelector.Invoke(item)).ToArray(), //apura as chaves de cada grupo
					item => _groups.Select(x => x.ValueSelector.Invoke(item)).ToList().Append(_columns.Select(x => x.ValueSelector.Invoke(item))).ToArray(), //apura o array como conteúdo da linha
					lArrayComparer
				).ToList();
			#endregion

			#region Excel Header

			foreach (string s in _headerText)
			{
				CreateContentRow(ws, row, new object[] { s }, new Type[] { typeof(string) }).Descendants<MSOpenXML.Cell>().ForEach(x => x.StyleIndex = 5);
				MergeRange(ws.Parent as MSOpenXML.Worksheet, CellName(row, 0), CellName(row, _columns.Count + _groups.Count - 1));
				row++;
			}

			if (_headerText.Length > 0) row++;

			#region Column Titles

			string[] groupColumns = _groups.Select(x => "").ToList().Append(_columns.Select(x => x.GroupHeader)).ToArray();
			Type[] types = groupColumns.Select(x => typeof(string)).ToArray();
			if (groupColumns.Count(x => !x.IsNullOrEmpty()) > 0)
			{
				CreateContentRow(ws, row, groupColumns, types).Descendants<MSOpenXML.Cell>().ForEach(x => x.StyleIndex = 5);
				groupColumns
					.Select((x, i) => new { Index = i, Str = x })
					.Where(x => !x.Str.IsNullOrEmpty())
					.GroupPartitionBy(x => x.Str)
					.ForEach(x => MergeRange(ws.Parent as MSOpenXML.Worksheet, CellName(row, x.First().Index), CellName(row, x.Last().Index)));
				row++;
			}
			string[] valores = _groups.Select(x => x.Header).Concat(_columns.Select(x => x.Header)).ToArray();
			CreateContentRow(ws, row, valores, types).Descendants<MSOpenXML.Cell>().ForEach(x => x.StyleIndex = 7);
			row++;
			#endregion

			#region Fix Column and Rows
			if (_freezePanes)
			{
				MSOpenXML.SheetViews sheetviews =
					(ws.Parent as MSOpenXML.Worksheet).GetFirstChild<MSOpenXML.SheetViews>() ??
					(ws.Parent as MSOpenXML.Worksheet).InsertBefore(new MSOpenXML.SheetViews(), ws);
				MSOpenXML.SheetView sv =
					sheetviews.GetFirstChild<MSOpenXML.SheetView>() ??
					sheetviews.AppendChild(new MSOpenXML.SheetView());
				//sv.TabSelected = true;
				sv.WorkbookViewId = 0;
				MSOpenXML.Pane p = sv.GetFirstChild<MSOpenXML.Pane>() ?? sv.AppendChild(new MSOpenXML.Pane());
				p.VerticalSplit = row - 1;
				p.HorizontalSplit = _groups.Count;
				p.TopLeftCell = CellName(row, _groups.Count);
				p.ActivePane = MSOpenXML.PaneValues.BottomRight;
				p.State = MSOpenXML.PaneStateValues.Frozen;
			}
			#endregion
			#endregion

			#region Excel Body
			Type[] typesValores = _groups.Select(x => x.Type).Concat(_columns.Select(x => x.Type)).ToArray();
			for (int i = 0; i < lGroupValues.Count; i++)
			{
				//Print all group rows
				foreach (object[] rowArray in lGroupValues[i])
				{
					CreateContentRow(ws, row, rowArray, typesValores);
					row++;
				}

				//Save counting
				for (int j = 0; j < lGroupIndexes.Length; j++)
					lGroupIndexes[j].Add(new int[] { row - lGroupValues[i].Count(), row - 1 });

				//Close Partial or Full Groups
				for (int j = lGroupValues[i].Key.Length - 1; j >= 0; j--)
					if (i == lGroupValues.Count - 1 || !lArrayComparer.Equals(lGroupValues[i].Key.Take(j + 1), lGroupValues[i + 1].Key.Take(j + 1)))
						row = PrintGroupFooter(ws, row, lGroupValues[i].Key, lGroupIndexes, j);
			}
			#endregion

			#region Excel Footer

			#region Final Total
			if (_summarize)
			{
				//Pular linha
				row++;
				MSOpenXML.Cell c = CreateCellIfNotExist(ws.Parent as MSOpenXML.Worksheet, CellName(row, 0));
				if (lGroupIndexes[_groups.Count].Count == 0)
				{
					c.DataType = MSOpenXML.CellValues.String;
					c.CellValue = new MSOpenXML.CellValue("Total: 0");
				}
				else
				{
					c.CellFormula = new MSOpenXML.CellFormula("CONCATENATE(\"Total: \"," + lGroupIndexes[_groups.Count].Aggregate<int[], string>("", (r, n) => r = r + "+ROWS(" + GetColumnName(_groups.Count) + n[0] + ":" + GetColumnName(_groups.Count + _columns.Count - 1) + n[1] + ")").Right(-1) + ")");
					Summarize(ws, row, lGroupIndexes[_groups.Count]);
				}
				(c.Parent as MSOpenXML.Row).Descendants<MSOpenXML.Cell>().ForEach(x => x.StyleIndex = 7);
				lGroupIndexes[_groups.Count].Clear();
				row++;
			}
			#endregion

			#region Footer
			foreach (string s in _footerText)
			{
				row++;
				CreateContentRow(ws, row, new object[] { s }, new Type[] { typeof(string) });
				MergeRange(ws.Parent as MSOpenXML.Worksheet, CellName(row, 0), CellName(row, _columns.Count + _groups.Count - 1));
				//ws.Rows[row].Font.Bold = true;
			}
			if (_footerText.Length > 0) row++;
			#endregion
			#endregion

			//dos.Columns cs =
			//    (ws.Parent as dos.Worksheet).Descendants<dos.Columns>().FirstOrDefault() ??
			//    (ws.Parent as dos.Worksheet).AppendChild(new dos.Columns());
			//dos.Column cl = cs.Descendants<dos.Column>().FirstOrDefault() ?? cs.AppendChild(new dos.Column());
			//cl.Min = 1;
			//if (cl.Max == null) cl.Max = UInt32Value.FromUInt32((uint)_columns.Count + (uint)_groups.Count);
			//else cl.Max = Math.Max(cl.Max.Value, (uint)_columns.Count + (uint)_groups.Count);
			//cl.CustomWidth = BooleanValue.FromBoolean(true);
			//cl.Width = DoubleValue.FromDouble(50);
			//cl.BestFit = BooleanValue.FromBoolean(true);
			return row;
		}

		private string CellName(int row, int column)
		{
			return GetColumnName(column) + row;
		}

		private MSOpenXML.Row CreateContentRow(MSOpenXML.SheetData ws, int row, object[] values, Type[] types)
		{
			//Create cells that contain data
			MSOpenXML.Row r = ws.AppendChild(new MSOpenXML.Row());
			r.RowIndex = (uint)row;
			for (int i = 0; i < values.Length; i++)
			{
				MSOpenXML.Cell c = r.AppendChild(new MSOpenXML.Cell());
				c.CellReference = CellName(row, i);
				//if (values[i] != null)
				Type t = types[i];

				if (t == null)
				{
					c.DataType = MSOpenXML.CellValues.String;
					c.CellValue = new MSOpenXML.CellValue();
				}
				else if (t.In(typeof(int), typeof(int?), typeof(long), typeof(long?)))
				{
					c.StyleIndex = 2;//DataType = dos.CellValues.Number;
					c.CellValue = new MSOpenXML.CellValue(values[i] == null ? null : ((long)Convert.ChangeType(values[i], typeof(long))).ToString());
				}
				else if (t.In(typeof(decimal), typeof(decimal?), typeof(float), typeof(float?), typeof(double), typeof(double?)))
				{
					c.StyleIndex = 3;//DataType = dos.CellValues.Number;
					c.CellValue = new MSOpenXML.CellValue(values[i] == null ? null : ((decimal)Convert.ChangeType(values[i], typeof(decimal))).ToString(_en_us_ci.NumberFormat));
				}
				else if (t.In(typeof(DateTime), typeof(DateTime?)))
				{
					c.StyleIndex = 1;//.DataType = dos.CellValues.Date;
					c.CellValue = new MSOpenXML.CellValue(values[i] == null ? null : ((DateTime)values[i]).ToOADate().ToString(_en_us_ci.NumberFormat));
				}
				else if (t.In(typeof(bool), typeof(bool?)))
				{
					c.DataType = MSOpenXML.CellValues.Boolean;
					c.CellValue = new MSOpenXML.CellValue(values[i].ToString());
				}
				else
				{
					c.DataType = MSOpenXML.CellValues.String;
					c.CellValue = new MSOpenXML.CellValue(values[i] == null ? null : values[i].ToString());
				}
			}
			return r;
		}

		private string GetColumnName(string cellName)
		{
			// Create a regular expression to match the column name portion of the cell name.
			Regex regex = new Regex("[A-Za-z]+");
			Match match = regex.Match(cellName);

			return match.Value;
		}

		private uint GetRowIndex(string cellName)
		{
			// Create a regular expression to match the row index portion the cell name.
			Regex regex = new Regex(@"\d+");
			Match match = regex.Match(cellName);

			return uint.Parse(match.Value);
		}

		private MSOpenXML.Cell CreateCellIfNotExist(MSOpenXML.Worksheet worksheet, string cellName)
		{
			string columnName = GetColumnName(cellName);
			uint rowIndex = GetRowIndex(cellName);

			IEnumerable<MSOpenXML.Row> rows = worksheet.Descendants<MSOpenXML.Row>().Where(r => r.RowIndex.Value == rowIndex);

			// If the Worksheet does not contain the specified row, create the specified row.
			// Create the specified cell in that row, and insert the row into the Worksheet.
			if (rows.Count() == 0)
			{
				MSOpenXML.Row row = new MSOpenXML.Row() { RowIndex = rowIndex };
				MSOpenXML.Cell cell = new MSOpenXML.Cell() { CellReference = cellName };
				row.Append(cell);
				worksheet.Descendants<MSOpenXML.SheetData>().First().Append(row);
				return cell;
			}
			else
			{
				MSOpenXML.Row row = rows.First();

				IEnumerable<MSOpenXML.Cell> cells = row.Elements<MSOpenXML.Cell>().Where(c => c.CellReference.Value == cellName);

				// If the row does not contain the specified cell, create the specified cell.
				if (cells.Count() == 0)
				{
					MSOpenXML.Cell cell = new MSOpenXML.Cell() { CellReference = cellName };
					row.Append(cell);
					return cell;
				}
				else return cells.First();
			}
		}

		private void MergeRange(MSOpenXML.Worksheet worksheet, string cell1Name, string cell2Name)
		{
			// Open the document for editing.
			if (worksheet == null || string.IsNullOrEmpty(cell1Name) || string.IsNullOrEmpty(cell2Name)) return;
			if (cell1Name == cell2Name) return;
			// Verify if the specified cells exist, and if they do not exist, create them.
			CreateCellIfNotExist(worksheet, cell1Name);
			CreateCellIfNotExist(worksheet, cell2Name);

			MSOpenXML.MergeCells mergeCells;
			if (worksheet.Elements<MSOpenXML.MergeCells>().Count() > 0)
			{
				mergeCells = worksheet.Elements<MSOpenXML.MergeCells>().First();
			}
			else
			{
				mergeCells = new MSOpenXML.MergeCells();
				// Insert a MergeCells object into the specified position.
				if (worksheet.Elements<MSOpenXML.CustomSheetView>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.CustomSheetView>().First());
				else if (worksheet.Elements<MSOpenXML.DataConsolidate>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.DataConsolidate>().First());
				else if (worksheet.Elements<MSOpenXML.SortState>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.SortState>().First());
				else if (worksheet.Elements<MSOpenXML.AutoFilter>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.AutoFilter>().First());
				else if (worksheet.Elements<MSOpenXML.Scenarios>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.Scenarios>().First());
				else if (worksheet.Elements<MSOpenXML.ProtectedRanges>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.ProtectedRanges>().First());
				else if (worksheet.Elements<MSOpenXML.SheetProtection>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.SheetProtection>().First());
				else if (worksheet.Elements<MSOpenXML.SheetCalculationProperties>().Count() > 0)
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.SheetCalculationProperties>().First());
				else
					worksheet.InsertAfter(mergeCells, worksheet.Elements<MSOpenXML.SheetData>().First());
			}

			// Create the merged cell and append it to the MergeCells collection.
			MSOpenXML.MergeCell mergeCell = new MSOpenXML.MergeCell() { Reference = cell1Name + ":" + cell2Name };
			mergeCells.Append(mergeCell);
		}

		private static string FormatoDecimal(int pQuantidadeDecimais)
		{
			string lRetorno = "#,##0.";
			if (pQuantidadeDecimais > 2)
			{
				for (int i = 0; i < pQuantidadeDecimais; i++)
				{
					lRetorno += "0";
				}

				return lRetorno;
			}
			else return "#,##0.00";
		}

		private static MSOpenXML.Stylesheet CreateStylesheet(int pQuantidadeDecimais = 2)
		{
			MSOpenXML.Stylesheet ss = new MSOpenXML.Stylesheet();

			#region Fontes
			MSOpenXML.Fonts fts = new MSOpenXML.Fonts();
			MSOpenXML.Font ft = new MSOpenXML.Font();
			MSOpenXML.FontName ftn = new MSOpenXML.FontName();
			ftn.Val = "Calibri";
			MSOpenXML.FontSize ftsz = new MSOpenXML.FontSize();
			ftsz.Val = 11;
			ft.FontName = ftn;
			ft.FontSize = ftsz;
			fts.Append(ft);

			ft = new MSOpenXML.Font();
			ft.Bold = new MSOpenXML.Bold();
			ftn = new MSOpenXML.FontName();
			ftn.Val = "Calibri";
			ftsz = new MSOpenXML.FontSize();
			ftsz.Val = 11;
			ft.FontName = ftn;
			ft.FontSize = ftsz;
			fts.Append(ft);

			fts.Count = (uint)fts.ChildElements.Count;
			#endregion

			#region Preenchimento
			MSOpenXML.Fills fills = new MSOpenXML.Fills();
			MSOpenXML.Fill fill;
			MSOpenXML.PatternFill patternFill;
			fill = new MSOpenXML.Fill();
			patternFill = new MSOpenXML.PatternFill();
			patternFill.PatternType = MSOpenXML.PatternValues.None;
			fill.PatternFill = patternFill;
			fills.Append(fill);

			/*fill = new dos.Fill();
			patternFill = new dos.PatternFill();
			patternFill.PatternType = dos.PatternValues.Gray125;
			fill.PatternFill = patternFill;
			fills.Append(fill);

			fill = new dos.Fill();
			patternFill = new dos.PatternFill();
			patternFill.PatternType = dos.PatternValues.Solid;
			patternFill.ForegroundColor = new dos.ForegroundColor();
			patternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728");
			patternFill.BackgroundColor = new dos.BackgroundColor();
			patternFill.BackgroundColor.Rgb = patternFill.ForegroundColor.Rgb;
			fill.PatternFill = patternFill;
			fills.Append(fill);
			*/
			fills.Count = (uint)fills.ChildElements.Count;

			#endregion

			#region Bordas
			MSOpenXML.Borders borders = new MSOpenXML.Borders();

			MSOpenXML.Border border = new MSOpenXML.Border();
			border.LeftBorder = new MSOpenXML.LeftBorder();
			border.RightBorder = new MSOpenXML.RightBorder();
			border.TopBorder = new MSOpenXML.TopBorder();
			border.BottomBorder = new MSOpenXML.BottomBorder();
			border.DiagonalBorder = new MSOpenXML.DiagonalBorder();
			borders.Append(border);

			border = new MSOpenXML.Border();
			border.LeftBorder = new MSOpenXML.LeftBorder();
			border.RightBorder = new MSOpenXML.RightBorder();
			border.TopBorder = new MSOpenXML.TopBorder();
			border.TopBorder.Style = MSOpenXML.BorderStyleValues.Thin;
			border.BottomBorder = new MSOpenXML.BottomBorder();
			border.DiagonalBorder = new MSOpenXML.DiagonalBorder();
			borders.Append(border);
			borders.Count = (uint)borders.ChildElements.Count;
			#endregion

			MSOpenXML.CellStyleFormats csfs = new MSOpenXML.CellStyleFormats();
			MSOpenXML.CellFormat cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = 0;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			csfs.Append(cf);
			csfs.Count = (uint)csfs.ChildElements.Count;

			uint iExcelIndex = 164;
			MSOpenXML.NumberingFormats nfs = new MSOpenXML.NumberingFormats();
			MSOpenXML.CellFormats cfs = new MSOpenXML.CellFormats();

			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = 0;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			MSOpenXML.NumberingFormat nfDateTime = new MSOpenXML.NumberingFormat();
			nfDateTime.NumberFormatId = iExcelIndex++;
			nfDateTime.FormatCode = "dd/mm/yyyy";
			nfs.Append(nfDateTime);

			MSOpenXML.NumberingFormat nf4decimal = new MSOpenXML.NumberingFormat();
			nf4decimal.NumberFormatId = iExcelIndex++;
			nf4decimal.FormatCode = "#,##0";
			nfs.Append(nf4decimal);

			// #,##0.00 is also Excel style index 4
			MSOpenXML.NumberingFormat nf2decimal = new MSOpenXML.NumberingFormat();
			nf2decimal.NumberFormatId = iExcelIndex++;
			nf2decimal.FormatCode = FormatoDecimal(pQuantidadeDecimais); //"#,##0.00"
			nfs.Append(nf2decimal);

			// @ is also Excel style index 49
			MSOpenXML.NumberingFormat nfForcedText = new MSOpenXML.NumberingFormat();
			nfForcedText.NumberFormatId = iExcelIndex++;
			nfForcedText.FormatCode = "@";
			nfs.Append(nfForcedText);

			// index 1
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nfDateTime.NumberFormatId;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			// index 2
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nf4decimal.NumberFormatId;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			// index 3
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nf2decimal.NumberFormatId;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			// index 4
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nfForcedText.NumberFormatId;
			cf.FontId = 0;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			// index 5
			// Header text
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nfForcedText.NumberFormatId;
			cf.FontId = 1;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cf.Alignment.Horizontal = MSOpenXML.HorizontalAlignmentValues.Center;
			cf.ApplyNumberFormat = true;
			cfs.Append(cf);

			// index 6
			// group text
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nf2decimal.NumberFormatId;
			cf.FontId = 1;
			cf.BorderId = 1;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			// index 7
			// Total text, ColumnHeader Text
			cf = new MSOpenXML.CellFormat();
			cf.NumberFormatId = nf2decimal.NumberFormatId;
			cf.FontId = 1;
			cf.BorderId = 0;
			cf.FormatId = 0;
			cf.ApplyNumberFormat = true;
			cf.Alignment = new MSOpenXML.Alignment() { WrapText = false };
			cfs.Append(cf);

			nfs.Count = (uint)nfs.ChildElements.Count;
			cfs.Count = (uint)cfs.ChildElements.Count;

			ss.Append(nfs);
			ss.Append(fts);
			ss.Append(fills);
			ss.Append(borders);
			ss.Append(csfs);
			ss.Append(cfs);

			MSOpenXML.CellStyles css = new MSOpenXML.CellStyles();
			MSOpenXML.CellStyle cs = new MSOpenXML.CellStyle();
			cs.Name = "Normal";
			cs.FormatId = 0;
			cs.BuiltinId = 0;
			css.Append(cs);
			css.Count = (uint)css.ChildElements.Count;
			ss.Append(css);

			MSOpenXML.DifferentialFormats dfs = new MSOpenXML.DifferentialFormats();
			dfs.Count = 0;
			ss.Append(dfs);

			MSOpenXML.TableStyles tss = new MSOpenXML.TableStyles();
			tss.Count = 0;
			//tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
			//tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");
			ss.Append(tss);

			return ss;
		}

		private int PrintGroupFooter(MSOpenXML.SheetData ws, int row, object[] lLastKey, List<int[]>[] lGroupIndexes, int i)
		{
			MergeRange(ws.Parent as MSOpenXML.Worksheet, CellName(row, i), CellName(row, _groups.Count - 1));
			//ws.Range[ws.Cells[row, i + 1], ws.Cells[row, _columns.Count + _groups.Count]].Font.Bold = true;
			//ws.Range[ws.Cells[row, i + 1], ws.Cells[row, _columns.Count + _groups.Count]].Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
			MSOpenXML.Cell c = CreateCellIfNotExist(ws.Parent as MSOpenXML.Worksheet, CellName(row, i));
			c.CellFormula = new MSOpenXML.CellFormula("CONCATENATE(\"Total \'" + lLastKey[i] + "\': \"," + lGroupIndexes[i].Aggregate<int[], string>("", (r, n) => r = r + "+ROWS(" + GetColumnName(_groups.Count) + n[0] + ":" + GetColumnName(_groups.Count + _columns.Count - 1) + n[1] + ")").Right(-1) + ")");
			Summarize(ws, row, lGroupIndexes[i]);
			(c.Parent as MSOpenXML.Row).Descendants<MSOpenXML.Cell>().ForEach(x => x.StyleIndex = 6);
			lGroupIndexes[i].Clear();
			return ++row;
		}

		private void Summarize(MSOpenXML.SheetData ws, int row, List<int[]> totais)
		{
			for (int i = 0; i < _columns.Count; i++)
			{
				string lCommand = "";
				switch (_columns[i].SummaryOperation)
				{
					case ESummaryOperation.None: break;
					case ESummaryOperation.Sum: lCommand = "SUBTOTAL(9,"; break;
					case ESummaryOperation.Average: lCommand = "AVERAGE("; break;
					case ESummaryOperation.Count: lCommand = ""; break;
					case ESummaryOperation.Multiply: lCommand = "PRODUCT("; break;
					case ESummaryOperation.Acumulate: throw new InvalidOperationException("Acumulate is not supported by MS Excel.");
					case ESummaryOperation.Mode: lCommand = "MODE("; break;
					default: break;
				}
				MSOpenXML.Cell c = CreateCellIfNotExist(ws.Parent as MSOpenXML.Worksheet, CellName(row, i + _groups.Count));
				if (lCommand != "")
					c.CellFormula = new MSOpenXML.CellFormula("{0}{1})".Formata(lCommand,
						totais.Aggregate<int[], string>("", (r, n) => r = r + "," + GetColumnName(i + _groups.Count) + n[0] + ":" + GetColumnName(i + _groups.Count) + n[1]).Right(-1)));
				if (_columns[i].SummaryOperation == ESummaryOperation.Count)
					c.CellFormula = new MSOpenXML.CellFormula(totais.Aggregate<int[], string>("", (r, n) => r = r + "+ROWS(" + GetColumnName(i + _groups.Count) + n[0] + ":" + GetColumnName(i + _groups.Count) + n[1] + ")").Right(-1));
			}
		}
		#endregion
		#endregion

		#region IExcel Implementation
		public FileInfo Export(string pFileName)
		{
			Directory.CreateDirectory(Directory.GetParent(pFileName).FullName);
			if ((object)this != (object)_parent) { return _parent.Export(pFileName); }
			return ExportXML(pFileName);
		}

		public Excel<TResult> ToExcel<TResult>()
		{
			return (dynamic)this;
		}

		public Excel<TResult> Append<TResult>(IEnumerable<TResult> pDataSource)
		{
			Excel<TResult> result = new Excel<TResult>(pDataSource, this.Parent);
			Parent._excels.Add(result);
			return result;
		}

		public Excel<TResult> Append<TResult>(IEnumerable<TResult> pDataSource, params string[] pHeaderText)
		{
			Excel<TResult> result = new Excel<TResult>(pDataSource, pHeaderText, this.Parent);
			Parent._excels.Add(result);
			return result;
		}

		public Excel<TResult> Append<TResult>(Excel<TResult> pExcel)
		{
			if (pExcel.Parent != null)
			{
				var p = pExcel.Parent;
				p._parent = this.Parent;
				Parent._excels.Add(p);
				foreach (var item in p._excels)
				{
					item._parent = this.Parent;
					Parent._excels.Add(item);
				}
			}
			else
			{
				pExcel._parent = this.Parent;
				Parent._excels.Add(pExcel);
			}
			return pExcel;
		}

		public IExcel Append(IExcel pExcel)
		{
			if (pExcel == null) return this;
			if (((dynamic)pExcel).Parent != null)
			{
				var p = ((dynamic)pExcel).Parent;
				p._parent = this.Parent;
				Parent._excels.Add(p);
				foreach (var item in p._excels)
				{
					item._parent = this.Parent;
					Parent._excels.Add(item);
				}
			}
			else
			{
				((dynamic)pExcel)._parent = this.Parent;
				Parent._excels.Add(pExcel);
			}
			return pExcel;
		}

		public Excel<TResult> InnerAppend<TResult>(IEnumerable<TResult> pDataSource)
		{
			Excel<TResult> result = new Excel<TResult>(pDataSource, this);
			_innerExcels.Add(result);
			return result;
		}

		public Excel<TResult> InnerAppend<TResult>(IEnumerable<TResult> pDataSource, params string[] pHeaderText)
		{
			Excel<TResult> result = new Excel<TResult>(pDataSource, pHeaderText, this);
			_innerExcels.Add(result);
			return result;
		}

		public Excel<TResult> InnerAppend<TResult>(Excel<TResult> pExcel)
		{
			pExcel._parent = this;
			_innerExcels.Add(pExcel);
			return pExcel;
		}

		public IExcel InnerAppend(IExcel pExcel)
		{
			((dynamic)pExcel)._parent = this;
			_innerExcels.Add(pExcel);
			return pExcel;
		}
		#endregion
	}
}
