var $table = $('#tableBanner');

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
        pageSize: '5',
        //fixedColumns: true,
        //fixedNumber: '5',
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        columns: [{
            field: 'ID',
            title: 'Action',
            //width: '180px',
            halign: 'center',
            align: 'center',
            formatter: operateFormatter({ Edit: true, Delete: true, Preview: true}),
            events: operateEvents
        },
        {
            field: 'Name',
            title: 'Name',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'Description',
            title: 'Description',
            halign: 'center',
            align: 'left',
            sortable: true
        },
        {
            field: 'StartedDate',
            title: 'Start Date',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'FinishedDate',
            title: 'Finish Date',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                return moment(data).format("DD MMM YYYY");
            }
        },
        {
            field: 'IsActive',
            title: 'Is Active',
            halign: 'center',
            align: 'center',
            sortable: true,
            formatter: function (data, row, index) {
                if (data) {
                    return "<span class='label label-success'>Yes</span>";
                } else {
                    return "<span class='label label-danger'>No</span>";
                }
            }
        },
        {
            field: 'Images',
            title: 'Images',
            halign: 'center',
            align: 'center',
            sortable: true,
            events: operateEvents,
            formatter: function (data, row, index) {
                var htm = "<div class='center'>";
                if (data !== "") {
                    htm += "<button class='btn btn-link btn-success btn-link preview'><i class='tim-icons icon-image-02'></i></button>";
                } else {
                    htm += "-";
                }
                htm += "</div>";
                return htm;
            }
        }]
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/emcs/BannerPage',
        urlPaging: '/emcs/BannerPageXt',
        autoLoad: true
    });
    $("#mySearch").insertBefore($("[name=refresh]"))

});


function operateFormatter(options) {
    var btn = [];
    btn.push('<div class="">');
    if (options.Add === true)
        btn.push('<button type="button" class="btn btn-success btn-link new" title="Add"><i class="tim-icons icon-simple-add"></i></button>')
    if (options.Edit === true)
        btn.push('<button type="button" class="btn btn-info btn-link btn-xs edit" title="Edit"><i class="tim-icons icon-pencil"></i></button>\
            <button type="button" class="btn btn-success btn-link btn-xs upload" title="Upload"><i class="tim-icons icon-cloud-upload-94"></i></button>');
    //if (options.Preview === true)
    //    btn.push('<button type="button" class="btn btn-default info" title="Info"><i class="fa fa-eye"></i></button>');
    //if (options.Upload === true)
    //    btn.push('<button type="button" class="btn btn-primary upload" title="Upload"><i class="fa fa-upload"></i></button>');
    if (options.Delete === true)
        btn.push('<button type="button" class="btn btn-danger btn-link btn-xs remove" title="Delete"><i class="tim-icons icon-simple-remove"></i></button>');
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
}

window.operateEvents = {
    'click .edit': function (e, value, row, index) {
        $(".editRecord").attr('href', '/EMCS/BannerEdit/' + row.Id).trigger('click');
    },
    'click .preview': function (e, value, row, index) {
        $(".previewImages").attr('href', '/EMCS/PreviewImage/' + row.Id).trigger('click');
    },
    'click .upload': function (e, value, row, index) {
        $(".uploadRecord").attr('href', '/EMCS/BannerUpload/' + row.Id).trigger('click');
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
    }
};

function deleteThis(id) {
    $.ajax({
        type: "POST",
        url: myApp.root + 'EMCS/BannerDelete',
        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
        data: { ID: id },
        dataType: "json",
        success: function (d) {
            if (d.Msg !== undefined) {
                sAlert('Success', d.Msg, 'success');
            }

            $("[name=refresh]").trigger('click');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });

};

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


});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
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
        return false;
    });
};
