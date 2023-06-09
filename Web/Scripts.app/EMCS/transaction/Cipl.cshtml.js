﻿// CIPL LIST
var $tableCipl = $("#tableCipl");
var $AllowDelete = true;
var $AllowUpdate = true;

$(function () {
  var columns = [
    //{
    //    field: "Id",
    //    title: "No",
    //    align: "center",
    //    formatter: runningFormatter
    //},
    {
      field: "",
      title: "Action",
      align: "center",
      halign: "center",
      sortable: true,
      width: "130",
      formatter: function (data, row, index) {
        return operateFormatter({ Edit: Boolean($AllowUpdate), Delete: Boolean($AllowDelete), Data: row });
      },
      events: operateEventsCipl,
    },
    {
      field: "CiplNo",
      title: "CIPL No.",
      halign: "center",
      align: "center",
      sortable: true,
    },
    {
      field: "EdoNo",
      title: "DO No",
      halign: "center",
      align: "left",
      sortable: true,
    },
    {
      field: "Category",
      title: "Category",
      halign: "center",
      align: "left",
      sortable: true,
    },
    {
      field: "ShippingMethod",
      title: "Shipping Method",
      halign: "center",
      align: "center",
      sortable: true,
    },
    {
      field: "Forwader",
      title: "Forwader",
      halign: "center",
      align: "center",
      sortable: true,
    },
    {
      field: "ConsigneeName",
      title: "Consignee",
      align: "left",
      halign: "center",
      sortable: true,
      class: "text-nowrap",
    },
    //, {
    //    field: "consignee",
    //    title: "DA No.",
    //    align: "center",
    //    sortable: true,
    //    class: "text-nowrap"
    //},
    //{
    //    field: "branch",
    //    title: "ETD",
    //    sortable: true,
    //    class: "text-nowrap"
    //},
    {
      field: "StatusViewByUser",
      title: "Status",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
      class: "text-nowrap",
    },
  ];

  $tableCipl.bootstrapTable({
    columns: columns,
    cache: false,
    pagination: true,
    search: false,
    striped: false,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: ".toolbar",
    toolbarAlign: "left",
    onClickRow: selectRow,
    sidePagination: "server",
    showColumns: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: "5",
    loadingFontSize: "16px",
    classes: "",
    icons: {
      paginationSwitchDown: "uil uil-arrow-circle-down",
      paginationSwitchUp: "uil uil-arrow-circle-up",
      refresh: "uil uil-sync",
      toggleOff: "uil uil-toggle-off",
      toggleOn: "uil uil-toggle-on",
      columns: "uil uil-columns",
      fullscreen: "uil uil-expand-arrows-alt",
      detailOpen: "uil uil-plus",
      detailClose: "uil uil-minus",
    },
    formatNoMatches: function () {
      return '<span class="noMatches">No task available</span>';
    },
  });

  window.pis.table({
    objTable: $tableCipl,
    urlSearch: "/EMCS/CiplListPage",
    urlPaging: "/EMCS/CiplListPageXt",
    autoLoad: true,
  });
  $("#mySearch").insertAfter($(".fixed-table-toolbar .columns-right"));
});

function operateFormatter(options) {
  var btn = [];
  btn.push("<div class='flex gap-x-1.5 items-center'>");
  if (options.Add === true)
    btn.push('<button type="button" class="btn-action btn-green new" title="Add"><i class="uil uil-plus-circle"></i></button>');
  if (
    (options.Edit === true && options.Data.Status === "Draft") ||
    options.Data.Status === "Revise" ||
    options.Data.StatusViewByUser === "Approve Revise By Imex"
  )
    // (options.Data.Status === "Revise" && options.Data.StatusViewByUser !== "Pickup Goods")
    btn.push('<button type="button" class="btn-action btn-amber edit" title="Edit"><i class="uil uil-edit"></i></button>');
  if (options.Delete === true && options.Data.Status === "Draft")
    btn.push('<button type="button" class="btn-action btn-red remove" title="Delete"><i class="uil uil-trash"></i></button>');
  if (options.Data.StatusViewByUser === "Waiting for pickup goods")
    btn.push('<button type="button" class="btn-action btn-red cancel" title="Cancel"><i class="uil uil-times-circle"></i></button>');
  //if (options.Info == true)
  if (options.Edit === false || options.Data.Status !== "Revise") {
    btn.push('<button type="button" class="btn-action btn-blue info" title="Info"><i class="uil uil-search-plus"></i></button>');
  } else if (options.Data.StatusViewByUser === "Pickup Goods") {
    btn.push('<button type="button" class="btn-action btn-blue info" title="Info"><i class="uil uil-search-plus"></i></button>');
  }

  btn.push("</div>");
  return btn.join("");
}

operateFormatter.DEFAULTS = {
  Add: false,
  Edit: false,
  Delete: false,
  Info: false,
  View: false,
  History: false,
  Cancel: false,
};

window.operateEventsCipl = {
  "click .edit": function (e, value, row, index) {
    location.href = "/EMCS/CiplEdit/" + row.Id;
  },
  "click .remove": function (e, value, row, index) {
    swal(
      {
        title: "Are you sure want to delete this data?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#F56954",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: true,
      },
      function (isConfirm) {
        if (isConfirm) {
          sweetAlert.close();
          return deleteThis(row.Id);
        }
      }
    );
  },
  "click .info": function (e, value, row, index) {
    location.href = "/EMCS/CiplView/" + row.Id;
  },
  "click .cancel": function (e, value, row, index) {
    location.href = "/EMCS/CiplCancel/" + row.Id;
  },
};

function deleteThis(id) {
  $.ajax({
    type: "POST",
    url: myApp.root + "EMCS/DeleteCiplById",
    beforeSend: function () {
      $(".fixed-table-toolbar").hide();
      $(".fixed-table-loading").show();
    },
    complete: function () {
      $(".fixed-table-toolbar").show();
      $(".fixed-table-loading").hide();
    },
    data: { id: id },
    dataType: "json",
    success: function (d) {
      if (d == 1) {
        sAlert("Success", "Document has been deleted", "success");
      } else {
        sAlert("Delete Failed !", "Unauthorized to delete this document !", "warning");
      }

      $("[name=refresh]").trigger("click");
    },
    error: function (jqXHR, textStatus, errorThrown) {
      sAlert("Error", jqXHR.status + " " + jqXHR.statusText, "error");
    },
  });
}

$(function () {
  $.ajaxSetup({ cache: false });
  $("a[data-modal]").on("click", function (e) {
    $("#myModalContent").load(this.href, function () {
      $("#myModalPlace").modal(
        {
          keyboard: true,
        },
        "show"
      );

      bindForm(this);
    });
    return false;
  });
});

function bindForm(dialog) {
  $("form", dialog).submit(function () {
    $("#progress").show();
    $.ajax({
      url: this.action,
      type: this.method,
      data: $(this).serialize(),
      success: function (result) {
        if (result.Status == 0) {
          if (result.Msg != undefined) sAlert("Success", result.Msg, "success");
          $("#myModalPlace").modal("hide");
          $("#progress").hide();
          $("[name=refresh]").trigger("click");
        } else {
          if (result.Msg != undefined) sAlert("Failed", result.Msg, "error");
          $("#progress").hide();
        }
      },
    });
    return false;
  });
}

function addEmail(tableId) {
  console.log(tableId);
  var i = parseInt($("#" + tableId + " tr:last").data("row")) + 1;
  console.log(i);

  var strTr = "<tr data-row=" + i + ">";
  strTr +=
    '<td><input type="email" name="email" id="email_' +
    i +
    '" ' +
    'required="required" class="form-control required" ' +
    'style="width: 250px;"></td>';
  strTr +=
    '<td><button type="button" value="Delete" class="btn-danger btn-md" onClick=\'removeTr(this)\' style="margin-left: 10px;">Delete</button></td>';
  strTr += "</tr>";
  $("#" + tableId + " tr:last").after(strTr);
}

function removeTr(obj) {
  event.preventDefault();
  $(obj).parent().parent().remove();
}

// =====================================================================================