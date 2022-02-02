function setOrigin() {
    $.getJSON("/DeliveryTrackingStatus/GetOrigin",
        function (results) {
            $('#OriginID').val('', 'ALL').change();
            $('#OriginID').empty();
            $('#OriginID').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#OriginID').append($("<option value=" + data.id + ">" + data.text + "</option>"));
                
            });
        });
    $("#Origin").select2();
}


function setDestination() {
    $.getJSON("/DeliveryTrackingStatus/GetDestination",
        function (results) {
            $('#DestinationID').val('', 'ALL').change();
            $('#DestinationID').empty();
            $('#DestinationID').append($("<option value=''>ALL</option>"));
            $.each(results, function (i, data) {
                $('#DestinationID').append($("<option value=" + data.id + ">" + data.text + "</option>"));
            });
        });
    $("#Origin").select2();
}

$(function () {
    $('#btn-clear').click(function () {
        $('#DestinationID').val('', 'ALL').change();
        $('#OriginID').val('', 'ALL').change();
        $('#Est_Departure_Date').val('');
        $('#Est_Arrival_Date').val('');
        $('#Actual_Departure_Date').val('');
        $('#Actual_Arrival_Date').val('');
        $('.inputSN.Active').val('');
        $('#SalesOrderNumber').val('');
        $('#model').val('');
        $('#OutBoundDelivery').val('');
    });
});