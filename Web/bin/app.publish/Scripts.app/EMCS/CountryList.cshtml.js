var $table = $("#tableCountry");
var $searchInput = $("#txtSearchData").val();

function SetAsEmbargo(row, stat) {
    console.log(row);
    $.ajax({
        url: "/EMCS/SetAsEmbargoCountry",
        type: "POST",
        data: { Id: row.Id, IsEmbargoCountry: stat },
        success: function (resp) {
            if (resp.Status === 0) {
                swal("Update Confirmation!", resp.Msg, "success");
            } else {
                swal("Update Failed!", resp.Msg, "error");
            }
        }
    });
}

var columnList = [
    {
        field: "IsEmbargoCountry",
        title: "Is <br>Embargo",
        width: "100px",
        halign: "center",
        checkbox: true,
        align: "center",
        class: "text-nowrap",
        sortable: true
    }, {
        field: "CountryCode",
        title: "Country Code",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    },
    {
        field: "Description",
        title: "Description",
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
        checkboxHeader: false,
        showColumns: false,
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: "10",
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
        urlSearch: "/EMCS/CountryEmcsPage",
        urlPaging: "/EMCS/CountryEmcsPageXt",
        autoLoad: true
    });

    $("#mySearch").insertBefore($("[name=refresh]"));

    $table.on("check.bs.table",
        function (e, row) {
            SetAsEmbargo(row, true);
        });

    $table.on("uncheck.bs.table",
        function (e, row) {
            SetAsEmbargo(row, false);
        });
});