$(function () {
    $("#FilterBy").change(function (e) {
        var val = $(this).val();
        $.getJSON("/partTracking/GetStore", {
            filter_type: $("input[name='filter_type']:checked").val(),
            id: val
        },
        function (results) {
            $('#StoreNumber').empty();
            $('#StoreNumber').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#StoreNumber').append($("<option value=" + data.StoreNo + ">" + data.Name + "</option>"));
            });
        });
        $('#StoreNumber').val('val', '').change();
    });
});

function setFilter(index) {
    $.getJSON("/partTracking/GetFilterBy", { index: index },
        function (results) {
            $('#FilterBy').val('', 'ALL').change();
            $('#FilterBy').empty();
            $('#FilterBy').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                if (index == 1)
                    $('#FilterBy').append($("<option value=" + data.HubID + ">" + data.Name + "</option>"));
                else
                    $('#FilterBy').append($("<option value=" + data.AreaID + ">" + data.Name + "</option>"));
            });
        });
    $("#FilterBy").select2();
}