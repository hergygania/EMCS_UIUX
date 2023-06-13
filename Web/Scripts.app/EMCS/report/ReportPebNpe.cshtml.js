var $table = $('#tbl-detailstracking');
var startDate = '';
var endDate = '';
var paramName = '';
var paramValue = '';


$(function () {

    $('#category').on('change', function () {

        $('.categoryspareparts, .categoryunit, .categorymisc').hide();

        if ($(this).val() === 'CATERPILLAR SPAREPARTS') { // Spareparts
            $('.categoryspareparts').show();
        } else {
            if ($(this).val() === 'MISCELLANEOUS') { // MISC
                $('#permanentCipl, .categorymisc').show();
            } else {
                if ($(this).val() === 'CATERPILLAR NEW EQUIPMENT' || $(this).val() === 'CATERPILLAR USED EQUIPMENT') { // USED EQUIPMENT && UNIT
                    $('.categoryunit').show();
                }
            }
        }
    });

    $.ajax({
        url: "/EMCS/GetDetailsTrackingFilter",
        success: function (data) {

            var category = new Array();
            $.each(data.category, function (index, element) {
                category.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#category").select2({
                data: category,
                placeholder: 'Please Select Filter',
            }).on("select2:select", function () {
                $('#filter-value').val('');
            });
            $('#category').val(null).trigger('change');


            var unit = new Array();
            $.each(data.categoryunit, function (index, element) {
                unit.push({ 'id': element.Id, 'text': element.Text });
            });
            $("#unitCipl").select2({
                data: unit,
                width: '100%',
                placeholder: 'Please Select Unit',
            });
            $('#unitCipl').val(null).trigger('change');

            var spareparts = new Array();
            $.each(data.categoryspareparts, function (index, element) {
                spareparts.push({ 'id': element.Id, 'text': element.Text === 'PRA' ? 'PRA / RMA' : element.Text });
            });

            $("#sparepartsCipl").select2({
                data: spareparts,
                width: '100%',
                placeholder: 'Please Select Spareparts',
            });
            $('#sparepartsCipl').val(null).trigger('change');
        }
    });

    $('.cal-start-date').click(function () {
        $('#inp-start-date').focus();
    });
    $('#inp-start-date').focus().datepicker({
        format: "mm/yyyy",
        viewMode: "months",
        minViewMode: "months",
        //autoclose: true
    }).on('changeMonth', function (e) {
        //    $('#inp-end-date').datepicker('setDate', '');
        //    $('#inp-end-date').datepicker('setStartDate', moment(e.date).format('MM/YYYY'));
        //    $('#inp-end-date').datepicker('setEndDate', '12/' + moment(e.date).format('YYYY'));
    });

    $('.cal-end-date').click(function () {
        $('#inp-end-date').focus();
    });
    $('#inp-end-date').focus().datepicker({
        format: "mm/yyyy",
        viewMode: "months",
        minViewMode: "months",
        //autoclose: true
    }).on('changeMonth', function (e) {
        //$('#inp-start-date').datepicker('setEndDate', moment(e.date).format('MM/YYYY'));
    });

    var columns = [
        [{
            field: "PebMonth",
            title: "MONTH &nbsp;&nbsp;&nbsp;",
            rowspan: 3,
            valign: "middle",
            align: "center",
            width: '200px'
        }, {
            field: "RowNumber",
            title: "NO",
            rowspan: 3,
            valign: "middle",
            align: "center"
        }, {
            field: "CustomsFacilityArea",
            title: "Custom Facility Area",
            rowspan: 3,
            sortable: true,
            align: "left",
            valign: "middle"
        },
        {
            field: "ReferenceNo",
            title: "Reference No",
            rowspan: 3,
            sortable: true,
            align: "left",
            valign: "middle"
        }, {
            title: "Commercial Invoice & Packing List <br/> (CIPL)",
            colspan: 3,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            field: "DescGoods",
            title: "Description of Goods",
            rowspan: 3,
            halign: "center",
            align: "left",
            class: "text-nowrap"
        }, {
            title: "Export Category",
            colspan: 2,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Export Type",
            colspan: 3,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            field: "Remarks",
            title: "Remarks",
            rowspan: 3,
            sortable: true,
            align: "left",
            halign: "center",
            class: "text-nowrap"
        }, {
            title: "Person In Charge <br/> (PIC)",
            colspan: 4,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Export Delivery Instruction <br/> (EDI)",
            colspan: 2,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Receipt Goods <br/> (RG)",
            colspan: 4,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Cargo List <br/> (CL)",
            colspan: 4,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Shipping Summary <br/> (SS)",
            colspan: 2,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Shipping Instruction <br/> (SI)",
            colspan: 2,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Other",
            colspan: 3,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Estimated Schedule",
            colspan: 2,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Pemberitahuan Ekspor Barang <br/> PEB",
            colspan: 7,
            rowspan: 2,
            sortable: true,
            align: "center",
        }, {
            title: "Bill of Lading/Air way Bill",
            colspan: 4,
            sortable: true,
            align: "center",
        }, {
            title: "Liner",
            colspan: 6,
            sortable: true,
            align: "center",
        }, {
            field: "",
            title: "Consignee",
            colspan: 7,
            rowspan: 2,
            sortable: true,
            align: "center"
        }, {
            field: "",
            title: "Notify Party",
            colspan: 7,
            rowspan: 2,
            sortable: true,
            align: "center"
        }, {
            field: "",
            title: "Ship To/Delivery To",
            colspan: 7,
            rowspan: 2,
            sortable: true,
            align: "center"
        },

        {
            field: "PortOfLoading",
            title: "Port of Loading",
            rowspan: 3,
            align: 'center',
            valign: "middle",
            class: "text-nowrap"
        }, {
            field: "PortOfDestination",
            title: "Port of Discharge",
            rowspan: 3,
            align: 'center',
            valign: "middle",
            class: "text-nowrap"
        }, {
            field: "ShippingMethod",
            title: "Shipping <br/> Method  <br/> ( Sea/Air )",
            rowspan: 3,
            align: 'center',
            valign: "middle"
        }, {
            field: "IncoTerms",
            title: "Shipping <br/> Terms <br/> ( incoterms )",
            rowspan: 3,
            align: 'center',
            valign: "middle"
        }, {
            field: "CargoType",
            title: "Cargo Type <br/> (FCL/LCL/RoRo)",
            rowspan: 3,
            align: 'center',
            valign: "middle"
        }, {
            field: "",
            title: "FCL",
            rowspan: 2,
            colspan: 2,
            align: 'center',
            valign: "middle"
        }, {
            title: "Quantity",
            rowspan: 2,
            colspan: 2,
            align: 'center',
            valign: "middle"
        }, {
            field: "TotalColly",
            title: "Colly",
            rowspan: 3,
            align: 'left',
            valign: "middle"
        }, {
            title: "Weight",
            rowspan: 2,
            colspan: 2,
            align: 'center',
            valign: "middle"
        }, {
            field: "Volume",
            title: "Total <br/> Volume <br/> in m3",
            rowspan: 3,
            align: 'right',
            halign: "center"
        }, {
            field: "Valuta",
            title: "Currency",
            rowspan: 3,
            align: 'center',
            valign: "middle"
        }, {
            field: "Rate",
            title: "Exchange <br/> Rate <br/> IDR/Curr",
            rowspan: 3,
            align: 'center',
            valign: "middle"
        }, {
            field: "",
            title: "Export Value",
            colspan: 5,
            rowspan: 2,
            sortable: true,
            align: "center"
        }, {
            field: "",
            title: "Export Service Charges",
            colspan: 3,
            rowspan: 2,
            sortable: true,
            align: "center"
        }, {
            field: "",
            title: "Value Received from Consignee",
            colspan: 4,
            rowspan: 2,
            sortable: true,
            align: "center"
        }, {
            field: "Status",
            title: "Status",
            rowspan: 3,
            sortable: true,
            align: "center"
        },
        ], //BAWAH
        [{
            field: "",
            title: "Master BL/AWB",
            colspan: 2,
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "House BL/AWB",
            colspan: 2,
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "Ocean",
            colspan: 3,
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "Air",
            colspan: 3,
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }],
        [{
            field: "CiplNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "CiplCreateDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            align: "center"
        }, {
            field: "CiplApprovalDate",
            title: "Approval Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            align: "center"
        }, {
            field: "Category",
            title: "Category",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SubCategory",
            title: "Sub",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "PermanentTemporary",
            title: "Permanent/Temporary",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SalesNonSales",
            title: "Sales/Non Sales",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Type",
            title: "PE/RR/R/E/-",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "PicName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "PicApproverName",
            title: "Approver Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Department",
            title: "Department",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Branch",
            title: "Branch",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "EdiNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "CiplApprovalDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "RgNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "RgDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "RgApproverName",
            title: "Approver Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "RgApprovalDate",
            title: "Approval Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ClNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ClDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ClApproverName",
            title: "Approver Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ClApprovalDate",
            title: "Approval Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SsNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ClApprovalDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SiNo",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SiDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Etd",
            title: "ETD",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Eta",
            title: "ETA",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "AjuNumber",
            title: "AJU NO",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "AjuDate",
            title: "AJU Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NpeNumber",
            title: "NPE Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Nopen",
            title: "NOPEN",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NpeDate",
            title: "NOPEN DATE",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            align: "center",
            halign: "center",
        }, {
            field: "PebApproverName",
            title: "Approver Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "PebApprovalDate",
            title: "Approval Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "MasterBlAwbNumber",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "MasterBlAwbDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "HouseBlAwbNumber",
            title: "Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "HouseBlAwbDate",
            title: "Date",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            //field: "Liner",
            field: "LinerVesselName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "VesselName",
            title: "Vessel",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "VesselVoyNo",
            title: "Sailing/Voyage Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            //field: "Liner",
            field: "LinerFlightName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "FlightName",
            title: "Flight",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "FlightVoyNo",
            title: "Flight Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeAddress",
            title: "Address",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeCountry",
            title: "Country",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneePic",
            title: "PIC Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeEmail",
            title: "Email",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeTelephone",
            title: "Phone Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ConsigneeFax",
            title: "Fax Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyAddress",
            title: "Address",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyCountry",
            title: "Country",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyPic",
            title: "PIC Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyEmail",
            title: "Email",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyTelephone",
            title: "Phone Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "NotifyFax",
            title: "Fax Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToName",
            title: "Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToAddress",
            title: "Address",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToCountry",
            title: "Country",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToPic",
            title: "PIC Name",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToEmail",
            title: "Email",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToTelephone",
            title: "Phone Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "SoldToFax",
            title: "Fax Number",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "ContainerNumber",
            title: "CNTR#",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
        }, {
            field: "Seal",
            title: "SEAL#",
            sortable: true,
            filterControl: true,
            class: 'text-nowrap',
            halign: "center",
            align: "left"
            //}, {
            //    field: "Type",
            //    title: "TYPE",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    halign: "center",
            //    align: "left"
        }, {
            field: "TotalUom",
            title: "TOTAL",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right",
            halign: "center",
        }, {
            field: "Uom",
            title: "UOM",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "center"
        }, {
            field: "Gross",
            title: "Gross",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "Net",
            title: "Nett",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "PebFob",
            title: "Value",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "FreightPyment",
            title: "Freight Cost",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "InsuranceAmount",
            title: "Insurance",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "TotalExtendedValue",
            title: "Total (In usd) based on invoice",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "TotalPeb",
            title: "Total (In usd) based on PEB",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "right"
        }, {
            field: "InvoiceNoServiceCharges",
            title: "Invoice Number",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "CurrencyServiceCharges",
            title: "Currency",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "TotalServiceCharges",
            title: "Total Service Charges",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "InvoiceNoConsignee",
            title: "Invoice Number",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "CurrencyValueConsignee",
            title: "Currency",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "TotalValueConsignee",
            title: "Total Value",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        }, {
            field: "ValueBalanceConsignee",
            title: "Balance",
            sortable: true,
            filterControl: true,
            halign: "center",
            class: 'text-nowrap',
            align: "left"
        },
            //    {
            //    field: "PermanentTemporary",
            //    title: "PERMANENT / TEMPORARY",
            //    sortable: true,
            //    filterControl: true,
            //    halign: "center",
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "SalesNonSales",
            //    title: "SALES / NON SALES",
            //    sortable: true,
            //    filterControl: true,
            //    halign: "center",
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "PortOfLoading",
            //    title: "LOADING",
            //    sortable: true,
            //    filterControl: true,
            //    halign: "center",
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "PortOfDestination",
            //    title: "DESTINATION",
            //    sortable: true,
            //    filterControl: true,
            //    halig: "center",
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "ETD",
            //    title: "ETD",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center"
            //}, {
            //    field: "ETA",
            //    title: "ETA",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center"
            //}, {
            //    field: "BlAwbNumber",
            //    title: "NO.",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "BlAwbDate",
            //    title: "DATE",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "SsNo",
            //    title: "NO. INVOICE CONSOLIDATION",
            //    sortable: true,
            //    filterControl: true,
            //    halign: "center",
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "PebDate",
            //    title: "DATE",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "IncoTerms",
            //    title: "INCOTERMS",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "Currency",
            //    title: "CURRENCY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "UnitPrice",
            //    title: "AMOUNT",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "right"
            //}, {
            //    field: "FreightPyment",
            //    title: "FREIGHT",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "InsuranceAmount",
            //    title: "INSURANCE",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "TotalAmount",
            //    title: "TOTAL AMOUNT",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "CiplNo",
            //    title: "NO",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Branch",
            //    title: "BRANCH",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "CiplCreateDate",
            //    title: "DATE",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Remarks",
            //    title: "REMARKS",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "ConsigneeName",
            //    title: "NAME",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center"
            //}, {
            //    field: "ConsigneeCountry",
            //    title: "COUNTRY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "CustomerName",
            //    title: "NAME",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center",
            //}, {
            //    field: "CustomerCountry",
            //    title: "COUNTRY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center"
            //}, {
            //    field: "PermanentTemporary",
            //    title: "TYPES",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left",
            //    halign: "center"
            //},
            //{
            //    field: "Category",
            //    title: "NAME",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Packages",
            //    title: "COLLY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Quantity",
            //    title: "QTY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "QuantityUom",
            //    title: "UOM",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Weight",
            //    title: "WEIGHT",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "WeightUom",
            //    title: "UOM",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "Currency",
            //    title: "CURRENCY",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "left"
            //}, {
            //    field: "UnitPrice",
            //    title: "AMOUNT",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "USDRate",
            //    title: "IDR / USD",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}, {
            //    field: "CurrencyRate",
            //    title: "IDR / CUR",
            //    sortable: true,
            //    filterControl: true,
            //    class: 'text-nowrap',
            //    align: "center"
            //}
        ]
    ]

    $table.bootstrapTable({
        //url: "/Json/Report/DetailTracking.json",
        columns: columns,
        cache: false,
        pagination: true,
        search: false,
        striped: false,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: true,
        smartDisplay: false,
        pageSize: [15],
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
    });

    searchDataReport();
});

function reportDetailTracking(startMonth, endMonth, paramName, paramValue, keynum) {

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RDetailsTrackingListPage?startMonth=' + startMonth + '&endMonth=' + endMonth + '&ParamName=' + paramName + '&ParamValue=' + paramValue + '&Keynum=' + keynum,
        urlPaging: '/EMCS/RDetailsTrackingPageXt',
        autoLoad: true
    });

}

function searchDataReport() {
    var keynum = '';
    if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[1] + '-' + date[0] + '-' + '01';
    }
    if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[1] + '-' + date[0] + '-' + '01';
    }
    if ($('#category').val() !== undefined && $('#category').val() !== null && $('#category').val() !== '') {
        paramName = $('#category').val();
        if ($('#unitCipl').val() != undefined)
            paramValue = $('#unitCipl').val();
        else if ($('#sparepartsCipl').val() != undefined)
            paramValue = $('#sparepartsCipl').val();
    }
    if ($('#keynum').val() !== undefined && $('#keynum').val() !== null && $('#keynum').val() !== '') {
        keynum = $('#keynum').val();
    }
    reportDetailTracking(startDate, endDate, paramName, paramValue, keynum);

}

function exportDataReport() {
    var keynum = '';
    if ($('#inp-start-date').val() !== undefined && $('#inp-start-date').val() !== null && $('#inp-start-date').val() !== '') {
        var date = $('#inp-start-date').val().split('/');
        startDate = date[1] + '-' + date[0] + '-' + '01';
    }
    if ($('#inp-end-date').val() !== undefined && $('#inp-end-date').val() !== null && $('#inp-end-date').val() !== '') {
        var date = $('#inp-end-date').val().split('/');
        endDate = date[1] + '-' + date[0] + '-' + '01';
    }
    if ($('#category').val() !== undefined && $('#category').val() !== null && $('#category').val() !== '') {
        paramName = $('#category').val();
        if ($('#unitCipl').val() != undefined)
            paramValue = $('#unitCipl').val();
        else if ($('#sparepartsCipl').val() != undefined)
            paramValue = $('#sparepartsCipl').val();
    }
    if ($('#keynum').val() !== undefined && $('#keynum').val() !== null && $('#keynum').val() !== '') {
        keynum = $('#keynum').val();
    }

    window.open('/EMCS/DownloadDetailsTracking?startDate=' + startDate + '&endDate=' + endDate + '&ParamName=' + paramName + '&ParamValue=' + paramValue + '&Keynum=' + keynum, '_blank');
}