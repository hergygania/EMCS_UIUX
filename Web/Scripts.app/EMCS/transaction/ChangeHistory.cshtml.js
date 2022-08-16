$tableChangeHistory = $('#tableChangeHistory');

var columnChangeHistory = [
    {
        field: 'FieldName',
        title: 'FieldName',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'BeforeValue',
        title: 'Old Value',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'AfterValue',
        title: 'New Value',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CreateBy',
        title: 'Change by',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CreateDate',
        title: 'Date of modification',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY hh:mm:ss");
        }
    },
];


$(function () {
    $tableChangeHistory.bootstrapTable({
        cache: false,
        url: "/Emcs/GetListSpRequestForChangeByFormType",
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        queryParams: function (params) {
            return {
                limit: params.limit,
                offset: params.offset,
                IdTerm: $("#IdTerm").val(),
                formType: $("#formType").val(),
                sort: params.sort,
                order: params.order
            };
        },
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">Data Not Found</span>';
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        columns: columnChangeHistory
    });
});