$table = $('#tableOutboundNonCkb');
var visible = false;
if (allowUpdate === "True" || allowDelete === "True") {
    visible = true;
}
function showDetail() {
    $("#formDetail").show(500);
}
function deleteFormatter(data, index, row) {
    var htm = [];
    htm.push('<div class="">');
    if (allowDelete === "True") htm.push("<button class='remove btn btn-danger btn-xs'><i class='fa fa-remove'></i></button>");
    if (allowUpdate === "True") htm.push("<button class='edit btn btn-primary btn-xs'><i class='fa fa-edit'></i></button>");
    htm.push('</div>');
    return htm.join(' ');
}
function showLoading() {
    $.LoadingOverlay("show", {
        image: "",
        fontawesome: "fa fa-cog fa-spin"
    });
}
function hideLoading() {
    $.LoadingOverlay("hide");
}
function deleteOutbound(Da) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/OutboundDelete',
        beforeSend: function () {
            //showLoading();
        },
        complete: function () {
            //hideLoading();
        },
        data: { DA: Da },
        dataType: "json",
        success: function (d) {
            console.log(d);
            //hideLoading();
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
window.operateEvents = {
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
                return deleteOutbound(row.ID);
            }
        });
    },
    'click .edit': function (e, value, row, index) {
        debugger;
        console.log(row.ID);
        window.location.href = myApp.root + 'DTS/FormOutbound?ID=' + row.ID;
    }
};
var $columnList = [
    { field: 'DA', title: 'ACTION', formatter: deleteFormatter, halign: 'center', events: operateEvents, class: 'text-center text-nowrap', sortable: false },
    { field: 'ID', title: 'DA', halign: 'center', visible: false, align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'DA', title: 'DA', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'DI', title: 'DI', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'DIDate', title: 'DI Date', halign: 'center', align: 'left', sortable: true, class: 'text-nowrap', formatter: dateFormatterCAT },
    { field: 'Moda', title: 'MODA', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'UnitModa', title: 'UNIT MODA', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'Origin', title: 'ORIGIN', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'Destination', title: 'DESTINATION', halign: 'center', align: 'left', class: "text-nowrap", sortable: true },
    { field: 'UnitType', title: 'UNIT TYPE', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'Model', title: 'MODEL', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'SerialNumber', title: 'SERIAL NUMBER', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'ETD', title: 'ETD', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true, formatter: dateFormatterCAT },
    { field: 'ETA', title: 'ETA', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true, formatter: dateFormatterCAT },
    { field: 'ATD', title: 'ATD CAKUNG', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true, formatter: dateFormatterCAT },
    { field: 'ATA', title: 'ATA', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true, formatter: dateFormatterCAT },
    { field: 'Status', title: 'STATUS', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'Position', title: 'POSITION', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true },
    { field: 'Remarks', title: 'NOTES', halign: 'center', align: 'left', class: "text-center text-nowrap", sortable: true }];

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        reorderableColumns: false,
        columns: $columnList,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        //onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10'
        //fixedColumns: true,
        //data: $dataList,
        //fixedNumber: '5',
    });
    window.pis.table({
        objTable: $table,
        urlSearch: '/DTS/OutboundNonCkbManagementPage',
        urlPaging: '/DTS/OutboundNonCkbManagementPageXt',
        searchParams: {
            searchName: $('input[name=searchText]').val()
        },
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));
    $("#moreFilter").click(function () {
        $("#formAdvanceSearch").toggle(700);
    });
});