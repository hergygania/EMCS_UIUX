var $table = $('#tabelackMessage');

$(function() {
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
        formatNoMatches: function() {
            return '<span class="noMatches">-</span>';
        },
        columns: [
            {
                title: 'No',
                halign: 'center',
                align: 'right',
                width: '3%',
                formatter: runningFormatter
            }, {
                field: 'ackm_SOS',
                title: 'SLoc',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ackm_StoreNo',
                title: 'Plant',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ackm_PartsNumber',
                title: 'Parts Number',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ackm_Description',
                title: 'Description',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ackm_Document',
                title: 'PO Number',
                halign: 'center',
                align: 'left',
                sortable: true
            }, {
                field: 'ackm_UNCS',
                title: 'Dealer Net',
                formatter: 'formatNumber',
                halign: 'center',
                align: 'right',
                sortable: true
            }, {
                field: 'ackm_GRSSWT',
                title: 'Gross Weight',
                halign: 'center',
                formatter: 'formatNumber',
                align: 'right',
                sortable: true
            }, {
                field: 'ackm_Message',
                title: 'Message',
                halign: 'center',
                align: 'left',
                sortable: true
            }]
    });


    $("#btnFilter").click(function() {
        var _starDate, _endDate, _groupType;
    
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var filterBy = $('#FilterBy').val();
        window.pis.table({
            objTable: $table,
            urlSearch: '/report/AckMessagePage',
            urlPaging: '/report/AckMessagePageXt',
            searchParams: {
                GroupType: _groupType,
                FilterBy: filterBy,
                StoreNumber: $('#StoreNumber').val(),
             
            },
            autoLoad: true
        });
    });

    $(".downloadExcel").click(function() {
        var _starDate, _endDate, _groupType;
      
        if ($('#hub:checked').length > 0) {
            _groupType = $('#hub').val();
        } else {
            _groupType = $('#area').val();

        }
        var groupType = _groupType;
        var storeNumber = $('#StoreNumber').val() == null ? '' : $('#StoreNumber').val();
        var filterBy = $('#FilterBy').val();

        window.open("/Report/ExportToExcelAckMessage?" +
         "groupType=" + groupType +
            "&filterBy=" + filterBy +
            "&storeNumber=" + storeNumber 
        );
    });


    $("#mySearch").insertBefore($("[name=refresh]"));
    enableLink(true);
});


function operateFormatter(value, row, index) {
    return [
        '<div class="btn-group" style="width:123px;white-space:nowrap; text-align:center">',
        '<button type="button" class="btn btn-xs btn-primary edit" title="Edit"><i class="fa fa-pencil"></i> Edit</button>',
        '<button type="button" class="btn btn-xs btn-info detail" title="Detail"><i class="fa fa-search-plus"></i> Detail</button>',
        '</div>'
    ].join('');
}


window.operateEvents = {
    
};


$(function() {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function(e) {
        enableLink(false);

        $('#myModalContent').load(this.href, function() {

            $('#myModalPlace').modal({ keyboard: true }, 'show');

            bindForm(this);

            enableLink(true);
        });
        return false;
    });


});

function bindForm(dialog) {
    $('form', dialog).submit(function() {

        if (!$("form#" + this.id).valid()) {
            return false;
        }

        $('#progress').show();
        enableLink(false);

        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function(result) {

                enableLink(true);

                if (result.Status == 0) {
                    if (result.Msg != undefined) sAlert('Success', result.Msg, 'success');
                    $('#myModalPlace').modal('hide');
                    $('#progress').hide();
                    $("#btnFilter").trigger('click');
                } else {
                    if (result.Msg != undefined) sAlert('Failed', result.Msg, 'error');
                    $('#progress').hide();
                    //$('#myModalContent').html(result);
                    //bindForm(dialog);
                }
            }
        });
        return false;
    });
}

;