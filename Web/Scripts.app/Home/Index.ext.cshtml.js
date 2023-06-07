var $table = $('#tableToDoList');

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
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { field: 'Name', title: 'Name', halign: 'center', align: 'left', formatter: 'linkFormatter', sortable: true },
            { field: 'Value', title: 'Total Item', width: '305px', halign: 'center', align: 'right', sortable: true },
            { field: 'TotalValue', title: 'Total Value', width: '305px', halign: 'center', formatter: 'formatNumber', align: 'right', sortable: true, visible: true }
        ]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/Home/IndexPageImex',
        urlPaging: '/Home/GetListToDoListImex',
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
