var $table = $('#tableRegulation');

$(function () {
    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    //Date picker
    $('#datePicker').daterangepicker();
    $('.date').datepicker({
        container: '#boxdate'
    });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        //fixedColumns: true,
        //fixedNumber: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'Id',
            title: 'Action',
            align: 'center',
            formatter: operateFormatter,
            events: operateEvents
        },
        {
            class: "text-nowrap",
            title: 'Instansi / Kementrian / Lembaga',
            field: 'Instansi',
            width: 250,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Nomor',
            field: 'Nomor',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Jenis Regulasi',
            field: 'RegulationType',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Kategori',
            field: 'Category',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Referensi',
            field: 'Reference',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Deskripsi',
            field: 'Description',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'Nomor Regulasi',
            field: 'RegulationNo',
            width: 100,
            align: 'left',
            halign: "center"
        },
        {
            title: 'Tanggal Penetapan',
            align: 'center',
            formatter: function (data, row) {
                return dateFormatterCAT(row.TanggalPenetapan);
            }
        },
        {
            title: 'Tanggal Diundangkan',
            align: 'center',
            formatter: function (data, row) {
                return dateFormatterCAT(row.TanggalDiUndangkan);
            }
        },
        {
            title: 'Tanggal Berlaku',
            align: 'center',
            formatter: function (data, row) {
                return dateFormatterCAT(row.TanggalBerlaku);
            }
        },
        {
            title: 'Tanggal Berakhir',
            align: 'center',
            formatter: function (data, row) {
                return dateFormatterCAT(row.TanggalBerakhir);
            }
        },
        {
            class: "text-nowrap",
            title: 'Keterangan',
            field: 'Keterangan',
            width: 100,
            halign: "center"
        },
        {
            class: "text-nowrap",
            title: 'File Name',
            field: 'Files',
            width: 100,
            halign: "center"
        }]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/emcs/RegulationPage',
        urlPaging: '/emcs/RegulationPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"));

});

function operateFormatter(e, value, row) {
    console.log(row);
    var btn = [];
    btn.push('<div class="text-center" style="width:180px;text-align:center;align:center;margin:0 auto;">');
    //btn.push('<button type="button" class="btn btn-xs btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
<<<<<<< HEAD
    btn.push('<button type="button" class="btn btn-xs btn-link btn-info edit" title="Edit"><i class="tim-icons icon-pencil"></i></button>');
    btn.push('<button type="button" class="btn btn-xs btn-link btn-danger preview" title="Preview"><i class="tim-icons icon-zoom-split"></i></button>');
    if (row.Files !== "") {
        btn.push('<button type="button" class="btn btn-xs btn-link btn-success download" title="Download"><i class="tim-icons icon-cloud-download-93"></i></button>');
    }
    btn.push('<button type="button" class="btn btn-xs btn-link btn-success upload" title="Upload"><i class="tim-icons icon-cloud-upload-94"></i></button>');
    btn.push('<button type="button" class="btn btn-xs btn-link btn-danger remove" title="Delete"><i class="tim-icons icon-simple-remove"></i></button>');
=======
    btn.push('<button type="button" class="btn btn-xs btn-info edit" title="Edit"><i class="fa fa-edit"></i></button>');
    btn.push('<button type="button" class="btn btn-xs btn-default preview" title="Preview"><i class="fa fa-search"></i></button>');
    if (row.Files !== "") {
        btn.push('<button type="button" class="btn btn-xs btn-success download" title="Download"><i class="fa fa-download"></i></button>');
    }
    btn.push('<button type="button" class="btn btn-xs btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    btn.push('<button type="button" class="btn btn-xs btn-danger remove" title="Delete"><i class="fa fa-trash"></i></button>');
>>>>>>> 639d8d0 (Intial commit)
    btn.push('</div>');
    return btn.join(' ');
}

operateFormatter.DEFAULTS = {
    Add: false, Edit: false, Delete: false, Info: false, View: false, History: false, Preview: false, Upload: false
};

window.operateEvents = {
    'click .edit': function (e, value, row) {
        $(".editRecord").attr('href', '/EMCS/RegulationEdit/' + row.Id).trigger('click');
    },
    'click .preview': function (e, value, row) {
        e.preventDefault();
        var url = "/Upload/EMCS/Regulation/" + row.Files;
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        showPreviewDocument(url);
    },
    'click .upload': function (e, value, row) {
        $(".uploadRecord").attr('href', '/EMCS/RegulationUpload/' + row.Id).trigger('click');
    },
    'click .remove': function (e, value, row) {
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
            return false;
        });
    },
    'click .download': function (e, value, row) {
        location.href = myApp.fullPath + "/EMCS/DownloadRegulation/" + row.Id;
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/RegulationDelete',
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
        error: function (jqXhr) {
            sAlert('Error', jqXhr.status + " " + jqXhr.statusText, "error");
        }
    });

};

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function () {
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({
                keyboard: true
            }, 'show');

            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        var isValid = $(this).valid();
        if (isValid) {
            $('#progress').show();
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.Status === 0) {
                        if (result.Msg !== undefined) sAlert('Success', result.Msg, 'success');
                        $('#myModalPlace').modal('hide');
                        $('#progress').hide();
                        $("[name=refresh]").trigger('click');
                    }
                    else {
                        if (result.Msg !== undefined) sAlert('Failed', result.Msg, 'error');
                        $('#progress').hide();
                    }
                }
            });
        } else {
            swal({
                title: "Warning",
                text: "Please complete the form",
                type: "warning",
                buttons: true,
                dangerMode: true
            });
        }
        return false;
    });
};
