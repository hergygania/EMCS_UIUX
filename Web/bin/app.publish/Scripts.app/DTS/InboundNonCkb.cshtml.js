$table = $('#tableInboundNonCkb');
function showLoading() {
    $.LoadingOverlay("show", {
        image: "",
        fontawesome: "fa fa-cog fa-spin"
    });
}
function hideLoading() {
    $.LoadingOverlay("hide");
}
function deleteInbound(PONo,SN) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/InboundDelete',
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            //hideLoading();
        },
        data: { PONo: PONo, SerialnumberInbound: SN },
        dataType: "json",
        success: function (d) {
            hideLoading();
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');
            }
            //window.location.reload();
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

}
function formatterPO(data, index, row) {
    var btns = [];
    btns.push("<div>");
    if (allowDelete === "True") btns.push("<button class='remove btn btn-danger btn-xs'><i class='fa fa-remove'></i></button> ");
    if (allowUpdate === "True") btns.push("<button class='edit btn btn-primary btn-xs'><i class='fa fa-edit'></i></button>");
    btns.push("</div>");
    return btns.join('');
}
window.eventPO = {
    'click .remove': function (e, value, row, index) {
        swal({
            title: "Are you sure want to delete this data?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F56954",
            confirmButtonText: "Yes",
            cancelButtonText: "No",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                sweetAlert.close();
                console.log(row);
                return deleteInbound(row.PONo, row.SerialNumber);
            }
        });
    },
    'click .edit': function (e, value, row, index) {
        window.location.href = myApp.root + 'DTS/FormInbound?ID=' + row.PONo + '&SN='+row.SerialNumber;
    }
};
var columnList = [
    {
        field: 'PONo',
        title: 'ACTION',
        formatter: formatterPO,
        events: eventPO,
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'AjuNo',
        title: 'AJU NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'PONo',
        title: 'PO NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'MSONo',
        title: 'MSO NO',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'SerialNumber',
        title: 'SERIAL NUMBER',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'BatchNumber',
        title: 'BATCH NUMBER',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'LoadingPort',
        title: 'LOADING PORT',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'DischargePort',
        title: 'DISCHARD PORT',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'Model',
        title: 'MODEL',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    {
        field: 'ModelDescription',
        title: 'MODEL DESCRIPTION',
        halign: 'center',
        class: 'text-nowrap',
        align: 'left',
        sortable: true
    },
    {
        field: 'Status',
        title: 'STATUS',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    },
    //{
    //    field: 'ETAPort',
    //    title: 'ETA PORT',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ETACakung',
    //    title: 'ETA CAKUNG',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ATAPort',
    //    title: 'ATA PORT',
    //    halign: 'center',
    //    class: 'text-nowrap',
    //    align: 'center',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'ATACakung',
    //    title: 'ATA CAKUNG',
    //    halign: 'center',
    //    align: 'center',
    //    class: 'text-nowrap',
    //    sortable: true,
    //    formatter: dateFormatterCAT
    //},
    //{
    //    field: 'Position',
    //    title: 'POSITION',
    //    halign: 'center',
    //    align: 'left',
    //    class: 'text-nowrap',
    //    sortable: true
    //},
    {
        field: 'Notes',
        title: 'NOTES 1',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: 'Remark',
        title: 'NOTES 2',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    }, {
        field: 'Plant',
        title: 'PLANT',
        class: 'nowrap text-nowrap',
        halign: 'center',
        align: 'left',
        sortable: true
    }];
function operateFormatter(options) {
    var btn = [];
    btn.push('<button onclick = "showHistoryTable()" class="btn btn-xs btn-circle">');
    btn.push('<i class="fa fa-plus-circle"></i>');
    btn.push('</button>');
    return btn.join('');
}
function showHistoryTable() {
    $("#myModalPlace").modal({
        keyboard: true
    }, "show");
}
function showFormDetail() {
    $("#inboundDetailOnce").show(500);
}
function showHistory() {
    $('#myModalPlace').modal({
        keyboard: true
    }, 'show');
}
$(function () {
    $("#moreFilter").click(function () {
        $("#formAdvanceSearch").toggle(700);
    });
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: false,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        detailView: true,
        showExport: false,
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        onExpandRow: function (index, row, $detail) {
            $detail.html('<span class="text-center" style="font-size:16px;"><i class="fa fa-spinner fa-pulse fa-fw"></i></span> Loading, please wait...');
            $.ajax({
                url: "/DTS/PartialListDetail",
                dataType: "html",
                method: 'GET',
                data: { InboundID: row.PONo, InboundSN: row.SerialNumber },
                success: function (resultHtml) {
                    $detail.html(resultHtml);
                },
                error: function (e) {
                    $detail.html("Data not found");
                }
            });
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/InboundNonCkbManagementPage',
        urlPaging: '/DTS/InboundNonCkbManagementPageXt',
        autoLoad: true,
        searchParams: {
            searchName: $("input[name=searchText]").val()
        }
    });

    $("#mySearch").insertBefore($("[name=refresh]"));
});