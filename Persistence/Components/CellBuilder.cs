using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Persistence.Components {
	public class CellBuilder<TModel> {
		private readonly ColumnBuilder<TModel> _columnBuilder;
		private readonly List<CellDef> _cells;

		public CellBuilder(ColumnBuilder<TModel> columnBuilder) {
			_columnBuilder = columnBuilder;
			_cells = new List<CellDef>();
		}

		public CellBuilder<TModel> Cell(string caption) {
			_cells.Add(new CellDef {
				Caption = caption,
				Alignment = _columnBuilder[_cells.Count]
			});

			return this;
		}

		public CellBuilder<TModel> Cell(bool caption) {
			return Cell(caption ? "Yes" : "No");
		}

		public CellBuilder<TModel> Cell(DateTime caption) {
			return Cell(caption.ToShortDateString());
		}

		public CellBuilder<TModel> Cell(DateTime? caption) {
			return
				caption == null
					? Cell((string) null)
					: Cell(caption.Value);
		}

		public object[] ToArray() {
			var cells =
				_cells
					.Select(x => new {
						caption = x.Caption,
						alignment = x.Alignment
					})
					.Cast<object>()
					.ToArray();

			_cells.Clear();

			return cells;
		}

		private class CellDef {
			public string Caption { get; set; }

			public CellAlignment Alignment { get; set; }
		}
	}
}