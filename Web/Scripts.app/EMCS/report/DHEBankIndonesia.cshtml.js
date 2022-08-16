var $table = $("#tbl-dhebi");

$(function () {
    get_parameter_select();
       
    var columns = [
        {
        //    field: "no",
        //    title: "No",
        //    align: 'center',
        //    valign: "middle",
        //    formatter: runningFormatter
        //}, {
            field: "NomorIdentifikasi",
            title: "No Identifikasi <br> (Transaction Reference)",
            align: "left",
            halign: "center"
        }, {
            field: "Npwp",
            title: "NPWP <br> (Tax Registration Number)",
            align: "left",
            halign: "center"
        }, {
            field: "NamaPenerimaDhe",
            title: "Nama Penerima DHE <br> (Export Proceed Receiver Name)",
            align: "left",
            halign: "center"
        }, {
            field: "SandiKantorPabean",
            title: "Sandi Kantor Pabean <br> (Custom Excise Code)",
            align: "left",
            halign: "center"
        }, {
            field: "NomorPendaftaranPeb",
            title: "Nomor Pendaftaran PEB <br> (Export Declaration Registration Number)",
            align: "left",
            halign: "center"
        }, {
            field: "TanggalPeb",
            title: "Tanggal PEB <br> (Export Declaration Date)",
            align: "left",
            halign: "center"
        }, {
            field: "JenisValutaDhe",
            title: "Jenis Valuta DHE <br> (Remit Currency)",
            align: "left",
            halign: "center"
        }, {
            field: "NilaiDhe",
            title: "Nilai DHE <br> (Export Proceed Amount)",
            align: "right",
            halign: "center"
        }, {
            field: "NilaiPeb",
            title: "Nilai PEB/FOB<br>  (Export Declaration Value)",
            align: "right",
            halign: "center"
        }, {
            field: "SandiKeterangan",
            title: "Sandi Keterangan <br> (Remarks Code)",
            align: "left",
            halign: "center"
        }, {
            field: "KelengkapanDokumen",
            title: "Kelengkapan Dokumen <br> (Supporting Documents Completion)",
            align: "left",
            halign: "center"
        }, {
            field: "JenisValutaPeb",
            title: "Jenis Valuta PEB <br> (Export Declaration Currency)",
            align: "left",
            halign: "center"
        }

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

    var StartDate = $("#inp-start-date").val();
    var EndDate = $("#inp-end-date").val();
    var Category = "";
    var Type = "";

    reportDHEBI(StartDate, EndDate, Category, Type);
});

function reportDHEBI(StartDate, EndDate , Category, Type) {

    window.pis.table({
        objTable: $table,
        urlSearch: '/EMCS/RDheBIListPage?StartDate=' + StartDate + '&EndDate=' + EndDate + '&Category=' + Category + '&Type=' + Type,
        urlPaging: '/EMCS/RDheBIPageXt',
        autoLoad: true
    });

}

function get_parameter_select() {
    $.ajax({
        url: "/emcs/GetSelectItemCipl",
        success: function (data) {
            var category = new Array();
            $.each(data.category, function (index, element) {
                category.push({ 'id': element.Id, 'text': element.Text === 'CATERPILLAR NEW EQUIPMENT' ? 'CATERPILLAR NEW EQUIPMENT' : element.Text });
            });
            $("#jenisBarangCipl").select2({
                data: category,
                width: '100%',
                placeholder: 'Please Select Category',
            })
            $('#jenisBarangCipl').val(null).trigger('change');
        }
    });
}

function searchDataReport() {
    StartDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    EndDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    Category = $('#jenisBarangCipl').val() == null || $('#jenisBarangCipl').val() == '' ? '' : $('#jenisBarangCipl').val();
    Type = $('#type').val() === null || $('#type').val() === '' ? '' : $('#type').val();
    reportDHEBI(StartDate, EndDate, Category, Type);
}

function exportDataReport() {
    var startDate = $('#inp-start-date').val() === null || $('#inp-start-date').val() === '' ? '' : moment($('#inp-start-date').val()).format('YYYY-MM-DD');
    var endDate = $('#inp-end-date').val() === null || $('#inp-end-date').val() === '' ? '' : moment($('#inp-end-date').val()).format('YYYY-MM-DD');
    var jenisBarangCipl = $('#jenisBarangCipl').val() === null || $('#jenisBarangCipl').val() === '' ? '' : $('#jenisBarangCipl').val();
    var type = $('#type').val() === null || $('#type').val() === '' ? '' : $('#type').val();
    window.open('/EMCS/DownloadDHEBI?StartDate=' + startDate + '&EndDate=' + endDate + '&Category=' + jenisBarangCipl + '&ExportType=' + type, '_blank');
}