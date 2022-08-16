$tablegrdocument = $('#tablegrDocuments');

//function load_data_tablegr() {
    debugger;
    var columnDocument = [
        {
            field: '',
            title: 'Action',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            events: operateEvents,
            formatter: function (data, row, index) {
                return operateFormatter(data, row, index);
            },

        },
        {
            field: 'Id',
            title: 'No',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        },
        {
            field: 'IdGr',
            title: 'RGBastid',
            halign: 'center',
            align: 'center',
            class: 'text-nowrap',
            sortable: true,
            visible: false
        },
        {
            field: 'DocumentDate',
            title: 'Date',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'DocumentName',
            title: 'Document Name',
            halign: 'center',
            align: 'left',
            class: 'text-nowrap',
            sortable: true
        },
        {
            field: 'Filename',
            title: 'Attachment',
            align: 'center',
            valign: 'center',
            halign: "center",
            class: 'text-nowrap',
            sortable: true,
            events: operateEventRight,
            formatter: function (data, row) {
                if (row.Filename !== "") {
                    var btnDownload = "<button class='btn btn-xs btn-success downloaddoc' type='button'><i class='fa fa-download'></i></button>";
                    var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocumentdoc' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                    return [btnDownload, btnPreview].join(' ');
                } else {
                    return "-";
                }
            },
            class: 'text-nowrap'
        }];
    $tablegrdocument.bootstrapTable({
        columns: columnDocument,
        cache: false,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbarDocument',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pagination: true,
        sidePagination: 'client',
        pageSize: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">No data found</span>';
        },
    });
//window.operateEventRight = {
//    'click .downloaddoc': function (e, value, row) {
//        debugger;
//        location.href = "/EMCS/DownloadGrDocument/" + row.Id;
//    },
//    'click .showDocumentdoc': function (e, value, row) {
//        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/GoodsReceive/" + row.Id + '/' + row.Filename;
//    }
//};

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

function operateFormatter(data, row, index) {
    var btn = [];
    btn.push('<div class="btn-toolbar row">');
    btn.push('<button type="button" class="btn btn-info btn-xs editDocument" data-toggle="modal" data-target="#myModalDocument" title="Edit" > <i class="fa fa-edit"></i></button >\
            <button type="button" class="btn btn-primary btn-xs uploadDocument" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="fa fa-upload"></i></button>');
    btn.push('<button type="button" class="btn btn-danger btn-xs removeDocument" title="Delete"><i class="fa fa-trash-o"></i></button>');
    btn.push('</div>');
    return btn.join('');
}
/*}*/

function ajaxInsertUpdateDocument() {
    debugger;
    var json = new Array();
    json.push({
        Id: $('#Id').val(),
        IdGr: $('#IdGr').val(),
        DocumentDate: $('#inp-doc-date').val(),
        DocumentName: $('#DocumentName').val()
    });
    $.ajax({
        url: '/EMCS/GRDocumentInsert',
        type: 'POST',
        data: {
            data: json
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success'
                , text: 'Saved, Your Data Has Been Saved',
            })
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })

}
function get_grdocumentlist() {
    debugger;
    $.ajax({
        url: '/EMCS/GetGRDocumentList',
        type: 'GET',
        data: {
            IdGr: $('#IdGr').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {
            debugger;
            var convert = GRDocumentConvert(data);
            convert.Id = 0;

            if (convert.length > 0) {
                $tablegrdocument.bootstrapTable('removeAll');
                $tablegrdocument.bootstrapTable('load', convert);
                //$tablegrdocument.bootstrapTable('remove', convert.Id);


                //$tableDocuments.bootstrapTable('refresh');
            } else {
                $tablegrdocument.bootstrapTable('removeAll');
            }
            convert.Id = 0;
        },

        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }

    })

}
function GRDocumentConvert(data) {
    debugger;
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            Id: element.Id,
            IdGr: element.IdGr,
            DocumentDate: moment(element.DocumentDate).format("DD MMM YYYY"),
            DocumentName: element.DocumentName,
            Filename: element.Filename
        }
        array.push(arraydata);
    })
    return array;
}
function GrDocumentDeleteById(id) {
    debugger;
    $.ajax({
        url: '/EMCS/GrDocumentDeleteById',
        type: 'POST',
        data: {
            id: id,
        },
        cache: false,
        async: false,
        success: function (data, response) {
            Swal.fire({
                type: 'success',
                title: 'Success',
                text: 'Success, Your Data Has Been Delete',
            })
        },
        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Update Data',
            })
        }
    })
    $("#Id").val("0");
}

$("#documentAddButton").on('click', function () {
    $('#Id').val(0),
        $('#DocumentName').val(null),
        $('#DocumentDate').val(null)
})
$("#btnAddDocument").on("click", function () {
    debugger;
    var id = $("#IdGr").val();

    if ($("#IdGr").val() == null || $("#IdGr").val() == 0) {
        Swal.fire({
            type: 'error',
            title: 'Opps..',
            html: 'Good Receive Id Is Not Found,Please save as Draft before Add Document'
        });
        return false;
    }
    else {
        ajaxInsertUpdateDocument();
        get_grdocumentlist();
    }


})

/*-------------------------------------------------------------------------------------*/

$("#listgetforview").on('click', function () {
   
    get_Grdocumentviewlist();
})
$tableDocuments1 = $('#tableGrDocuments1');
var columnDocument1 = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return "-";
        },

    },
    {
        field: 'Id',
        title: 'No',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'IdGr',
        title: 'RG Bast Id',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'DocumentDate',
        title: 'Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'DocumentName',
        title: 'Document Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Filename',
        title: 'Attachment',
        align: 'center',
        valign: 'center',
        halign: "center",
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row) {
            //if (row.Filename !== "") {
            //    var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
            //    var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
            //    return [btnDownload, btnPreview].join(' ');
            //} else {
            return "-";
            //}
        },
        class: 'text-nowrap'
    }];
$tableDocuments1.bootstrapTable({
    columns: columnDocument1,
    cache: false,
    search: false,
    striped: false,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: '.toolbarDocument',
    toolbarAlign: 'left',
    onClickRow: selectRow,
    showColumns: true,
    showRefresh: false,
    smartDisplay: false,
    pagination: true,
    sidePagination: 'client',
    pageSize: '5',
    formatNoMatches: function () {
        return '<span class="noMatches">No data found</span>';
    },
});
function get_Grdocumentviewlist() {

    $.ajax({
        url: '/EMCS/GetGRDocumentList',
        type: 'POST',
        data: {
            IdGr: $('#IdGr').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {
            
            var convert = GRDocumentConvert(data);
            convert.Id = 0;
            if (convert.length > 0) {
                $tableDocuments1.bootstrapTable('removeAll');
                $tableDocuments1.bootstrapTable('load', convert);
                $tableDocuments1.bootstrapTable('remove', convert.Id);


                //$tableDocuments.bootstrapTable('refresh');
            } else {
                $tableDocuments1.bootstrapTable('removeAll');
            }
            convert.Id = 0;
        },

        error: function (e) {
            Swal.fire({
                type: 'error',
                title: 'Oops...',
                text: 'Something went wrong! Fail Get Data',
            })
        }

    })

}



