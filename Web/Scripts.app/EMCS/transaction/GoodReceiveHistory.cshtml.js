$tableHistory = $('#tableGoodReceiveHistory');
$tableProblemHistory = $('#tableGoodReceiveProblemHistory');

var columnProblemHistory =
    [
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
            field: 'Category',
            title: 'Category',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Case',
            title: 'Cases',
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
            field: 'CaseDate',
            title: 'Case Date',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY hh:mm:ss");
            }
        },
        {
            field: 'PIC',
            title: 'PIC',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        }];

var columnHistory = [
    {
        field: 'IdGR',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: runningFormatter
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
        field: 'CreateBy',
        title: 'PIC',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }];

$(function () {
    $tableProblemHistory.bootstrapTable({
        cache: false,
        url: "/Emcs/GetProbemList",
        pagination: true,
        search: false,
        queryParams: function (params) {
            params.Id = $("#IdGR").val();
            params.Cat = "rg";
            return params;
        },
        striped: false,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp.data, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        columns: columnProblemHistory
    });

    $tableHistory.bootstrapTable({
        cache: false,
        url: myApp.fullPath + "Emcs/GetGRHistoryList",
        pagination: true,
        search: false,
        striped: false,
        queryParams: function (params) {
            return { Id: $("#IdGR").val() }
        },
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        columns: columnHistory
    });

    //window.pis.table({
    //    objTable: $tableHistory,
    //    urlSearch: '/EMCS/CiplHistoryPage/'+$("#idCipl").val(),
    //    urlPaging: '/EMCS/CiplHistoryPageXt',
    //    autoLoad: true
    //});

});