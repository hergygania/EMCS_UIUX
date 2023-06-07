
$(function () {
    $('.cal').click(function () {
        $('#EntryDate').focus();
    });

    $("#FilterBy").change(function (e) {
        var val = $(this).val();
        $.getJSON("/report/GetStore",
            {
                filter_type: $("input[name='filter_type']:checked").val(),
                id: val
            },
        function (results) {
            //$('#selStoreList_Nos').val('', '').change();
            $('#StoreNumber').empty();
            $('#StoreNumber').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#StoreNumber').append($("<option value=" + data.id + ">" + data.text + "</option>"));

            });
        });
        $('#StoreNumber').val('val', '').change();

    });

    $('#btn-clear').click(function () {
        $('#FilterBy').val('', 'ALL').change();
        $('#StoreNumber').val('', 'ALL').change();
        $('#EntryDate').val('');
        $('#CustomerId').val('val', '').change();
    });
});


function setFilter(index) {
    $.getJSON("/report/GetFilterBy", { index: index },
        function (results) {
            $('#FilterBy').val('', 'ALL').change();
            $('#FilterBy').empty();
            $('#FilterBy').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#FilterBy').append($("<option value=" + data.id + ">" + data.text + "</option>"));
            });
        });
    $("#FilterBy").select2();
}