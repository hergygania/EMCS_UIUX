var $table = $('#tableDTSMilestone');

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        showExport: false,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'Milestone', title: 'Milestone', halign: 'center', width: '200px', align: 'left', formatter: 'linkFormatter', sortable: true },
            { field: 'SalesOrder', title: 'Sales Order', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            //{ field: 'Workable', title: 'Workable', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'Allocation', title: 'Allocation', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            //{ field: 'RA', title: 'RA', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'OD', title: 'Out Bound Delivery', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
            { field: 'Depature', title: 'Depature', width: '200px', halign: 'center', align: 'right', formatter: dateFormatter, sortable: true },
            { field: 'GI', title: 'Good Issue', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, },
        { field: 'Arrival', title: 'Arrival', width: '200px', halign: 'center', align: 'right', sortable: true, visible: true, }
        ]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/DeliveryTrackingStatus/IndexPageLogImport',
        urlPaging: '/DeliveryTrackingStatus/GetLogImportPrimeProduct',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))
});

function linkFormatter(value, row, index) {
    return "<a href='" + row.Url + "'>" + value + "</a>";
}

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {

                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("[name=refresh]").trigger('click');
                }
                else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                }
            }
        });
        return false;
    });
};