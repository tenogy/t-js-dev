import {ListItem} from "listItem"
import {AlertMessage, LoadingProgress, AjaxServices, ListWithServerHtml, registerComputed, computed, SortRule,
	SortDirection} from "./../container"

@registerComputed
export class List {
	constructor(config) {
		config = config || {};
		this.model = config.model || {};

		this.ajax = new AjaxServices(config.services);
		this.sortTypes = config.sortTypes;
		this.list = ko.observable();
		this.loadingProgress = new LoadingProgress();
		this.sortRule = new SortRule(this.sortTypes.byDate, SortDirection.desc);
		this.sortRule.subscribe(() => this.loadList(1), this);
		this.mode = null;
		this.pageSize = ko.observable(10);

		this.loadList(1);
	}

	@computed
	changeMode(mode) {
		this.mode = mode;
		this.list(null);
		this.loadList(1);
	}

	@computed
	noItems() {
		const list = this.list();
		return list == null || list.items().length === 0;
	}

	@computed
	listIsEmpty() {
		const list = this.list();
		return list != null && list.items().length === 0 && !this.loadingProgress.isVisible();
	}

	@computed
	listIsVisible() {
		return !this.listIsEmpty();
	}

	loadList(pageIndex) {
		this.loadingProgress.start();
		const request = this.createRequest(pageIndex);
		this.ajax.getList(request, this.loadResultsCompleted, this);
	}

	gotoPage(pageIndex) {
		this.loadList(pageIndex);
	}

	createRequest(pageIndex) {
		return {
			Mode: this.mode,
			SortRule: this.sortRule.toModel({ defaultType: this.sortTypes.byDate, defaultDirection: SortDirection.desc }),
			PageIndex: pageIndex || 1,
			PageSize: this.pageSize()
		};
	}

	loadResultsCompleted(response) {
		this.loadingProgress.stop();

		if (response == null) {
			this.list(null);
			alert("Error");
			return;
		}

		this.list(this.createList(response));
	}

	createList(listModel) {
		return new ListWithServerHtml(
			listModel.Items.map(model => new ListItem(model)),
			listModel.ItemsHtml,
			listModel.Paging,
			{
				gotoPageHandler: (pageIndex) => this.gotoPage(pageIndex)
			}
		);
	}

	model: any;
	sortTypes;
	list: KnockoutObservable<any>;
	loadingProgress: LoadingProgress;
	sortRule: SortRule;
	mode: any;
	pageSize: KnockoutObservable<number>;
	ajax: any;
}