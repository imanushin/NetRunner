using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NetRunner.ExternalLibrary;

namespace NetRunner.InternalTests
{
    internal sealed class TableAnalyser : BaseTableArgument
    {
        internal enum Operation
        {
            Contains,
            NotContains,
            Count
        }

        private HtmlNode table;
        private HtmlNode[][] cells;

        public TableAnalyser(HtmlNode table)
        {
            this.table = table;

            var rows = table.ChildNodes.Where(n => string.Equals("tr", n.Name, StringComparison.OrdinalIgnoreCase)).ToArray();

            cells = rows.Select(r => r.ChildNodes.Where(n => string.Equals("td", n.Name, StringComparison.OrdinalIgnoreCase)).ToArray()).ToArray();
        }

        public bool CellMatching(IndexRange column, IndexRange row, Operation operation, string parameter)
        {
            var actualCells = GetAllCells(column, row);

            switch (operation)
            {
                case Operation.Contains:
                    return actualCells.All(n => n.OuterHtml.Contains(parameter)) && actualCells.Any();
                case Operation.NotContains:
                    return !actualCells.Any(n => n.OuterHtml.Contains(parameter)) && actualCells.Any();
                case Operation.Count:
                    return int.Parse(parameter) == actualCells.Count();
                default:
                    throw new ArgumentOutOfRangeException("operation", operation.ToString());
            }
        }

        private HtmlNode[] GetAllCells(IndexRange column, IndexRange row)
        {
            var result = new List<HtmlNode>();

            for (int rowIndex = row.FirstIndex; rowIndex <= row.LastIndex && rowIndex < cells.Length; rowIndex++)
            {
                var currentRow = cells[rowIndex];

                for (int columnIndex = column.FirstIndex; columnIndex <= column.LastIndex && columnIndex < currentRow.Length; columnIndex++)
                {
                    result.Add(currentRow[columnIndex]);
                }
            }

            return result.ToArray();
        }
    }
}
