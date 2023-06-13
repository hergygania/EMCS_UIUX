var $tableFormDocuments1 = $('#tableBLAWBFormDocument');

var columnDocument = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row });
        },
        events: operateEventsBlAWb
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
        field: 'Number',
        title: 'Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true,
        visible: false
    },
    {
        field: 'MasterBlDate',
        title: 'Master Bl Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'HouseBlNumber',
        title: 'House Bl Number',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'HouseBlDate',
        title: 'House Bl Date',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true,
        formatter: function (data, row, index) {
            return moment(data).format("DD MMM YYYY");
        }
    },
    {
        field: 'Publisher',
        title: 'Publish',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    //{
    //    field: 'Filename',
    //    title: 'Attachment',
    //    align: 'center',
    //    valign: 'center',
    //    halign: "center",
    //    class: 'text-nowrap',
    //    sortable: true,
    //    events: operateEventRight,
    //    formatter: function (data, row) {
    //        if (row.Filename !== "") {
    //            var btnDownload = "<button class='btn btn-xs btn-success download' type='button'><i class='fa fa-download'></i></button>";
    //            var btnPreview = "<button class='btn btn-xs btn-primary btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='fa fa-file-pdf-o'></i></button>";
    //            return [btnDownload, btnPreview].join(' ');
    //        } else {
    //            return "-";
    //        }
    //    },
    //    class: 'text-nowrap'
    //}
];

function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="btn-group">');
    btn.push('<button type="button" class="btn btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    btn.push('</div>');
    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false,
    Preview: false,
    Upload: false
};

window.operateEventsBlAWb = {
    'click .edit': function (e, value, row, index) {
        getbyid(row);
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
                return deleteThis(row.Id);
            }
        });
    },
};


$(function () {
    $tableFormDocuments1.bootstrapTable({
        cache: false,
        url: "/Emcs/GetBlAwbListByCargo",
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        queryParams: function (params) {
            return {
                limit: params.limit,
                offset: params.offset,
                Cargoid: $("#CargoID").val(),
                sort: params.sort,
                order: params.order
            };
        },
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        formatNoMatches: function () {
            return '<span class="noMatches">Data Not Found</span>';
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value, key) {
                data[value.Key] = value.Value;
            });
            return data;
        },
        columns: columnDocument
    });

});
window.operateEventRight = {
    'click .download': function (e, value, row) {
        location.href = "/EMCS/DownloadBlAWBDocument/" + row.Filename;
    },
    'click .showDocument': function (e, value, row) {
        document.getElementById('framePreview').src = myApp.fullPath + "/Upload/EMCS/BLAWB/" + row.Filename;
    }
};

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr("href", `/EMCS/UpdateGrItem/?Id=${row.Id}&IdGr=${row.IdGr}`).trigger("click");
    },
    'click .upload': function (e, value, row) {
        $(".uploadRecord").attr("href", `/EMCS/UploadGrItem/${row.Id}`).trigger("click");
    },
    'click .remove': function (e, value, row) {
        Swal.fire({
            title: "Confirmation",
            text: "Are you sure want to remove this data?",
            type: "question",
            showCancelButton: true,
            cancelButtonColor: "#d33",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Yes, Remove!",
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        }).then((result) => {
            if (result.value) {
                sweetAlert.close();
                return deleteThis(row.Id);
            }
            return false;
        });
    },
    'click .editDocument': function (e, value, row, index) {

        $('#Id').val(row.Id);
        $('#inp-doc-date').val(row.DocumentDate);
        $('#DocumentName').val(row.DocumentName);
    },
    'click .removeDocument': function (e, value, row, index) {

        GrDocumentDeleteById(row.Id);
        get_grdocumentlist();
    },
    'click .uploadDocument': function (e, value, row, index) {

        $('#IdDocumentUpload').val(row.Id);
        //$(".uploadRecord").attr('href', '/EMCS/CiplDocumentUpload/' + row.Id).trigger('click');
    }
};
