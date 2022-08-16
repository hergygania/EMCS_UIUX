﻿$tableChangeHistory = $("#tableRequestForChangeDetail");

var columnChangeHistory = [
  {
    field: "FieldName",
    title: "FieldName",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "CreateDate",
    title: "Date",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
    formatter: function (data, row, index) {
      return moment(data).format("DD MMM YYYY hh:mm:ss");
    },
  },
  {
    field: "BeforeValue",
    title: "Old Value",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  {
    field: "AfterValue",
    title: "New Value",
    halign: "center",
    align: "left",
    class: "text-nowrap",
    sortable: true,
  },
  //{
  //    field: 'Reason',
  //    title: 'Reason',
  //    halign: 'center',
  //    align: 'left',
  //    class: 'text-nowrap',
  //    sortable: true
  //}
];
//$('#btnApproveChangeHistory').on('click', function () {
//    paramSearch = {
//        idTerm: $('#RFCId').val(),
//        formtype: 'CIPL',
//        formId: $('#formId').val(),
//    };
//    $.ajax({
//        url: '/EMCS/ApproveChangeHistory',
//        type: 'POST',
//        data: paramSearch,
//        success: function (guid) {

//        },
//        cache: false
//    });
//});
function ApproveRequestForChange() {
  var url = "";
  if ($("#FormType").val() === "CIPL") url = "/EMCS/ApproveChangeHistory";
  else $("#FormType").val() == "Cargo";
  url = "/EMCS/ApproveChangeHistoryCl";
  paramSearch = {
    idTerm: $("#RFCId").val(),
    formtype: $("#FormType").val(),
    formId: $("#formId").val(),
  };
  debugger;
  $.ajax({
    url: url,
    type: "POST",
    data: paramSearch,
    success: function (guid) {},
    cache: false,
  });
}
function RejectRequestForChange(data) {
  paramSearch = {
    idTerm: data.Id,
    reason: data.Notes,
  };
  $.ajax({
    url: "/EMCS/RejectChangeHistory",
    type: "POST",
    data: paramSearch,
    success: function (guid) {
      Swal.fire({
        type: "success",
        title: "Success",
        text: "Request rejected successfully",
      }).then((result) => {
        window.location.href = "/EMCS/Mytask";
      });
    },
    cache: false,
  });
}
$("#btnApproveChangeHistory").on("click", function () {
  Swal.fire({
    title: "Approve Confirmation",
    text: "By approving this changes, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this request of change?",
    type: "question",
    showCancelButton: true,
    cancelButtonColor: "#d33",
    confirmButtonColor: "#3085d6",
    confirmButtonText: "Yes, Approve!",
    allowEscapeKey: false,
    allowOutsideClick: false,
    showCloseButton: true,
  }).then((result) => {
    if (result.value) {
      ApproveRequestForChange();
      Swal.fire({
        type: "success",
        title: "Success",
        text: "Saved, Your Data Has Been Saved",
      }).then((result) => {
        window.location.href = "/EMCS/Mytask";
      });
    }
    return false;
  });
});
$("#btnRejectChangeHistory").on("click", function () {
  Swal.fire({
    title: "Reject this Request?",
    text: "Are you sure you want to reject this request for change?",
    type: "question",
    showCancelButton: true,
    cancelButtonColor: "#d33",
    confirmButtonColor: "#3085d6",
    confirmButtonText: "Yes, Reject!",
    allowEscapeKey: false,
    allowOutsideClick: false,
    showCloseButton: true,
  }).then((result) => {
    if (result.value) {
      Swal.fire({
        input: "textarea",
        allowEscapeKey: false,
        allowOutsideClick: false,
        inputPlaceholder:
          "Please add reason for rejecting this request for change...",
        inputAttributes: {
          "aria-label":
            "Please add reason for rejecting this request for change...",
        },
        showCancelButton: false,
      }).then((result) => {
        var Notes = result.value;
        var Status = "Approve";
        var Id = $("#RFCId").val();
        var data = { Notes: Notes, Status: Status, Id: Id };
        RejectRequestForChange(data);
      });
    }
    return false;
  });
});

$(function () {
  $.ajax({
    url: "/EMCS/GetChangeHistoryReason",
    type: "POST",
    data: {
      IdTerm: $("#RFCId").val(),
      formtype: $("#FormType").val(),
    },
    cache: false,
    async: false,
    success: function (data, response) {
      $("#changehistory").text(data);
    },
    error: function (e) {
      Swal.fire({
        type: "error",
        title: "Oops...",
        text: "Something went wrong! Fail Update Data",
      });
    },
  });
  $.ajax({
    url: "/EMCS/CheckRequestExists",
    type: "POST",
    data: {
      IdTerm: $("#RFCId").val(),
      formtype: $("#FormType").val(),
    },
    cache: false,
    async: false,
    success: function (data, response) {
      if (data == 0) {
        $("#btnRejectChangeHistory").hide();
        $("#btnApproveChangeHistory").hide();
      }
    },
    error: function (e) {
      Swal.fire({
        type: "error",
        title: "Oops...",
        text: "Something went wrong! Fail Update Data",
      });
    },
  });
  $tableChangeHistory.bootstrapTable({
    cache: false,
    url: "/Emcs/GetChangeHistoryList",
    pagination: true,
    search: false,
    striped: false,
    clickToSelect: false,
    sidePagination: "server",
    showColumns: false,
    queryParams: function (params) {
      return {
        limit: params.limit,
        offset: params.offset,
        IdTerm: $("#RFCId").val(),
        formType: $("#formType").val(),
        sort: params.sort,
        order: params.order,
      };
    },
    searchOnEnterKey: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: "10",
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
      return '<span class="noMatches">Data Not Found</span>';
    },
    responseHandler: function (resp) {
      var data = {};
      $.map(resp, function (value, key) {
        data[value.Key] = value.Value;
      });

      return data;
    },
    columns: columnChangeHistory,
  });
});