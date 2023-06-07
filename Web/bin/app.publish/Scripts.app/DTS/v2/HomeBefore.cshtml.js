var GroupStatus = [
    { Status: 'DR Draft / Submit / Revised', Count: 0 },
    { Status: 'DR Approved', Count: 0 },
    { Status: 'DR Revise', Count: 0 },
    { Status: 'DR Rejected', Count: 0 },
];
function GeDRtGroupByStatus() {
    $.ajax({
        type: "POST",
        url: myApp.root + 'DTS/GetDRGroupByStatusBefore',
        beforeSend: function () {
            ShowLoading();
        },
        complete: function () {
            HideLoading();
        },
        dataType: "json",
        success: function (d) {
            if (d.message !== undefined) {
                sAlert('Error', d.message, 'error');
            } else {
                for (var ix in d.result) {
                    if (['draft', 'submit', 'revised'].indexOf(d.result[ix].Status) > -1) {
                        GroupStatus[0].Count = parseInt(GroupStatus[0].Count || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'approve') {
                        GroupStatus[1].Count = parseInt(GroupStatus[1].Count || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'revise') {
                        GroupStatus[2].Count = parseInt(GroupStatus[2].Count || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                    if (d.result[ix].Status == 'reject') {
                        GroupStatus[3].Count = parseInt(GroupStatus[3].Count || 0) + parseInt(d.result[ix].Count_Id || 0);
                    }
                }
                $('#dr-notification li').each(function (idx) {
                    $(this).find('span.status').html(GroupStatus[idx].Status);
                    $(this).find('span.count').html(GroupStatus[idx].Count);
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            sAlert('Error', jqXHR.status + " " + jqXHR.statusText, "error");
        }
    });
}

$(function () {
    GeDRtGroupByStatus();
    $('.info-box.dr').click(function () {
        window.location = myApp.fullPath + 'DTS/DeliveryRequisitionList';
    });
    $('.info-box.di').click(function () {
        window.location = myApp.fullPath + 'DTS/DeliveryInstructionList';
    });
    $('#dr-link').click(function () {
        window.location = myApp.fullPath + 'DTS/Outbound';
    });
});



