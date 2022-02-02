$tableDocuments = $('#tableCargoDocuments');

var columnDocument = [
    {
        field: '',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: runningFormatter
    },
    {
        field: 'CaseDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Name',
        title: 'Document Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return operateFormatter(row);
        }
        //,
        //events: operateEvents
    }];

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

function operateFormatter(options) {
    var btn = [];
    console.log(options);
    btn.push('<div class="btn-group">');

    btn.push('</button><button type="button" class="btn btn-default download" title="Download"><i class="fa fa-download"></i></button>')

    btn.push('</div>');

    return btn.join('');
}


window.operateEvents = {
    'click .download': function (e, value, row, index) {
        location.href = "/EMCS/ReportDO/" + row.id;
    }
};

$(function () {
    $tableDocuments.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        url: "/Emcs/GetDocumentList",
        striped: false,
        showRefresh: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        smartDisplay: false,
        queryParams: function (params) {
            return {
                Id: $("#idCargo").val(),
                Category: "CL"
            };
        },
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No Document available</span>';
        },
        columns: columnDocument
    });

    //window.pis.table({
    //    objTable: $tableDocuments,
    //    urlSearch: '/EMCS/CargoDocumentsPage?id=' + $("#idCargo").val(),
    //    urlPaging: '/EMCS/CargoDocumentsPageXt',
    //    autoLoad: true
    //});


});