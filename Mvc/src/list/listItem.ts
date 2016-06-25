import {computed, registerComputed, dateFromModel} from "tenogy";

@registerComputed
export class ListItem {
	constructor(model) {
		this.stringItem = model.StringItem;
		this.dateItem = dateFromModel(model.DateItem);
	}

	stringItem: string;
	dateItem: any;
}