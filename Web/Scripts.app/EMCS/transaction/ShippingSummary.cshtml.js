var $table = $('#tableSS');
var $AllowDelete = $('#AllowDelete').val();
var $AllowUpdate = $('#AllowUpdate').val();


$(function () {
    var columns = [
        {
            field: "Id",
            title: "Action",
            align: "center",
            sortable: true,
            width: "125",
            formatter: function (data, row, index) {
                return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row });
            },
            events: operateEvents
        },
        //{
        //    field: "IdCargo",
        //    title: "IdCargo",
        //    sortable: true,
        //    visiable : false
        //},
        {
            field: "SsNo",
            title: "Ss No.",
            sortable: true,
            
        },
        {
            field: "ClNo",
            title: "CL No.",
            sortable: true,

        },
        {
            field: "CreateDate",
            title: "Date",
            sortable: true,
            formatter: function (data, row, index) {
                if (data) {
                    if (data) {
                        return moment(data).format("DD MMM YYYY");
                    } else {
                        return "-";
                    }
                } else {
                    return "-";
                }
            }
        },
        {
            field: "CreateBy",
            title: "PreparedBy",
            sortable: true
        },
        {
            field: "CiplNo",
            title: "Referrence",
            sortable: true
        },
        {
            field: "ConsigneeName",
            title: "Consignee Name.",
            sortable: true
        },
        {
            field: "ConsigneeAddress",
            title: "Consignee Address.",
            sortable: true
        },
        {
            field: "SoldToName",
            title: "Sold To Name.",
            sortable: true
        },
        {
            field: "SoldToAddress",
            title: "Sold To Address.",
            sortable: true
        },
        {
            field: "TotalPackage",
            title: "Total Package.",
            sortable: true
        },{
            field: "TotalVolume",
            title: "Total Amount.",
            sortable: true
        }
        //{
        //    field: "Category",
        //    title: "Category",
        //    sortable: true,
        //    class: "text-nowrap"
        //}, {
        //    field: "ConsingeeName",
        //    title: "ConsingeeName",
        //    sortable: true,
        //    class: "text-nowrap"
        //},
    ]

    $table.bootstrapTable(
        {
            url: "/EMCS/GetShippingSummary",
            columns: columns,
            cache: false,
            pagination: true,
            search: false,
            striped: true,
            clickToSelect: true,
            reorderableColumns: true,
            toolbar: '.toolbar',
            toolbarAlign: 'left',
            onClickRow: selectRow,
            sidePagination: 'server',
            showColumns: true,
            showRefresh: true,
            smartDisplay: false,
            pageSize: '5',
            queryParams: function (params) {
                params.term = $("#mySearch input[name=searchText]").val();
                return params;
            },
            responseHandler: function (resp) {
                var data = {};
                $.map(resp, function (value, key) {
                    data[value.Key] = value.Value;
                });
                return data;
            },
            formatNoMatches: function () {
                return '<span class="noMatches">No Data Found</span>';
            },

        });


    //window.pis.table({
    //    objTable: $table,
    //    //urlSearch: '/Json/Transaction/Table.json',
    //    //urlPaging: '/Json/Transaction/Table.json',
    //    autoLoad: true
    //});
    $("#mySearch").insertBefore($("[name=refresh]"));

});
//function get_SIdocumentlist() {

//    $.ajax({
//        url: '/EMCS/GetSIList',
//        type: 'Get',
//        cache: false,
//        async: false,
//        success: function (data, response) {
//            return data;
//        },

//        error: function (e) {
//            Swal.fire({
//                type: 'error',
//                title: 'Oops...',
//                text: 'Something went wrong! Fail Get Data',
//            })
//        }

//    })

//}


function operateFormatter(options) {

    var btn = [];
    btn.push('<div class="btn-group">');
    //if (options.Add == true)
    //    btn.push('<button type="button" class="btn btn-success new" title="Add"><i class="fa fa-plus"></i></button>')
    //if (options.Edit == true && options.Data.status === 'Draft')
    //    btn.push('<button type="button" class="btn btn-primary edit" title="Edit"><i class="fa fa-edit"></i></button>');
    //if (options.Delete == true && options.Data.status === 'Draft')
    //    btn.push('<button type="button" class="btn btn-danger remove" title="Delete"><i class="fa fa-trash-o"></i></button>');
    ////if (options.Info == true)
    btn.push('<button type="button"  class="btn btn-info info" title="Info"><i class="fa fa-search""></i></button>')

    btn.push('</div>');

    return btn.join('');
}

operateFormatter.DEFAULTS = {
    Add: false,
    Edit: false,
    Delete: false,
    Info: false,
    View: false,
    History: false
}

window.operateEvents = {
    'click .info': function (e, value, row, index) {
        var string = "si";
        var cargoid = row.Id;
        window.location.href = '/EMCS/ShippingSummaryView?cargoid=' + cargoid;
    }
};

//function deleteThis(id) {
//    $.ajax({
//        type: "POST",
//        url: myApp.root + 'Master/EmailRecipientManagementDeleteById',
//        beforeSend: function () { $('.fixed-table-toolbar').hide(); $('.fixed-table-loading').show(); },
//        complete: function () { $('.fixed-table-toolbar').show(); $('.fixed-table-loading').hide(); },
//        data: { EmailRecipientId: id },
//        dataType: "json",
//        success: function (d) {
//            if (d.Msg != undefined) {
//                sAlert('Success', d.Msg, 'success');
//            }

//            $("[name=refresh]").trigger('click');
//        },
//        error: function (jqXHR, textStatus, errorThrown) {
//            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
//        }
//    });

//};

//$(function () {
//    $.ajaxSetup({ cache: false });
//    $("a[data-modal]").on("click", function (e) {
//        $('#myModalContent').load(this.href, function () {
//            $('#myModalPlace').modal({
//                keyboard: true
//            }, 'show');

//            bindForm(this);
//        });
//        return false;
//    });


//});

//function bindForm(dialog) {
//    $('form', dialog).submit(function () {
//        $('#progress').show();
//        $.ajax({
//            url: this.action,
//            type: this.method,
//            data: $(this).serialize(),
//            success: function (result) {

//                if (result.Status == 0) {
//                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
//                    $('#myModalPlace').modal('hide');
//                    $('#progress').hide();
//                    $("[name=refresh]").trigger('click');
//                }
//                else {
//                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
//                    $('#progress').hide();
//                }
//            }
//        });
//        return false;
//    });
//};


//function addEmail(tableId) {
//    console.log(tableId);
//    var i = parseInt($('#' + tableId + ' tr:last').data('row')) + 1;
//    console.log(i);

//    var strTr = "<tr data-row=" + i + ">";
//    strTr += "<td><input type=\"email\" name=\"email\" id=\"email_" + i + "\" " +
//        "required=\"required\" class=\"form-control required\" " +
//        "style=\"width: 250px;\"></td>";
//    strTr += "<td><button type=\"button\" value=\"Delete\" class=\"btn-danger btn-md\" onClick='removeTr(this)' style=\"margin-left: 10px;\">Delete</button></td>";
//    strTr += "</tr>";
//    $('#' + tableId + ' tr:last').after(strTr);
//}


//function removeTr(obj) {
//    event.preventDefault();
//    $(obj).parent().parent().remove();

//}

// ********************************************************************************************************* 





