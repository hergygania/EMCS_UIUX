$tableDocuments = $('#tableCargoDocuments');

function load_data_tabledoc() {
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
            field: 'Cargoid',
            title: 'Cargo No',
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
                    var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
                    var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                    return [btnDownload, btnPreview].join(' ');
                } else {
                    return "-";
                }
            },
            class: 'text-nowrap'
        }];

    window.operateEventRight = {
        'click .download': function (e, value, row) {
            
            location.href = "/EMCS/DownloadCargoDocument/" + row.Id;
        },
        'click .showDocument': function (e, value, row) {
            document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/Cargo/" + row.Id + '/' + row.Filename;
        }
    };

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
        btn.push('<button type="button" class="btn btn-info btn-xs edit" data-toggle="modal" data-target="#myModalDocument" title="Edit" > <i class="fa fa-edit"></i></button >\
            <button type="button" class="btn btn-primary btn-xs upload" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="fa fa-upload"></i></button>');
        btn.push('<button type="button" class="btn btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
        btn.push('</div>');
        return btn.join('');
    }
    


    //window.operateEvents = {
    //    'click .download': function (e, value, row, index) {
    //        location.href = "/EMCS/ReportDO/" + row.id;
    //    }
    //};

    //function SetEditedRow(Id, DocumentName) {
    //    
    //    $('#Id').val(Id);
    //    $("input[name=Id]").val(Id);
    //    //$('#inp-doc-date').val(DocumentDate);
    //    $('#DocumentName').val(DocumentName);
    //}
    $tableDocuments.bootstrapTable({
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
    
}


function ajaxInsertUpdateDocument() {
    
    var json = new Array();
    json.push({
        Id: $('#Id').val(),
        IdCargo: $('#CargoID').val(),
        DocumentDate: $('#inp-doc-date').val(),
        DocumentName: $('#DocumentName').val()
    });
    $.ajax({
        url: '/EMCS/CargoDocumentInsert',
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
function CargoDocumentConvert(data) {
    var array = new Array();
    $.each(data, function (index, element) {
        var arraydata = {
            Id: element.Id,
            Cargoid: element.IdCargo,
            DocumentDate: moment(element.DocumentDate).format("DD MMM YYYY"),
            DocumentName: element.DocumentName,
            Filename: element.Filename
        }
        array.push(arraydata);
    })
    return array;
}

$("#documentAddButton").on('click', function () {
    
    $('#Id').val(0);
    $('#DocumentName').val(null);
})

$("#listgetforview").on('click', function () {
    
    get_cargodocumentviewlist();
})
function get_cargodocumentlist() {
    
    $.ajax({
        url: '/EMCS/GetCargoDocumentList',
        type: 'POST',
        data: {
            Cargoid: $('#CargoID').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {
            var convert = CargoDocumentConvert(data);
            convert.Id = 0;

            if (convert.length > 0) {
                $tableDocuments.bootstrapTable('removeAll');
                $tableDocuments.bootstrapTable('load', convert);
                $tableDocuments.bootstrapTable('remove', convert.Id);


                //$tableDocuments.bootstrapTable('refresh');
            } else {
                $tableDocuments.bootstrapTable('removeAll');
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
$("#btnAddItem").on('click', function () {
    $('#DocumentName').val() = null;
})
$("#btnAddDocument").on("click", function () {
    
    var id = $("#CargoID").val();

    if ($("#CargoID").val() == null || $("#CargoID").val() == 0) {
        Swal.fire({
            type: 'error',
            title: 'Opps..',
            html: 'Cargo Id Is Not Found,Please save as Draft before Add Document'
        });
        return false;
    }
    else {
        ajaxInsertUpdateDocument();
        get_cargodocumentlist();
    }


})

//******************************************Get For Only View Document*********************************************//
window.operateEventRightForView = {
    'click .download': function (e, value, row) {
        debugger;
        location.href = "/EMCS/DownloadCargoDocument/" + row.Id;
    },
    'click .showDocument': function (e, value, row) {
        debugger;
        document.getElementById('framePreview').src = myApp.fullPath + "Upload/EMCS/Cargo/" + row.Id + '/' + row.Filename;
    }
};
$tableDocuments1 = $('#tableCargoDocuments1');
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
        field: 'Cargoid',
        title: 'Cargo No',
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
        events: operateEventRightForView,
        formatter: function (data, row) {
            if (row.Filename !== "") {
                var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
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
function get_cargodocumentviewlist() {
    
    $.ajax({
        url: '/EMCS/GetCargoDocumentList',
        type: 'POST',
        data: {
            Cargoid: $('#CargoID').val(),

        },

        cache: false,
        async: false,
        success: function (data, response) {
            var convert = CargoDocumentConvert(data);
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
