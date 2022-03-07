var selected = "PO Number";
var check = `<span class="fa fa-check-circle text-primary"></span>`;
var uncheck = `<span class="fa fa-times-circle text-disabled"></span>`;
var current = `<span class="fa fa-map-marker-alt text-success fa-spinner fa-pulse"></span>`;

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
Dropzone.autoDiscover = false;
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
function showLoading() {
    $("#loadingModal").modal({ backdrop: 'static', keyboard: false });
    $("#loadingModal").modal("show");
}

function closeLoading() {
    $("#loadingModal").modal("hide");
}

function DownloadGR() {
 
    var param = {
        "poNo": $("#poNo").val()
       
    };
    $.ajax({
        cache: false,   
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


function getColumns(types) {
    var type = $("#poType").val() ?? types;
    var labelEtd = "ETD";
    var labelAtd = "ATD";
    var labelEta = "ETA";
    var labelAta = "ATA";
    var labelPosition = "Position";

    if (type === "D") {
        labelEtd = "Plan Start Date";
        labelAtd = "Actual Complete Date";
        labelEta = "Plan Complete Date";
        labelAta = "Actual Finish Date";
        labelPosition = "Progress";
    }
    var columns = [{
        title: 'No',
        align: 'center',
        class: 'text-nowrap',
        width: '10',
        formatter: runningFormatterNoPaging
    },
    {
        title: 'Action',
        field: 'Item_Id',
        class: 'text-nowrap',
        align: 'center',
        visible: false,
        width: '110'
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
        //visible: statusPoVisible,
        formatter: function (value, row, index) {
            var data = '';
            if (value != null) data = value;
            if (data === "POD" || data === "PICK UP" || data === "ON PROGRESS") {
                return `<span class='badge badge-success'>` + data + `</span>`;
            } else {
                return `<span class='badge badge-success'>` + data + `</span>`;
            }
        }
    }, {
        title: labelPosition,
        field: 'POSITION',
        class: 'text-nowrap',
        halign: 'center',
        align: 'left',
        width: '110'
    },

    {
        title: 'Delivery Type',
        field: 'Delivery_Type',
        class: 'text-nowrap',
        align: 'center',
        //visible: deliveryTypeVisible,
        width: '180',
        visible: false,
    },
    {
        title: 'Forwarder',
        field: 'Forwarder',
        class: 'text-nowrap',
        align: 'center',
        width: '110',
        visible: false
    },
    {
        title: labelEtd,
        field: 'ETD',
        align: 'center',
        class: 'text-nowrap',
        width: '110',
        //visible: AtdVisible,
        formatter: dateSAPFormatter

    },
    {
        title: labelAtd,
        field: 'ATD',
        class: 'text-nowrap',
        align: 'center',
        width: '110',
        //visible: AtdVisible,
        formatter: dateSAPFormatter
    },
    {
        title: labelEta,
        field: 'ETA',
        align: 'center',
        class: 'text-nowrap',
        width: '110',
        //visible: AtaVisible,
        formatter: dateSAPFormatter

    },
    {
        title: labelAta,
        field: 'ATA',
        class: 'text-nowrap',
        align: 'center',
        width: '110',
        //visible: AtaVisible,
        formatter: dateSAPFormatter
    },
    {
        title: 'QTY',
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
        visible: false
    },
    {
        title: 'Currency',
        field: 'Currency',
        class: 'text-nowrap',
        halign: 'center',
        align: 'center',
        width: '10',
        visible: false
    },
    {
        title: 'Price / Item',
        field: 'Price_Item',
        class: 'text-nowrap',
        halign: 'center',
        align: 'right',
        width: '110',
        formatter: currencyFormatter,
        visible: false
    },
    {
        title: 'Total',
        field: 'Total',
        class: 'text-nowrap',
        align: 'right',
        halign: 'center',
        width: '110',
        formatter: currencyFormatter,
        visible: false
    },
    {
        title: 'Delivery Date',
        field: 'DeliveryDate',
        class: 'text-nowrap',
        align: 'center',
        formatter: dateSAPFormatter,
        width: '180',
        visible: false
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
        width: '110',

    },
    {
        title: 'GR No',
        field: 'GRNo',
        class: 'text-nowrap',
        align: 'left',
        width: '110',
        visible: false
        //visible: GRVisible,
    },
    {
        title: 'GR Date',
        field: 'GRDate',
        class: 'text-nowrap',
        align: 'left',
        width: '110',
        visible: false,
        formatter: dateSAPFormatter
    },
    {
        title: 'GR Posting Date',
        field: 'GRPostingDate',
        class: 'text-nowrap',
        align: 'left',
        width: '110',
        visible: false
        //visible: GRVisible,
    },
    {
        title: 'Notes',
        field: 'Notes',
        class: 'text-nowrap',
        align: 'center',
        width: '110',
        visible: false
    },
    {
        title: 'Doc. BAST',
        class: 'text-nowrap',
        align: 'center',
        width: '110',
        visible: false,
        formatter: function (row, data, index) {
            //var Item_Id = data.Item_Id;
            //var btnUpload = `<a class="btn btn-light btn-xs" onClick="InitModalUploadBast(${Item_Id})"  title="View Notes"><i class="fa fa-upload" title="View Notes"></i></a>`;
            //var ActionItem = [btnUpload].join('&nbsp;');
            //var btnUploadINVOICE = `<a class="btn btn-light btn-xs" onClick="InitModalUploadInvoice(${Item_Id})"  title="View Notes"><i class="fa fa-upload" title="View Notes"></i></a>`;

            //return ActionItem
        }
    }, {
        title: 'Doc. Invoice',
        class: 'text-nowrap',
        align: 'center',
        visible: false,
        width: '110',
        formatter: function (row, data, index) {
            //var Item_Id = data.Item_Id;
            //var btnUpload = `<a class="btn btn-light btn-xs" onClick="InitModalUploadInvoice(${Item_Id})"  title="View Notes"><i class="fa fa-upload" title="View Notes"></i></a>`;
            //var ActionItem = [btnUpload].join('&nbsp;');

            //return ActionItem
        }
    }]

    return columns;

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
    title: 'GR Amount',
    field: 'GRValue',
    class: 'text-nowrap',
    halign: 'center',
    align: 'left',
    width: '450',
    formatter: currencyFormatter
    
}]

function setMileStone(res) {
    if (res.status == "SUCCESS") {
        var dataSingle;
        if (res.result.length == 1) {
            dataSingle = res.result[0];
        } else {
            dataSingle = res.result;
        }

        setShowDetail(dataSingle.Request_Id);
        $('#poNo').val(dataSingle.PO_Number);
        $('#poType').val(dataSingle.POType);
        $('#VendorName').text(dataSingle.VendorName)
        $('#VendorAddress').text((dataSingle.VendorAddress === null || dataSingle.VendorAddress === "") ? "-" : dataSingle.VendorAddress)
        $('#DeliveryDate').text((dataSingle.DeliveryDate))
        $('#Destination').text(dataSingle.Destination)
        $('#DestinationAddress').text(dataSingle.DestinationAddress === "" ? PttuAdress : dataSingle.DestinationAddress)
        $('#RequiredDate').text(dateSAPFormatter(dataSingle.RequiredDate))

        $('#PrReleaseDate').text(dateSAPFormatter(dataSingle.PrReleaseDate))
        $('#PoReleaseDate').text(dateSAPFormatter(dataSingle.PoReleaseDate))
        $('#VendorReceivePoDate').html(dateSAPFormatter(dataSingle.VendorReceivePoDate) + '</br><b>PO Number : </b>' + dataSingle.PO_Number)
        $('#OrderConfirmDate').text(dateSAPFormatter(dataSingle.OrderConfirmDate))
        $('#ProcessingDate').text(dateSAPFormatter(dataSingle.ProcessingDate))

        if (!dataSingle.DeliveringDate) {
            $('#DeliveringDate').text("-")
        } else {
            $('#DeliveringDate').text(dateSAPFormatter(dataSingle.DeliveringDate))
        }

        $('#BastDate').text(dateSAPFormatter(dataSingle.BastDate))

        if (dataSingle.InvoiceDate === null) {
            $('#InvoiceDate').text("-")
        } else {
            $('#InvoiceDate').text(dateSAPFormatter(dataSingle.InvoiceDate))
        }

        $('#CloseDate').text(dateSAPFormatter(dataSingle.CloseDate))

        var hasGr = parseFloat(dataSingle.CountHasGR);
        var hasNotGr = parseFloat(dataSingle.CountNotGR);
        var totalGr = hasGr + hasNotGr;
        var percentGr = Math.ceil(((hasGr / totalGr) * 100));
        if (!percentGr) {
            percentGr = 0;
        }
        var poTypes = dataSingle.POType;

        var hasPod = parseFloat(dataSingle.CountItemPOD);
        var hasNotPod = parseFloat(dataSingle.CountItemNotPOD);
        var totalPod = hasPod + hasNotPod;
        var percentPod = Math.ceil(((hasPod / totalPod) * 100));
        if (!percentPod) {
            percentPod = 0;
        }

        var hasBast = parseFloat(dataSingle.CountItemHasbast);
        var hasNotBast = parseFloat(dataSingle.CountItemNotbast);
        var totalBast = hasBast + hasNotBast;
        var percentBast = Math.ceil(((hasBast / totalBast) * 100));

        var hasInvoice = parseFloat(dataSingle.CountItemhasInvoice);
        var hasNotInvoice = parseFloat(dataSingle.CountItemNotInvoice);
       
        var totalInvoice = hasInvoice + hasNotInvoice;
        var percentInvoice = Math.ceil(((hasInvoice / totalInvoice) * 100));

        if (poTypes === "D") {
            //$('#DeliveringDate').text(dataSingle.ProgressDelivering);
            $('#TotalGr').text(percentGr + "%");
            //$('#BastDate').text(dataSingle.ProgressBAST);
            if (dataSingle.ProgressDelivering === null) {
                $('#DeliveringDate').text("-");
            }
            else {
                $('#DeliveringDate').text(dataSingle.ProgressBAST);
            }
            if (dataSingle.ProgressDelivering === null) {
                $('#BastDate').text("-");
            }
            else {
                $('#BastDate').text(dataSingle.ProgressBAST);
            }
            if (dataSingle.ProgressInvoice === null) {
                $('#InvoiceDate').text("-");
            } else {
                $('#InvoiceDate').text(dataSingle.ProgressInvoice);
            }
            $('#DeliveringDate').text(hasPod + " of " + totalPod + " (" + percentPod + "%)")
            $('#TotalGr').text(hasGr + " of " + totalGr + " (" + percentGr + "%)")
            $('#BastDate').text(hasBast + " of " + totalBast + " (" + percentBast + "%)");

            if (totalInvoice === null) {
                $('#InvoiceDate').text("-");
            } else {
                $('#InvoiceDate').text(totalInvoice + " INVOICE");
            }
        } else {
            $('#DeliveringDate').text(hasPod + " of " + totalPod + " (" + percentPod + "%)")
            $('#TotalGr').text(hasGr + " of " + totalGr + " (" + percentGr + "%)")
            $('#BastDate').text(hasBast + " of " + totalBast + " (" + percentBast + "%)");

            if (totalInvoice === null) {
                $('#InvoiceDate').text("-");
            } else {
                $('#InvoiceDate').text(totalInvoice + " INVOICE");
            }
        }

        $("#id").val(dataSingle.Request_Id);
        var poNo = $("#SearchInput").val();
        $("#poNo").val(poNo);

        var VendorReceivePoDate = dateSAPFormatter(dataSingle.VendorReceivePoDate)
        var OrderConfirmDate = dateSAPFormatter(dataSingle.OrderConfirmDate)
        var ProcessingDate = dateSAPFormatter(dataSingle.ProcessingDate)
        var DeliveringDate = dateSAPFormatter(dataSingle.DeliveringDate)
        var CloseData = parseInt(dataSingle.CloseData);
        //var CloseDate = dateSAPFormatter(dataSingle.CloseDate)
        var HasGr = parseInt(dataSingle.CountHasGR);
        var HasBast = parseInt(dataSingle.CountItemHasbast);
        var HasInvoice = parseInt(dataSingle.CountItemhasInvoice);
        var HasInvoiceFinance = parseInt(dataSingle.CountItemHasInvoiceFinance);
        var HasInvoiceSAP = parseInt(dataSingle.CountItemHasInvoiceSAP);
        if (VendorReceivePoDate == '-') {
            document.getElementById("Circle-Receive").className = "btn btn-default btn-circle";
        } else {
            document.getElementById("Circle-Receive").className = "btn btn-primary btn-circle";
        }
        if (OrderConfirmDate == '-') {
            document.getElementById("Circle-Confirm").className = "btn btn-default btn-circle";
        } else {
            document.getElementById("Circle-Confirm").className = "btn btn-primary btn-circle";
        }
        if (ProcessingDate == '-') {
            document.getElementById("Circle-processing").className = "btn btn-default btn-circle";
        } else {
            document.getElementById("Circle-processing").className = "btn btn-primary btn-circle";
        }
        if (DeliveringDate == '-') {
            document.getElementById("Circle-delivering").className = "btn btn-default btn-circle";
        } else {
            var pointColorPod = (dataSingle.CountItemNotPOD == 0) ? "btn-primary" : "btn-warning";
            if (poTypes === "D") {
                pointColorPod = dataSingle.ProgressDelivering == "100%" ? "btn-primary" : "btn-warning";
            }
            document.getElementById("Circle-delivering").className = "btn " + pointColorPod + " btn-circle";
        }
        if (HasBast == 0) {
            document.getElementById("Circle-bast").className = "btn btn-default btn-circle";
        } else {
            var pointColorBast = (dataSingle.CountItemNotbast == 0) ? "btn-primary" : "btn-warning";
            //if (poTypes === "D") {
            //    pointColorBast = "btn-primary";
            //}
            document.getElementById("Circle-bast").className = "btn " + pointColorBast + " btn-circle";
        }
        if (HasGr == 0) {
            document.getElementById("Circle-gr").className = "btn btn-default btn-circle";
        } else {
            var pointColorGr = (dataSingle.CountNotGR == 0) ? "btn-primary" : "btn-warning";
            document.getElementById("Circle-gr").className = "btn " + pointColorGr + " btn-circle";
        }
        if (HasInvoice == 0) {
            document.getElementById("Circle-invoice").className = "btn btn-default btn-circle";
        } else {
            var pointColorInv = (dataSingle.CountItemNotInvoice == 0) ? "btn-primary" : "btn-warning";
            //if (poTypes === "D") {
            //    pointColorInv = dataSingle.ProgressInvoice == "100%" ? "btn-primary" : "btn-warning";
            //}
            document.getElementById("Circle-invoice").className = "btn " + pointColorInv + " btn-circle";
        }
        if (HasInvoiceFinance == 0) {
            document.getElementById("Circle-invoiceFinance").className = "btn btn-default btn-circle";
        } else {
            var pointColorInv = (dataSingle.CountItemNotInvoice == 0) ? "btn-primary" : "btn-warning";
            if (poTypes === "D") {
                pointColorInv = dataSingle.ProgressInvoice == "100%" ? "btn-primary" : "btn-warning";
            }
            document.getElementById("Circle-invoiceFinance").className = "btn " + pointColorInv + " btn-circle";
        }
        if (HasInvoiceSAP == 0) {
            document.getElementById("Circle-invoiceSAP").className = "btn btn-default btn-circle";
        } else {
            var pointColorInv = (dataSingle.CountItemNotInvoice == 0) ? "btn-primary" : "btn-warning";
            if (poTypes === "D") {
                pointColorInv = dataSingle.ProgressInvoice == "100%" ? "btn-primary" : "btn-warning";
            }
            document.getElementById("Circle-invoiceSAP").className = "btn " + pointColorInv + " btn-circle";
        }
       
        if (CloseData == '0') {
            document.getElementById("Circle-ClosePO").className = "btn btn-default btn-circle";
        } else {
            document.getElementById("Circle-ClosePO").className = "btn btn-primary btn-circle";
        }
    } else {
        $("#result-none").show();
        $("#result-multiple").hide();
        $("#result-single").hide();
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
    }
}
function InitDateRange() {
    $('#SearchInputDate').datepicker({
        format: 'dd.mm.yyyy',
        autoclose: true,
        clearBtn: true,
        orientation: "bottom"
    });
}

function GetPOByPONo() {
    showLoading();
    $.ajax({
        cache: false,
        async: false,
        url: '/Post/GetPOByPONo',
        data: {
            search: $('#SearchInput').val(),
        },
        method: 'GET',
        success: function (res) {
            setMileStone(res);
            setTimeout(function () {
                closeLoading();
            }, 1500);
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 1500);
            return;
        }
    })
}

function setShowDetail(idReq) {
    $("#LinkShowDetail").attr("href", "/POST/DetailDone?id=" + idReq);
}

function GetPOByPRNo() {
    $.ajax({
        cache: false,
        async: false,
        url: '/Post/GetPOByPRNo',
        data: {
            search: $('#SearchInput').val(),
        },
        method: 'GET',
        success: function (res) {
            if (res.status == "SUCCESS") {
                if (res.result.length == 0) {
                    $("#result-none").show();
                } else {
                    if (res.result.length == 1) {
                        $("#result-single").show();
                        setMileStone(res);
                    } else {
                        $("#table-done").bootstrapTable("refresh");
                        $("#result-multiple").show();
                        $("#result-none").hide();
                        setMileStone(res);
                    }
                }
            } else {
                $("#result-none").show();
                $("#result-multiple").hide();
                $("#result-single").hide();
                $("#SearchInput").show();
                $("#SearchInputDate").hide();
            }

            setTimeout(function () {
                closeLoading();
            }, 1500);
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 1500);
            return;
        }
    })
}

function GetPOByDate() {
    $.ajax({
        cache: false,
        async: false,
        url: '/Post/GetQuickSearchPOList',
        data: {
            search: $('#SearchInput').val(),
            filterBy: "Date"
        },
        method: 'GET',
        success: function (res) {
            if (res.status == "SUCCESS") {
                if (res.result.length == 0) {
                    $("#result-none").show();
                } else {
                    if (res.result.length == 1) {
                        $("#result-single").show();
                        $("#result-none").hide();
                        setMileStone(res);

                    } else {
                        $("#result-multiple").show();
                        $("#result-none").hide();
                    }
                }
            } else {
                $("#result-none").show();
                $("#result-multiple").hide();
                $("#result-single").hide();
                $("#SearchInput").show();
                $("#SearchInputDate").hide();
            }
            setTimeout(function () {
                closeLoading();
            }, 1500);
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 1500);
            return;
        }
    })
}

function GetPOByGoods() {
    $.ajax({
        cache: false,
        async: false,
        url: '/Post/GetPOByGoods',
        data: {
            search: $('#SearchInput').val(),
        },
        method: 'GET',
        success: function (res) {
            if (res.status == "SUCCESS") {
                if (res.result.length == 0) {
                    $("#result-none").show();
                } else {
                    $("#result-multiple").show();
                    $("#result-none").hide();
                    $("#result-single").hide();
                    $("#SearchInputDate").hide();
                }
            } else {
                $("#result-none").show();
                $("#result-multiple").hide();
                $("#result-single").hide();
                $("#SearchInput").show();
                $("#SearchInputDate").hide();
            }
            setTimeout(function () {
                closeLoading();
            }, 1500);
        },
        error: function (err) {
            setTimeout(function () {
                closeLoading();
            }, 1500);
            return;
        }
    })
}

function SubmitSearch() {
    $("#SearchInput").show();
    $("#SearchInputDate").hide();
    var name = selected;

    if (name === "PR Number") {
        $("#result-single").hide();
        $("#result-none").hide();
        $("#result-multiple").hide();
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
        GetPOByPRNo();
    } else if (name === "PO Number") {
        $("#result-single").show();
        $("#result-none").hide();
        $("#result-multiple").hide();
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
        GetPOByPONo();
    } else if (name === "PO Date") {
        $("#result-single").hide();
        $("#result-none").hide();
        $("#result-multiple").hide();
        $("#SearchInput").hide();
        $("#SearchInputDate").show();
        $("#table-done").bootstrapTable("refresh");
        GetPOByDate();
    } else if (name === "Vendor Name") {
        $("#result-single").hide();
        $("#result-none").hide();
        $("#result-multiple").show();
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
        $("#table-done").bootstrapTable("refresh");
       
    } else if (name === "Goods Name") {
        $("#table-done").bootstrapTable("refresh");
        GetPOByGoods();
    } else {
        $("#result-none").show();
        $("#result-single").hide();
        $("#result-multiple").hide();
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
        setTimeout(function () {
            closeLoading();
        }, 1500);
    }
}
function initTableDone() {
    $('#table-done').bootstrapTable({
        detailView: false,
        url: '/Post/GetQuickSearchPOList',
        queryParams: function (params) {
            var search = "";
            if (selected == "PO Date") {
                search = datePickerSAPFormatter($("#SearchInputDate").val())
            } else {
                search = $('#SearchInput').val();
            }
            var query = {
                search: search,
                filterBy: selected,
                type: 'public'
            }
            return query;
        },
        responseHandler: function (res) {
            setTimeout(function () {
                closeLoading();
            }, 1500);
            return res.result;
        },
        pagination: true,
        serverSort: false,
        columns: [
            {
                title: 'No',
                class: 'text-center',
                align: 'center',
                width: '50',
                formatter: runningFormatterNoPaging

            },
            {
                title: 'Action',
                class: 'text-center',
                dataAlign: 'center',
                width: '80',
                formatter: function (data, row, index) {
                    return `<a href="/Post/DetailDone?Id=${row.Request_Id}" class="btn btn-default btn-xs " style="background:#fcba12;color:white!important;" title="view"><i class="fa fa-eye"></i></a>`;
                }
            },
            {
                title: 'Company',
                class: 'text-nowrap',
                align: 'center',
                width: '110',
                visible: false,
                formatter: function (data, row, index) {
                    return "PT. Trakindo Utama"
                }
            },
            {
                title: 'PO No',
                field: 'PO_No',
                class: 'text-center',
                align: 'center',
                width: '110'

            },
            {
                title: 'Count of Line Number',
                field: 'CountLineNumber',
                halign: 'center',
                align: 'center',
                width: '110',
                fixed: true

            },
            {
                title: 'Subtotal',
                field: 'SubTotal',
                halign: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,

            },
            {
                title: 'Total Include Tax (10%)',
                field: 'SubTotalTax10',
                halign: 'center',
                align: 'right',
                width: '110',
                formatter: currencyFormatter,

            },
            {
                title: 'Delivery Date',
                field: 'Delivery_Date',
                align: 'center',
                class: 'text-nowrap',
                width: '110',

            },
            {
                title: 'Contact Name',
                field: 'PICName',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'Contact Email',
                field: 'PICEmail',
                class: 'text-center',
                align: 'center',
                width: '110'
            },
            {
                title: 'PO Receipt Date',
                field: 'PO_Receipt_Date',
                class: 'text-center',
                align: 'center',
                width: '110',
                formatter: dateSAPFormatter
            },
            {
                title: 'Status PO',
                field: 'Status_PO',
                class: 'text-center',
                align: 'center',
                width: '110',
                formatter: function (value, row, index) {
                    var Processing = row.Processing;
                    var data = "";
                    var color = "badge-danger";
                    if (value != '-') {
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
                title: 'Confirmation',
                field: 'Confirmation',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetConfirmFlowChecklist,
            },
            {
                title: 'Processing',
                field: 'Processing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklist,
            },
            {
                title: 'Delivering',
                field: 'Delivering',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetDeliveryFlowChecklist,
            },
            {
                title: ' BAST',
                field: 'BAST',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistBast,
            },
            {
                title: 'GR/SA',
                field: 'GRSA',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistGRSA
            },
            {
                title: 'Invoicing',
                field: 'Invoicing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistInvoice,
            },
            {
                title: 'Payment',
                field: 'Invoicing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistPayment
            },
            {
                title: 'Close',
                field: 'Invoicing',
                class: 'text-center',
                align: 'center',
                width: '100px',
                formatter: GetProcessFlowChecklistClose
            }]
    });
    $('#table-done').on('post-body.bs.table', function (field, value, row, $element) {
        $('[data-toggle="tooltip"]').tooltip()
    })
}
function setValueTitle(name) {
    $("#search-by-text").html(name);
    $("#SearchInput").show();
    $("#SearchInputDate").hide();
    selected = name;
    if (name === "PR Number" || name === "PO Number" || name === "Vendor Name") {
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
    } else if (name === "PO Date") {
        $("#SearchInput").hide();
        $("#SearchInputDate").show();
    } else {
        $("#SearchInput").show();
        $("#SearchInputDate").hide();
    }
}
function GetProcessFlowChecklist(data, row, index) {
    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return check;
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetProcessFlowChecklistBast(data, row, index) {
    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return check;
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetProcessFlowChecklistInvoice(data, row, index) {
    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return check;
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetListItemByRequestId(params) {
    var requestId = $("#id").val();
    var url = 'GetListItemByRequestId?requestId=' + requestId;
    $.get(url).then(function (res) {
        params.success(res.result)
    })
}

function GetListGrByRequestId(params) {
    showLoading();
    var poNo = $("#poNo").val();
    var url = 'GetDataGRByPO?poNo=' + poNo;
    $.get(url).then(function (res) {
        params.success(res.result);
        setTimeout(function () {
            closeLoading();
        }, 3000);
    }).fail(function () {
        setTimeout(function () {
            closeLoading();
        }, 3000);
    }).done(function () {
        setTimeout(function () {
            closeLoading();
        }, 3000);
    })
}
function InitTableUploadBast() {
    var IdItem = $("#id").val();
    $("#BtnSaveInvoice").hide();
    $("#BtnSaveBast").hide();
    $('#table-uploadBast').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: IdItem,//id Item
                type: "PO_BAST",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 5,
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
                    var btnDownload = `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>&nbsp;`;
                    return [btnDownload].join('&nbsp;');
                }
            },
            {
                title: 'FileName',
                field: 'FileName',
                halign: 'center',
                align: 'left',
                width: '250'
            },
            {
                title: 'Progress',
                width: '300',
                align: 'left',
                halign: 'center',
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}_Progress" class="Select2Progress form-control gosearch">`
                }
            }, {
                title: 'BAST Date',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450',
                visible: false,
                formatter: function (row, data, index) {
                    return `<input type="datetime" name="BastDate" class="form-control input-sm" id="BastDate" placeholder="BAST Date" />`
                }
            },
            {
                title: 'Item',
                width: '900',
                align: 'left',
                halign: 'center',
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}" disabled  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                }
            }
        ]
    });
}
function InitTableUploadInvoice() {
    var IdItem = $("#id").val();
    $("#BtnSaveInvoice").hide();
    $("#BtnSaveBast").hide();
    $('#table-uploadInvoice').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: IdItem, //id Item
                type: "PO_INVOICE",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 5,
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
                    return `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>`;
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
            {
                title: 'Progress',
                width: '300',
                align: 'left',
                halign: 'center',
                formatter: function (row, data, index) {
                    return `<select id="${data.ID}_Progress"  class="Select2Progress form-control gosearch">`
                }
            },
            {
                title: 'Item',
                width: '400',
                field: 'ItemDescription',
                align: 'left',
                halign: 'center',
                ////visible: false,
                //formatter: function (row, data, index) {
                //    return `<select id="${data.ItemId}" disabled  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                //}
            }
        ]
    });
}
function InitTableUploadInvoiceFinance() {
   
    var IdItem = $("#id").val();
    $("#BtnSaveInvoice").hide();
    $("#BtnSaveBast").hide();
    $('#table-uploadInvoice').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachment',
        queryParams: function (params) {
            var query = {
                id: IdItem, //id Item
                type: "PO_INVOICE",
                status:"Review_Finance"
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 5,
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
                    return `<a href="#" onclick="DownloadFileUpload(${data.ID});" class="btn btn-light btn-xs" title="Download"><span class="fa fa-download"></span></a>`;
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
            {
                title: 'Item',
                width: '200',
                align: 'left',
                halign: 'center',
                //formatter: function (row, data, index) {
                //    return `<select id="${data.ID}" disabled  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                field: 'ItemDescription'
                //}
            },
            {
                title: 'Review Date',
                field: 'UploadedDate',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '350',
                formatter: dateSAPFormatter
            }, 
        ]
    });
}
function InitTableUploadInvoiceSAP() {
    var IdItem = $("#id").val();
    $("#BtnSaveInvoice").hide();
    $("#BtnSaveBast").hide();
    $('#table-uploadInvoice').bootstrapTable({
        pagination: true,
        serverSort: false,
        loadingTemplate: "loadingTemplate",
        url: '/Post/GetListAttachmentSAP',
        queryParams: function (params) {
            var query = {
                id: IdItem, //id Item
                type: "PO_INVOICE",
            }
            return query;
        },
        responseHandler: function (res) {
            return res.result;
        },
        pageSize: 5,
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
                title: 'Invoice No',
                field: 'InvoiceNo',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450'
            },
            {
                title: 'Invoice Date',
                field: 'InvoiceDate',
                class: 'text-nowrap',
                halign: 'center',
                align: 'left',
                width: '450',
                formatter: dateSAPFormatter

            },
            {
                title: 'Item',
                width: '200',
                align: 'left',
                halign: 'center',
                //formatter: function (row, data, index) {
                //    return `<select id="${data.ID}" disabled  multiple="multiple" class="Select2MappingItem form-control gosearch">`
                field: 'ItemDescription'
                //}
            }

        ]
    });
}
$(document).ready(function () {
    $("#SearchButtonQuick").click(function (e) {
        showLoading();
        setTimeout(function () {
            SubmitSearch();
        }, 1000);
    })

    $table = $("#table-received-po").bootstrapTable({
        url: 'GetListItemByRequestId?requestId',
        detailView: false,
        pagination: true,
        pageSize: 5,
        pageList: [5, 10, 25, 50, 100, 200],
        columns: getColumns(poType),
        responseHandler: function (resp) {
            var data = [];
            $.map(resp.result, function (value, key) {
                data.push(value);
            });
            return data;
        }
    });

    $('#Circle-Receive').click(function () {
        $('#modalPoDetail').modal();
        var poType = $("#poType").val();
        var requestId = $("#id").val();
        $table.bootstrapTable('refreshOptions', {
            url: 'GetListItemByRequestId?requestId=' + requestId
        })
    })

    $('#Circle-Confirm').click(function () {
        $('#modalPoDetail').modal();

        var poType = $("#poType").val();
        var requestId = $("#id").val();
        $table.bootstrapTable('refreshOptions', {
            url: 'GetListItemByRequestId?requestId=' + requestId
        })
    })

    $('#Circle-processing').click(function () {
        $('#modalPoDetail').modal();
        var poType = $("#poType").val();
        var requestId = $("#id").val();
        $table.bootstrapTable('refreshOptions', {
            url: 'GetListItemByRequestId?requestId=' + requestId
        })
    })

    $('#Circle-delivering').click(function () {
        $('#modalPoDetail').modal();
        var poType = $("#poType").val();
        var requestId = $("#id").val();
        $table.bootstrapTable('refreshOptions', {
            url: 'GetListItemByRequestId?requestId=' + requestId
        })
    })

    $('#Circle-bast').click(function () {
        InitTableUploadBast();
        $("#modalUploadBAST").find("h4.modal-title").text("BAST LIST");
        $('#table-uploadBast').on('post-body.bs.table', function (field, value, row, $element) {
            $('#BastDate').datepicker({
                format: 'dd.mm.yyyy',
            });
            $('#BastDate').change(function (e) {
                return;
            });
            $(".modalUpload").hide();
            $(".Select2Progress").select2({
                placeholder: "Select Progress",
                data: ProgpressPercent,
                dropdownParent: $('#modalUploadInvoicing'),
                width: "100%",
                disabled: "disabled",
            }).on("select2:select", function (obj) {
                var attachId = obj.target.id;
                attachId = attachId.replace("_Progress", "");
                var progress = obj.params.data.id
                var intprogress = progress.replace('%', '');
                intprogress = parseInt(intprogress);

                $.getJSON("GetListAttachment?id=" + IdItem + "&type=PO_BAST", function (data) {
                    var value = data.result[0].Progress
                    value = value.replace('%', '');
                    value = parseInt(value);
                    if (value > intprogress) {
                        $("#table-uploadBast").bootstrapTable('refresh');
                        return swalWarning("Progress must be more than before!");
                    } else {
                        $.getJSON("UpdateAttachItemProgressPercent?progress=" + progress + "&attachmentId=" + attachId + "&type=INVOICE")
                            .then(function (datas) {
                                return;
                            })
                    }
                })
            });

            if (value.length != 0) {
                value.forEach(function (element) {
                    $('#' + element.ID + '_Progress').val(element.Progress).trigger('change');
                });
            }

            var requestId = $("#id").val();
            $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "BAST", function (json) {
                $(".Select2MappingItem").select2({
                    placeholder: "Select Item",
                    data: json.result,
                    dropdownParent: $('#modalUploadBAST'),
                    width: "100%"
                }).on("select2:select", function (obj) {
                    var attachId = obj.target.id;
                    var selected1 = obj.params.data.selected
                    var itemId = obj.params.data.id
                    $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected1 + "")
                        .then(function (data) {
                            return;
                        })
                }).on("select2:unselecting", function (obj) {
                    var attachId = obj.target.id;
                    var selected2 = obj.params.args.data.selected
                    var itemId = obj.params.args.data.id
                    $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected2 + "")
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
        $('#modalUploadBAST').modal();
    })
    $('#Circle-gr').click(function () {
        $('#modalGrList').modal("show");
        $("#table-gr-list").bootstrapTable({
            detailView: false,
            pagination: true,
            pageSize: 5,
            pageList: [5, 10, 25, 50, 100, 200],
            ajax: GetListGrByRequestId,
            columns: columnsGr
        });
    })
    $('#Circle-invoice').click(function () {
        $("#table-uploadInvoice").bootstrapTable('destroy');
        InitTableUploadInvoice();
        $(".modalUpload").hide();
        var requestId = $("#id").val();
        $("#modalUploadInvoicing").find("h4.modal-title").text("INVOICE LIST");
        $('#table-uploadInvoice').on('post-body.bs.table', function (field, value, row, $element) {
            $(".Select2Progress").select2({
                placeholder: "Select Progress",
                data: ProgpressPercent,
                dropdownParent: $('#modalUploadInvoicing'),
                width: "100%",
                disabled: "disabled",
            }).on("select2:select", function (obj) {
                var attachId = obj.target.id;
                attachId = attachId.replace("_Progress", "");
                var progress = obj.params.data.id
                var intprogress = progress.replace('%', '');
                intprogress = parseInt(intprogress);

                $.getJSON("GetListAttachment?id=" + IdItem + "&type=PO_BAST", function (data) {
                    var value = data.result[0].Progress
                    value = value.replace('%', '');
                    value = parseInt(value);
                    if (value > intprogress) {
                        $("#table-uploadBast").bootstrapTable('refresh');
                        return swalWarning("Progress must be more than before!");
                    } else {
                        $.getJSON("UpdateAttachItemProgressPercent?progress=" + progress + "&attachmentId=" + attachId + "&type=INVOICE")
                            .then(function (datas) {
                                return;
                            })
                    }
                })
            });

            if (value.length != 0) {
                value.forEach(function (element) {
                    $('#' + element.ID + '_Progress').val(element.Progress).trigger('change');
                });
            }

            $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "INVOICE", function (json) {
                $(".Select2MappingItem").select2({
                    placeholder: "Select Item",
                    data: json.result,
                    dropdownParent: $('#modalUploadBAST'),
                    width: "100%"
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
        $('#modalUploadInvoicing').modal();
    })
    $('#Circle-invoiceFinance').click(function () {
        $("#table-uploadInvoice").bootstrapTable('destroy');
        InitTableUploadInvoiceFinance();
        $(".modalUpload").hide();
        var requestId = $("#id").val();
        $("#modalUploadInvoicing").find("h4.modal-title").text("INVOICE LIST");
        $('#table-uploadInvoice').on('post-body.bs.table', function (field, value, row, $element) {
            $(".Select2Progress").select2({
                placeholder: "Select Progress",
                data: ProgpressPercent,
                dropdownParent: $('#modalUploadInvoicing'),
                width: "100%",
                disabled: "disabled",
            }).on("select2:select", function (obj) {
                var attachId = obj.target.id;
                attachId = attachId.replace("_Progress", "");
                var progress = obj.params.data.id
                var intprogress = progress.replace('%', '');
                intprogress = parseInt(intprogress);

                $.getJSON("GetListAttachment?id=" + IdItem + "&type=PO_BAST", function (data) {
                    var value = data.result[0].Progress
                    value = value.replace('%', '');
                    value = parseInt(value);
                    if (value > intprogress) {
                        $("#table-uploadBast").bootstrapTable('refresh');
                        return swalWarning("Progress must be more than before!");
                    } else {
                        $.getJSON("UpdateAttachItemProgressPercent?progress=" + progress + "&attachmentId=" + attachId + "&type=INVOICE")
                            .then(function (datas) {
                                return;
                            })
                    }
                })
            });

            if (value.length != 0) {
                value.forEach(function (element) {
                    $('#' + element.ID + '_Progress').val(element.Progress).trigger('change');
                });
            }

            if (value.length != 0) {
                value.forEach(function (element) {
                    $('#' + element.ID + '_Progress').val(element.Progress).trigger('change');
                });
            }

            $.getJSON("GetSelectItemByRequestId?search=" + "" + "&requestId=" + requestId + "&type=" + "BAST", function (json) {
                $(".Select2MappingItem").select2({
                    placeholder: "Select Item",
                    data: json.result,
                    dropdownParent: $('#modalUploadBAST'),
                    width: "100%"
                }).on("select2:select", function (obj) {
                    var attachId = obj.target.id;
                    var selected3 = obj.params.data.selected
                    var itemId = obj.params.data.id
                    $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected3 + "")
                        .then(function (data) {
                            return;
                        })
                }).on("select2:unselecting", function (obj) {
                    var attachId = obj.target.id;
                    var selected4 = obj.params.args.data.selected
                    var itemId = obj.params.args.data.id
                    $.getJSON("UpdateItemMappingUpload?idItem=" + itemId + "&attachmentId=" + attachId + "&selected=" + selected4 + "")
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
        $('#modalUploadInvoicing').modal();
    })
    $('#Circle-invoiceSAP').click(function () {
        $("#table-uploadInvoice").bootstrapTable('destroy');
        InitTableUploadInvoiceSAP();
        $(".modalUpload").hide();
        var requestId = $("#id").val();
        $("#modalUploadInvoicing").find("h4.modal-title").text("INVOICE LIST");       
        $('#modalUploadInvoicing').modal();
    })
    $('#Circle-ClosePO').click(function () {
        $('#modalPOInvoice').modal();
        $("#table-received-po").bootstrapTable({
            detailView: false,
            pagination: true,
            pageSize: 15,
            pageList: [5, 10, 25, 50, 100, 200],
            columns: columns
        });
    })
    setValueTitle(selected);
    initTableDone();
    InitDateRange();
})
var check = `<span class="fa fa-check-circle text-primary"></span>`;
var uncheck = `<span class="fa fa-minus-circle text-disabled"></span>`;
var uploadInProgress = `<span class="fa fa-check-circle text-warning"></span>`;
var uploadInDone = `<span class="fa fa-check-circle text-primary"></span>`;
var current = `<span class="fa fa-map-marker-alt text-danger fa-spinner fa-pulse"></span>`;
var attachmentBast = `<a href='#' onClick="InitModalUploadBast()"><span class="fa fa-paperclip"></span></a>`;
var attachmentInvoice = `<a href='#' onClick="InitModalUploadInvoice()"><span class="fa fa-paperclip"></span></a>`

function GetProcessFlowChecklistConfirmation(data, row, index) {
    var Confirmation_Date = dateSAPFormatter(row.Confirmation_Date);
    var checks = `<span data-toggle="tooltip" data-placement="right" title="${Confirmation_Date}" class="fa fa-check-circle text-primary"></span>`;

    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return checks;
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}

function GetProcessFlowChecklistProcessing(data, row, index, type) {
    var Processing_Date = dateSAPFormatter(row.Processing_Date);
    var check = `<span data-toggle="tooltip" data-placement="right" title="${Processing_Date}" class="fa fa-check-circle text-primary"></span>`;

    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return check;
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



function GetDeliveryFlowChecklist(data, row, index) {

    if (typePO == "D") {
        if (row.TotalNotComplete == 0) {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';
        } else if (row.TotalNotComplete > 0) {
            if (data == 'current') {
                return current;
            } else {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
            }
        } else {
            return current;
        }
    } else {
        if (row.TotalNotPOD == 0) {
            return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + check + '</span>';

        } else if (row.TotalNotPOD > 0) {
            if (data == 'current') {
                return current;
            } else {
                return '<span class="showDeliveryDate" data-toggle="tooltip" data-placement="top" title="' + data + '">' + uncheck + '</span>';
            }
        } else {
            return current;
        }
    }
}
function GetProcessFlowChecklistDelivering(data, row, index, type) {
    var Delivering_Date = dateSAPFormatter(row.Delivering_Date);
    var check = `<span data-toggle="tooltip" data-placement="right" title="${Delivering_Date}" class="fa fa-check-circle text-primary"></span>`;

    if (data == 'current') {
        return current;
    }
    else if (data == 'check') {
        return check;
    }
    else if (data == 'uncheck') {
        return uncheck;
    }
    else {
        return uncheck;
    }
}
function GetProcessFlowChecklistBast(data, row, index) {
    if (row.POType == "D") {
        if (row.CountItemHasbast == 0) {
            return uncheck;
        }
        else if (row.CountItemNotbast == 0) {
            return '<span class="showUploadedBAST">' + uploadInDone + '</span>';//attachmentInvoice;
        }
        else {
            return '<span class="showUploadedBAST">' + uploadInProgress + '</span>';
        }


    }
    else {
        if (row.CountItemHasbast == 0) {
            return uncheck;
        }
        else if (row.CountItemNotbast == 0) {
            return '<span class="showUploadedBAST">' + uploadInDone + '</span>';//attachmentInvoice;
        }
        else {
            return '<span class="showUploadedBAST">' + uploadInProgress + '</span>';
        }
    }
}

var CountInvoiceUpload = 0
var CountInvoiceNotUpload = 0

function GetProcessFlowChecklistInvoice(data, row, index) {
    CountInvoiceUpload = row.CountItemhasInvoice;
    CountInvoiceNotUpload = row.CountItemNotInvoice;
    if (row.CountItemhasInvoice == 0) {
        return uncheck;
    }
    else if (row.CountItemNotInvoice == 0) {
        return '<span class="showUploadedInvoice">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="showUploadedInvoice">' + uploadInProgress + '</span>';
    }
}


function GetProcessFlowChecklistInvoicing(data, row, index) {
    if (row.INVOICING == "uncheck") {
        return uncheck;
    } else {
        return check;
    }
}

var CounthasGRSA = 0
var CountnotyetGRSA = 0
function GetProcessFlowChecklistGRSA(data, row, index) {
    CounthasGRSA = row.TotalHasGR;
    CountnotyetGRSA = row.TotalNotGR;
    if (CounthasGRSA == 0) {
        return uncheck;
    }
    else if (CountnotyetGRSA <= 0) {
        return '<span class="">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="">' + uploadInProgress + '</span>';
    }   
}


function GetProcessFlowChecklistPayment(data, row, index) {
    return uncheck;
}

function GetProcessFlowChecklistClose(data, row, index) {
    return uncheck;
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip()
});