var $table = $('#tabelOutstandingBackorderItem');

$(function () {
    enableLink(false);
    $table.bootstrapTable({
        cache: false,
        pagination: true,
        search: false,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        toolbar: '.toolbar',
        toolbarAlign: 'left',
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: true,
        showRefresh: false,
        smartDisplay: false,
        pageSize: '5',
        formatNoMatches: function () { return '<span class="noMatches">-</span>'; },
        columns: [
            { title: 'No', align: 'center', width: '45px', formatter: runningFormatter },
            { field: 'obkitm_Store', title: 'Store', align: 'center', width: '70px', sortable: true },
            { field: 'obkitm_RefDoc', title: 'Ref Document', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_OrgDate', title: 'Org Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '180px', sortable: true },
            { field: 'obkitm_NeedByDate', title: 'Need by Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '180px', sortable: true },
            { field: 'obkitm_CommittedDate', title: 'Commited Date', halign: 'center', align: 'right', width: '180px', formatter: 'newDateFormatter', sortable: true },
            { field: 'obkitm_CustPO', title: 'Customer PO', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_SOS', title: 'SOS', halign: 'center', align: 'center', width: '70px', sortable: true },
            { field: 'obkitm_PartNo', title: 'Part Number', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'obkitm_Description', title: 'Description', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_OrderQty', title: 'Order Quantity', halign: 'center', align: 'right', width: '130px', sortable: true },
            { field: 'obkitm_Commodity', title: 'Comodity', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_OrderMethod', title: 'Order Method', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'obkitm_ActivityInd', title: 'Activity Ind', halign: 'center', align: 'left', width: '110px', sortable: true },
            { field: 'obkitm_SKNSKI', title: 'SKNSKI', halign: 'center', align: 'left', width: '110px', sortable: true },
            { field: 'obkitm_PackQty', title: 'Pack Qty', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'obkitm_GrossWt', title: 'Gross Wt', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'obkitm_HoseAdmInd', title: 'Hose AdmInd', halign: 'center', align: 'right', width: '120px', sortable: true },
            { field: 'obkitm_UnitList', title: 'Unit List', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'obkitm_BO12M', title: 'BO12M', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'obkitm_CALL12M', title: 'CALL12M', halign: 'center', align: 'right', width: '100px', sortable: true },
            { field: 'obkitm_Demand12M', title: 'DEMAND12M', halign: 'center', align: 'right', width: '120px', sortable: true },
            { field: 'obkitm_Model', title: 'Model', halign: 'center', align: 'left', width: '120px', sortable: true },
            { field: 'obkitm_SerialNumber', title: 'Serial Number', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_MachineID', title: 'Machine Id', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_CustNo', title: 'Customer Number', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'obkitm_CustName', title: 'Customer Name', halign: 'center', align: 'left', width: '280px', sortable: true },
            { field: 'obkitm_Facility', title: 'Facility', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'obkitm_EntryClass', title: 'Entry Class', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'obkitm_BOShipInd', title: 'BO Ship IND', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_TransferOrderNo', title: 'Transfer Order No', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'obkitm_FacOrdNo', title: 'Fac Order No', halign: 'center', align: 'left', width: '150px', sortable: true },
            { field: 'obkitm_Comments', title: 'Comments', halign: 'center', align: 'left', width: '160px', sortable: true },
            { field: 'obkitm_DACKB', title: 'DA CKB', halign: 'center', align: 'left', width: '160px', sortable: true },
            { field: 'obkitm_PickupDate', title: 'Pickup Dat', halign: 'center', align: 'left', width: '130px', sortable: true },
            { field: 'obkitm_LeadTime', title: 'Lead Time', halign: 'center', align: 'left', width: '130px', sortable: true },
            { field: 'obkitm_ETADate', title: 'Eta Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '130px', sortable: true },
            { field: 'obkitm_OrdToCurrDate', title: 'Ord To Cur Date', halign: 'center', align: 'right', width: '130px', sortable: true },
            { field: 'obkitm_OrdToNeedByDate', title: 'Ord To Need By Date', formatter: 'newDateFormatter', halign: 'center', align: 'left', width: '180px', sortable: true },
            { field: 'obkitm_OrdToCommitedDate', title: 'Ord To Commited Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '180px', sortable: true },
            { field: 'obkitm_NeedByDateToCurrDate', title: 'Need By Date to Curr Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '200px', sortable: true },
            { field: 'obkitm_CommitedDateToCurrDate', title: 'Commited By Date to Curr Date', formatter: 'newDateFormatter', halign: 'center', align: 'right', width: '220px', sortable: true }
        ]
    });


    $("#btnFilter").click(function () {
        var _starDate, _endDate, _groupType;

        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var filterBy = $('#FilterBy').val();

        window.pis.table({
            objTable: $table,
            urlSearch: '/report/OutstandingBackorderItemPage',
            urlPaging: '/report/OutstandingBackorderItemPageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: $('#StoreNumber').val(),
                CustomerId: $('#CustomerId').val(),

            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function () {
        var _starDate, _endDate, _groupType;

        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var filterBy = $('#FilterBy').val();
        var groupType = _groupType;
        var storeNumber = $('#StoreNumber').val() == null ? '' : $('#StoreNumber').val();
        var custId = $('#CustomerId').val();

        window.open("/Report/ExportToExcelOutstandingBackorderItem?" +
            "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber +
           "&custId=" + custId
        );
    });



    $("#mySearch").insertBefore($("[name=refresh]"))
    enableLink(true);
});

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        enableLink(false);
        $('#myModalContent').load(this.href, function () {
            $('#myModalPlace').modal({ keyboard: true }, 'show');
            bindForm(this);
            enableLink(true);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {

        if (!$("form#" + this.id).valid()) { return false; }

        $('#progress').show();
        enableLink(false);

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {

                enableLink(true);

                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("#btnFilter").trigger('click');
                }
                else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                    //$('#myModalContent').html(result);
                    //bindForm(dialog);
                }
            }
        });
        return false;
    });
};

