setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

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

    $("#Surplus").val($("#SurplusValue").val()).change();
    initTrackingStatus('tblTrackingStatus', $("#InventoryID").val());
    initAllocation('tblAllocation', $("#KAL").val());
    initTrackingDelivery('tblTrackingDelivery', $("#InventoryID").val());
});

function loadDialog(url) {
    enableLink(false);
    $('#myModalContent').load(url, function () {
        $('html').css('overflow', 'hidden');
        $('body').css('overflow', 'hidden');

        $('#myModalPlace').modal({ keyboard: true }, 'show');
        bindForm(this);
        enableLink(true);
    });

    $('#myModalPlace').on('shown.bs.modal', function (e) {
        showMainScrollbars(false);
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    });
    $('#myModalPlace').on('hidden.bs.modal', function (e) {
        showMainScrollbars(true);
    });
};

function showMainScrollbars(v) {
    if (v) {
        $('html').css('overflow', '');
        $('body').css('overflow', '');
        $(".modal-dialog").css('width', '800px').css('top', '');
        $(".modal-dialog").removeClass("width").removeClass("top");
    }
    else {
        $('html').css('overflow', 'hidden');
        $('body').css('overflow', 'hidden');
        $('#myModalPlace').css('overflow', 'auto');
    }
};

function initTrackingStatus(_table, InventoryID) {
    $("#" + _table).bootstrapTable({
        url: '/cat/getTrackingStatus/',
        toolbar: '#toolbar-trackstatus',
        toolbarAlign: 'left',
        striped: true,
        cache: false,
        queryParams: function (p) {
            return {
                InventoryID: InventoryID,
                searchkey: $('#status').val()
            }
        },
        pagination: true,
        pageSize: '5',
        search: false,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            { field: 'action', title: 'Action', width: '10%', align: 'center', formatter: detailTrackStatus, switchable: false },
            //{ field: 'ID', title: 'ID', halign: 'center', align: 'left', sortable: true, switchable: false, visible: false },
            //{ field: 'PartNumber', title: 'Part Number', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'StoreNumber', title: 'Store', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'SOS', title: 'SOS', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'DocNumber', title: 'Doc Number', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'UnitNumber', title: 'Unit Number', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'SerialNumber', title: 'Serial Number', halign: 'center', align: 'left', sortable: true, switchable: false },
            //{ field: 'Notes', title: 'Notes', halign: 'center', align: 'left', sortable: true, switchable: false },
            { field: 'LastUpdate', title: 'Date', halign: 'center', align: 'center', formatter: dateFormatterCAT, sortable: true, switchable: false },
            { field: 'Status', title: 'Status', halign: 'center', align: 'center', sortable: true, switchable: false }
        ]
    });
}

function initAllocation(_table, KAL) {
    var $AllowDelete = $('#AllowDelete').val();
    var $AllowUpdate = $('#AllowUpdate').val();
    window.operatingEvents = {
        'click .edit': function (e, value, row, index) {
            $(".editRecord").attr('href', '/cat/EditAllocation/?ID=' + row.ID + '&KAL=' + row.KAL).trigger('click');
        },
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
                    return deleteThis(row.ID, row.KAL);
                }
            });
        }
    };

    $("#" + _table).bootstrapTable({
        url: '/cat/getAllocation/',
        toolbar: '#toolbar-allocation',
        toolbarAlign: 'left',
        striped: true,
        cache: false,
        queryParams: function (p) {
            return {
                KAL: KAL,
                searchkey: $('#invallocation').val()
            }
        },
        pagination: true,
        pageSize: '5',
        search: false,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },

        columns: [
            { field: 'action', title: 'Action', width: '150px', align: 'center', formatter: formatterAllocation({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete) }), events: operatingEvents, switchable: false },
            { field: 'status', title: 'Status', width: '150px', align: 'center', formatter: formatterAllocationStatus, events: operatingEvents, switchable: false },
            { field: 'ID', visible: false },
            //{ field: 'AllocationDate', title: 'Allocation Date', halign: 'center', width: '120px', align: 'left', formatter: dateFormatter, sortable: true, switchable: false },
            { field: 'OriginalSchedule', title: 'Original Schedule', halign: 'center', width: '120px', align: 'left', formatter: dateFormatterCAT, sortable: true, switchable: false },
            { field: 'UnitNo', title: 'Unit No.', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'SerialNo', title: 'Serial No.', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'PONumber', title: 'PO No.', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'Customer', title: 'Customer', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            //{ field: 'Cycle', title: 'Cycle', width: '100px', align: 'center', formatter: formatterCycle, events: operatingEvents, switchable: false },
            //{ field: 'CreatedBy', title: 'Created By', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            //{ field: 'CreatedDate', title: 'Created Date', halign: 'center', width: '120px', align: 'left', formatter: dateFormatterCAT, sortable: true, switchable: false },
            { field: 'UpdatedBy', title: 'Updated By', halign: 'center', width: '120px', align: 'left', sortable: true, switchable: false },
            { field: 'UpdatedDate', title: 'Updated Date', halign: 'center', width: '120px', align: 'left', formatter: dateFormatterCAT, sortable: true, switchable: false }
        ]
    });

}

function initTrackingDelivery(_table, ID) {
    $("#" + _table).bootstrapTable({
        url: '/cat/getTrackingDelivery/',
        toolbar: '#toolbar-trackdelivery',
        toolbarAlign: 'left',
        striped: true,
        cache: false,
        queryParams: function (p) {
            return {
                InventoryID: ID,
                searchkey: $('#status').val()
            }
        },
        pagination: true,
        pageSize: '5',
        search: false,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            { field: 'action', title: 'Action', width: '10%', align: 'center', formatter: detailTrackDelivery, switchable: false },
            //{ field: 'ID', title: 'ID', halign: 'center', width: '40%', align: 'left', sortable: true, switchable: false, visible: false },
            { field: 'DANumber', title: 'DA Number', halign: 'center', align: 'center', sortable: true, switchable: false },
            { field: 'TrackingDate', title: 'Date', halign: 'center',align: 'center', formatter: dateFormatterCAT, sortable: true, switchable: false }
        ]
    });
}

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

function formatterAllocation(options) {
    var btn = [];

    btn.push('<div class="btn-group">');
    if (options.Add == true)
        btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    if (options.Edit == true)
        btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    if (options.Delete == true)
        btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    if (options.Info == true)
        btn.push('<button type="button" class="btn btn-info info" title="Info"><i class="fa fa-info-circle"></i></button>')

    btn.push('</div>');

    return btn.join('');
}

function formatterAllocationStatus(value, row, index) {
    if (row.Cycle > 0) {
        if (row.IsActive == true && row.IsUsed == false) {
            return "Cycle " + row.Cycle + " - <div class='label label-success' style='white-space:nowrap;'>Active</div>";
        } else {
            return "Cycle " + row.Cycle;
        }
    }

    return "<div class='label label-danger' style='white-space:nowrap;font-size: 11px;'>Inactive</div>";
    //if (row.IsActive == true && row.IsUsed == false) {
    //    return "<div class='label label-success' style='white-space:nowrap;font-size: 11px;'>Active</div>";
    //} else {
    //    return "<div class='label label-danger' style='white-space:nowrap;font-size: 11px;'>Inactive</div>";
    //}
}

function formatterCycle(value, row, index) {
    if (row.Cycle > 0) {
        return "Cycle " + row.Cycle;
    } else {
        return "";
    }
}

formatterAllocation.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

function deleteThis(id, kal) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'cat/DeleteAllocation',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined) {
                sAlert('Success', d.Msg, 'success');

            }
            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
};

function detailTrackStatus(value, row, index) {
    return [
        '<div class="btn-group">',
        '<button type="button" class="btn btn-info detail" onclick="viewStatus(\''
        + row.ID + '\', \'' + row.Status + '\')" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}

function viewStatus(ID, status) {
    //alert('Tracking ID:' + ID);
    switch (status) {
        case "OH":
            loadDialog('/cat/TrackingStatusDetailOH/?ID=' + ID);
            break;
        case "WOC":
            loadDialog('/cat/TrackingStatusDetailWOC/?ID=' + ID);
            break;
        case "TTC":
            loadDialog('/cat/TrackingStatusDetailTTC/?ID=' + ID);
            break;
        case "SQ":
        case "BER":
        case "WIP":
            loadDialog('/cat/TrackingStatusDetailCMS/?ID=' + ID + '&Status=' + status);
            break;
        case "JC":
            loadDialog('/cat/TrackingStatusDetailJC/?ID=' + ID);
            break;
        case "ST":
            loadDialog('/cat/TrackingStatusDetailST/?ID=' + ID);
            break;
        case "IA-":
        case "IA+":
            loadDialog('/cat/TrackingStatusDetailIA/?ID=' + ID);
            break;
    }
}

function detailTrackDelivery(value, row, index) {
    return [
        '<div class="btn-group">',
        '<button type="button" class="btn btn-info detail" onclick="viewDelivery(\''
        + row.DANumber + '\')" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}

function viewDelivery(DANo) {
    var ID = $("#InventoryID").val()
    //alert('DA Number:' + DANo);
    loadDialog('/cat/TrackingDeliveryDetail/?InventoryID=' + ID + '&DANumber=' + DANo);
}

function goToParent() {
    enableFilter(true);
    $("#detail").empty();
    $("#detail").hide();
    $("#parent").show();
    enableLink(true);
}

function SaveSurplus() {
    enableLink(false);
    $.ajax({
        method: "POST",
        url: myApp.root + 'cat/UpdateSurplus',
        data: { InventoryID: $("#InventoryID").val(), Surplus: $("#Surplus").val(), Remarks: $("#Remarks").val() },
        dataType: "json",
        success: function (d) {
            if (d.Msg != undefined)
                sAlert('Success', d.Msg, 'success');

            enableLink(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            enableLink(true);
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
