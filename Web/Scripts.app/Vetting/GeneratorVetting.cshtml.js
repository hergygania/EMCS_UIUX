
window.operateEvents = {
	'click .detail': function (e, value, row, index) {
		showMainScrollbar(false);
        loadModal('/vetting-process/partsOrderEdit?id=' + row.PartsOrderID);
    }
};

window.operateEventsOM = {
	'click .editOm': function (e, value, row, index) {
		$("#modalOM").modal('show');
		$('#DetailID').val(row.DetailID);
		$('#OMID').val(row.OMID).change();
		$('#PartsNumber').val(row.PartsNumber);
		$('#CaseNo').val(row.CaseNo);
		$('#Remark').val('');
		//resolve bugid 1153
		if (vettingRoute == 3 && vettingRoute == row.VettingRoute) {
			$('.divReturntToVendor').show();
		}
		else {
			$('.divReturntToVendor').hide();
		}
		
		$('#ReturnToVendor').val(row.ReturnToVendor);
		if (row.ReturnToVendor == null || row.ReturnToVendor == '0')
			$('#return-n').click();
		else
			$('#return-y').click();
	}
};

$(function () {
	enableLink(false);

	helpers.buildDropdown('/Picker/GetAgreementType', $('#selAgreementType'), true, null);
	helpers.buildDropdown('/Picker/GetJCode', $('#selJCode'), true, null);

	$.ajaxSetup({ cache: false });
	var $table = $('#tablePartsOrder');

	$('.cal').click(function () {
	    $('#InvoiceDate').focus();
	});

	$("#btnPreview").click(function () {
        var _staDate, _endDate;
        if ($('#InvoiceDate').val() != '') {
            _staDate = $('#InvoiceDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#InvoiceDate').data('daterangepicker').endDate.format('MM/DD/YYYY');


            var data = {
                params: JSON.stringify({
                    Freight: freight,
                    FreightShippId: $('#DefaultShippId').val(),
                    vettingRoute: 3,
                    InvoiceNo: $('#InvoiceNo').val(),
                    DateSta: _staDate,
                    DateFin: _endDate,
                    selJCode: $('#selJCode').val(),
                    selAgreementType: $('#selAgreementType').val(),
                    StoreNumber: $('#StoreNumber').val()
                }),
                target: 0
            };
            var strData = "";
            for (var key in data) {
                if (strData != "") {
                    strData += "&";
                }
                strData += key + "=" + encodeURIComponent(data[key]);
            }
            window.location.href = "/vetting-process/GeneratorVettingPreview?" + strData;
        } else {
            alert("Periode Date Is Required");
        }
    });

    $("#btnSend").click(function () {
        var _staDate, _endDate;
        if ($('#Subject').val() == '') {
            alert("Subject Is Required");

        } else if ($('#Body').val() == '') {
            alert("Body Is Required");

        } else if ($('#Email').val() == '') {
            alert("Email Is Required");

        } else if ($('#InvoiceDate').val() == '') {
            alert("Periode Date Is Required");

        } else {

            $this = $(this);
            $this.attr("disabled", "true");
            $this.html(`<div class="loader" id="loader-2"><span></span><span></span><span></span><span></span><span></span><span></span><span></span></div>`);


            _staDate = $('#InvoiceDate').data('daterangepicker').startDate.format('MM/DD/YYYY');
            _endDate = $('#InvoiceDate').data('daterangepicker').endDate.format('MM/DD/YYYY');

            setTimeout(function () {

                $.ajax
                    ({
                        type: "POST",
                        url: '/vetting-process/GeneratorVettingPreview',
                        dataType: 'json',
                        async: false,
                        data: {
                            params: JSON.stringify({
                                Freight: freight,
                                FreightShippId: $('#DefaultShippId').val(),
                                vettingRoute: 3,
                                InvoiceNo: $('#InvoiceNo').val(),
                                DateSta: _staDate,
                                DateFin: _endDate,
                                selJCode: $('#selJCode').val(),
                                selAgreementType: $('#selAgreementType').val(),
                                StoreNumber: $('#StoreNumber').val()
                            }),
                            target: 1,
                            email: $('#Email').val(),
                            subject: $('#Subject').val(),
                            body: $('#Body').val()
                        }
                    }).done(function (html) {
                        alert("Successfully Sent");
                    });

                $this.removeAttr("disabled");
                $this.html(`<i class="fa fa-times-circle"></i> Send Email`);
                $("#notif").append(`
                <div class="col-sm-6">
                    <div class="col-sm-6 alert alert-success alert-dismissible" role="alert" style="margin-top: 10px;font-size: 15px;">
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" style="font-size: 21px !important;"><span aria-hidden="true">x</span></button>Email has been sent
                    </div>
                </div>`);
            }, 1000);

            
         }
    });
    
	$('#btn-clear').click(function () {
	    $('#InvoiceNo').val('');
	    $('#InvoiceDate').val('');
	    $('#AgreementType').val('');
	    $('#JCode').val('');
	    $('#StoreNumber').val('');
    });

	enableLink(true);
});



function bindForm(dialog) {
	return;
};
