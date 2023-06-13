$table = $('#containertableCl');

var columnList = [
    {
        field: "CiplNo",
        title: "No.",
        sortable: true
    }, {
        field: "Category",
        title: "Container",
        sortable: true
    }, {
        field: "ShippingMethod",
        title: "Incoterms",
        sortable: true
    }, {
        field: "Forwader",
        title: "Case Number",
        sortable: true
    }, {
        field: "ConsigneeName",
        title: "Reference",
        sortable: true,
        class: "text-nowrap"
    }, {
        field: "consignee",
        title: "Cargo Description",
        sortable: true,
        class: "text-nowrap"
    },
    {
        field: "branch",
        title: "Volume",
        sortable: true,
        class: "text-nowrap"
    },
    {
        field: "Status",
        title: "Net Weight",
        sortable: true,
        filterControl: true,
        class: "text-nowrap"
    },
    {
        field: "Status",
        title: "Gross Weight",
        sortable: true,
        filterControl: true,
        class: "text-nowrap",
        formatter: currencyFormatter
    }];

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });
});