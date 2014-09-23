using System;

namespace Core {
	namespace Framework {
		public class ApplicationAttribute : Attribute {
			public ApplicationEnvironment Environment { get; set; }
		}
	}
}