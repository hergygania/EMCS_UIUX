function calculate() {    
    var Model = $('#searchModel').val()
    var Origin = $('#searchOrigin').val()
    var Destination = $('#searchDestination').val()
    $("#route").text(''),
    $("#validto").text('');
    $("#amount").text('');  
    $.ajax({
        url: '/DTS/GetFreightRouteSalesData',
        data: "{'Origin': '" + Origin + "', 'Destination': '" + Destination + "','Model': '" + Model + "'}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result == null ) {
                sAlert('Warning', "Data Not Available, Please Contact SCM", "warning");
                return;
            }
            else {
                $("#route").text(data.result.Route),
                $("#validto").text(data.result.ValidTo);
                $("#amount").text(data.result.Amount);   
                if (data.result.Valid == '0') {
                    document.getElementById('validto').style.backgroundColor = 'red';
                }
                else {
                    document.getElementById('validto').style.backgroundColor = 'white';
                }
               
            }
                    
        },      
    });
}

$(function () {
    $("#searchOrigin").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/DTS/GetFreightRouteSalesOption',               
                data: "{'type': 'Origin', 'key': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Origin, value: item.Origin, name: item.Origin
                        };
                    }))

                },
            
            });
        },
        select: function (event, ui) {
            $(".ui-helper-hidden-accessible").hide();
            event.preventDefault();
            $(this).val(ui.item.label);
            //$('#searchOrigin').html(ui.item.name);           
            //$("#searchOrigin").val(ui.item.value);          
        },
        minLength: 3,
        //appendTo: '#EstimateFilter'
    });
    $("#searchDestination").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/DTS/GetFreightRouteSalesOption',
                data: "{ 'type': 'Destination', 'key': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Destination, value: item.Destination, name: item.Destination
                        };
                    }))

                },              
            });
        },
        select: function (event, ui) {
            //event.preventDefault();
            $(this).val(ui.item.label);
            $('#searchDestination').html(ui.item.name);
            $("#searchDestination").val(ui.item.value);
        },
        minLength: 3,
        //appendTo: '#EstimateFilter'
    });
    $("#searchModel").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/DTS/GetFreightRouteSalesOption',
                data: "{ 'type': 'Model', 'key': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Model, value: item.Model, name: item.Model
                        };
                    }))

                },               
            });
        },
        select: function (event, ui) {
            //event.preventDefault();
            $(this).val(ui.item.label);
            $('#searchModel').html(ui.item.name);
            $("#searchModel").val(ui.item.value);
        },
        minLength: 2,
        //appendTo: '#EstimateFilter'
    });
})