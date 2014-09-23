module Shared {
	"use strict";

	export class Parameters {
		public index: number;

		public size: number = 10;

		public name: string;

		public direction: string;

		public filter: string;

		public url(path: string): string {
			var url: string = path;
			var separator: string = "?";

			if (this.index) {
				url += (separator + "index=" + this.index);
				separator = "&";
			}

			if (this.size) {
				url += (separator + "size=" + this.size);
				separator = "&";
			}

			if (this.name) {
				url += (separator + "name=" + this.name);
				separator = "&";
			}

			if (this.direction) {
				url += (separator + "direction=" + this.direction);
				separator = "&";
			}

			if (this.filter) {
				url += (separator + "filter=" + encodeURIComponent(this.filter));
			}

			return url;
		}
	}
}