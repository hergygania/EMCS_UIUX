var $table = $('#table-summary-pex');
setTimeout(function () { $(".model-parent").find(".select2").addClass("width30"); }, 300);

$(function () {
    clearScreen();

    $(".js-states").select2({ width: 'resolve', dropdownAutoWidth: 'false' });

    var width = $(".select2-container--default").width() - 5;
    $(".select2-container--default").css('width', width + 'px');

    $table.bootstrapTable({
        cache: false,
        search: false,
        toolbar: '.toolbar',
        pagination: true,
        striped: true,
        clickToSelect: true,
        reorderableColumns: true,
        onClickRow: selectRow,
        sidePagination: 'server',
        showColumns: false,
        showRefresh: true,
        smartDisplay: false,
        pageSize: '5',
        showExport: false,
        exportTypes: ['excel'],
        exportOptions: {
            ignoreColumn: [0],
            fileName: 'file.xls'
        },
        formatNoMatches: function () {
            return '<span class="noMatches">-</span>';
        },
        //fixedColumns:true,
        //fixedNumber:4,
        onAll: function (name, args) {
            $('[data-field="Allocated"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_oh"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_st"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_woc"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_ttc"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_sq"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_wip"]').css("background-color", "#fe9d00");
            $('[data-field="allocated_jc"]').css("background-color", "#fe9d00");

            $('[data-field="FreeAllocation"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_oh"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_st"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_woc"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_ttc"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_sq"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_wip"]').css("background-color", "#fe9d00");
            $('[data-field="free_allocation_jc"]').css("background-color", "#fe9d00");
        },
        columns:
        [
            [
                {
                    title: 'Ref Part No.',
                    field: 'ref_part_no',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '100px',
                    sortable: true
                }, {
                    title: 'Model',
                    field: 'model',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '100px',
                    sortable: true
                }, {
                    title: 'Component',
                    field: 'component',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '110px',
                    sortable: true
                }, {
                    title: 'Prefix',
                    field: 'prefix',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '80px',
                    sortable: true
                }, {
                    title: 'SMCS',
                    field: 'scms',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '80px',
                    sortable: true
                }, {
                    title: 'MOD',
                    field: 'mod',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '80px',
                    sortable: true
                }, {
                    title: 'Total Availability',
                    field: 'total_availability',
                    rowspan: 2,
                    halign: 'center',
                    valign: 'middle',
                    width: '180px',
                    sortable: true
                }, {
                    title: 'Allocated',
                    field: 'Allocated',
                    colspan: 7,
                    width: '500px',
                    align: 'center'
                },
                {
                    title: 'Free Allocation',
                    field: 'FreeAllocation',
                    colspan: 7,
                    width: '500px',
                    align: 'center'
                },
                {
                    title: 'Total Allocation',
                    field: 'total_allocation',
                    rowspan: 2,
                    align: 'center',
                    valign: 'middle',
                    width: '150px',
                    sortable: true
                },
                {
                    title: 'Allocation',
                    field: 'Allocation',
                    colspan: 5,
                    width: '500px',
                    align: 'center'
                }
            ],
            [
                {
                    field: 'allocated_oh',
                    title: 'OH',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocated_st',
                    title: 'ST',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocated_woc',
                    title: 'WOC',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'allocated_ttc',
                    title: 'TTC',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocated_sq',
                    title: 'SQ',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocated_wip',
                    title: 'WIP',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'allocated_jc',
                    title: 'JC',
                    sortable: true,
                    align: 'center'
                },

                {
                    field: 'free_allocation_oh',
                    title: 'OH',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'free_allocation_st',
                    title: 'ST',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'free_allocation_woc',
                    title: 'WOC',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'free_allocation_ttc',
                    title: 'TTC',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'free_allocation_sq',
                    title: 'SQ',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'free_allocation_wip',
                    title: 'WIP',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'free_allocation_jc',
                    title: 'JC',
                    sortable: true,
                    align: 'center'
                }
                , {
                    field: 'allocation_cycle1',
                    title: 'Cycle 1',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'allocation_cycle2',
                    title: 'Cycle 2',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocation_cycle3',
                    title: 'Cycle 3',
                    sortable: true,
                    align: 'center'
                }, {
                    field: 'allocation_cycle4',
                    title: 'Cycle 4',
                    sortable: true,
                    align: 'center'
                },
                {
                    field: 'allocation_cycle5',
                    title: 'Cycle 5',
                    sortable: true,
                    align: 'center'
                }
            ]
        ]
    });
    $("[name=refresh]").css('display', 'none');

    $('#btn-filter').click(function () {
        window.pis.table({
            objTable: $table,
            urlSearch: '/cat/SummaryPEXPage',
            urlPaging: '/cat/SummaryPEXPageXt',
            searchParams: {
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                component: $('#component').val(),
                SOS: $('#SOS').val()
            },
            dataHeight: 412,
            autoLoad: true
        });
        $('.fixed-table-body-columns').css('display', 'block');
    });

    $('#btn-clear').click(function () {
        clearScreen();
    });

    $(".downloadExcel").click(function () {
        enableLink(false);
        $.ajax({
            url: "DownloadSummaryPEX",
            type: 'GET',
            data: {
                ref_part_no: $('#ref_part_no').val(),
                model: $('#model').val(),
                component: $('#component').val(),
                SOS: $('#SOS').val()
            },
            success: function (guid) {
                enableLink(true);
                window.open('DownloadToExcel?guid=' + guid, '_blank');
            },
            cache: false,
        });
    });
});

function clearScreen() {
    $('#ref_part_no').val('');
    $('#model').val('');
    $('#component').val('');
    $('#SOS').val('').change();
}