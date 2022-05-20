var Role = $('#Role').val();
var dropDownDeliveryTypeVisible = dropDownDeliveryTypeVisible ?? false;
var currentStep = currentStep ?? 'process';
var dropDownShippingVisible = dropDownShippingVisible ?? false;
var statusPoVisible = statusPoVisible ?? false;
var deliveryTypeVisible = deliveryTypeVisible ?? false;
var shippingByVisible = shippingByVisible ?? false;
var AtdVisible = AtdVisible ?? true;
var AtaVisible = AtaVisible ?? true;
var GRVisible = GRVisible ?? true;
var canEdit = canEdit ?? false;




if (($('#Role').val() === "Administrator,POSTEXPEDITOR") && $("#Shipment").val() === "PTTU") {
    canEdit = true;
}


var btnViewNote = `<a class="btn btn-light btn-xs" onClick="InitModalViewNotes()" href="#" title="View Notes"><i class="fa fa-list" title="View Notes"></i></a>`;
var btnUpdate = canEdit ? `<a class="btn btn-light btn-xs" onClick="InitModalUpdateStatus()" href="#" title="Update Status" data-role="updatestatus"><i class="fa fa-edit" title="View Notes"></i></a>` : ``;

var ActionItem = [btnViewNote, btnUpdate].join('&nbsp;');
var dropDownDeliveryType = `<select class='form-control select2'><option value='' selected>Full Delivery</option><option value=''>Partial Delivery</option></select>`;
var statusEtdAtd = statusEtdAtd ?? false;
var requestId = parseInt($('#requestId').val());
var isTaskUser = $('#isTaskUser').val();
var UpdateItemVarType = "ALL";
var IdItem = 0;
var PrePayment = $('#PrePayment').val();
var Shipment = $('#Shipment').val();
var BastVisible = BastVisible ?? false;
var RemoveHCVisible = RemoveHCVisible ?? false;
if (Role == "Administrator,POSTVENDOR") {
    RemoveHCVisible = true
}
var InvoiceVisible = InvoiceVisible ?? false;
if (Shipment == "") {
    if (PrePayment == 1) {
        BastVisible = true;
        InvoiceVisible = true
    }
    else {
        BastVisible = false;
        InvoiceVisible = false
    }

}
else {
    if (Role == "Administrator,POSTTAX") {
        BastVisible = false
    }
    else {
        BastVisible = true;
        InvoiceVisible = true
    }

}

var CountBASTUpload = 0
var CountBASTNotUpload = 0
var typePO = $('#TypePO').val();
var titleATD = typePO != "D" ? "ATD" : "Actual Start Date";
var titleATA = typePO != "D" ? "ATA" : "Actual Complete Date";
var titleETD = typePO != "D" ? "ETD" : "Plan Start Date";
var titleETA = typePO != "D" ? "ETA" : "Plan Complete Date";
var titlePosition = typePO != "D" ? "Position" : "Progress %";
var visibleCKB = typePO != "D" ? true : false;
var visibleProgressUpload = typePO != "D" ? false : true;
var counterTblQty = 0;
var ckbClick = 0;
function showLoading() {
    $("#loadingModal").modal({ backdrop: 'static', keyboard: false });
    $("#loadingModal").modal("show");
}
function closeLoading() {
    $("#loadingModal").modal("hide");
}

var columnsGr = [{
    title: 'No',
    align: 'center',
    halign: 'center',
    class: 'text-nowrap',
    width: '10',
    formatter: runningFormatterNoPaging
}, {
    title: 'GR Number',
    field: 'GRNo',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '100'
},
{
    title: 'GR Date',
    field: 'GRDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter

},
{
    title: 'GR Posting Date',
    field: 'GRPostingDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'Amount',
    field: 'GRValue',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: currencyFormatter
}]
//{
//    title: 'Progress Percent',
//    field: 'GRProgressPercent',
//    class: 'text-nowrap',
//    halign: 'center',
//    align: 'left',
//    width: '450',
//}]

var columnsInv = [{
    title: 'No',
    align: 'center',
    halign: 'center',
    class: 'text-nowrap',
    width: '10',
    formatter: runningFormatterNoPaging
}, {
    title: 'Inv. No',
    field: 'InvoiveNo',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '100'
},
{
    title: 'Inv. Date',
    field: 'InvDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter

},
{
    title: 'Inv. Posting Date',
    field: 'InvPostingDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'Inv. Payment Date',
    field: 'InvPaymentDate',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
}]
//{
//    title: 'Progress Percent',
//    field: 'InvProgressPercent',
//    class: 'text-nowrap',
//    halign: 'center',
//    align: 'left',
//    width: '450',
//}]

var columnsCKB = [{
    title: 'No',
    align: 'center',
    halign: 'center',
    class: 'text-nowrap',
    width: '10',
    formatter: runningFormatterNoPaging
}, {
    title: 'PO No',
    field: 'PONo',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '100'
},
{
    title: 'DA No',
    field: 'DANo',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',

},
{
    title: 'origin',
    field: 'origin',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'service id',
    field: 'service_id',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'description',
    field: 'description',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'tracking station desc',
    field: 'tracking_station_desc',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'Tracking Date',
    field: 'tracking_date',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'ETD',
    field: 'etd',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'ATD',
    field: 'atd',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'ETA',
    field: 'eta',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'ATA',
    field: 'ata',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
},
{
    title: 'Etl Date',
    field: 'etl_date',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: dateSAPFormatter
}, {
    title: 'Status',
    field: 'Status',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'Cost',
    field: 'Cost',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'Dimension',
    field: 'Dimension',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'Weight',
    field: 'Weight',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'height',
    field: 'Height',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
},
{
    title: 'Volume',
    field: 'Volume',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
}]

function operateFormatterShippingBy(value, row, index) {
    var options = ''
    if (typePO == "D") {
        options += '<option  value="SERVICE"' + ("SERVICE" === value ? 'selected' : '') + '>Service</option>'
        return '<select disabled class="form-control select2 select2-table" onchange="UpdateShipment();" id="Select2Shipment">' + options + '</select>'
    } else {
        options += '<option value="Forwarder"' + ("Forwarder" === value ? 'selected' : '') + '>by Vendor</option>'
        options += '<option value="CKB"' + ("CKB" === value ? 'selected' : '') + '>Deliver To CKB</option>'
        options += '<option value="PTTU"' + ("PTTU" === value ? 'selected' : '') + '>by PTTU</option>'
      
        return '<select class="form-control select2 select2-table" onchange="UpdateShipment();" id="Select2Shipment">' + options + '</select>'
    }
}

window.operateEventHeader = {
    'click .showUploadedBAST': function (e, value, row, index) {

    },
    'click .showUploadedInvoice': function (e, value, row, index) {

    },
    'click .GrListItem': function (e, value, row, index) {
        var data = $("#table-inprogress").bootstrapTable("getData");
        var item = data[0];
        var PoNo = item.PO_No;
        $("#PoNo").val(PoNo);
        $("#ItemId").val(row.Item_Id);
        InitTableGrGet(PoNo, row.Item_Id);
        $("#table-gr-list").bootstrapTable('refresh');
        $("#modalGrList").modal("show");
    },
    'click .InvListItem': function (e, value, row, index) {
        var data = $("#table-inprogress").bootstrapTable("getData");
        var item = data[0];
        var PoNo = item.PO_No;
        $("#PoNo").val(PoNo);
        $("#ItemId").val(row.Item_Id);
        InitTableInvoiceGet(PoNo, row.Item_Id);
        $("#table-inv-list").bootstrapTable('refresh');
        $("#modalInvoiceList").modal("show");
    }
}


function InitModalViewCKB(id) {
    ckbClick = 0;
    idItem = id;
    InitTableCKB();

}

function InitTableCKB() {
    $('#modalFormCKB').modal();
    $("#table-CKB").bootstrapTable({
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetDataItemCKBByPO',
        queryParams: function (params) {
            var PoNumber = $('#table-inprogress').bootstrapTable('getData')[0].PO_No;
            var query = {
                poNo: PoNumber,
                poItem: idItem,
            }
            return query;
        },
        responseHandler: function (res) {
            if (ckbClick == 1) {

                return res.result;
            }
            else {
                ckbClick = 1;
                $("#table-CKB").bootstrapTable('refresh');
            }

            return res.result;
        },
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: columnsCKB
    });

}

function InitTableGrGet(PoNo, ItemId) {
    $("#table-gr-list").bootstrapTable('destroy');
    $("#table-gr-list").bootstrapTable({
        cache: false,
        async: false,
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        url: 'GetDataGRByPO',
        queryParams: function (params) {           
            var query = {
                poNo: PoNo,
                ItemId: ItemId,
            }
            return query;
        },        
        responseHandler: function (res) {
            return res.result;
        },
        columns: columnsGr
    });
}
function InitTableInvoiceGet(PoNo, ItemId) {
    $("#table-inv-list").bootstrapTable('destroy');
    $("#table-inv-list").bootstrapTable({
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        url: 'GetDataInvoiceByPO',
        //ajax: GetListInvoiceByRequestId,
        queryParams: function (params) {
            var query = {
                poNo: PoNo,
                ItemId: ItemId,
            }
            return query;
        },
      
        responseHandler: function (res) {
            return res.result;
        },
        columns: columnsInv
    });
}
function InitTableGr() {
    $("#table-gr-list").bootstrapTable({
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        ajax: GetListGrByRequestId,
        columns: columnsGr
    });
}
function InitTableInvoice() {
    $("#table-inv-list").bootstrapTable({
        detailView: false,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        ajax: GetListInvoiceByRequestId,
        columns: columnsInv
    });
}

function InitTableInProgress() {
    $('#table-inprogress').bootstrapTable({
        detailView: false,
        ajax: "GetListPOByRequestId",
        loadingTemplate: "loadingTemplate",
        columns: [{
            title: 'Company',
            field: 'Vendor_Name',
            class: 'text-nowrap',
            align: 'left',
            width: '110',
            visible: VendorNameVisible,
        }, {
            title: 'PO No',
            field: 'PO_No',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'PO Date',
            field: 'PO_Date',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'Delivery Date',
            field: 'Delivery_Date',
            class: 'text-nowrap',
            align: 'center',
            width: '110',
        },
        {
            title: 'Delivery Status PO',
            field: 'Status_PO',
            class: 'text-center',
            align: 'center',
            width: '110',
            visible: statusPoVisible,
            formatter: function (value, row, index) {

                var Processing = row.Processing;
                var data = "";
                var color = "badge-danger";
                if (value != null) {
                    data = value;
                    color = getBadgeColor(value);
                }
                else if (Processing == "current") {
                    data = "CONFIRMED";
                    color = "badge-warning";
                }
                else {
                    data = "OUTSTANDING";
                }

                if (data === "Delay" || data === "delay" || data === "DELAY") {
                    color = "badge-danger";
                }
                return `<span class='badge ` + color + `'>` + data + `</span>`;
            }
        },
        {
            title: 'PO Material',
            field: 'TypePO',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetTypePOChecklist,
            events: operateEventHeader
        },
        {
            title: 'ETA',
            field: 'ETA',
            align: 'center',
            class: 'text-nowrap',
            width: '110',
            visible: false,
            formatter: dateSAPFormatter

        },
        {
            title: 'ATA',
            field: 'ATA',
            class: 'text-nowrap',
            align: 'center',
            width: '110',
            visible: false,
            formatter: function (data, row, index) {
                if (AtdVisible) {
                    return dateSAPFormatter;
                }
                return "-";
            }
        },
        {
            title: 'Shipment',
            field: 'Shipment',
            class: 'text-center select2-column',
            align: 'center',
            visible: dropDownShippingVisible,
            width: '160',
            formatter: operateFormatterShippingBy,
        },
        {
            title: 'Shipment',
            field: 'Shipment',
            class: 'text-center',
            align: 'center',
            visible: shippingByVisible,
            width: '160',
            formatter: function (data, row, index) {
                if (data == "CKB") {
                    return `<a class="btn btn-light btn-xs" onClick="InitModalViewCKB('')" >${data}</a>`;
                } else {
                    return `<a class="btn btn-light btn-xs">${data}</a>`;
                }
            }
        },
        {
            title: 'Confirmation',
            field: 'Confirmation',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetConfirmFlowChecklist,
            events: operateEventHeader
        },
        {
            title: 'Processing',
            field: 'Processing',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklist,
            events: operateEventHeader
        },
        {
            title: 'Delivering',
            field: 'Delivering',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetDeliveryFlowChecklist,
            events: operateEventHeader
        },
        {
            title: 'BAST/COP',
            field: 'BAST',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistBast,
            events: operateEventHeader
        }, {
            title: 'GR/SA',
            field: 'GRSA',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistGRSA,
            events: operateEventHeader
        },
        {
            title: 'Invoice Submited',
            field: 'INVOICING',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistInvoice,
            events: operateEventHeader
        },
        {
            title: 'Invoice Validation',
            field: 'CountKOFAXUpload',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistInvoiceKOFAX,
            events: operateEventHeader
        },
        {
            title: 'Invoice Hardcopy',
            field: 'CountInvoiceHardcopy',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistInvoiceHardcopy,
            events: operateEventHeader
        },
        {
            title: 'Invoice Posted',
            field: 'CountPOSTINGSAP',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistInvoiceSAP,
            events: operateEventHeader
        },
        {
            title: 'Paid',
            field: 'PAID',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistPaidPO,
            events: operateEventHeader
        },
        {
            title: 'Close',
            field: 'INVOICING',
            class: 'text-center',
            align: 'center',
            width: '100px',
            formatter: GetProcessFlowChecklistClosePO,
            events: operateEventHeader
        }]
    });
    $('#table-inprogress').on('post-body.bs.table', function (field, value, row, $element) {
        $('[data-toggle="tooltip"]').tooltip()
    })
}
function InitTableItem() {
    $('#table-item').bootstrapTable({
        pagination: true,
        serverSort: false,
        ajax: "GetListItemByRequestId",
        loadingTemplate: "loadingTemplate",
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: [
            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'text-nowrap',
                width: '10',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'Action',
                field: 'Item_Id',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    var Item_Id = data.Item_Id;
                    if (Role === "Administrator,POSTEXPEDITOR" || Role === "Administrator,POSTVENDOR") {
                        console.log(Role);
                        var btnViewNote = `<a class="btn btn-light btn-xs" onClick="InitModalViewNotes(${Item_Id})"  title="View Notes"><i class="fa fa-history" title="View Notes"></i></a>`;
                        var btnUpdate = `<a class="btn btn-light btn-xs" id="updateStatusItem" onClick="InitModalUpdateStatus(${Item_Id})" title="Update Status" data-role="updatestatus"><i class="fa fa-edit" title="Update Status"></i></a>`;                       
                    }
                    else {
                        var btnViewNote = `<a class="btn btn-light btn-xs" onClick="InitModalViewNotes(${Item_Id})"  title="View Notes"><i class="fa fa-history" title="View Notes"></i></a>`;
                        var btnUpdate = ``;
                    }


                    var ActionItem4 = [btnViewNote, btnUpdate].join('&nbsp;');
                    return ActionItem4
                }
            },
            {
                title: 'GR No',
                field: 'GR',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show GR List' alt='Show GR List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default GrListItem'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            }, {
                title: 'Invoice No',
                field: 'Inv',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: function (data, row, index) {
                    var htm = "<button title='Show Inv. List' alt='Show Inv. List' data-toggle='tooltip' data-placement='top' class='btn btn-xs btn-default InvListItem'><i class='fa fa-list'></i></button>";
                    return htm;
                },
                events: operateEventHeader
            }, {
                title: 'BAST',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: BastVisible,
                formatter: function (row, data, index) {
                    var Item_Id = data.Item_Id;
                    var statusItem = data.Item_Status;
                    var color = (data.TotalBAST == 0) ? 'btn-light' : 'btn-primary';
                    var btnUpload = '';
                    if (Role.includes('POSTBRANCH')) {
                        btnUpload = `<a class="btn ` + color + ` btn-xs" data-toggle="tooltip" data-placement="bottom" title="Enable if status is POD" onClick="InitModalUploadBast(${Item_Id},'${statusItem}')"  title="View Notes"><i class="fa fa-file" title="View Notes"></i></a>`;
                    } else {
                        btnUpload = `<a class="btn ` + color + ` btn-xs" data-toggle="tooltip" data-placement="bottom" title="Enable if status is POD" onClick="InitModalUploadBast(${Item_Id},'${statusItem}')"  title="View Notes"><i class="fa fa-upload" title="View Notes"></i></a>`;
                    }
                    var ActionItem2 = [btnUpload].join('&nbsp;');
                    return ActionItem2
                }
            }, {
                title: 'Invoice',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: InvoiceVisible,
                formatter: function (row, data, index) {
                    var Item_Id = data.Item_Id;
                    var Vendor_Id = data.VendorId;
                    var color = (data.TotalINV === 0) ? 'btn-light' : 'btn-primary';
                    var btnUpload = '';
                    if (Role.includes('POSTBRANCH')) {
                        btnUpload = `<a class="btn ` + color + ` btn-xs" data-toggle="tooltip" data-placement="bottom" title="Enable if BAST is Uploaded" onClick="InitModalUploadInvoice(${Item_Id},${Vendor_Id})"  title="View Notes"><i class="fa fa-file" title="View Notes"></i></a>`;
                    } else {
                        btnUpload = `<a class="btn ` + color + ` btn-xs" data-toggle="tooltip" data-placement="bottom" title="Enable if BAST is Uploaded" onClick="InitModalUploadInvoice(${Item_Id},${Vendor_Id})"  title="View Notes"><i class="fa fa-upload" title="View Notes"></i></a>`;
                    }
                    var ActionItem1 = [btnUpload].join('&nbsp;');

                    return ActionItem1
                }
            }, {
                title: 'HardCopy Invoice',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: InvoiceVisible,
                formatter: function () {                   
                    var btnHardCopy = '';
             
                    btnHardCopy = `<a class="btn btn-light btn-xs" id="updateHardCopy" onClick="InitModalHardCopy()" title=" Submit Hardcopy Invoice" data-role="hardcopyinvoice"><i class="fa fa-file" title="Submit HardCopy Invoice"></i></a>`;
                    var ActionItem1 = [btnHardCopy].join('&nbsp;');

                    return ActionItem1
                }
            }, {
                title: 'PR Number',
                field: 'PR_Number',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '100'
            },
            {
                title: 'Item',
                field: 'Item_Description',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
            {
                title: 'Status',
                field: 'Item_Status',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: statusPoVisible,
                formatter: function (value, row, index) {
                    var partial = row.isPartial;
                    var CountNotPOD = row.CountPartialNotPOD;
                    var CountPOD = row.CountPartialPOD;
                    var qty = row.Qty;
                    var color = 'badge-warning';
                    var data = '';

                    if (value != null) data = value;
                    color = getBadgeColor(value);

                    //for PO Service
                    if (value != "Complete" && row.Shipment == "SERVICE") {
                        color = 'badge-warning';
                    }

                    //for PO Material
                    if (value == "POD" && partial > 0 && CountPOD < partial) {
                        color = 'badge-warning';
                    }
                    else if(partial > 0 && CountPOD < partial){
                        color = 'badge-warning';
                    }
                    return `<span class='badge ` + color + `'>` + data + `</span>`;
                }
            },
            {
                title: titlePosition,
                field: 'POSITION',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '110',
                visible: true,
            },
            {
                title: 'CKB',
                field: 'Shipment',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: visibleCKB,
                formatter: function (value, row, index) {
                    idItem = row.Item_Id;
                    if (value != 'CKB') {
                        return `<a onClick="InitModalViewCKB(${idItem})"><span class="fa fa-minus-circle text-disabled"></span></a>`;
                    }
                    else {
                        return `<a onClick="InitModalViewCKB(${idItem})"><span class="fa fa-check-circle text-primary"></span></a>`;
                    }


                }
            },
            {
                title: 'Delivery Type',
                field: 'Delivery_Type',
                class: 'text-nowrap',
                align: 'center',
                width: '180',
                visible: false,
            },
            {
                title: 'Forwarder',
                field: 'Forwarder',
                class: 'text-center',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: false
            },
            {
                title: titleETD,
                field: 'ETD',
                align: 'center',
                class: 'text-nowrap',
                width: '110',
                visible: AtdVisible,
                formatter: dateSAPFormatter

            }, {
                title: titleETA,
                field: 'ETA',
                align: 'center',
                class: 'text-nowrap',
                width: '110',
                visible: AtaVisible,
                formatter: dateSAPFormatter
            },
            {
                title: titleATD,
                field: 'ATD',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: AtdVisible,
                formatter: dateSAPFormatter
            },

            {
                title: titleATA,
                field: 'ATA',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: AtaVisible,
                formatter: dateSAPFormatter
            },
            {
                title: ' QTY',
                field: 'Qty',
                class: 'text-nowrap',
                align: 'center',
                width: '110',

            },
            {
                title: 'UOM',
                field: 'UOM',
                class: 'text-nowrap',
                align: 'center',
                width: '110',

            },
            {
                title: 'Currency',
                field: 'Currency',
                class: 'text-nowrap',
                halign: 'center',
                align: 'center',
                width: '10',
            },
            {
                title: 'Price / Item',
                field: 'Price_Item',
                class: 'text-nowrap',
                halign: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,

            },
            {
                title: 'Total',
                field: 'Total',
                class: 'text-nowrap',
                align: 'right',
                halign: 'center',
                width: '110',
                formatter: currencyFormatter,

            },
            {
                title: 'Delivery Date',
                field: 'DeliveryDate',
                class: 'text-nowrap',
                align: 'center',
                formatter: dateSAPFormatter,
                width: '180',
            },
            {
                title: 'Destination',
                field: 'Destination',
                class: 'text-nowrap',
                align: 'left',
                halign: 'center',
                width: '200',

            },
            {
                title: 'User',
                field: 'USER',
                class: 'text-nowrap',
                align: 'left',
                width: '110'
            },
            {
                title: 'GR No',
                field: 'GRNo',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                visible: false,
            },
            {
                title: 'GR Date',
                field: 'GRDate',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                visible: false,
                formatter: dateSAPFormatter,
            },
            {
                title: 'GR Posting Date',
                field: 'GRPostingDate',
                class: 'text-nowrap',
                align: 'left',
                width: '110',
                formatter: dateSAPFormatter,
                visible: false,
            },
            {
                title: 'Notes',
                field: 'Notes',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: false
            },
        ]
    });
    $('#table-item').on('post-body.bs.table', function (field, value, row, $element) {
        $('[data-toggle="tooltip"]').tooltip()
    })
}

function GetProcessFlowChecklistClosePO(data, row, index) {

    if (row.INVOICING == "0") {
        return uncheck;
    }
    else {
        return check;
    }
}
function GetProcessFlowChecklistPaidPO(data, row, index) {

    if (row.PAID == 'current') {
        return uploadInProgress;
    }
    else if (row.PAID == 'check') {
        return uploadInDone;
    }
    else if (row.PAID == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}

function SendEmailAttachment(type) {
    var dataInvoice = $('#table-uploadInvoice').bootstrapTable("getData");
    var isValid = true;
    for (i = 0, j = dataInvoice.length; i<j;i++) {
        var idAttachment = dataInvoice[i].ID;
        var BAST = $('#' + idAttachment).select2('val');
        if (BAST == null) {
            isValid = false;
        }
    }

    if (isValid) {
        UpdateInvoice();
        dataMapItem = IdItem;
        $.getJSON("SendEmail?id=" + requestId + "&type=" + type + "&idItem=" + dataMapItem, function (data) {
            RefreshAllTable();
        });

        if(type == "Invoice"){
            $('#modalUploadInvoicing').modal('toggle');
        }
    }
    else {
        swalError("BAST tidak boleh kosong (Jika tidak ada pilihan BAST silahkan untuk dilakukan proses GR terlebih dahulu)");
    }
    
}
function UpdateInvoice() {

    var checked = $("input[name=pajak]:checked").val();


    $.ajax({
        cache: false,
        async: false,
        url: '/Post/UpdateInvoice',
        data: {
            id: requestId,
            suratketerangan: checked
        },
        method: 'POST',
        success: function (res) {

        },
        error: function (err) {

        }
    })
}



function ApproveAttachment(id, type) {

    swal({
        title: " Please Input Approve Note",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Type a description"
    }, function (inputValue) {
        if (inputValue === false) return false;

        if (inputValue === "") {
            swal.showInputError("You must type something!");
            return false;
        }
        $.ajax({
            cache: false,
            async: false,
            url: '/Post/ApproveAttachment',
            data: {
                attachmentId: id,
                approve: true,
                role: Role,
                note: inputValue
            },
            method: 'GET',
            success: function (res) {
                swalSuccess("Success Approve")
                var table = "";
                if (type == "INVOICE") table = "table-uploadInvoice";
                if (type == "BAST") table = "table-uploadBast";
                $('#' + table).bootstrapTable('refresh', {
                    query: {
                        id: IdItem,
                        type: type,
                    }
                });
            },
            error: function (err) {
                swalError("failed Approve");
            }
        })

    })
}


function RejectAttachment(id, type) {

    swal({
        title: " Please Input Reject Note",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Type a description"
    }, function (inputValue) {
        if (inputValue === false) return false;

        if (inputValue === "") {
            swal.showInputError("You must type something!");
            return false;
        }
        $.ajax({
            cache: false,
            async: false,
            url: '/Post/RejectAttachment',
            data: {
                attachmentId: id,
                reject: true,
                rejectnote: inputValue
            },
            method: 'GET',
            success: function (res) {
                swalSuccess("Success Reject");
                var table = "";
                if (type == "INVOICE") table = "table-uploadInvoice";
                if (type == "BAST") table = "table-uploadBast";

                $('#' + table).bootstrapTable('refresh', {
                    query: {
                        id: IdItem,
                        type: type,
                    }
                });
            },
            error: function (err) {
                swalError("failed Reject");
            }
        })

    })


}


function DeleteFileUpload(id, type) {
    if (type == "BAST") {
        $.getJSON("GetListAttachment?id=" + IdItem + "&type=INVOICE", function (data) {
            if (data.result.length > 0) {
                return swalWarning("Cannot Delete BAST,Please Delete Invoice First!");
            }

            $.ajax({
                cache: false,
                async: false,
                url: '/Post/DeleteRequestAttachment',
                data: {
                    id: id,
                },
                method: 'GET',
                success: function (res) {
                    swalSuccess('Successfully Deleted!');
                    $('#table-uploadBast').bootstrapTable('refresh', {
                        query: {
                            id: IdItem,
                            type: "BAST",
                        }
                    });
                    $('#table-item').bootstrapTable('refresh');
                    $('#table-inprogress').bootstrapTable('refresh');
                },
                error: function (err) {
                    swalError("failed delete");
                }
            })

        });
    } else {
        $.ajax({
            cache: false,
            async: false,
            url: '/Post/DeleteRequestAttachment',
            data: {
                id: id,
            },
            method: 'GET',
            success: function (res) {
                swalSuccess('Successfully Deleted!');
                $('#table-uploadInvoice').bootstrapTable('refresh', {
                    query: {
                        id: IdItem,
                        type: "INVOICE",
                    }
                });
                $('#table-item').bootstrapTable('refresh');
                $('#table-inprogress').bootstrapTable('refresh');
            },
            error: function (err) {
                swalError("failed delete");
            }
        })
    }



}

function DownloadFileUpload(id) {
    url = "/POST/DownloadFileRequest?id=" + id;
    window.open(url, '_blank');
}

function DownloadFileRequestAll() {
    var requestId = parseInt($('#requestId').val());
    url = "/POST/DownloadFileRequestAll?requestid=" + requestId;
    window.open(url, '_blank');
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})



$("#addrowqty").on("click", function () {
    var newRow = $("<tr>");
    var cols = "";
    if (counterTblQty == 0) {
        //cols += '<td><input type="text" class="form-control" readonly name="no' + counterTblQty + '"/></td>';
        cols += '<td><input type="number" placeholder="Input Numeric Only" class="form-control" id="QtyPartial" name="name' + counterTblQty + '"/></td>';
        cols += '<td class="text-center">';
        cols += '<button type="button" class="ibtnSave btn btn-md btn-primary" value="Save"><i class="fa fa-save"></i>&nbsp;Save</button>&nbsp;';
        cols += '<button type="button" class="ibtnDelete btn btn-md btn-danger " value="Delete"><i class="fa fa-trash"></i>&nbsp;Delete</button>';
        cols += '</td>';

        newRow.append(cols);
        $("#table-partialQty").append(newRow);
        counterTblQty++;
    }
});




$("#table-partialQty").on("click", ".ibtnSave", function (event, row) {

    var qty = $('#QtyPartial').val();

    var data = { 'idItem': IdItem, 'qtyPartial': qty }
    $.ajax({
        cache: false,
        async: false,
        url: 'UpadateItemQtyPartial',
        method: 'POST',
        data: data,
        success: function (res) {
            $("#table-partialQty").bootstrapTable('refresh', {
                query: { id: IdItem }
            });
        },
        error: function (err) {
            swalError("failed save");
        }
    })


    $(this).closest("tr").remove();
    counterTblQty -= 1
});


$("#table-partialQty").on("click", ".ibtnDelete", function (event, row) {
    $(this).closest("tr").remove();
    counterTblQty -= 1
});

function InitTablepartialQtyRefresh(ItemId) {
    $('#table-partialQty').bootstrapTable({
        detailView: false,
        url: '/Post/GetItemPartialListById',
        queryParams: function (params) {
            var query = {
                id: ItemId ?? 0,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            field: 'QtyPartial_Id',
            title: 'Action',
            align: 'center',
            class: 'clickable-row',
            events: EventsFormatter,
            width: '10',
            formatter: function (row, data, index) {
                var btnViewNote6 = `<a class="remove btn btn-light btn-xs"" title="Remove"><i class="fa fa-trash" title="Remove"></i></a>`;
                var ActionItem6 = [btnViewNote6].join('&nbsp;');
                return ActionItem6
            }
        },
        {
            title: 'Qty Partial',
            field: 'QtyPartial',
            class: 'clickable-row',
            align: 'center',
            width: '120'
        }
        ]
    })
}
function InitTablepartialQty() {
    $('#table-partialQty').bootstrapTable({
        detailView: false,
        url: '/Post/GetItemPartialListById',
        queryParams: function (params) {
            var query = {
                id: params.id ?? 0,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            field: 'QtyPartial_Id',
            title: 'Action',
            align: 'center',
            class: 'clickable-row',
            events: EventsFormatter,
            width: '10',
            formatter: function (row, data, index) {              
                var btnViewNote6 = `<a class="remove btn btn-light btn-xs"" title="Remove"><i class="fa fa-trash" title="Remove"></i></a>`;
                var ActionItem6 = [btnViewNote6].join('&nbsp;');
                return ActionItem6
            }
        },
        {
            title: 'Qty Partial',
            field: 'QtyPartial',
            class: 'clickable-row',
            align: 'center',
            width: '120'
        }
        ]
    })
}
function InitTablehardcopyInvoice() {
    $('#table-hardcopyinvoice').bootstrapTable({
        detailView: false,
        url: '/Post/GetHardCopyInvoiceById',
        queryParams: function (params) {
            var query = {
                id: requestId,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            field: 'Id',
            title: 'Action',
            align: 'center',
            class: 'clickable-row',
            visible: RemoveHCVisible,
            events: EventsFormatter,
            width: '10',
            formatter: function (row, data, index) {

                var btnViewNote6 = `<a class="removehardcopy btn btn-light btn-xs"" title="Remove"><i class="fa fa-trash" title="Remove"></i></a>`;
                var ActionItem6 = [btnViewNote6].join('&nbsp;');
                return ActionItem6
            }
        },

        {
            title: 'PO_Number',
            field: 'PO_Number',            
            align: 'center',
            width: '120'
        },
        {
            title: 'FileName',
            field: 'FileNameOri',           
            align: 'center',
            width: '120'
        },
        {
            title: 'Recipients Name/ Receipt Number',
            field: 'ReceiptNameOrNumber',           
            align: 'center',
            width: '120'
        },
        {
            title: 'Submission Type',
            field: 'SubmissionType',
            align: 'center',
            width: '120'
        },
        {
            title: 'Submission Date',
            field: 'SubmissionDate',
            align: 'center',
            width: '120',
            formatter: dateSAPFormatter
        }
        ]
    })
}

$('#table-partialQty tr').click(function (event) {
    if (event.target.type !== 'radio') {
        $(':radio', this).trigger('click');
    }
});
window.EventsFormatter = {
    'click .remove': function (e, value, row, index) { 
        var ItemId = row.Item_Id;
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
                deletePartialQty(row.QtyPartial_Id);
                $("#table-partialQty").bootstrapTable('destroy')
                return InitTablepartialQtyRefresh(ItemId);
               
            }
            });
       
    },

    'click .removehardcopy': function (e, value, row, index) {
        var HardCopyId = row.Id;
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
                deleteHarcopyInvoice(HardCopyId);
                $("#table-hardcopyinvoice").bootstrapTable('destroy')
                return InitTablehardcopyInvoice();

            }
        });

    },
}
$(":radio[name=radios]").change(function () {
    $(".table tr.active").removeClass("active");
    $(this).closest("tr").addClass("active");
});

function deletePartialQty(ID) {
    $.ajax({
        type: "POST",
        url: '/Post/PartialQtyDelete',
        //beforeSend: function () {
        //    ShowLoading();
        //},
        //complete: function () {
        //    HideLoading();
        //},
        data: { ID: ID },
        dataType: "json",
        success: function (d) {
            swalSuccess("Success Remove");
             //$("#table-partialQty").bootstrapTable('destroy', {
            //    query: { id: id }
            //});
            //$("#table-partialQty").bootstrapTable('destroy')
            //InitTablepartialQty();
       
            //if (d.Msg !== undefined) {
            //    sAlert('Success', d.Msg, 'success');
            //}
            //$("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}
function deleteHarcopyInvoice(ID) {
    $.ajax({
        type: "POST",
        url: '/Post/HarcopyInvoiceDelete',
       
        data: { ID: ID },
        dataType: "json",
        success: function (d) {
            swalSuccess("Success Remove");
           
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}

function GetPartialItem(id) {
    $.getJSON("GetItemPartialById?Id=" + id)
        .then(function (data) {

            $('#item_id').val(data.result.Item_Id);
            $('#qtyPartial').val(data.result.QtyPartial);
            $('#QtyPartial_Id').val(data.result.QtyPartial_Id)

            //if(data.result.Item_Status != null){
            //    $("#status :selected").val(data.result.Item_Status);
            //    //$('#status').select2('data', {id: '123', text: 'res_data.primary_email'});
            //}


            
            $("#status option").each(function (e, a) {
                $("#status").val(data.result.Item_Status).trigger('change');
                //if ($(this).context.value == data.result.Item_Status) {
                //    $("#status").val(data.result.Item_Status);
                //}
            })

            var etd = dateSAPFormatter(data.result.ETD, 0, 0);
            var atd = dateSAPFormatter(data.result.ATD, 0, 0);
            var eta = dateSAPFormatter(data.result.ETA, 0, 0);
            var ata = dateSAPFormatter(data.result.ATA, 0, 0);

            $('#etd').val(etd);
            $('#atd').val(atd);
            $('#eta').val(eta);
            $('#ata').val(ata);
            $('#NotesStatus').val(data.result.Notes);
            $('#position').val(data.result.POSITION);


            if (currentStep == 'confirm') {
                UpdateItemVarType = "NOTES";
                $('#columnLeft').removeClass('col-md-6');
                $('#columnLeft').addClass('col-md-12');
                $('#statusForm').hide();
                $('#etdForm').hide();
                $('#atdForm').hide();
                $('#etaForm').hide();
                $('#ataForm').hide();
                $('#positionForm').hide();

            }

        })
}

function InitTableViewNotes() {
    $('#table-viewnotes').bootstrapTable({
        detailView: false,
        url: '/Post/GetItemHistoryById',
        queryParams: function (params) {
            var query = {
                id: params.id ?? 0,
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        columns: [{
            title: 'Date',
            field: 'TimeStamp',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'Step',
            field: 'Step',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Status',
            field: 'Status',
            class: 'text-center',
            align: 'center',
            width: '110'
        },
        {
            title: 'Notes',
            field: 'Notes',
            class: 'text-center',
            align: 'center',
            width: '110',
        },
        {
            title: 'ETA',
            field: 'ETA',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ATA',
            field: 'ATA',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ATD',
            field: 'ATD',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'ETD',
            field: 'ETD',
            class: 'text-center',
            align: 'center',
            width: '110',
            formatter: dateSAPFormatter
        },
        {
            title: 'Qty Partial',
            field: 'QtyPartial',
            class: 'text-center',
            align: 'center',
            width: '110',
        }]
    })
}

function OnChangeApplyFor(value) {
    if (value == "MULTIPLE") {
        document.getElementById("ItemForm").style.display = "block";
    } else {
        document.getElementById("ItemForm").style.display = "none";
    }
}


function ChangeLabelModalUpdateStatus(POType) {
    var LabelETD = "";
    var LabelETA = "";
    var LabelATA = "";
    var LabelATD = "";
    var LabelPosition = "";
    if (POType == "D") {
        LabelETD = document.getElementById("LabelETD");
        LabelETA = document.getElementById("LabelETA");
        LabelATA = document.getElementById("LabelATA");
        LabelATD = document.getElementById("LabelATD");
        LabelPosition = document.getElementById("LabelPosition");
        txtLabelPosition = document.getElementById("position");
        LabelETD.innerHTML = "Plan Start Date";
        LabelETA.innerHTML = "Plan Complete Date";
        LabelATA.innerHTML = "Actual Complete Date";
        LabelATD.innerHTML = "Actual Start Date";
        LabelPosition.innerHTML = "Tracking Progress (%)";
        txtLabelPosition.type = 'number';
        txtLabelPosition.placeholder = 'Input Numeric Only (1-100)';
        txtLabelPosition.min = 0;
        txtLabelPosition.max = 100;
      
        //txtLabelPosition.readOnly = true;
    } else {
        LabelETD = document.getElementById("LabelETD");
        LabelETA = document.getElementById("LabelETA");
        LabelATA = document.getElementById("LabelATA");
        LabelATD = document.getElementById("LabelATD");
        LabelPosition = document.getElementById("LabelPosition");
        LabelETD.innerHTML = "ETD";
        LabelETA.innerHTML = "ETA";
        LabelATA.innerHTML = "ATA";
        LabelATD.innerHTML = "ATD";
        LabelPosition.innerHTML = "Position";
    }
}
function InitModalUpdateStatus(id) {
    IdItem = id;
    counterTblQty = 0;
    $('#modalUpdateStatusForm').modal('show');
    $('#modalUpdateStatusForm').modal({ backdrop: "static" });
    InitSelect2ItemMultiple();
    $("#table-partialQty").bootstrapTable('refresh', {
        query: { id: id }
    });

    var POType = $('#table-inprogress').bootstrapTable('getData')[0].POType;
    if (POType == 'D') {
        $('#addrowqty').hide();
        $('#table-partialQty').hide()
    }
    else {
        $('#addrowqty').show();
        $('#table-partialQty').show()
    }
    ChangeLabelModalUpdateStatus(POType);
    InitSelect2StatusPO(POType);
    $.getJSON("GetItemListById?Id=" + id)
        .then(function (data) {
            $('#po_type').val(POType);
            $('#item_id').val(data.result.Item_Id);
            $('#item_name').val(data.result.Item_Description);
            $('#qtyPartial').val(0);

            $("#status option").each(function () {
                if ($(this).val() == data.result.Item_Status) {
                    $(this).attr('selected', 'selected');
                }
            })

            var etd = dateSAPFormatter(data.result.ETD, 0, 0);
            var atd = dateSAPFormatter(data.result.ATD, 0, 0);
            var eta = dateSAPFormatter(data.result.ETA, 0, 0);
            var ata = dateSAPFormatter(data.result.ATA, 0, 0);

            $('#etd').val(etd);
            $('#atd').val(atd);
            $('#eta').val(eta);
            $('#ata').val(ata);
            $('#NotesStatus').val(data.result.Notes);
            $('#position').val(data.result.POSITION);

            if (currentStep == 'confirm') {
                UpdateItemVarType = "NOTES";
                $('#columnLeft').removeClass('col-md-6');
                $('#columnLeft').addClass('col-md-12');
                $('#statusForm').hide();
                $('#etdForm').hide();
                $('#atdForm').hide();
                $('#etaForm').hide();
                $('#ataForm').hide();
                $('#positionForm').hide();

            }

        })
}

function InitModalHardCopy(id) {   
    if (Role.includes("Administrator,POSTVENDOR")) {
        $('#btnSave').show();
    }
    else {
        $('#btnSave').hide();
    }
   
    $('#modalhardCopyInvoiceForm').modal('show');
    $('#modalhardCopyInvoiceForm').modal({ backdrop: "static" });
    InitSelect2filenameinvoice();

    $("#table-hardcopyinvoice").bootstrapTable('refresh', {
        query: { id: requestId }
    });
}



function InitModalViewNotes(id) {
    $.getJSON("GetItemListById?Id=" + id)
        .then(function (data) {

            $('#modalViewStatusForm').modal();
            $('#modalStatusFormItem').text(data.result.Item_Description);
            $('#modalStatusFormDestination').text(data.result.Destination ?? "");
            $("#table-viewnotes").bootstrapTable('refresh', {
                query: { id: id }
            });
        })
}

function InitSelect2Destination() {
    $('#select2Destination').select2({
        placeholder: "Destination",
        id: "id",
        ajax: {
            url: 'GetSelectBranch',
            data: function (params) {
                var query = {
                    search: params.term,
                    type: 'public'
                }
                return query;
            },
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                var newData = $.map(data.result, function (obj) {
                    return obj;
                });
                return {
                    results: newData
                };
            },
            cache: true
        }
    }).on("select2:select", function (obj) {
        return;
    });
}
function InitSelect2StatusPO(POType) {
    var StatusPO = [];
    if (POType == '') {
        StatusPO = ["On Schedule"
            , "Risk Delay"
            , "Delay"
            , "POD"
            , "In Transit"
            , "On Progress"
            , "Pick Up"
        ]
    }
    else if (POType == 'D') {
        StatusPO = [
            "Start"
            , "On Progress"
            , "Complete"
        ]
    }
    $('#status').select2({
        placeholder: "Status PO",
        data: StatusPO,
    }).on("select2:select", function (obj) {
        return;
    });
    $('#select2StatusPO').select2({
        placeholder: "Status PO",
        data: StatusPO,
    }).on("select2:select", function (obj) {
        return;
    });
}
function InitDateRange() {

    $('#deliveryDateItem').daterangepicker({
        opens: 'right'
    }, function (start, end, label) {
        return;
    });

    $('#etd').datepicker({
        format: 'dd.mm.yyyy',
    });

    $('#atd').datepicker({
        format: 'dd.mm.yyyy',
    });

    $('#eta').datepicker({
        format: 'dd.mm.yyyy',

    });

    $('#ata').datepicker({
        format: 'dd.mm.yyyy',
    });
    $('#submissiondate').datepicker({
        format: 'dd.mm.yyyy',
    });

}
function clearSearchDetail() {
    $("#itemName").val("");
    $("#deliveryDateItem").val("");
    $('#select2StatusPO').select2("val", "");
    $("#select2Destination").select2("val", "");
    $("#table-item").bootstrapTable('refresh');

}
function InitSelect2filenameinvoice() {
    $('#select2file').select2({
        placeholder: "File Name Invoice",
        id: requestId,
        ajax: {
            url: 'GetSelectFileNameInvoice',
            data: function (params) {
                var query = {
                    id: requestId,
                    type: 'public'
                }
                return query;
            },
            dataType: 'json',
            delay: 250,
            processResults: function (data) {
                var newData = $.map(data.result, function (obj) {
                    return obj;
                });
                return {
                    results: newData
                };
            },
            cache: true
        }
    }).on("select2:select", function (obj) {
        return;
    });
}
function GetListPOByRequestId(params) {
    var url = 'GetListPOByRequestId?requestId=' + requestId;
    ajaxGetData(url, params);
}
function GetListItemByRequestId(params) {
    var startDateDeliveryDate = dateFormatter($("#deliveryDateItem").data("daterangepicker").startDate._d) ?? "";
    var endDateDeliveryDate = dateFormatter($("#deliveryDateItem").data("daterangepicker").endDate._d) ?? "";

    var DeliveryDate = $("#deliveryDateItem").val();

    if (DeliveryDate == "") {
        startDateDeliveryDate = "";
        endDateDeliveryDate = "";
    }
    var Destination = $("#select2Destination option:selected").val() ?? "";
    var Status = $("#select2StatusPO option:selected").val() ?? "";

    params.search = "&Item=" + $("#itemName").val() + "&StartDateDeliveryDate=" + startDateDeliveryDate + "&EndDateDeliveryDate=" + endDateDeliveryDate +
        "&Destination=" + Destination + "&Status=" + Status;

    var url = 'GetListItemByRequestId?requestId=' + requestId + params.search;
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}
function ajaxGetData(url, params) {
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}
function ClickSubmitPrepay() {
    swal({
        title: "Are you sure confirm Prepayment?",
        text: "",
        type: "info",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        cancelButtonText: "No",
        confirmButtonText: "Yes",
    }, function (isConfirm) {
        if (isConfirm) {
            ClickSubmitPrepayment();
        }
    })
}
function ClickSubmit(FlowProcessStatusID, OldFlowProcessStatusID, mandatory) {
    if (FlowProcessStatusID == 2927) {
        var StatusPO = $('#table-inprogress').bootstrapTable('getData')[0].Status_PO;
        if (StatusPO != "POD") {
            swalWarning("Status PO Must Be POD!");
            return;
        }
    }
    if ($("#ChkPrePayment").is(":checked")) {
        var data = {
            "request": {
                "ID": requestId,
                "FlowProcessStatusID": 4217,
                "Comment": "",
                "PrePayment": true
            }
        };
        $.ajax({
            cache: false,
            async: false,
            url: 'SaveRequestPrePayment',
            method: 'POST',
            data: data,
            success: function (res) {
                swalRequestSuccess('Data Successfully Saved!')
                window.location = 'HomeVendor';
            },
            error: function (err) {
                swalError("failed save");
            }
        })
    }
    else {
        var data = {
            "request": {
                "ID": requestId,
                "FlowProcessStatusID": FlowProcessStatusID,
                "Comment": "",
            }
        };

        $.ajax({
            cache: false,
            async: false,
            url: 'SaveRequest',
            method: 'POST',
            data: data,
            success: function (res) {

                swalRequestSuccess('Data Successfully Saved!')
                window.location = 'HomeVendor';
            },
            error: function (err) {
                swalError("failed save");
            }
        })
    }

}
function ClickSubmitWithComment(FlowProcessStatusID, OldFlowProcessStatusID, mandatory) {
    swal({
        title: "Enter Comment",
        text: "<textarea id='text'></textarea>",
        // --------------^-- define html element with id
        html: true,
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Write something"
    }, function (inputValue) {
        if (inputValue === false) return false;
        if (inputValue === "") {
            swal.showInputError("You need to write something!");
            return false
        }
        var val = document.getElementById('text').value;

        var data = {
            "request": {
                "ID": requestId,
                "FlowProcessStatusID": FlowProcessStatusID,
                "Comment": val,
            }
        };

        $.ajax({
            cache: false,
            async: false,
            url: 'SaveRequest',
            method: 'POST',
            data: data,
            success: function (res) {

                swalRequestSuccess('Data Successfully Saved!')
                window.location = 'HomeVendor';
            },
            error: function (err) {
                swalError("failed save");
            }
        })
        return;
    });


}
function ClickSubmitDelivery() {
    var Shipment = $('#Select2Shipment').val();
    if (Shipment == null || Shipment == '' ) {
        swalWarning('Please Choose Shipment');
        return false;
    }
    else {
        UpdateShipment();
    }
  
   
    var FlowProcessStatusID = 4217//Default Forwarder
    if (Shipment == 'CKB') {
        FlowProcessStatusID = 2499
    }
    else if (Shipment == 'PTTU') {
        FlowProcessStatusID = 4216
    } else {
        FlowProcessStatusID = 4217//Default Forwarder
    }

    swal({
        title: "Are you sure?",
        text: "",
        type: "info",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        cancelButtonText: "No",
        confirmButtonText: "Yes",
    }, function (isConfirm) {
        if (isConfirm) {
            ClickSubmit(FlowProcessStatusID);
        }
    })
}
function ClickSubmitBast() {

    ClickSubmit(2938);

}
function ClickSubmitInvoice() {

    ClickSubmit(3217)

}


function ClickSubmitClosePOS() {


    $.getJSON("ValidateClosePO?id=" + requestId, function (data) {

        if (data.result.CountInvNoUploaded == 0 && data.result.CountInvNotPaid == 0) {
            console.log('closed')
            ClickSubmit(3217)
        }
        else if (data.result.CountInvNoUploaded > 0) {
            swalWarning('Supplier Need Upload Invoice For All Item!');
        }
        else if (data.result.CountInvNotPaid > 0) {
            swalWarning('Please pay for the invoice first!');
        }
        else {
            swalWarning('Please Complete Data Invoice');
        }
    });
}
//PrePayment Expeditor
function ClickSubmitPrepayment() {

    if ($("#ChkPrePayment").is(":checked")) {
        var data = {
            "request": {
                "ID": requestId,
                "FlowProcessStatusID": 4217,
                "Comment": "",
                "PrePayment": true
            }
        };
        $.ajax({
            cache: false,
            async: false,
            url: 'SaveRequestPrePayment',
            method: 'POST',
            data: data,
            success: function (res) {
                swalRequestSuccess('Data Successfully Saved!')
                window.location = 'HomeVendor';
            },
            error: function (err) {
                swalError("failed save");
            }
        })
    }
    else {
        var data = {
            "request": {
                "ID": requestId,
                "FlowProcessStatusID": 2489,
                "Comment": "",
                "PrePayment": false
            }
        };

        $.ajax({
            cache: false,
            async: false,
            url: 'SaveRequestPrePayment',
            method: 'POST',
            data: data,
            success: function (res) {

                swalRequestSuccess('Data Successfully Saved!')
                window.location = 'HomeVendor';
            },
            error: function (err) {
                swalError("failed save");
            }
        })
    }

}

function UpdateShipment() {
    var data = {
        "requestId": requestId,
        "shipment": $('#Select2Shipment').val(),
    };

    $.ajax({
        cache: false,
        async: false,
        url: 'UpdateShipment',
        method: 'POST',
        data: data,
        success: function (res) {
            if (res.status == "SUCCESS") {
                swalSuccess('Shipment Data Successfully Saved!');
            } else {
                swalWarning('Shipment failed Saved!');
            }
            $('#table-inprogress').bootstrapTable('refresh');
        },
        error: function (err) {
            swalSuccess('Shipment failed Saved!');
        }
    })
}

function UpdateHardCopy() { 
    var attachmentId = $('#select2file').val();
  

    var data = {
        "hardcopy": {
            "requestId": requestId,
            "attachmentId": attachmentId,
            "submissionDate": datePickerSAPFormatter($('#submissiondate').val()),
            "submissiontype": $('#submissiontype').val(),
            "receiptnameornumber": $('#ReceiptNameOrNumber').val(),
           
        },
       
    }

    $.ajax({
        cache: false,
        async: false,
        url: 'SaveHardCopy',
        method: 'POST',
        data: data,
        success: function (res) {
            if (res.status == "SUCCESS") {
                swalSuccess('HardCopy Data Successfully Saved!');
            } else {
                swalWarning('HardCopy failed save!');
            }           
        },
        error: function (err) {
            swalWarning('HardCopy failed save!');
        }
    })
}

function UpdateItem() {
    var ApplyFor = $('#ApplyFor').val();
    var idItem = $('#item_id').val();
    var status = $('#status').val();
    if (status == "POD" || status == "COMPLETE") {
        var ata = datePickerSAPFormatter($('#ata').val())
        if (ata == "" || ata == "-" || ata == null && typePO != "D") {
            return swalWarning('ATD And ATA Is Mandatory!');
        }
        if (ata == "" || ata == "-" || ata == null && typePO == "D") {
            return swalWarning('Actual Start Date And Actual Complete Date Is Mandatory!');
        }
    }

    if (ApplyFor == "MULTIPLE") {
        var dataMapItem = $("#ItemSelect").select2('data').map(function (item) {
            return item['id'];
        });

        idItem = dataMapItem.join();
    }

    var data = {
        "item": {
            "id": idItem,
            "status": status,
            "eta": datePickerSAPFormatter($('#eta').val()),
            "ata": datePickerSAPFormatter($('#ata').val()),
            "etd": datePickerSAPFormatter($('#etd').val()),
            "atd": datePickerSAPFormatter($('#atd').val()),
            "applyFor": ApplyFor,
            "notes": $('#NotesStatus').val(),
            "position": $('#position').val(),
            "QtyPartial": $('#qtyPartial').val(),
            "QtyPartialId": $('#QtyPartial_Id').val(),
        },
        "saveType": UpdateItemVarType,
    }

    $.ajax({
        cache: false,
        async: false,
        url: 'SaveItem',
        method: 'POST',
        data: data,
        success: function (res) {
            if (res.status == "SUCCESS") {
                swalSuccess('Item Data Successfully Saved!');
            } else {
                swalWarning('Item failed save!');
            }
            $('#table-item').bootstrapTable('refresh');
            $('#table-inprogress').bootstrapTable('refresh');
        },
        error: function (err) {
            swalWarning('Item failed save!');
        }
    })
}
function GetProcessFlowChecklist(data, row, index) {
    if (data == 'current') {
        return `<span class="fa fa-map-marker-alt text-success fa-spinner fa-pulse"></span>`;
        //return current;
    }
    else if (data == 'check') {
        return '<span class="showProcessDate" data-toggle="tooltip" data-placement="top" title="' + moment(row.Processing_Date).format("DD.MM.YYYY") + '">' + check + '</span>';
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetConfirmFlowChecklist(data, row, index) {
    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return '<span class="showConfirmDate" data-toggle="tooltip" data-placement="top" title="' + moment(row.Confirmation_Date).format("DD.MM.YYYY") + '">' + check + '</span>';
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetTypePOChecklist(data, row, index) {
    if (typePO == "") {
        return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="PO Material">' + check + '</span>';
    }
    else {
        return `<span class="fa fa-minus-circle text-disabled"  title="PO Service"></span>`;;
    }
}
function GetDeliveryFlowChecklist(data, row, index) {
    var PartialInProgress = `<span class="fa fa-check-circle text-warning"></span>`;
    var CurrentProgress = `<span class="fa fa-minus-circle text-disabled"></span>`;
    var Running = `<span class="fa fa-map-marker-alt text-success fa-spinner fa-pulse"></span>`;

    if (typePO == "D") {
        if (data == 'running') {
            if (row.TotalComplete > 0 && row.TotalNotComplete > 0)
                return PartialInProgress
            else if (row.TotalComplete > 0 && row.TotalNotComplete == 0) {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
            }
            else
                return Running
        }
        else if (data == 'check') {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        }
        else
            return CurrentProgress;

        // if (row.TotalNotComplete == 0) {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        // } else if (row.TotalNotComplete > 0) {
        // if (data == 'current') {
        // return current;
        // } else {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
        // }
        // } else {
        // return current;
        // }
    } else {
        if (data == 'running') {
            if (row.TotalNotPOD > 0 && row.TotalPOD == 0)
                return Running
            else if (row.TotalNotPOD == 0 && row.TotalPOD > 0) {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
            }
            else
                return PartialInProgress
        }
        else if (data == 'check') {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        }
        else
            return CurrentProgress;



        // if (row.TotalNotPOD == 0) {
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';

        // } else if (row.TotalNotPOD > 0) {
        // if (data == 'current') {              
        // //return PartialInProgress;
        // return CurrentProgress;
        // } else {               
        // return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
        // }
        // } else {
        // console.log("masuk3");
        // return current;
        // }
    }
}

function loadingTemplate(message) {
    return '<i class="fa fa-spinner fa-spin fa-fw fa-2x"></i>'
}
function searchItem() {
    $("#table-item").bootstrapTable("refresh");
}
function InitDropZoneBast() {
    var myDropzoneBast = new Dropzone("#FormUploadBAST", { // Make the bodyFormUpload a dropzone
        url: "/Post/SaveFileAttachmentRequest", // Set the url
        thumbnailHeight: 100,
        thumbnailWeight: 100,
        method: 'POST',
        acceptedFiles: ".pdf",
        filesizeBase: 1024,
        autoProcessQueue: true,
        maxFiles: 10,
        maxFilesize: 2, // MB
        parallelUploads: 1,
        dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import File.</h4>",
        uploadMultiple: false

    });

    myDropzoneBast.on("addedfile", function (file) {
        // Hookup the start button
        $("#actions .start").on("click", function () {
            myDropzoneBast.enqueueFile(file);
        });
        $("#placeholderUpload").hide();
    });

    myDropzoneBast.on("totaluploadprogress", function (progress) {
        $("#total-progress .progress-bar").css("width", progress + "%");
        $("#progressPercent").html(progress + "%");
    });

    myDropzoneBast.on("sending", function (file, xhr, formData) {
        // Show the total progress bar when upload starts
        $("#total-progress").css("opacity", 1);
        $("#actions .delete").attr("disabled", "disabled");
        formData.append('requestId', parseInt(IdItem));
        formData.append('fileNameOri', file.name);
        formData.append('codeAttachment', 'BAST');

        // And disable the start button
        $(".start").attr("disabled", "disabled");
    });

    myDropzoneBast.on("complete", function (resp) {
        if (resp.status === "success") {
            var respText = resp.xhr.response;
            var respData = JSON.parse(respText);

            $("#actions .delete").prop("disabled", false);
            var type = respData.status ? "success" : "error";
            swal("Upload Status", respData.msg, type);
            $("#table-uploadBast").bootstrapTable('refresh');
            $('#table-item').bootstrapTable('refresh');
            $('#table-inprogress').bootstrapTable('refresh');
            setTimeout(function () {
                myDropzoneBast.removeAllFiles(true);
            }, 1800);
        }
    });

    // Hide the total progress bar when nothing's uploading anymore
    myDropzoneBast.on("queuecomplete", function (progress) {
        $("#total-progress").css("opacity", "0");
    });

    $("#actions .start").on("click", function () {
        myDropzoneBast.enqueueFiles(myDropzoneBast.getFilesWithStatus(Dropzone.QUEUED));
    });

    $("#actions .cancel").on("click", function () {
        myDropzoneBast.removeAllFiles(true);
        $("#placeholderUpload").hide();
    });
}
function InitDropZoneInvoice() {
    var myDropzoneInvoice = new Dropzone("#FormUploadInvoice", { // Make the bodyFormUpload a dropzone
        url: "/Post/SaveFileAttachmentRequest", // Set the url
        thumbnailHeight: 100,
        method: 'POST',
        acceptedFiles: ".pdf",
        filesizeBase: 1024,
        autoProcessQueue: true,
        maxFiles: 1,
        maxFilesize: 2, // MB
        parallelUploads: 1,
        dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import File.</h4>",
        uploadMultiple: false

    });
    myDropzoneInvoice.on("addedfile", function (file) {
        // Hookup the start button
        $("#actions .start").on("click", function () {
            myDropzoneInvoice.enqueueFile(file);
        });
        $("#placeholderUpload").hide();
    });
    //$('#BtnSaveInvoice').on("click", function () {
    //    myDropzoneInvoice.enqueueFile(file);
    //})
    myDropzoneInvoice.on("totaluploadprogress", function (progress) {
        $("#total-progress .progress-bar").css("width", progress + "%");
        $("#progressPercent").html(progress + "%");
    });

    myDropzoneInvoice.on("sending", function (file, xhr, formData) {
        // Show the total progress bar when upload starts
        $("#total-progress").css("opacity", 1);
        $("#actions .delete").attr("disabled", "disabled");
        formData.append('requestId', parseInt(IdItem));
        formData.append('fileNameOri', file.name);
        formData.append('codeAttachment', 'INVOICE');
        // And disable the start button
        $(".start").attr("disabled", "disabled");
    });

    myDropzoneInvoice.on("complete", function (resp) {
        if (resp.status === "success") {
            var respText = resp.xhr.response;
            var respData = JSON.parse(respText);
            //var failedList = respData.failedList;

            $("#actions .delete").prop("disabled", false);
            var type = respData.status ? "success" : "error";
            swal("Upload Berhasil, Silahkan lanjutkan untuk memilih refer to BAST ", respData.msg, type);
            $("#table-uploadInvoice").bootstrapTable('refresh');

            setTimeout(function () {
                myDropzoneInvoice.removeAllFiles(true);
            }, 1800);
        }
    });

    // Hide the total progress bar when nothing's uploading anymore
    myDropzoneInvoice.on("queuecomplete", function (progress) {
        $("#total-progress").css("opacity", "0");
    });

    $("#actions .start").on("click", function () {
        myDropzoneInvoice.enqueueFiles(myDropzoneInvoice.getFilesWithStatus(Dropzone.QUEUED));
    });

    $("#actions .cancel").on("click", function () {
        myDropzoneInvoice.removeAllFiles(true);
        $("#placeholderUpload").hide();
    });
}
function InitDropZonePO() {
    var myDropzonePO = new Dropzone("#FormUploadPO", { // Make the bodyFormUpload a dropzone
        url: "/Post/SaveFileAttachmentRequest", // Set the url
        thumbnailHeight: 100,
        method: 'POST',
        acceptedFiles: '.xlsx',
        filesizeBase: 1024,
        autoProcessQueue: true,
        maxFiles: 1,
        maxFilesize: 10, // MB
        parallelUploads: 1,
        dictDefaultMessage: "<h4>Drop the Import File Here or Click this Section for Browse the Import File.</h4>",
        uploadMultiple: false

    });
    myDropzonePO.on("addedfile", function (file) {
        // Hookup the start button
        $("#actions .start").on("click", function () {
            myDropzonePO.enqueueFile(file);
        });
        $("#placeholderUpload").hide();
    });

    myDropzonePO.on("totaluploadprogress", function (progress) {
        $("#total-progress .progress-bar").css("width", progress + "%");
        $("#progressPercent").html(progress + "%");
    });

    myDropzonePO.on("sending", function (file, xhr, formData) {
        // Show the total progress bar when upload starts
        $("#total-progress").css("opacity", 1);
        $("#actions .delete").attr("disabled", "disabled");
        formData.append('requestId', parseInt($('#requestId').val()));
        formData.append('fileNameOri', file.name);
        formData.append('codeAttachment', 'PO');
        // And disable the start button
        $(".start").attr("disabled", "disabled");
    });

    myDropzonePO.on("complete", function (resp) {
        if (resp.status === "success") {
            var respText = resp.xhr.response;
            var respData = JSON.parse(respText);

            $("#actions .delete").prop("disabled", false);
            var type = respData.status ? "success" : "error";
            swal("Upload Status", respData.msg, type);
            $("#table-uploadPO").bootstrapTable('refresh');

            setTimeout(function () {
                myDropzonePO.removeAllFiles(true);
            }, 1800);
        }
    });

    // Hide the total progress bar when nothing's uploading anymore
    myDropzonePO.on("queuecomplete", function (progress) {
        $("#total-progress").css("opacity", "0");
    });

    $("#actions .start").on("click", function () {
        myDropzonePO.enqueueFiles(myDropzonePO.getFilesWithStatus(Dropzone.QUEUED));
    });

    $("#actions .cancel").on("click", function () {
        myDropzonePO.removeAllFiles(true);
        $("#placeholderUpload").hide();
    });
}
function InitModalUploadBast(id, statusItem) {
    IdItem = id;

    if (statusItem.toUpperCase() == "POD" || statusItem.toUpperCase() == "ON PROGRESS" || statusItem.toUpperCase() == "COMPLETE") {
        document.getElementById("FormUploadBAST").style.display = "block";
    } else {
        document.getElementById("FormUploadBAST").style.display = "none";
    }
    $('#modalUploadBAST').modal();
    $('#table-uploadBast').bootstrapTable('refresh', {
        query: {
            id: IdItem,
            type: "BAST",
        }
    });

    if (parseInt(isTaskUser) == 0) {
        document.getElementById("BtnSaveBast").style.display = "none";

        if (Role.includes("Administrator,POSTHOVIEWER") || Role.includes("Administrator,POSTPLANTVIEWERFINANCE") || Role.includes("Administrator,POSTVIEWER")) {
            document.getElementById("FormUploadBAST").style.display = "none";
        }
    }

}
function InitModalUploadInvoice(id) {
    var IsPopUP = false

    var PoNo = $('#table-inprogress').bootstrapTable('getData')[0].PO_No;
    $.getJSON("CheckPopUpInvoice?PoNo=" + PoNo, function (data) {
        if (Role.includes('POSTVendor')) {
            if (data.IsAgreeInvoice == false) {
                IsPopUP = true;
            }
            else {
                IsPopUP = false;
            }
        }
        
        if (IsPopUP == true) {
            ViewPopup()
        }
        else {
            InitData(id)
        }
    });
}
function SavePopup() {
    var agree = chkagree.checked;
    var PoNo = $('#table-inprogress').bootstrapTable('getData')[0].PO_No;
    $.getJSON("SavePopUpInvoice?isChecked=" + agree + "&description=Invoice&PoNo=" + PoNo, function (data) {
        if (data.IsAgreeInvoice == true) {
            $('#modalPopUpInvoice').hide();
            $('#modalUploadInvoicing').modal();
        }
        else {
            $('#modalPopUpInvoice').hide();
        }

    });

}
function ViewPopup() {
    $('#modalPopUpInvoice').modal();
}

function InitData(id) {

    IdItem = id;
    $.getJSON("GetListAttachment?id=" + IdItem + "&type=BAST", function (data) {
        var countBastUpload = data.result.length;
        var IsNotApprove = data.result.find(element => element.IsApprove == false) ? true : false;

        var PoNo = $('#table-inprogress').bootstrapTable('getData')[0].PO_No;
        var url = 'GetDataGRByPO?poNo=' + PoNo + '&itemId=' + IdItem;
        $.get(url).then(function (res) {
            var CountGR = res.result.length;


            if (countBastUpload == 0) {
                document.getElementById("FormUploadInvoice").style.display = "none";
            }
            else if (IsNotApprove) {
                document.getElementById("FormUploadInvoice").style.display = "none";
            }
            else if (CountGR == 0) {
                document.getElementById("FormUploadInvoice").style.display = "none";
            }
            else {
                if (Role.includes("Administrator,POSTHOVIEWER") || Role.includes("Administrator,POSTPLANTVIEWERFINANCE") || Role.includes("Administrator,POSTVIEWER")) {
                    document.getElementById("FormUploadInvoice").style.display = "none";
                 }
                else {
                    document.getElementById("FormUploadInvoice").style.display = "block";
                 }
             
            }

            if (parseInt(isTaskUser) == 0) {
                document.getElementById("BtnSaveInvoice").style.display = "none";
                if (Role.includes('POSTFINANCE')) {
                    document.getElementById("BtnRejectInvoice").style.display = "block";
                }
            }
            if (PrePayment == 1) {
                document.getElementById("FormUploadInvoice").style.display = "block";
            }

        });
      
        $('#modalUploadInvoicing').modal();
        $('#table-uploadInvoice').bootstrapTable('refresh', {
            query: {
                id: IdItem,
                type: "INVOICE",
            }
        });
    })
 
}

function InitModalUploadPO() {
    $('#modalUploadPO').modal();
    $("#table-uploadPO").bootstrapTable('refresh');
}
function InitTableUploadBast() {
    $('#table-uploadBast').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: IdItem,//id Item
                type: "BAST",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 15,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: [

            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'text-nowrap align-top',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'Action',
                field: 'Action',
                class: 'text-nowrap align-top',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    var btnReject = ''
                    var btnApprove = ''
                    var btnDownload = ''
                    if (isTaskUser > 0) {
                        btnDownload = `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>&nbsp;`;
                        btnDelete = `<a href="#" onclick="DeleteFileUpload(${data.ID},'BAST');" class="btn btn-light btn-xs" title="Delete"><span class="fa fa-trash"></span></a>`;
                        btnReject = ''
                        btnApprove = ''
                        return [btnDownload, btnReject, btnDelete].join('&nbsp;');
                    } else {
                        btnDownload = `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>&nbsp;`;

                        //if (Role.includes('POSTBRANCH')) {
                        //    btnReject = `<a href="#" onclick="RejectAttachment(${data.ID},'BAST');" class="btn btn-light btn-xs" title="Reject"><span class="fa fa-times "></span></a>`;
                        //    btnApprove = `<a href="#" onclick="ApproveAttachment(${data.ID},'BAST');" class="btn btn-light btn-xs" title="Approve"><span class="fa fa-check"></span></a>`;
                        //}
                        return [btnDownload, btnReject, btnApprove].join('&nbsp;');
                    }

                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                class: 'align-top',
                halign: 'center',
                align: 'left',
                width: '250'
            }, {
                title: 'BAST Date',
                class: 'text-nowrap align-top',
                halign: 'center',
                align: 'left',
                width: '200',
                visible: false,
                formatter: function (row, data, index) {
                    return `<input type="datetime" name="BastDate" class="form-control input-sm" id="BastDate" placeholder="BAST Date" />`
                }
            },
            //{
            //    title: 'Progress (%)',
            //    width: '300',
            //    align: 'left',
            //    class: 'align-top',
            //    halign: 'center',
            //    visible: visibleProgressUpload,
            //    formatter: function (row, data, index) {
            //        //return `<select id="${data.ID}_Progress"   class="Select2Progress form-control gosearch">`
            //        return `<input type="number" id="${data.ID}_Progress" class="Select2Progress form-control input-sm"  placeholder="Input Numeric Only" />`
            //    }
            //},
            {
                title: 'Item Qty',
                width: '200',
                align: 'left',
                halign: 'center',
                class: 'align-top',
                visible: visibleCKB,
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}_itemQty"  multiple="multiple" class="Select2MappingItemQty form-control gosearch">`
                }
            },
            {
                title: 'Item',
                width: '400',
                class: 'align-top',
                align: 'left',
                halign: 'center',
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}"  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                }
            }, {
                title: 'Notes',
                field: 'Notes',
                width: '900',
                align: 'left',
                class: 'align-top',
                halign: 'center',
                formatter: function (row, data, index) {
                    if (isTaskUser == 0) {
                        if (Role.includes('POSTBRANCH')) {
                            return `<textarea  class="form-control" id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                        } else {
                            return `<textarea  class="form-control" disabled id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                        }
                    } else {
                        return `<textarea  class="form-control" id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                    }
                }
            }, {
                title: 'Upload Date',
                field: 'UploadedDate',
                class: 'align-top',
                halign: 'center',
                align: 'left',
                width: '250',
                formatter: dateFormattertime
            }, {
                title: 'Status',
                visible: false,
                field: 'IsRejected',
                class: 'align-top',
                align: 'center',
                halign: 'center',
                formatter: function (row, data, index) {
                    var IsRejected = row;
                    var IsApprove = data.IsApprove;
                    var btnStatus = '';

                    var idAttach = data.ID;
                    if (IsRejected) {
                        btnStatus = `<a class="btn btn-light btn-xs" id="statusbast_${idAttach}" title="Rejected">Rejected</a>`;
                    }
                    else if (IsApprove) {
                        btnStatus = `<a class="btn btn-light btn-xs" id="statusbast_${idAttach}"  title="Approved">Approved</a>`;
                    }
                    else {
                        btnStatus = `<a class="btn btn-light btn-xs" id="statusbast_${idAttach}"  title="Waiting">Waiting Approval</a>`;
                    }
                    var ActionItem5 = [btnStatus].join('&nbsp;');
                    return ActionItem5
                }
            }
        ]
    });
}
$('#table-uploadBast').on('post-body.bs.table', function (field, value, row, $element) {
    var disabled = false;
    if (isTaskUser == 0) {
        disabled = true;
    }
    $('#BastDate').datepicker({
        format: 'dd.mm.yyyy',
    });
    $('#BastDate').change(function (e) {
        console.log(e);
    });

    $('.Select2Progress').on('focusout', function (obj) {
        //alert(obj.target.value);
        var attachid = obj.target.id;
        attachid = attachid.replace("_progress", "");
        var progress = obj.target.value;
        //var checkinput = checkinputprogress(progress)
        //if (!checkinput) {
        //    $('#' + obj.currenttarget.id).val(null).trigger('change');
        //    return swalwarning("progress must be numeric!");
        //}

        var intprogress = progress.replace('%', '');
        //intprogress = parsefloat(intprogress);
        $.getjson("getlistattachment?id=" + iditem + "&type=po_bast_top", function (data) {

            var valuess = data.result[0].progress
            valuess = valuess.replace('%', '');
            valuess = parseint(valuess);
            if (valuess > intprogress) {
                $("#table-uploadbast").bootstraptable('refresh');
                return swalwarning("progress must be more than before!");
            } else {
                $.getjson("updateattachitemprogresspercent?progress=" + progress + "&attachmentid=" + attachid + "&type=bast")
                    .then(function (datassss) {
                        return;
                    })
            }
        })
    })

    //$(".Select2Progress").select2({
    //    placeholder: "Select Progress",
    //    data: ProgpressPercent,
    //    tags: true,
    //    dropdownParent: $('#modalUploadBAST'),
    //    width: "100%",
    //    disabled: disabled,
    //}).on("select2:select", function (obj) {
    //    var attachId = obj.target.id;
    //    attachId = attachId.replace("_Progress", "");
    //    var progress = obj.params.data.id
    //    var checkinput = CheckinputProgress(progress)
    //    if (!checkinput) {
    //        $('#' + obj.currentTarget.id).val(null).trigger('change');
    //        return swalWarning("Progress must be numeric!");
    //    }

    //    var intprogress = progress.replace('%', '');
    //    intprogress = parseFloat(intprogress);
    //    $.getJSON("GetListAttachment?id=" + IdItem + "&type=PO_BAST_TOP", function (data) {

    //        var valuess = data.result[0].Progress
    //        valuess = valuess.replace('%', '');
    //        valuess = parseInt(valuess);
    //        if (valuess > intprogress) {
    //            $("#table-uploadBast").bootstrapTable('refresh');
    //            return swalWarning("Progress must be more than before!");
    //        } else {
    //            $.getJSON("UpdateAttachItemProgressPercent?progress=" + progress + "&attachmentId=" + attachId + "&type=BAST")
    //                .then(function (datassss) {
    //                    return;
    //                })
    //        }
    //    })
    //});

    if (value.length != 0) {
        value.forEach(function (element) {
            var newOption = new Option(element.Progress, element.Progress, true, true);
            $('#' + element.ID + '_Progress').append(newOption).trigger('change');
        });
    }


    $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "BAST_PARTIAL", function (json) {
        $(".Select2MappingItemQty").select2({
            placeholder: "Select Item Qty",
            data: json.result,
            dropdownParent: $('#modalUploadBAST'),
            width: "100%",
            disabled: disabled,
        }).on("select2:select", function (obj) {
            var idSelect2 = obj.target.id;
            attachId = idSelect2.replace("_itemQty", "");
            var dataSelectedDoc = $('#' + idSelect2).select2('data');
            var dataSelected = dataSelectedDoc.map(function (item) {
                return item['id'];
            });
            dataSelected = dataSelected.toString();

            $.getJSON("UpdateItemPartialForBast?idItem=" + IdItem + "&attachmentId=" + attachId + "&dataselected=" + dataSelected + "")
                .then(function (data) {
                    return;
                })
        }).on("select2:unselect", function (obj) {
            var idSelect2 = obj.target.id;
            attachId = idSelect2.replace("_itemQty", "");
            var dataSelectedDoc = $('#' + idSelect2).select2('data');
            var dataSelected = dataSelectedDoc.map(function (item) {
                return item['id'];
            });
            dataSelected = dataSelected.toString();
            $.getJSON("UpdateItemPartialForBast?idItem=" + IdItem + "&attachmentId=" + attachId + "&dataselected=" + dataSelected + "")
                .then(function (data) {
                    return;
                })
        });


        if (value.length != 0) {
            value.forEach(function (elementss) {
                if (value.length != 0) {
                    value.forEach(function (element) {
                        if (element.QtyPartial != null) {
                            var datas = element.QtyPartial.split(','); // split string on comma space
                            $('#' + element.ID + '_itemQty').val(datas).trigger('change');

                        }
                    });
                }
            });
        }



        if (value.length != 0) {
            value.forEach(function (element) {
                $.getJSON("GetSelectItemByAttachId?search=" + "" + "&attachId=" + element.ID, function (data) {
                    var dataMapItem = data.result.map(function (item) {
                        return item['id'];
                    });

                    $('#' + element.ID).val(dataMapItem).trigger('change');
                });
            });
        }
    });

    $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "BAST", function (json) {
        $(".Select2MappingItem").select2({
            placeholder: "Select Item",
            data: json.result,
            dropdownParent: $('#modalUploadBAST'),
            width: "100%",
            disabled: disabled,
        }).on("select2:select", function (obj) {
            var data = obj.params.data.text;
            var idSelect2 = obj.target.id
            var attachId = obj.target.id;
            var selected = obj.params.data.selected;
            var itemId = obj.params.data.id

            if (data == 'All') {
                $("#" + idSelect2 + " > option").prop("selected", "selected");
                $("#" + idSelect2).trigger("change");
                var AllItem = $("#" + idSelect2 + " > option").prop("selected", "selected");


                for (x in AllItem) {
                    var itemIds = AllItem[x].value;
                    if (itemIds != 0) {
                        if (itemId != undefined) {
                            $.getJSON("UpdateItemMappingUpload?idItem=" + itemIds + "&attachmentId=" + attachId + "&selected=" + selected + "")
                                .then(function (datass) {
                                    return;
                                })
                        }
                    }
                }
                return;
            }

            $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected + "")
                .then(function (datas) {
                    return;
                })
        }).on("select2:unselecting", function (obj) {
            var attachId = obj.target.id;
            var selected = obj.params.args.data.selected
            var itemId = obj.params.args.data.id

            if (itemId != 0) {
                if (itemId != undefined) {
                    $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected + "")
                        .then(function (data) {
                            return;
                        })
                }
            }
        });


        var newOption = new Option("All", 0, true, true);
        $('.Select2MappingItem').append(newOption).trigger('change');


        if (value.length != 0) {
            value.forEach(function (element) {
                $.getJSON("GetSelectItemByAttachId?search=" + "" + "&attachId=" + element.ID, function (data) {
                    var dataMapItem = data.result.map(function (item) {
                        return item['id'];
                    });

                    $('#' + element.ID).val(dataMapItem).trigger('change');
                });
            });
        }
    });
});


function InitTableUploadInvoice() {
    $('#table-uploadInvoice').bootstrapTable({
       
        pagination: false,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: IdItem, //id Item
                type: "INVOICE",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: [
            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'align-top',
                formatter: runningFormatterNoPaging
            },
            {
                title: 'Action',
                field: 'Action',
                class: 'text-nowrap align-top',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    var btnReject = ''
                    var btnApprove = ''
                    var btnDownload = ''
                    var IsApprove = data.IsApprove;
                    if (isTaskUser > 0) {
                        btnDownload = `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>&nbsp;`;
                        btnDelete = `<a href="#" onclick="DeleteFileUpload(${data.ID},'INVOICE');" class="btn btn-light btn-xs" title="Delete"><span class="fa fa-trash"></span></a>`;
                        return [btnDownload, btnDelete].join('&nbsp;');
                    } else {
                        btnDownload = `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>&nbsp;`;
                        //Role.includes("Administrator,POSTFINANCEVIEWER") || Role.includes("Administrator,POSTVIEWER")

                        if (Role.includes("Administrator,POSTTAX") || Role.includes("Administrator,POSTFINANCE") || Role.includes("Administrator,POSTFINANCEBRANCH")) {
                            if (data.CountItemHasInvoiceSAP == 0) {
                                if (data.IsRejected == 0 && IsApprove == 0) {

                                    btnApprove = `<a href="#" onclick="ApproveAttachment(${data.ID},'INVOICE');" class="btn btn-light btn-xs" title="Approve"><span class="fa fa-check"></span></a>`;
                                }

                            }
                            if (data.IsRejected == 0) {
                                btnReject = `<a href="#" onclick="RejectAttachment(${data.ID},'INVOICE');" class="btn btn-light btn-xs" title="Reject"><span class="fa fa-times "></span></a>`;
                            }

                        }
                        return [btnDownload, btnApprove, btnReject].join('&nbsp;');
                    }
                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                class: 'align-top',
                halign: 'center',
                align: 'left',
                width: '100'
            },
            {
                title: 'Progress',
                width: '300',
                align: 'left',
                halign: 'center',
                class: 'align-top',
                visible: false,
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}_Progress"  class="Select2Progress form-control gosearch">`
                }
            },
            {
                title: 'Refer to BAST',
                width: '300',
                align: 'left',
                class: 'align-top',
                halign: 'center',
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}"  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                }
            }, {
                title: 'Notes',
                field: 'Notes',
                width: '750',
                
                align: 'left',
                class: 'align-top',
                halign: 'center',
                formatter: function (row, data, index) {
                    if (isTaskUser == 0) {
                        if (Role.includes('POSTBRANCH')) {
                            return `<textarea  class="form-control" style="height : 45px" id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                        } else {
                            return `<textarea  class="form-control" style="height : 45px"  disabled id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                        }
                    } else {
                        return `<textarea  class="form-control"  style="height : 45px" id="${data.ID}_notes" value="${data.Notes}" onchange="UpdateAttachmentNotes(${data.ID},this.value)" value="${data.Notes}">${data.Notes}</textarea>`
                    }
                }

            }, {
                title: 'Upload Date',
                field: 'UploadedDate',
                class: 'align-top',
                halign: 'center',
                align: 'left',
                width: '100',
                formatter: dateFormattertime


            },{
                title: 'Approve Date Finance',
                field: 'CreateDate',
                class: 'align-top',
                halign: 'center',
                align: 'left',
                width: '100',
                formatter: dateFormattertime


            },{
                title: 'Status',
                field: 'Status',
                class: 'align-top',
                align: 'center',
                halign: 'center',
                //visible: VisibleKOFAXMessage

            }, {
                title: 'Validation Message',
                field: 'StatusMessage',
                class: 'align-top',
                align: 'center',
                halign: 'center',
                //visible: VisibleKOFAXMessage

            }, {
                title: 'WithHolding Tax',
                field: 'WHTaxAmount',
                class: 'align-top',
                align: 'center',
                halign: 'center',
                //visible: VisibleKOFAXMessage

            }
        ]
    });
}


$('#table-uploadInvoice').on('post-body.bs.table', function (field, value, row, $element) {
   

    var disabled = false;
    if (isTaskUser == 0) {
        disabled = true;
    }
    $.getJSON("GetSelectItemAttachmentProgress?search=" + "" + "&id=" + IdItem + "&type=BAST", function (json) {
        $(".Select2Progress").select2({
            placeholder: "Select Progress",
            data: json.result,
            width: "50%",
            dropdownParent: $('#modalUploadInvoicing'),
            disabled: disabled,
        }).on("select2:select", function (obj) {
            var attachId = obj.target.id;
            var progress = obj.params.data.id
            var intprogress = progress.replace('%', '');

        });
    });

    if (value.length != 0) {
        value.forEach(function (element) {
            $('#' + element.ID + '_Progress').val(element.Progress).trigger('change');
        });
    }

    $(".Select2DocType").select2({
        placeholder: "Select Type",
        data: DocTypeSelect2,
        width: "50%",
        dropdownParent: $('#modalUploadInvoicing'),
        disabled: disabled,
    }).on("select2:select", function (obj) {
        var idSelect2 = obj.target.id;
        attachId = idSelect2.replace("_DocType", "");
        var dataSelectedDoc = $('#' + idSelect2).select2('data');
        var dataSelected = dataSelectedDoc.map(function (item) {
            return item['id'];
        });
        dataSelected = dataSelected.toString();

        $.getJSON("UpdateItemDocType?idItem=" + IdItem + "&attachmentId=" + attachId + "&dataselected=" + dataSelected + "")
            .then(function (data) {
                return;
            })
    }).on("select2:unselect", function (obj) {
        var idSelect2 = obj.target.id;
        attachId = idSelect2.replace("_DocType", "");
        var dataSelectedDoc = $('#' + idSelect2).select2('data');
        var dataSelected = dataSelectedDoc.map(function (item) {
            return item['id'];
        });
        dataSelected = dataSelected.toString();
        $.getJSON("UpdateItemDocType?idItem=" + IdItem + "&attachmentId=" + attachId + "&dataselected=" + dataSelected + "")
            .then(function (data) {
                return;
            })
    });


    if (value.length != 0) {
        value.forEach(function (elements) {
            if (value.length != 0) {
                value.forEach(function (element) {
                    if (element.DocType != null) {
                        var datas = element.DocType.split(','); // split string on comma space
                        $('#' + element.ID + '_DocType').val(datas).trigger('change');

                    }
                });
            }
        });
    }



    $.getJSON("GetSelectItemAttachmentProgress?search=" + "" + "&id=" + IdItem + "&type=BAST", function (json) {

        $(".Select2MappingItem").select2({
            placeholder: "Select Item",
            data: json.result,
            dropdownParent: $('#modalUploadInvoicing'),
            width: "100%",
            disabled: disabled,
        }).on("select2:select", function (obj) {
            var attachId = obj.target.id;
            var selected = obj.params.data.selected
            var itemId = obj.params.data.id

            var dataInvoice = $('#table-uploadInvoice').bootstrapTable("getData");
            var isValid = true;
            for (i = 0, j = dataInvoice.length; i < j; i++) {
                var idAttachment = dataInvoice[i].ID;               
                if (idAttachment != attachId) {
                    var BAST = $('#' + idAttachment).select2('val');
                    if(BAST != null){
                        if (itemId == BAST[0]) {
                            isValid = false;
                            i = dataInvoice.length;
                            $('#' + attachId).select2('val','');
                        } else {
                            isValid = true;
                        }
                    }
                }
            }

            if (isValid) {
                $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected + "")
                    .then(function (data) {
                        return;
                    })
            } else {
                swalError("BAST sudah digunakan di Invoice yang lain.");
            }
            
        }).on("select2:unselecting", function (obj) {
            var attachId = obj.target.id;
            var selected = obj.params.args.data.selected
            var itemId = obj.params.args.data.id
            $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected + "")
                .then(function (data) {
                    return;
                })
        });

        if (value.length != 0) {
            value.forEach(function (element) {
                $.getJSON("GetSelectItemByAttachId?search=" + "" + "&attachId=" + element.ID, function (data) {
                    var dataMapItem = data.result.map(function (item) {
                        return item['id'];
                    });

                    $('#' + element.ID).val(dataMapItem).trigger('change');
                });
            });
        }
    });
});




function RefreshAllTable() {
    $("#table-uploadBast").bootstrapTable('refresh');
    $("#table-uploadInvoice").bootstrapTable('refresh');
    $("#table-inprogress").bootstrapTable('refresh');
    $("#table-item").bootstrapTable('refresh');
}

function InitSelect2ItemMultiple() {
    $('#ItemSelect').val(null).trigger('change');
    $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "ITEM", function (json) {
        $("#ItemSelect").select2({
            placeholder: "Select Item",
            data: json.result,
            width: "100%"
        });
    });
}

function UpdateInvoiceName(id, name) {
    $.getJSON("UpdateNameAttachment?id=" + id + '&name=' + name)
        .then(function (data) {
            return;
        })
}
function UpdateAttachmentNotes(id, notes) {
    $.getJSON("UpdateAttachmentNotes?id=" + id + '&notes=' + notes)
        .then(function (data) {
            return;
        })
}
function GetListGrByRequestId(params) {
    //var param = {}
    var PoNo = $("#PoNo").val();
    var ItemId = $("#ItemId").val();
    var url = 'GetDataGRByPO?poNo=' + PoNo + '&itemId=' + ItemId;
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}

function GetListInvoiceByRequestId(params) {
    //var param = {}
    var PoNo = $("#PoNo").val();
    var ItemId = $("#ItemId").val();
    var url = 'GetDataInvoiceByPO?poNo=' + PoNo + '&itemId=' + ItemId;
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}

function InitTableUploadPO() {
    $('#table-uploadPO').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: requestId,
                type: "PO",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 10,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: [

            {
                title: 'No',
                align: 'center',
                halign: 'center',
                class: 'text-nowrap',
                formatter: runningFormatterNoPaging
            },

            {
                title: 'Action',
                field: 'Action',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                formatter: function (row, data, index) {
                    return ` <a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>
                              <a href="#" onclick="DeleteFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Delete"><span class="fa fa-trash"></span></a>`;
                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'

            },
        ]
    });
}

function DownloadGR() {

    var param = {
        "poNo": $("#PoNo").val(),
        "ItemId" : $("#ItemId").val()
    };
    $.ajax({
        cache: false,
        //async: false,
        url: 'DownloadGRData',
        method: 'GET',
        data: param,
        success: function (res) {
            setTimeout(function () {
                closeLoading();
            }, 3000);
            url = "/POST/DownloadResultGRData?guid=" + res
            window.open(url, '_blank');
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 3000);
            swalSuccess(' failed Download!');
        }
    })
}






Dropzone.autoDiscover = false;
$(document).ready(function () {
     
    $('[data-toggle="tooltip"]').tooltip()
    $('.select2').select2();
    InitSelect2Destination();
    InitDateRange();
    InitTableInProgress();
    InitTableUploadBast();
    InitTableUploadInvoice();
    InitTableItem();
    InitTableUploadPO();
    InitDropZoneInvoice();
    InitDropZoneBast();
    InitDropZonePO();
    InitTableViewNotes();
    InitTablepartialQty();
    InitTablehardcopyInvoice();
    //InitTableGr();
    //InitTableInvoice();
})