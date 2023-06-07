var $table = $('#tableDeliveryRequisition');
var $searchInput = $("#txtSearchData").val();

var columnList = [
    {
        field: 'CustNr',
        title: 'Customer Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CustName',
        title: 'Customer Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Country',
        title: 'Country',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'City',
        title: 'City',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Street',
        title: 'Street',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Region',
        title: 'Region',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Telp',
        title: 'Telp',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Fax',
        title: 'Fax',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
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
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        queryParams: function (params) {
            params.SearchName = $("#mySearch input[name=searchText]").val();
            return params;
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/CustomerEmcsPage',
        urlPaging: '/EMCS/CustomerEmcsPageXt',
        autoLoad: true
    });

    $("#mySearch").insertBefore($("[name=refresh]"));
});