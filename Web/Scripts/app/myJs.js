mySelect2 = function (params) {
	this._init(params);
}

$.extend(mySelect2.prototype, {
	// object variables
	objParams: '',

	_init: function (params) {
		// do initialization here
		this.objParams = params;
	},

	load: function () {
		var _p = this.objParams;
		var _minInputLength = _p.minimumInputLength || 1;
		var pageSize = 25;
		// object method

		$('#' + _p.id).select2({
			minimumInputLength: _minInputLength,
			width: 'resolve',
			dropdownAutoWidth: 'false',
			ajax: {
				url: _p.url,
				dataType: 'jsonp',
				beforeSend: function () { },
				//How long the user has to pause their typing before sending the next request
				quietMillis: 175,
				data: function (params) {
					var queryParameters = {
						pageSize: pageSize,
						page: params.page || 1,
						searchTerm: params.term || '',
						term: params.term || ''
					}
					return queryParameters;
				},
				processResults: function (data, page) {
					enableLink(true);
					return {
						results: data.Results,
						pagination: { more: data.pagination } //data.more }
					};
				}
			},
			//initSelection: function (element, callback) {
			//	var id = $(element).val();
			//	var txt = _p.text == null ? $(element).text() : _p.text;
			//	if (id !== '') {
			//		var obj = { id: id, text: txt };
			//		callback(obj);
			//	}
			//},
			formatNoMatches: function (term) {
				return "<div>No matches found</div>";
				//if (field == undefined)
				//	return "<div>No matches found</div>";
				//else
				//	return "<div onclick='addNew" + field + "()' title='add new'>No matches found <a href='#' style='float:right'><img src='" + "/" + "Content/icons/plus.png' border='0px'/></a></div>";
			}
		});

	}
});
