var $table = $('#tableDeliveryRequisition');
var $searchInput = $("#txtSearchData").val();

$.get("/EMCS/GetGrList", function (resp) {
    console.log(resp);
});

function ActionFormatter(value, row) {
    var htm = [];
    if (row.Status === "Draft" || row.Status === "Revise" || row.Status === "Complete") {
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        if (allowUpdate === "True")
<<<<<<< HEAD
            htm.push('<button class="edit btn btn-info btn-link btn-xs"><i class="tim-icons icon-pencil"></i></button> ');
        if (row.Status !== "complete") {
            // ReSharper disable once UseOfImplicitGlobalInFunctionScope
            if (allowDelete === "True")
                htm.push('<button class="remove btn btn-danger btn-link btn-xs"><i class="tim-icons icon-simple-remove"></i></button> ');
=======
            htm.push('<button class="edit btn btn-primary btn-xs"><i class="fa fa-edit"></i></button> ');
        if (row.Status !== "complete") {
            // ReSharper disable once UseOfImplicitGlobalInFunctionScope
            if (allowDelete === "True")
                htm.push('<button class="remove btn btn-danger btn-xs"><i class="fa fa-remove"></i></button> ');
>>>>>>> 639d8d0 (Intial commit)
        }
    } else {
        htm.push("-");
    }
    return htm.join("");
}

function operateFormatter() {
    const btn = [];
    btn.push('<button onclick = "showHistoryTable()" class="btn btn-xs btn-primary" alt="Edit remarks">');
    btn.push('<i class="fa fa-edit"></i>');
    btn.push("</button>");
    return btn.join("");
}

function showModal() {
    $("#formRequest")[0].reset();
    $(".error").hide();
    $("#formRequest").find("input").attr("disabled", false);
    $("select[name=CustID]").attr("disabled", false);
    $form.find("input[name=formType]").val("I");
    //$("#RequestID").attr("disabled", false);
    $form.find("textarea[name=CustAddress]").attr("disabled", false);
    $("button[name=Cancel]").show();
    $("button[name=SaveAsDraft]").show();
    $("button[name=SaveAsRevised]").hide();
    $("#myModalRequest").modal("show");
}

window.EventsFormatter = {
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
        },
            function (isConfirm) {
                if (isConfirm) {
                    sweetAlert.close();
                    return deleteDeliveryRequisition(row.ID);
                }
                return false;
            });
    },
    'click .edit': function (e, value, row) {
        var isdisabled = (row.Status === "complete") ? "disabled" : false;
        if (row.Status === "reject" || row.Status === "revise") {
            $("#RejectNoteSpace").show();
            $("textarea[name=RejectNote]").val(row.RejectNote);
        }
        $form.find("input[name=Origin]").val(row.Origin).attr("disabled", isdisabled);
        $form.find("input[name=Model]").val(row.Model).attr("disabled", isdisabled);
        $form.find("input[name=SerialNumber]").val(row.SerialNumber).attr("disabled", isdisabled);
        $form.find("input[name=Batch]").val(row.Batch).attr("disabled", isdisabled);
        $form.find("input[name=ReqHp]").val(row.ReqHp).attr("disabled", isdisabled);
        $form.find("textarea[name=CustAddress]").val(row.CustAddress).attr("disabled", isdisabled);
        $form.find("input[name=Kecamatan]").val(row.Kecamatan).attr("disabled", isdisabled);
        $form.find("input[name=Kabupaten]").val(row.Kabupaten).attr("disabled", isdisabled);
        $form.find("input[name=Province]").val(row.Province).attr("disabled", isdisabled);
        $form.find("input[name=PicName]").val(row.PicName).attr("disabled", isdisabled);
        $form.find("input[name=PicHP]").val(row.PicHP).attr("disabled", isdisabled);

        var toDs = row.TermOfDelivery.split(",");
        $.each(toDs,
            function (index, itemTod) {
                $form.find(`input[name=TOD][value='${itemTod}']`).prop("checked", true);
            });
        $form.find("input[name=TOD]").attr("disabled", isdisabled);

        var soDs = row.SupportingOfDelivery.split(",");
        $.each(soDs,
            function (index, itemSod) {
                $form.find(`input[name=SOD][value='${itemSod}']`).prop("checked", true);
            });
        $form.find("input[name=SOD]").attr("disabled", isdisabled);

        var incTs = row.Incoterm.split(",");
        $.each(incTs,
            function (index, itemInct) {
                $form.find(`input[name=INCT][value='${itemInct}']`).prop("checked", true);
            });
        $form.find("input[name=INCT]").attr("disabled", isdisabled);

        $form.find("input[name=ExpectedTimeArrival]").val(dateFormatterCAT(row.ExpectedTimeArrival))
            .attr("disabled", isdisabled);
        $form.find("input[name=ExpectedTimeLoading]").val(dateFormatterCAT(row.ExpectedTimeLoading))
            .attr("disabled", isdisabled);
        $form.find("input[name=ID]").val(row.ID).attr("disabled", isdisabled);
        $form.find("input[name=SoNo]").val(row.SoNo);
        $form.find("input[name=DoNo]").val(row.DoNo);

        //var newOption1 = new Option(row.ReqName, row.ReqID, false, false);
        //$('#RequestID').append(newOption1).trigger('change');
        //$("#RequestID").attr("disabled", isdisabled);

        var newOption2 = new Option(row.CustName, row.CustID, false, false);
        $("#CustomerID").append(newOption2).trigger("change");
        $("#CustomerID").attr("disabled", isdisabled);

        $form.find("select[name=CustID]").val(row.CustID).trigger("change");
        $("select[name=CustID]").attr("disabled", isdisabled);

        $form.find("input[name=formType]").val("U");
        $form.find("input[name=OdDate]").val(dateFormatterCAT(row.OdDate));

        if (row.PenaltyLateness) {
            $form.find("input[name=PenaltyLateness][value=true]").prop("checked", true).attr("disabled", isdisabled);
            $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);
        } else {
            $form.find("input[name=PenaltyLateness][value=false]").prop("checked", true).attr("disabled", isdisabled);
            $form.find("input[name=PenaltyLateness]").attr("disabled", isdisabled);
        }

        //if (isdisabled === "disabled") {
        //    $("button[name=Cancel]").hide();
        //    $("button[name=SaveAsDraft]").hide();
        //} else {
        //    $("button[name=Cancel]").show();
        //    $("button[name=SaveAsDraft]").show();
        //}
        $form.find("input[name=unit][value=MACHINE]").prop("checked", false);

        if (row.Unit === "MACHINE") {
            $form.find("input[name=unit][value=MACHINE]").prop("checked", true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit === "ENGINE") {
            $form.find("input[name=unit][value=ENGINE]").prop("checked", true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit === "FORKLIP") {
            $form.find("input[name=unit][value=FORKLIP]").prop("checked", true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        } else if (row.Unit === "RUE") {
            $form.find("input[name=unit][value=RUE]").prop("checked", true);
            $form.find("input[name=unit]").attr("disabled", isdisabled);
        }

        $form.find("input[name=Transportation][value=PTTU]").prop("checked", false);

        if (row.Transportation === "PTTU") {
            $form.find("input[name=Transportation][value='PTTU']").prop("checked", true).attr("disabled", isdisabled);
            $form.find("input[name=Transportation]").attr("disabled", isdisabled);
        } else if (row.Transportation === "Customer") {
            $form.find("input[name=Transportation][value='Customer']").prop("checked", true)
                .attr("disabled", isdisabled);
            $form.find("input[name=Transportation]").attr("disabled", isdisabled);
        }
        if (row.Status === "complete") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        } else if (row.Status === "reject") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").show();
            $("button[name=SubmitForm]").show();
            $("button[name=SaveAsRevised]").hide();
        } else if (row.Status === "revise") {
            $("button[name=Cancel]").show();
            $("button[name=SaveAsDraft]").hide();
            $("button[name=SubmitForm]").hide();
            $("button[name=SaveAsRevised]").show();
        }

        $("#myModalRequest").modal("show");
    }
};
function hideModal() {
    $("#myModalRequest").modal("hide");
}
var columnList = [
    {
        field: 'CustNr',
        title: 'Customer Number',
        halign: 'center',
        align: 'center',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'CustName',
        title: 'Customer Name',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Country',
        title: 'Country',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'City',
        title: 'City',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Street',
        title: 'Street',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Region',
        title: 'Region',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Telp',
        title: 'Telp',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    },
    {
        field: 'Fax',
        title: 'Fax',
        halign: 'center',
        align: 'left',
        class: 'text-nowrap',
        sortable: true
    }];

$(function () {
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: false,
        sidePagination: 'server',
        showColumns: false,
        searchOnEnterKey: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '10',
        queryParams: function (params) {
            params.SearchName = $("#mySearch input[name=searchText]").val();
            return params;
        },
        responseHandler: function (resp) {
            var data = {};
            $.map(resp, function (value) {
                data[value.Key] = value.Value;
            });
            return data;
            //return res.messages
        },
        formatNoMatches: function () {
            return '<span class="noMatches">No Data Found</span>';
        },
        columns: columnList
    });

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/CustomerEmcsPage',
        urlPaging: '/EMCS/CustomerEmcsPageXt',
        autoLoad: true
    });

    $("#mySearch").insertBefore($("[name=refresh]"));
});