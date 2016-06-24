import {computed, registerComputed, dateFromModel} from "./../container";

@registerComputed
export class ListItem {
	constructor(model) {
		this.stringItem = model.StringItem;
		this.dateItem = dateFromModel(model.DateItem);
	}

	stringItem: string;
	dateItem: any;
}