﻿$(function () {
  window.operateEvents = {
    "click #si-submit": function (e, value, row, index) {
      //$('#idCL').val(row.Id);
      //$("#")
      //location.href = myApp.fullPath + "EMCS/SubmitSi?Id=" + parseInt(row.IdCl);
      //location.href = myApp.fullPath + "EMCS/SubmitSi?Id=" + row.IdCl;
      location.href = "/EMCS/SubmitSi/" + row.IdCl;
    },
  };
  var columns_si = [
    {
      //    field: "IdCl",
      //    formatter: runningFormatter,
      //    title: "No",
      //    align: 'center'
      //}, {
      field: "",
      title: "Action",
      align: "left",
      sortable: false,
      width: "100",
      events: window.operateEvents,
      formatter: function (data, row, index) {
        var btn = [];
        btn.push('<div class="flex gap-x-1.5 items-center">');
        //btn.push("<button class='btn btn-xs btn-primary si-submit' title='Input' data-target='#InputInstruction' data-toggle='modal'><i class='fa fa-search'></i></button>");
        btn.push(
          "<button class='btn-action btn-blue' id='si-submit' title='Info' ><i class='uil uil-search'></i></button>"
        );
        btn.push("</div>");
        return btn.join("");
      },
    },
    {
      field: "ClNo",
      title: "Cargo Number",
      class: "text-nowrap",
      halign: "center",
      sortable: true,
    },
    {
      field: "CreateDate",
      title: "Date",
      align: "center",
      halign: "center",
      sortable: true,
      class: "text-nowrap",
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "PreparedBy",
      halign: "center",
      title: "Prepared By",
      class: "text-nowrap",
      sortable: true,
    },
    {
      field: "Consignee",
      align: "left",
      title: "Consignee Name",
      halign: "center",
      class: "text-nowrap",
      sortable: true,
    },
    {
      field: "StuffingDateStarted",
      title: "Stuffing Date (Start)",
      halign: "center",
      class: "text-nowrap",
      sortable: true,
      align: "center",
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "StuffingDateFinished",
      title: "Stuffing Date (Finished)",
      halign: "center",
      class: "text-nowrap",
      align: "center",
      sortable: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "SailingSchedule",
      title: "ETD",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "ArrivalDestination",
      title: "ETA",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
      formatter: function (data, row, index) {
        if (data) {
          return moment(data).format("DD MMM YYYY");
        } else {
          return "-";
        }
      },
    },
    {
      field: "StatusViewByUser",
      title: "Status",
      class: "text-nowrap",
      align: "center",
      halign: "center",
      sortable: true,
      filterControl: true,
    },
  ];

  $("#tbl-task-si").bootstrapTable({
    url: "/EMCS/GetTaskSiData",
    columns: columns_si,
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
      return '<span class="noMatches">No task found</span>';
    },
  });

  Dropzone.autoDiscover = false;

  myDropzoneSI = new Dropzone("div#UploadDocumentSI", {
    addRemoveLinks: true,
    autoProcessQueue: false,
    uploadMultiple: false,
    parallelUploads: 1,
    maxFiles: 1,
    paramName: "file",
    clickable: true,
    url: "/EMCS/TaskSi/", // Set the url
    init: function () {
      var myDropzoneSI = this;
      // Update selector to match your button
      $("#SaveDocumentSI").click(function (e) {
        e.preventDefault();
        myDropzoneSI.processQueue();
        return false;
      });

      this.on("sending", function (file, xhr, formData) {
        formData.append("IdCL", $("#idCL").val());
        formData.append("Description", $("#descriptionSI").val());
        formData.append("SpecialInstruction", $("#specialInstructionSI").val());
      });
    },
    error: function (file, response) {
      var message = response; //dropzone sends it's own error messages in string
      if ($.type(response) === "string")
        message = response; //dropzone sends it's own error messages in string
      else message = response.message;
      file.previewElement.classList.add("dz-error");
      _ref = file.previewElement.querySelectorAll("[data-dz-errormessage]");
      _results = [];
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        node = _ref[_i];
        _results.push((node.textContent = message));
      }
      return _results;
    },
    success: function (file, response) {
      console.log(file, response);
      location.reload();
    },
    complete: function (file, response) {
      location.reload();
    },
    reset: function () {
      this.removeAllFiles(true);
    },
  });
});