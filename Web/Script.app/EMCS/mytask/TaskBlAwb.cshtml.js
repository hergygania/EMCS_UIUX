function ApproveBlAwb(obj) {
    $.ajax({
        url: "/EMCS/BlAwbApproval",
        type: "POST",
        data: {
            Id: obj.Id,
            Status: obj.Status,
            Notes: obj.Notes,
        },
        success: function (resp) {
            var result = resp.result;
            var idPrev = result.Id;
            //location.href = "/EMCS/PreviewGR/" + IdPrev;
        }
    });
}


window.operateEvents = {
    'click .btn-create-bl': function (e, data, row, index) {
        location.href = "/EMCS/CreateBlAwb?Id=" + parseInt(row.IdCl) + "&Type=New";
    },
    'click .btn-approval-bl': function (e, data, row, index) {
        location.href = "/EMCS/BlAwbApproval?Id=" + parseInt(row.IdCl);
    },
    'click .btn-revise-bl': function (e, data, row, index) {
        location.href = "/EMCS/BlAwbRevise?Id=" + parseInt(row.IdCl);
    },
    'click .btn-view-bl': function (e, data, row, index) {
        location.href = "/EMCS/BlAwbView?Id=" + parseInt(row.IdCl);
    },
    'click .btn-saveapprove-bl': function (e, data, row, index) {
        location.href = "/EMCS/CreateBlAwb?Id=" + parseInt(row.IdCl);
    },
}

var columns_bl = [
    {
        field: "id",
        title: "No",
        align: 'center',
        formatter: runningFormatter
    }, {
        field: "",
        title: "Action",
        align: "center",
        sortable: true,
        width: "160",
        events: window.operateEvents,
        formatter: function (data, row, index) {
            console.log(row);
            var btn = [];
            btn.push('<div>'); 
            if (row['StatusViewByUser'] === 'Waiting for BL or AWB' || row['StatusViewByUser'] === 'Need revision review by imex' || row['StatusViewByUser'] === 'BL or AWB need revision') {
                btn.push("<button class='btn btn-info btn-xs btn-create-bl'><i class='fa fa-pencil'></i></button>");
            } else if (row['StatusViewByUser'] === 'Waiting for BL or AWB approval') {
                btn.push("<button class='btn btn-xs btn-primary btn-saveapprove-bl' data-toggle='tooltip' data-placement='top' title='Save & Approve Npe Peb'><i class='fa fa-pencil'></i></button>");
                btn.push("<button class='btn btn-success btn-xs btn-approval-bl' data-toggle='tooltip' title='Approve'><i class='fa fa-check'></i></button>");
                //btn.push("<button class='btn btn-warning btn-xs btn-revise-bl' data-toggle='tooltip' title='Revise'><i class='fa fa-pencil'></i></button>");
                btn.push("<button class='btn btn-details btn-xs btn-view-bl btn-info' data-toggle='tooltip' data-placement='top' title='View'><i class='fa fa-search'></i></button>");
            } else {
                btn.push("<button class='btn btn-details btn-xs btn-view-bl btn-info' data-toggle='tooltip' data-placement='top' title='View'><i class='fa fa-search'></i></button>");
            }  
            
            btn.push('</div>');
            return btn.join(' ');
        }
    }, {
        field: "PebNumber",
        title: "PEB Number",
        sortable: true
    }, {
        field: "ETA",
        title: "ETA",
        sortable: true,
        formatter: function (data, row, index) {
            console.log(row);
            if (data) {
                return moment(row.ETA).format("DD MMM YYYY");
            } else {
                return "-";
            }
        }
    }, {
        field: "ShippingMethod",
        title: "Shipment method",
        sortable: true
    }, {
        field: "Forwader",
        title: "Forwarder",
        sortable: true
    }, {
        field: "SlNo",
        title: "SL No.",
        sortable: true
    }, {
        field: "ClNo",
        title: "CL No.",
        sortable: true
    },
    {
        field: "PortOfLoading",
        title: "Loading Port",
        sortable: true
    }, {
        field: "PortOfDestination",
        title: "Destination Port",
        sortable: true
    }, {
        field: "StatusViewByUser",
        title: "Status",
        sortable: true,
        filterControl: true
    }
]

$("#tbl-task-bl").bootstrapTable({
    url: "/EMCS/GetTaskBlData",
    columns: columns_bl,
    cache: false,
    pagination: true,
    search: false,
    striped: true,
    clickToSelect: true,
    reorderableColumns: true,
    toolbar: '.toolbar',
    toolbarAlign: 'left',
    sidePagination: 'server',
    showColumns: true,
    showRefresh: true,
    smartDisplay: false,
    pageSize: '5',
    formatNoMatches: function () {
        return '<span class="noMatches">-</span>';
    },
});

$(function () {
    Dropzone.autoDiscover = false;

    myDropzoneBlAwb = new Dropzone('div#UploadDocumentBlAwb', {
        addRemoveLinks: true,
        autoProcessQueue: false,
        uploadMultiple: false,
        parallelUploads: 1,
        maxFiles: 1,
        paramName: 'file',
        clickable: true,
        url: "/EMCS/TaskBlAwb/", // Set the url
        init: function () {

            var myDropzoneBlAwb = this;
            // Update selector to match your button
            $('#SaveDocumentBlAwb').click(function (e) {
                e.preventDefault();
                myDropzoneBlAwb.processQueue();
                return false;
            });

            this.on('sending', function (file, xhr, formData) {
                formData.append("IdCL", $('#idCL').val());
                formData.append("NoBlAwb", $('#blAWbNo').val());
                formData.append("Date", $('#datelLAwb').val());
            });
        },
        error: function (file, response) {
            if ($.type(response) === "string")
                var message = response; //dropzone sends it's own error messages in string
            else
                var message = response.message;
            file.previewElement.classList.add("dz-error");
            _ref = file.previewElement.querySelectorAll("[data-dz-errormessage]");
            _results = [];
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
                node = _ref[_i];
                _results.push(node.textContent = message);
            }
            return _results;
        },
        success: function (file, response) {
            console.log(file, response);
        },
        complete: function (file, response) {
        },
        reset: function () {
            this.removeAllFiles(true);
        }
    });
})
