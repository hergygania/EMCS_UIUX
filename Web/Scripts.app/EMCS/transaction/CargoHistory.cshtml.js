$tableHistory = $('#tableCargoHistory');
$tableCargoProblemHistory = $('#tableCargoProblemHistory');
$tableNpePebProblemHistory = $('#tableNpePebProblemHistory');

var columnHistory = [
    {
        field: '',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: runningFormatterNoPaging
    },
    //{
    //    field: 'Flow',
    //    title: 'Flow',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true
    //},
    {
        field: 'Step',
        title: 'Step',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Status',
        title: 'Status',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'ViewByUser',
        title: 'View By User',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Notes',
        title: 'Notes',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CreateDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY hh:mm:ss");
        }
    },
    {
        field: 'CreateBy',
        title: 'PIC',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }];
var columnProblemHistory = [
    {
        field: '',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: runningFormatterNoPaging
    },
    {
        field: 'Category',
        title: 'Category',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Case',
        title: 'Case',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Causes',
        title: 'Causes',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Impact',
        title: 'Impact',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Comment',
        title: 'Note',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CaseDate',
        title: 'Case Date',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Pic',
        title: 'PIC',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }];

$(function () {
    $tableHistory.bootstrapTable({
        url: myApp.fullPath + "emcs/GetCargoHistory",
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        showRefresh: true,
        queryParams: function (params) {
            return { Id: $("#CargoID").val() };
        },
        clickToSelect: false,
        sidePagination: 'client',
        responseHandler: function (resp) {
            return resp;
        },
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        columns: columnHistory
    });

    $tableCargoProblemHistory.bootstrapTable({
        url: myApp.fullPath + "/Emcs/GetProbemList",
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        showRefresh: true,
        queryParams: function (params) {
            return {
                Id: $("#idCargo").val(),
                Cat: "Cargo"
            };
        },
        clickToSelect: false,
        sidePagination: 'server',
        responseHandler: function (resp) {
            var data = {};
            $.map(resp.data, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        columns: columnProblemHistory
    });

    $tableNpePebProblemHistory.bootstrapTable({
        url: myApp.fullPath + "/Emcs/GetProbemList",
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        showRefresh: true,
        queryParams: function (params) {
            return {
                Id: $("#idCargo").val(),
                Cat: "NpePeb"
            };
        },
        clickToSelect: false,
        sidePagination: 'server',
        responseHandler: function (resp) {
            var data = {};
            $.map(resp.data, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        columns: columnProblemHistory
    });
});