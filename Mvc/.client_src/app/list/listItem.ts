import { computed, registerComputed, dateFromModel, formatDate} from "tenogy";

@registerComputed
export class ListItem {
	constructor(model) {
		this.stringItem = model.StringItem;
		this.dateItem = dateFromModel(model.DateItem);
	}

	@computed
	dateFormatted() {
		return formatDate(this.dateItem);
	}

	stringItem: string;
	dateItem: any;
}