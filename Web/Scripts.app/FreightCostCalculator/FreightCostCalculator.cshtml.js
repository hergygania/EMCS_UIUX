
function excelSubmit() {
    var fileUpload = $("#filexls").get(0);
    var files = fileUpload.files;
    var formData = new FormData();
    formData.append('filexls', files[0]);

    $('#uploadResult').empty();
    $('#progress').show();
    var bar = $('.progress-bar');
    var percent = $('.progress-bar');
    var status = $('#status');

    $.ajax({
        url: 'SaveUpload',
        type: 'POST',
        data: formData,
        async: true,
        beforeSend: function () {
            status.empty();
            status.html("Please Wait While We Upload Your File(s)");
            var percentValue = '0%';
            bar.width(percentValue);
            percent.html(percentValue);
            $("#Import").addClass("disabled");
        },
        progress: function (event, position, total, percentComplete) {
            var percentValue = percentComplete + '%';
            bar.width(percentValue);
            percent.html(percentValue);
            $("#Import").addClass("disabled");
        },
        success: function (result) {
            var percentValue = '100%';
            bar.width(percentValue);
            percent.html(percentValue);
            status.hide();
            $("#Import").removeClass("disabled");

            if (result.Status == 0) {
                if (result.Msg != undefined)
                    sAlert('Success', result.Msg, 'success');
                $('#myModalPlace').modal('hide');
                $('#progress').hide();
                $("[name=refresh]").trigger('click');
            }
            else {
                if (result.Msg != undefined)
                    sAlert('Failed', result.Msg, 'error');
                $('#progress').hide();
            }


            //if (result.Status == 0) {
            //    if (result.Msg != undefined) {
            //        sAlert('Success', result.Msg, 'success');
            //        $('#progress').hide();
            //    }
            //    $('#progress').hide();
            //}
            //else {
            //    if (result.Msg != undefined) {
            //        sAlert('Failed', result.Msg, 'error');
            //        $('#progress').hide();
            //    }
            //}
        },

        cache: false,
        contentType: false,
        processData: false
    });
}
