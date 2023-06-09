var $tableFormDocuments1 = $('#tableBLAWBFormDocument');

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

    function operateFormatter() {
        var btn = [];
        btn.push('<div class="btn-toolbar row">');
        btn.push('<button type="button" class="btn btn-info btn-xs edit" data-toggle="modal" data-target="#myModalDocument" title="Edit"><i class="fa fa-edit"></i></button>\
            <button type="button" class="btn btn-primary btn-xs upload" data-toggle="modal" data-target="#myModalUploadPlace" title="Upload"><i class="fa fa-upload"></i></button>');
        btn.push('<button type="button" class="btn btn-danger btn-xs remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
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
            IdCipl: $('#idCl').val(),
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