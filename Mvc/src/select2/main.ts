import { registerComputed, computed } from "tenogy"

@registerComputed
export class Select2Page {
	select2Data: any;
	selectedItem: KnockoutObservable<any>;

	constructor(config) {
		config = config || {};
		let model = config.model || {};
		this.select2Data = model.items ? model.items.map((i) => { return { value: i.Id, name: i.StringItem } }) : {};
		this.selectedItem = ko.observable();
		this.selectedItem.subscribe((i) => alert(`You choose ${i.name} with value ${i.value}`));
	}
}