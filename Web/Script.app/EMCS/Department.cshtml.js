var $table = $('#tableDepartment');
var $searchInput = $("#txtSearchData").val();

var columnList = [
    {
        field: "DepartmentId",
        title: "Department ID",
        halign: "center",
        align: "center",
        class: "text-nowrap",
        sortable: true
    }, {
        field: "DepartmentName",
        title: "Department Name",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    }, {
        field: "CreateBy",
        title: "Created By",
        halign: "center",
        align: "left",
        class: "text-nowrap",
        sortable: true
    }, {
        field: "CreateDate",
        title: "Created Date",
        halign: "center",
        align: "center",
        class: "text-nowrap",
        sortable: true,
        formatter: function(data) {
            return moment(data).format("DD MMM YYYY");
        }
    }
];

function bindForm(dialog) {
    $("form", dialog).submit(function() {
        $("#progress").show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(result) {
                if (result.Status === 0) {
                    if (result.Msg != undefined) sAlert("Success", result.Msg, "success");
                    $("#myModalPlace").modal("hide");
                    $("#progress").hide();
                    $("[name=refresh]").trigger("click");
                } else {
                    if (result.Msg != undefined) sAlert("Failed", result.Msg, "error");
                    $("#progress").hide();
                }
            }
        });
        return false;
    });
};

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        toolbar: '.toolbar',
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
        urlSearch: '/EMCS/DepartmentPage',
        urlPaging: '/EMCS/DepartmentPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
});