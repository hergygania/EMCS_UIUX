var $tableFormDocuments1 = $('#tableCiplFormDocuments1');

function load_data_tabledoc() {
    var columnDocument = [
    {
        field: '',
        title: 'Action',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'ID',
        title: 'No',
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
                var btnDownload = "<button class='btn btn-xs btn-link btn-success download' type='button'><i class='tim-icons icon-cloud-download-93'></i></button>";
                var btnPreview = "<button class='btn btn-xs btn-link btn-info btn-outline showDocument' type='button' data-toggle='modal' data-target='#myModalUploadPreview'><i class='tim-icons icon-single-copy-04'></i></button>";
                return [btnDownload, btnPreview].join(' ');
            } else {
                return "-";
            }
        },
        class: 'text-nowrap'
    }];

    function operateFormatter() {
        var btn = [];
        btn.push('<div class="btn-toolbar row">');
        btn.push('<button type="button" class="btn btn-info btn-link btn-xs edit" data-toggle="modal" data-target="#myModalDocument" title="Edit"><i class="tim-icons icon-pencil"></i></button>\
            <button type="button" class="btn btn-success btn-link btn-xs upload" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="tim-icons icon-cloud-upload-94"></i></button>');
        btn.push('<button type="button" class="btn btn-danger btn-link btn-xs remove" title="Delete"><i class="tim-icons icon-simple-remove"></i></button>');
        btn.push('</div>');
        return btn.join('');
    }

    $tableFormDocuments1.bootstrapTable({
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
function get_ciplviewdocument_by_id() {
    $.ajax({
        url: '/EMCS/GetCiplDocumentList',
        type: 'POST',
        data: {
            IdCipl: $('#idCipl').val(),
        },
        cache: false,
        async: false,
        success: function (data, response) {
            var convert = CiplDocumentConvert(data);
            if (convert.length > 0) {
                $tableFormDocuments1.bootstrapTable('removeAll');
                $tableFormDocuments1.bootstrapTable('load', convert);
            } else {
                $tableFormDocuments1.bootstrapTable('removeAll');
            }

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