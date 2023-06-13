var $table = $('#tableArea');
var $searchInput = $("#txtSearchData").val();

var columnList = [
    {
        field: "BAreaCode",
        title: "Area Code",
        halign: "center",
        align: "center",
        class: "text-nowrap",
        sortable: true
    }, {
        field: "BAreaName",
        title: "Area Name",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    },
    {
        field: "BLatitude",
        title: "Latitude",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    },
    {
        field: "BLongitude",
        title: "Longitude",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    }];

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: "server",
        showColumns: false,
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: "10",
        queryParams: function(params) {
            params.SearchName = $("#mySearch input[name=searchText]").val();
            return params;
        },
        formatNoMatches: function() {
            return '<span class="noMatches">-</span>';
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: "/EMCS/AreaEmcsPage",
        urlPaging: "/EMCS/AreaEmcsPageXt",
        autoLoad: true
    });

    $("#mySearch").insertBefore($("[name=refresh]"));

});