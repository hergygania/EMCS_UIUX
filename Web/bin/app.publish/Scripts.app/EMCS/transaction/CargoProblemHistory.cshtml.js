$tableProblemHistory = $('#tableCargoProblemHistory');

var columnProblemHistory = [
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
        title: 'CargoCase',
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

$(function () {
    $tableProblemHistory.bootstrapTable({
        cache: false,
        striped: false,
        url: "/Emcs/GetProbemList",
        pagination: true,
        search: false,
        data: {
            Id: $("#idCargo").val(),
            Cat: "CL"
        },
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">Data not found</span>';
        },
        columns: columnProblemHistory
    });

    //window.pis.table({
    //    objTable: $tableProblemHistory,
    //    urlSearch: '/EMCS/CargoProblemHistoryPage/' + $("#idCipl").val(),
    //    urlPaging: '/EMCS/CargoProblemHistoryPageXt',
    //    autoLoad: true
    //});

});