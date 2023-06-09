var lastpath_id = window.location.href.split("/");
var getlast_id = Number(lastpath_id[lastpath_id.length - 1]);

// =====================================================================================
// CIPL FORM
$(function () {
    var last_approve = lastpath_id[lastpath_id.length - 2];
    if (lastpath_id[4] === "CiplApprove" || last_approve === "CiplView") {
        $("input, textarea, select").prop('disabled', true);
        $('#partAddButton').hide();
    }
});

//  ==================================================================== VIEW APPROVE ====================================================================
function ApproveCipl(obj) {
    $.ajax({
        url: "/EMCS/CiplApproval",
        type: "POST",
        data: {
            Id: obj.Id,
            Status: obj.Status,
            Notes: obj.Notes
        },
        success: function (resp) {
            Swal.fire({
                title: 'Approve!',
                text: 'Data Confirmed Successfully',
                type: 'success'
            }).then((result) => {
                window.location.href = "/EMCS/Mytask";
            });
        }
    });
}

function diff_hours(dt2, dt1) {
    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= (60 * 60);
    return Math.abs(Math.round(diff));
}

$("#btnApprove").on("click", function () {
    var SubmitDate = new Date($('#SubmitDate').val());
    var DateTimeNow = new Date();
    var DifferenceTime = diff_hours(DateTimeNow, SubmitDate);
    if (DifferenceTime > 24) {
        $("#myModalProblem").modal("show");
        $("#YesApproveBtn").show();
        $("#YesRejectBtn").hide();
        $("#YesReviseBtn").hide();
        $("#YesCancelBtn").hide();
        $("#myModalProblemContent form").find("input").removeAttr("disabled");
        $("#myModalProblemContent form").find("textarea").removeAttr("disabled");
        $("#myModalProblemContent form").find("select").removeAttr("disabled");
    } else {
        //Swal.fire({
        //    icon: 'success',
        //    title: 'Approve!',
        //    text: 'Data Confirmed Successfully',
        //    showConfirmButton: true
        //});
        Swal.fire({
            title: 'Approve Confirmation',
            text: 'By approving this document, you are responsible for the authenticity of the documents and data entered. Are you sure you want to process this document?',
            type: 'question',
            showCancelButton: true,
            cancelButtonColor: '#d33',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, Approve!',
            allowEscapeKey: false,
            allowOutsideClick: false,
            showCloseButton: true
        }).then((result) => {
            if (result.value) {
                Swal.fire({
                    input: 'textarea',
                    allowEscapeKey: false,
                    allowOutsideClick: false,
                    inputPlaceholder: 'Type your notes here...',
                    inputAttributes: {
                        'aria-label': 'Type your notes here'
                    },
                    showCancelButton: false
                }).then((result) => {
                    $('#btnApprove').prop('disabled', true);
                    $('#btnRevise').prop('disabled', true);
                    $('#btnReject').prop('disabled', true);
                    var Notes = result.value;
                    var Status = "Approve";
                    var Id = $('#idCipl').val();
                    var data = { Notes: Notes, Status: Status, Id: Id };
                    ApproveCipl(data);
                });
                
            }
            return false;
        });
    }

});
