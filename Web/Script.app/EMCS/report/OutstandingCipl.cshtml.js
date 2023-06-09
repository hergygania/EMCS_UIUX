var $table = $('#tbl-outstandingcipl');

$(function () {
    getOriginList();
    getDestinationList();

    var columns = [
        //{
        //    field: "DestinationCountry",
        //    title: "Cycle",
        //    sortable: true,
        //    rownspan: 2,
        //    align: "left",
        //    halign: "center",
        //    class: "text-nowrap"
        //},
        {
            field: "PicName",
            title: "PIC Name",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "Department",
            title: "Department",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "Branch",
            title: "Branch",
            sortable: true,
            rownspan: 2,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "CiplNo",
            title: "CIPL No",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "Status",
            title: "Status",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            field: "SubmitDate",
            title: "Submit Date",
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap",
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            },
        }
    ]


    $table.bootstrapTable({
        //url: "/EMCS/GetOutstandingCiplList",
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        onBeforeLoad: function (row, param) {
            param.Origin = '';
            param.Destination = '';
            return param;
        }
    });

    var StartDate = $("#inp-start-date").val();
    var EndDate = $("#inp-end-date").val();

    reportOutstandingCipl(StartDate, EndDate);
});

function reportOutstandingCipl(StartDate, EndDate) {

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/ROutstandingCiplListPage?StartDate=' + StartDate + '&EndDate=' + EndDate,
        urlPaging: '/EMCS/ROutstandingCiplPageXt?StartDate=' + StartDate + '&EndDate=' + EndDate,
        autoLoad: true
    });

}

function getOriginList() {
    $.ajax({
        url: "/emcs/GetAreaList",
        success: function (data) {
            $("#origin").select2({
                data: data,
                width: '100%',
                placeholder: 'All',
            })
            $('#origin').val(null).trigger('change');
        }
    });
}

function getDestinationList() {
    $.ajax({
        url: "/emcs/GetCountryList",
        success: function (data) {
            $("#destination").select2({
                data: data,
                width: '100%',
                placeholder: 'All',
            })
            $('#destination').val(null).trigger('change');
        }
    });
}

function searchDataReport() {
    StartDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    EndDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');

    reportOutstandingCipl(StartDate, EndDate);
}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    window.open('/EMCS/DownloadOutstandingCipl?StartDate=' + startDate + '&EndDate=' + endDate, '_blank');
}