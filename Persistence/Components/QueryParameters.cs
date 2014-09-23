namespace Persistence.Components {
	public class QueryParameters {
		public QueryParameters(int index, int size, string name, string direction, string filter) {
			Index = index;
			Size = size;

			Name =
				string.IsNullOrWhiteSpace(name)
					? "Id"
					: name;

			Direction =
				string.IsNullOrWhiteSpace(name) || direction == "asc"
					? "ASC"
					: "DESC";

			Filter = filter;
		}

		public int Index { get; private set; }

		public int Size { get; private set; }

		public string Name { get; private set; }

		public string Direction { get; private set; }

		public string Filter { get; private set; }
	}
}