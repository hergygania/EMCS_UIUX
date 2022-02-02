function runningFormatter(value, row, index, colname) {
    return row.index;
}

var Role = $('#Role').val();
function getBadgeColor(status) {
    var color = "";
    switch (status) {
        case "RISK DELAY":
            color = "badge-warning";
            break;
        case "DELAY":
            color = "badge-danger";
            break;
        default:
            color = "badge-success";
            break;
    }
    return color;
}

//-- Henry: Running number for no paging table
function runningFormatterNoPaging(value, row, index) {
    return index + 1;
}
//--

function IstaskByuser(id) {
    $.getJSON("IsTaskByUser?Id=" + id)
        .then(function (data) {
            return data;
        })
}

function LinkUrlRequest(NextProcessName, Request_Id) {
    var url = "";

    if (NextProcessName == 'CONFIRM PO') {
        url = `/Post/DetailOutstanding?Id=${Request_Id}`
    }
    else if (NextProcessName == 'PROCESS PO') {
        url = `/Post/DetailProcess?Id=${Request_Id}`
    }
    else if (NextProcessName == 'DELIVERY') {
        url = `/Post/DetailDelivering?Id=${Request_Id}`
    }
    else if (NextProcessName == 'DELIVERY CKB') {
        url = `/Post/DetailDeliveryConfirm?Id=${Request_Id}`
    }
    else if (NextProcessName == 'DELIVERY SUPPLIER') {
        url = `/Post/DetailDeliveryConfirm?Id=${Request_Id}`
    }
    else if (NextProcessName == 'DELIVERY HO') {
        url = `/Post/DetailDeliveryConfirm?Id=${Request_Id}`
    }
    else if (NextProcessName == 'BAST') {
        url = `/Post/DetailBast?Id=${Request_Id}`
    }
    else if (NextProcessName == 'INVOICING') {
        url = `/Post/DetailApprove?Id=${Request_Id}`
    }
    else if (NextProcessName == 'PO CLOSE') {
        url = `/Post/DetailProcess?Id=${Request_Id}`
    }
    else {
        url = `/Post/DetailDone?Id=${Request_Id}`
    }


    //if (Role.includes("POSTEXPEDITOR")) {
    //    url = `/Post/DetailDone?Id=${Request_Id}`
    //}



    return url;
}

function runningFormatterC(value, row, index) {
    return index + 1;
}

function showIframe(path) {
    var htm = "<object style=\"width:100%;min-height:500px;\" data=\"" + path + "\" type=\"application/pdf\"><iframe src=\"" + path + "\"></iframe></object>";
    $("#PDFPreview").html(htm);
    $("#PDFViewer").modal("show");
}


function swalSuccess(message) {
    swal({
        title: "Success",
        text: message,
        type: "success",
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false
    }, function (isConfirm) {
        if (isConfirm) {
            console.log('ok')
        }
    })
}

var VendorNameVisible = false;
if ($('#Role').val().substr($('#Role').val().length - 4) == "POST") {
    VendorNameVisible = true;
}



function CheckinputProgress(value) {
    var x;
    x = value;

    // If x is Not a Number or less than one or greater than 9999999999999999999
    if (isNaN(x) || x < 1 || x > 9999999999999999999) return false

    return true;

}

var DocTypeSelect2 = [
    "Invoice"
    //, "E-Faktur"
    //, "Surat Jalan"
    //, "Form GR/SA"
    //, "Certificate of Payment"
    //, "Copy PO"
    //, "Form QPAY"
    //, "Copy SKB"
]
var ProgpressPercent = [
    "10"
    , "20"
    , "30"
    , "40"
    , "50"
    , "60"
    , "70"
    , "80"
    , "90"
    , "100"
]

function swalInputComment() {
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
        // get value using textarea id
        swalSuccess("Successfully Saved!");
        return;
    });
}


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

function GetProcessFlowChecklistPrePayment(data, row, index, type) {
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


var check = `<span class="fa fa-check-circle text-primary"></span>`;
var uncheck = `<span class="fa fa-minus-circle text-disabled"></span>`;
var uploadInProgress = `<span class="fa fa-check-circle text-warning"></span>`;
var uploadInDone = `<span class="fa fa-check-circle text-primary"></span>`;
var current = `<span class="fa fa-map-marker-alt text-danger fa-spinner fa-pulse"></span>`;
var attachmentBast = `<a href='#' onClick="InitModalUploadBast()"><span class="fa fa-paperclip"></span></a>`;
var attachmentInvoice = `<a href='#' onClick="InitModalUploadInvoice()"><span class="fa fa-paperclip"></span></a>`

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
<<<<<<< HEAD
function GetProcessFlowChecklistInvoiceHardcopy(data, row, index) {
    if (row.CountHasSubmitHardcopyInvoice == 0) {
        return uncheck;
    }
    else if (row.CountNotSubmitHardcopyInvoice == 0) {
        return '<span class="showSubmithardcopy">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="showSubmithardcopy">' + uploadInProgress + '</span>';
    }

}
function GetProcessFlowChecklistInvoiceKOFAX(data, row, index) {
    if (row.CountKOFAXHasUpload == 0) {
        return uncheck;
    }
    else if (row.CountKOFAXNotUpload == 0) {
        return '<span class="showUploadedKOFAX">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="showUploadedKOFAX">' + uploadInProgress + '</span>';
    }

}
=======
>>>>>>> 639d8d0 (Intial commit)
function GetProcessFlowChecklistBast(data, row, index) {
    if (row.POType == "D") {
        if (row.CountHasUploadedBAST == 0) {
            return uncheck;
        }
        else if (row.CountNotUploadedBAST == 0) {
            return '<span class="showUploadedBAST">' + uploadInDone + '</span>';//attachmentInvoice;
        }
        else {
            return '<span class="showUploadedBAST">' + uploadInProgress + '</span>';
        }


    }
    else
    {
        if (row.CountHasUploadedBAST == 0) {
            return uncheck;
        }
        else if (row.CountNotUploadedBAST == 0) {
            return '<span class="showUploadedBAST">' + uploadInDone + '</span>';//attachmentInvoice;
        }
        else {
            return '<span class="showUploadedBAST">' + uploadInProgress + '</span>';
        }
    }
}

var CountInvoiceUpload = 0
var CountInvoiceNotUpload = 0
var CountInvoiceUploadVendor = 0
var CountInvoiceApprove = 0

function GetProcessFlowChecklistInvoiceDocument(data, row, index) {
    CountInvoiceUploadVendor = row.JmlInv;
    CountInvoiceApprove = row.JmlInvApprove;
    if (row.JmlInvApprove == 0) {
        return uncheck;
    }
    else if (CountInvoiceUploadVendor == CountInvoiceApprove) {
        return '<span class="showUploadedInvoice">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="showUploadedInvoice">' + uploadInProgress + '</span>';
    }
}

function GetProcessFlowChecklistInvoice(data, row, index) {
    CountInvoiceUpload = row.CountHasUploadedInvoice;
    CountInvoiceNotUpload = row.CountNotUploadedInvoice;
    if (row.CountHasUploadedInvoice == 0) {
        return uncheck;
    }
    else if (row.CountNotUploadedInvoice == 0) {
        return '<span class="showUploadedInvoice">' + uploadInDone + '</span>';//attachmentInvoice;
    }
    else {
        return '<span class="showUploadedInvoice">' + uploadInProgress + '</span>';
    }
}

function GetProcessFlowChecklistInvoiceFinance(data, row, index) {

    if (row.CountReviewInvoice == 0) {
        return uncheck;
    }
    else if (row.CountReviewInvoice > 0) {
        return '<span class="showUploadedInvoice">' + uploadInDone + '</span>';//attachmentInvoice;
    }
}

function GetProcessFlowChecklistInvoiceSAP(data, row, index) {
    console.log(row.CountPOSTINGSAP);
    if (row.CountPOSTINGSAP == 0) {
        return uncheck;
    }
    else if (row.CountPOSTINGSAP > 0) {
        return '<span class="showUploadedInvoice">' + uploadInDone + '</span>';//attachmentInvoice;
    }
}
function GetsuratketerangantidakpotongpajakChecklist(data, row, index) {
    if (row.suratketerangantidakpotongpajak == "NO") {
        return uncheck;
    } else {
        return check;
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
    if (row.TotalHasGR == 0) {
        return uncheck;
    }
    else if (row.TotalNotGR <= 0) {
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
function swalRequestSuccess(message) {
    swal({
        title: "Success",
        text: message,
        type: "success",
        focusConfirm: true,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        timer: 10000,
        //cancelButtonText: "No",
        showCloseButton: true
    }, function (isConfirm) {
        window.location = 'HomeVendor';
    })
}

function swalConfirmDelete(data, message) {
    swal({
        title: "Are you sure?",
        text: message,
        type: "info",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        showCloseButton: true

    }, function (isConfirm) {
        if (isConfirm) {
            var url = urlDelete + data.Id
            sendItem(url, methodPost, data)
        }

    })

}

function swalWarning(message) {
    swal({
        title: "Warning",
        text: message,
        type: "warning",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false,

    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}
function swalSuccess(message) {
    swal({
        title: "Success",
        text: message,
        type: "success",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false,

    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}
function swalError(message) {
    swal({
        title: "Failed",
        text: message,
        type: "error",
        showCancelButton: true,
        focusConfirm: false,
        confirmButtonColor: "#00c0ef",
        confirmButtonText: "OK",
        //cancelButtonText: "No",
        showCloseButton: false

    }, function (isConfirm) {
        if (isConfirm) {
            return;
        }
    })
}
function dateFormatter(value, row, index) {
    if (value) {
        return moment(value).format("DD/MM/YYYY");
    } else {
        return "-";
    }
}

function dateTimeFormatter(value, row, index) {
    if (value) {
        return moment(value).format("DD/MM/YYYY LT");
    } else {
        return "-";
    }
}

function datePickerSAPFormatter(value) {
    if (value == "-" || value == "") {
        return null;
    } else {
        return moment(value, 'DD.MM.YYYY').format('YYYY-MM-DD');
    }
}
function dateFormattertime(value, row, index) {
    if (value) {
        return moment(value).format("DD/MM/YYYY hh:mm:ss");
    } else {
        return "-";
    }
}

function dateSAPFormatter(value, row, index) {
    if (value) {
        return moment(value).format("DD.MM.YYYY");
    } else {
        return "-";
    }
}

function formatCurrency(amount, decimalSeparator, thousandsSeparator, nDecimalDigits) {
    decimalSeparator = typeof decimalSeparator != 'undefined' ? decimalSeparator : ',';
    thousandsSeparator = typeof thousandsSeparator != 'undefined' ? thousandsSeparator : '.';
    nDecimalDigits = typeof nDecimalDigits != 'undefined' ? nDecimalDigits : 0;
    amount = typeof amount != 'undefined' || amount != 'null' ? amount : 0;

    var num = parseFloat(amount); //convert to float

    if (isNaN(num)) {
        return 0;
    }
    // //default values

    var fixed = num.toFixed(nDecimalDigits); //limit or add decimal digits
    //separate begin [$1], middle [$2] and decimal digits [$4]
    var parts = new RegExp('^(-?\\d{1,3})((?:\\d{3})+)(\\.(\\d{' + nDecimalDigits + '}))?$').exec(fixed);

    if (parts) { //num >= 1000 || num < = -1000
        return parts[1] + parts[2].replace(/\d{3}/g, thousandsSeparator + '$&') + (parts[4] ? decimalSeparator + parts[4] : '');
    } else {
        return fixed.replace('.', decimalSeparator);
    }
}

// Make bootstrap table column as a money format
function currencyFormatter(value, index, row) {
    if (!value) {
        return 0;
    }
    var data = formatCurrency(value);
    return data;
}

function toInteger(str) {
    if (!str) {
        str = 0;
    }
    var data = str.toString().replace(/\./g, '');
    if ($.isNumeric(data)) {
        return parseInt(data);
    } else {
        return 0;
    }
}

Object.size = function (obj) {
    var size = 0, key;
    for (key in obj) {
        if (obj.hasOwnProperty(key)) size++;
    }
    return size;
}



function goToLastPage() {
    var lastPage = $(this).data("last");
    location.href = "/Post/HomeVendor";
}
