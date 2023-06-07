window.ajax_method_POST = 'POST'

$.fn.serializeFormJSON = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function SubmitFieldset(fieldsetName) {    
    $('body').append('<form id="form-to-submit" style="visibility:hidden;"></form>');
    $('#form-to-submit').html($(fieldsetName).clone());
    var data = $('#form-to-submit').serializeFormJSON();
    $('#form-to-submit').remove();

    return data;
}

window.dataTable_general_opts  = {
    destroy       : true,
    lengthChange  : false,
    //- ajax          : "data/advance-filter.json",
    deferRender   : true,
    fixedColumns  : {
        leftColumns : 2
    },
    scrollX       : true,
    fixedColumns  : true,
    pageLength    : 100,
};


$(document).ready(function() {

    var inputAdvSrc             = '#input-search-filter';
    var tableAdvSrcResult       = '#table-adv-src-result';

    // -------------
    // DATATABLE QUICK SEARCH
    // -------------
    function quickTrackingSO() {
        var ajax_url    = $('#btn-track-now').data('ajax_url');
        var error_empty = $('#btn-track-now').data('ajax_empty');
  
        var $table_wrap = $('#table-quick-result-wrap')
  
        var $btn_refresh        = $table_wrap.find('.refresh-data-table');
        var $latest_date        = $table_wrap.find('.latest_date');
        var $latest_time        = $table_wrap.find('.latest_time');
        var $btn_export_excel   = $table_wrap.find('.btn-export-excel');
        var $showing_data       = $table_wrap.find('.showing-data');
  
        $('body').addClass('preload-truck-show'); // PRELOADER
        Pace.start

        var $table_target   = $('#table-quick-result');

        $table_target.DataTable().clear().destroy()

        var data_obj    = $('#quick-search-form').serializeFormJSON();

        var xhr = $.ajax({
            type        : ajax_method_POST,
            url         : ajax_url,
            data        : JSON.stringify(data_obj),
            dataType    : 'json',
            contentType : 'application/json; charset=utf-8',
            success     : function (resp) {

                $('#quick-search-form').hide();
                $('#quick-search-result').show();

                setTimeout(function () {
                    $('body').removeClass('preload-truck-show');
                    // ------------------
                    // GENERATE COLUMN FILTER
                    // ------------------
                    $table_target.find('thead tr')
                      .clone(true)
                      .appendTo( $table_target.find('thead') );
          
                    $table_target.find('thead tr:eq(1)').addClass('cloned')
                    $table_target.find('thead tr:eq(1) th').each( function (i) {
                        var title = $(this).text();
                        $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
                        $( 'input', this ).on( 'keyup change', function () {
                            if ( table_quick_result.column(i).search() !== this.value ) {
                                table_quick_result
                                  .column(i)
                                  .search( this.value )
                                  .draw();
                            }
                        });
                    });
  
                    // ------------------
                    // DATATABLES
                    // ------------------
                    var opts = {
                        // data          : resp.data,
                        processing    : true,
                        serverSide    : true,
                        ajax          : {
                            url           : ajax_url + '?get_data_only=true',
                            type          : 'POST'
                        },
                        dom           : 'Bfrtip',
                        orderCellsTop : true,
                        buttons       : [
                          {
                              extend  : 'excelHtml5',
                              text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
                          }
                        ],
                        columns   : quickSearchColumns
                    }
  
                    var merge_opts = Object.assign(opts, dataTable_general_opts);

                    var table_quick_result = $table_target.DataTable(merge_opts);

                    var table_info = table_quick_result.page.info();

                    // console.log(table_info);

                    var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
                    var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

                    $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);
                    var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
                    var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

                    $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);

                    var data_latest_update = resp.latest_update;

                    $latest_date.html(getDateDDMMYYY(data_latest_update));
                    $latest_time.html(getTime(data_latest_update));
          
                    // ------------------
                    // BUTTON EXPORT
                    // ------------------
                    $btn_export_excel.on('click', function () {
                        $table_wrap.find('.buttons-excel.buttons-html5').click()
                    });
  
                    // ------------------
                    // REFRESH DATA TABLE
                    // ------------------
                    $btn_refresh.on('click', function() {
                        $table_target.DataTable().clear().destroy();
                        $table_target.find('thead .cloned').remove()
          
                        $table_target.find('thead tr, thead th').each(function() {
                            var attributes = $.map(this.attributes, function(item) {
                                return item.name;
                            });
                            var img = $(this);
                            $.each(attributes, function(i, item) {
                                img.removeAttr(item);
                            });
                        });
          
                        setTimeout(function () {
                            quickTrackingSO();
                        }, 500 );
                    });
  
  
                    // ------------------
                    // OPEN ORDER TABLE
                    // ------------------
                    $table_target.on('click', '.btn-open-order-table', function() {
                        // console.log('klik')
                        var order = $(this).data('order_item')
  
                        quickOrderItem(order);
                    });
                }, 500);
            }
        });
  
        // prevent submitting
        return false;
    }
  
  
    // -------------
    // DATATABLE QUICK ORDER ITEM
    // -------------
    function quickOrderItem(value) {
        var $table_wrap = $('#table-quick-order-wrap');
  
        var ajax_url    = $table_wrap.data('ajax_url');
  
        var $btn_refresh        = $table_wrap.find('.refresh-data-table');
        var $latest_date        = $table_wrap.find('.latest_date');
        var $latest_time        = $table_wrap.find('.latest_time');
        var $btn_export_excel   = $table_wrap.find('.btn-export-excel');
        var $showing_data       = $table_wrap.find('.showing-data');
  
  
        $('body').addClass('preload-truck-show'); // PRELOADER
        Pace.start
  
        var $table_target   = $('#table-quick-order');
  
        $table_target.DataTable().clear().destroy()
  
        var data_obj    = {
            'order_item': value
        }
  
        var xhr = $.ajax({
            type      : ajax_method_POST,
            url       : ajax_url,
            data      : JSON.stringify(data_obj),
            dataType  : 'json',
            contentType : 'application/json; charset=utf-8',
            success   : function (resp) {
  
                $('#table-quick-result-wrap').hide();
                $('#table-quick-order-wrap').show();
                $('.quicksdnumbershow').html('Sales Document No : '+resp.salesdocument);
                $('body').removeClass('preload-truck-show'); // PRELOADER
                setTimeout(function () {
  
                    // ------------------
                    // GENERATE COLUMN FILTER
                    // ------------------
                    $table_target.find('thead tr')
                      .clone(true)
                      .appendTo( $table_target.find('thead') );
          
                    $table_target.find('thead tr:eq(1)').addClass('cloned')
                    $table_target.find('thead tr:eq(1) th').each( function (i) {
                        var title = $(this).text();
                        $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
                        $( 'input', this ).on( 'keyup change', function () {
                            if ( table_quick_result.column(i).search() !== this.value ) {
                                table_quick_result
                                  .column(i)
                                  .search( this.value )
                                  .draw();
                            }
                        });
                    });

                    // console.log('ajax_url')
                    // console.log(ajax_url)
  
                    // ------------------
                    // DATATABLES
                    // ------------------
                    var opts = {
                        data          : resp.data,
                        processing    : true,
                        serverSide    : true,
                        ajax          : {
                            url           : ajax_url + '?get_data_only=true',
                            type          : 'POST'
                        },
                        dom           : 'Bfrtip',
                        orderCellsTop : true,
                        buttons       : [
                          {
                              extend  : 'excelHtml5',
                              text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
                          }
                        ],
                        columns   : quickOrderColumns
                    }
  
                    var merge_opts = Object.assign(opts, dataTable_general_opts);
  
                    var table_quick_result = $table_target.DataTable(merge_opts);
  
                    var table_info = table_quick_result.page.info();

                    // console.log(table_info);

                    var row_txt      = ( table_info.recordsDisplay < 2 ) ? 'Row' : 'Rows';
                    var showing_info = ( table_info.end < table_info.length) ? table_info.end : table_info.length

                    $showing_data.html('Showing ' + showing_info + ' ' + row_txt + ' from Total ' + table_info.recordsTotal + ' ' + row_txt);

                    var data_latest_update = resp.latest_update;

                    $latest_date.html(getDateDDMMYYY(data_latest_update));
                    $latest_time.html(getTime(data_latest_update));

                    // ------------------
                    // BUTTON EXPORT
                    // ------------------
                    $btn_export_excel.on('click', function () {
                        $table_wrap.find('.buttons-excel.buttons-html5').click()
                    });

                    // ------------------
                    // MODAL OPEN - SHIPPING TIMELINE
                    // ------------------
                    $table_target.on('click', '.btn-modal-shipment-stat', function () {
                        var collect_data    = $(this).data('to_send').replace(/'/g, '"');
                        var modal_target    = $(this).data('target');
                        var error_tracking  = $(this).data('error_tracking');

                        var ajax_url        = $(this).data('ajax_url');

                        vertical_timeline(collect_data, modal_target, ajax_url, error_tracking)
                    });

                    // ------------------
                    // REFRESH DATA TABLE
                    // ------------------
                    $btn_refresh.on('click', function() {
                        $table_target.DataTable().clear().destroy();
                        $table_target.find('thead .cloned').remove()
          
                        $table_target.find('thead tr, thead th').each(function() {
                            var attributes = $.map(this.attributes, function(item) {
                                return item.name;
                            });
                            var img = $(this);
                            $.each(attributes, function(i, item) {
                                img.removeAttr(item);
                            });
                        });
          
                        setTimeout(function () {
                            quickOrderItem();
                        }, 500 );
                    });

                }, 500);
            }
        });
    }


    $('#btn-track-now').on('click', function(e) {
        if(quickSearchValidation() == false){
        }else{
            e.preventDefault()
            quickTrackingSO();
        }
    });

    $("#quick-search-form #qs-so-no, #quick-search-form #qs-cn").keyup(function(e) {
        const code = e.which

        if (code === 13) {
            // quickTrackingSO();
            $('#btn-track-now').click();
        }
    });

    // -------------
    // HIDE TRUCK PRELOADER
    // -------------
    var hide_truck_preload = function () {
        setTimeout(function() {
            $('body').removeClass('preload-truck-show');
        }, 1000);
    }

    // -------------
    // COLUMN SETTING
    // -------------
    window.checked_summary_on_init   = [];
    window.checked_order_on_init     = [];
    window.checked_source_on_init    = [];

    window.checked_summary   = [];
    window.checked_order     = [];
    window.checked_source    = [];

    window.column_set_default_valid = false;
    // window.params_set_default_valid = false

    var checked_summary_dirty_check = function (new_value) {
        if (checked_summary_on_init.length > 0) {
            var new_checked_summary   = JSON.stringify( new_value.sort() )
            var saved_checked_summary = JSON.stringify( checked_summary_on_init.sort() )

            if (new_checked_summary != saved_checked_summary) {
                return false
            } else {
                return true
            }
        }
    }


    var checked_order_dirty_check = function (new_value) {
        if (checked_order_on_init.length > 0) {
            var new_checked_order   = JSON.stringify( new_value.sort() )
            var saved_checked_order = JSON.stringify( checked_order_on_init.sort() )

            if (new_checked_order != saved_checked_order) {
                return false
            } else {
                return true
            }
        }
    }

    var checked_source_dirty_check = function (new_value) {
        if (checked_source_on_init.length > 0) {
            var new_checked_source   = JSON.stringify( new_value.sort() )
            var saved_checked_source = JSON.stringify( checked_source_on_init.sort() )

            if (new_checked_source != saved_checked_source) {
                return false
            } else {
                return true
            }
        }
    }

    var check_all_column__dirty = function (summary, order, source) {
        if ( summary == true && order == true & source == true ) {
            column_set_default_valid = true
        } else {
            column_set_default_valid = false
        }

        // console.log('column_set_default_valid: ' + column_set_default_valid)
    }

    // COLUMN - SUMMARY
    $('.chk-summary-checkall').change(function() {
        var target = $(this).data('checkall_target');
  
        checked_summary = [];
  
        if ($(this).is(':checked')) {
            target.forEach(function(e){
                checked_summary.push('checkedAll');
            })
        } else {
            target.forEach(function(e){
                checked_summary.remove('checkedAll');
            })
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);

    })
  
    $('.chk-summary')
    .each(function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_summary.push(item);
            checked_summary_on_init.push(item);
            column_set_default_valid = true
        }
    })
    .on('change', function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_summary.push(item);
        } else {
            checked_summary.remove(item);
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);

    });
  
    // COLUMN - ORDER
    $('.chk-item-order-checkall').change(function() {
        var target = $(this).data('checkall_target');
  
        checked_order = [];
  
        if ($(this).is(':checked')) {
            target.forEach(function(e){
                checked_order.push('checkedAll');
            })
        } else {
            target.forEach(function(e){
                checked_order.remove('checkedAll');
            })
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);  })
  
    $('.chk-item-order')
    .each(function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_order.push(item);
            checked_order_on_init.push(item);
            column_set_default_valid = true
        }
    })
    .on('change', function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_order.push(item);
        } else {
            checked_order.remove(item);
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);
    });

    // COLUMN - SOURCE
    $('.chk-item-source-checkall').change(function() {
        var target = $(this).data('checkall_target');
  
        checked_source = [];
  
        if ($(this).is(':checked')) {
            target.forEach(function(e){
                checked_source.push('checkedAll');
            })
        } else {
            target.forEach(function(e){
                checked_source.remove('checkedAll');
            })
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);  });
  
    $('.chk-item-source')
    .each(function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_source.push(item);
            checked_source_on_init.push(item);
            column_set_default_valid = true
        }
    })
    .on('change', function() {
        var item = $(this).val();
  
        if ($(this).is(':checked')) {
            checked_source.push(item);
        } else {
            checked_source.remove(item);
        }

        var summary = checked_summary_dirty_check(checked_summary),
        order       = checked_order_dirty_check(checked_order),
        source      = checked_source_dirty_check(checked_source);

        check_all_column__dirty(summary, order, source);
    });
  
  
    // --------------------
    // BUTTON SET TO DEFAULT
    // --------------------
    $('#btn-col-filter-setDefault').on('click', function() {
        var ajax_url      = $(this).data('ajax_url');
        var alert_error   = $(this).data('alert_error')
        var alert_success = $(this).data('alert_success')
  
  
        var set_as_default = function (collect_data, ajax_url, alert_success) {
            var xhr = $.ajax({
                type      : ajax_method_POST,
                url       : ajax_url,
                data      : JSON.stringify(collect_data),
                dataType  : 'json',
                contentType : 'application/json; charset=utf-8',
                success   : function (resp) {
                    if (resp.is_valid === true) {
                        Swal.fire({
                            type  : 'success',
                            html  : alert_success,
                            timer : 1000
                        });
                    } else {
                        Swal.fire({
                            type  : 'error',
                            html  : 'Save Failed'
                        });
                    }
                }
            });
        }
  
        if ( $(this).attr('id') == 'btn-col-filter-setDefault' ) {
            if ( checked_summary.length !== 0 || checked_order.length !== 0 || checked_source.length !== 0 ) {
                var collect_data = SubmitFieldset('#fieldset-column-setting')
                set_as_default(collect_data, ajax_url, alert_success)
                column_set_default_valid = true
            } else {
                Swal.fire({
                    type : 'error',
                    html : alert_error
                })
            }
        } else {
            var collect_data = SubmitFieldset('#fieldset-parameter-setting')
            set_as_default(collect_data, ajax_url, alert_success)
            // params_set_default_valid = true
        }
    });

    // ----------
    // BTN NEXT
    // ----------
    $('#btn-adv-search-next, #btn-tab-filter-2').on('click', function(e) {
        var $this = $(this)
        var alert_error = $(this).data('alert_error')

        var btn_id = ( $(this).attr('id') === 'btn-adv-search-next' ) ? '#btn-adv-search-next' : '#btn-tab-filter-2'

        if ( checked_summary.length !== 0 || checked_order.length !== 0 || checked_source.length !== 0) {
            if ( column_set_default_valid == true ) { // VALIDATE "SET AS DEFAULT"
                if ($this.prop('id') === 'btn-adv-search-next') {
                    $('#btn-tab-filter-2').click()
                }
            } else {

                Swal.fire({
                    type : 'error',
                    html : 'Please click "Set as Default"'
                });

                return false
            }

        } else {
            Swal.fire({
                type : 'error',
                html : 'Please click "Set as Default"'
            });
  
            return false
        }
    });
  

    // -------------
    // DATATABLE ADV SEARCH
    // -------------
    function advResultTable() {
        var $table_wrap         = $('#table-adv-src-result-wrap')
        var ajax_url            = $table_wrap.data('ajax_url');
        var ajax_table_adv      = $table_wrap.data('ajax_table_adv');
        var ajax_url_order      = $table_wrap.data('ajax_table_order');
        var ajax_url_sourcing   = $table_wrap.data('ajax_table_sourcing');

        var error_empty         = $table_wrap.data('ajax_empty');

        var $btn_refresh        = $table_wrap.find('.refresh-data-table');
        var $latest_date        = $table_wrap.find('.latest_date');
        var $latest_time        = $table_wrap.find('.latest_time');
        var $btn_export_excel   = $table_wrap.find('.btn-export-excel');

        // $('body').addClass('preload-truck-show'); // PRELOADER
        Pace.start

        var $table_target   = $('#table-adv-src-result');

        $table_target.DataTable().clear().destroy()

        var data_obj    = $('#advance-src-form').serializeFormJSON();

        var xhr = $.ajax({
            type      : ajax_method_POST,
            url       : ajax_url,
            data      : JSON.stringify(data_obj),
            dataType  : 'json',
            contentType : 'application/json; charset=utf-8',
            success     : function (resp) {

                $('#advance-src-form').hide();
                $('#advanced-search-result').show();

                setTimeout(function () {

                    // ------------------
                    // GENERATE COLUMN FILTER
                    // ------------------
                    $table_target.find('thead tr')
                      .clone(true)
                      .appendTo( $table_target.find('thead') );
          
                    $table_target.find('thead tr:eq(1)').addClass('cloned')
                    $table_target.find('thead tr:eq(1) th').each( function (i) {
                        var title = $(this).text();
                        $(this).html( '<input type="text" class="form-control rounded-0" placeholder="Find" />' );
      
                        $( 'input', this ).on( 'keyup change', function () {
                            if ( table_quick_result.column(i).search() !== this.value ) {
                                table_quick_result
                                  .column(i)
                                  .search( this.value )
                                  .draw();
                            }
                        });
                    });

                    // ------------------
                    // DATATABLES
                    // ------------------
                    var opts = {
                        // data          : resp.data,
                        processing    : true,
                        serverSide    : true,
                        ajax          : {
                            url           : ajax_url + '?get_data_only=true',
                            type          : 'POST'
                        },
                        dom           : 'Bfrtip',
                        orderCellsTop : true,
                        buttons       : [
                          {
                              extend  : 'excelHtml5',
                              text    : '<span class="btn-generate-export-excel">Export to Excel</span>'
                          }
                        ],
                        columnDefs : resp.columnDefs,
                        columns   : advanceAllcolumns
                    }

                    var merge_opts          = Object.assign(opts, dataTable_general_opts);
                    var table_quick_result  = $table_target.DataTable(merge_opts);
                    var data_latest_update  = resp.latest_update;
                    var table_info          = table_quick_result.page.info();

                    $latest_date.html(getDateDDMMYYY(data_latest_update));
                    $latest_time.html(getTime(data_latest_update));

                    // ------------------
                    // BUTTON EXPORT
                    // ------------------
                    $btn_export_excel.on('click', function () {
                        $table_wrap.find('.buttons-excel.buttons-html5').click()
                    });

                    // ------------------
                    // REFRESH DATA TABLE
                    // ------------------
                    $btn_refresh.on('click', function() {
                        $table_target.DataTable().clear().destroy();
                        $table_target.find('thead .cloned').remove()

                        $table_target.find('thead tr, thead th').each(function() {
                            var attributes = $.map(this.attributes, function(item) {
                                return item.name;
                            });
                            var img = $(this);
                            $.each(attributes, function(i, item) {
                                img.removeAttr(item);
                            });
                        });

                        setTimeout(function () {
                            advResultTable();
                        }, 1000 );
                    });


                    // TRACKING MODAL
                    $table_target.on('click', '.btn-open-tracking-so-number', function() {
                        var number    = $(this).data('salesdocumentnumber');
                        var modal     = $(this).attr('href');
                        // var ajax_url  = $('#table-adv-src-result').data('ajax_table_order');

                        var ajax_url      = $('#tracking-modal').data('ajax_url');

                        $(modal).modal('show');

                        // manual_table_so_tracking(ajax_url, number)

                        $('#tracking-so-loop').html('')

                        var resp_data       = ''
          
                        var i = 0
                        setTimeout(() => {
                            $.ajax({
                                type      : ajax_method_POST,
                                url       : ajax_url,
                                data      : JSON.stringify({
                                    salesDocumentNumber : number
                                }),
                                dataType  : 'json',
                                contentType : 'application/json; charset=utf-8',
                                success   : function (resp) {
                                    var so_number   = resp.so_number;
                                    resp_data       = resp.data;
                                    total_table     = resp_data.length;
          
                                    $('#tracking-so-number').html(so_number);

                                    setTimeout(function () {


                                        var id = 'table-tracking-so-' + i
          
                                        var thead = ''
                                        trackNumber.title.forEach(function(item, index){
                                            thead += '<th>'+item+'</th>'
                                        });
          
                                        var tbody       = ''
                                        var div         = ''
                                        var da_number   = ''

                                        resp_data.forEach(function(item_resp, index_resp){
                                            da_number = item_resp.DANumber57;

                                            tbody     += '<tr>';
                                            tbody     += '<td><a href="javascript: void(0);" class="btn-link text-danger btn-modal-track-detail" data-modal_target="#track-detail-stat-modal-adv" data-da_number="' + da_number + '">' + da_number + '</a></td>';

                                            Object.keys(item_resp.Trakingdata).forEach(function(item_td, index_td){
                                                if (trackNumber.data.includes(item_td)) {
                                                    tbody += '<td>'+ item_resp.Trakingdata[item_td] + '</td>';
                                                }
                                            })
                                            tbody += '</tr>';

                                        });

                                        div += '<div class="row no-gutters">' +
                                                  '<div class="table-responsive mb-3">' +
                                                    '<table class="table table-striped table-bordered table-tracking table-text-nowrap" id="' + id + '">' +
                                                      '<thead class="thead-light">' +
                                                        '<tr class="small text-center align-middle">' +
                                                          thead +
                                                        '</tr>' +
                                                      '</thead>' +
                                                      '<tbody>' +
                                                        tbody +
                                                      '</tbody>' +
                                                    '</table>' +
                                                  '</div>' +
                                                '</div>';
                
                                        tbody = '';
                                        $('#tracking-so-loop').append(div);
                                    }, 500);

                                }
                            });
          
                        
                    }, 500);

                });

                $(document).on('click', '.btn-modal-track-detail', function () {
                    var modal_target    = $(this).data('modal_target');
                    var error_tracking  = $('#table-adv-src-result-wrap').data('error_not_found');
                    var ajax_url        = $('#table-adv-src-result-wrap').data('ajax_vert_timeline');
                    var collect_data    = JSON.stringify(
                      {
                          da: $(this).data('da_number')
                      }
                    );

                    vertical_timeline(collect_data, modal_target, ajax_url, error_tracking)
                });

            }, 500);
    }
});

return false;
}

// -------------
// ADVANCE SEARCH ON CLICK
// -------------
$('#btn-run-adv-search').on('click', function(e) {
    e.preventDefault()

    advResultTable();
});
  
  
// --------------------
// OPEN ORDER & STATIC SO NUMBER - DATATABLE WITHIN MODAL
// --------------------
$('#table-adv-src-result').DataTable().on('draw', function() {
    // ORDER MODAL
    $('.btn-open-order-modal').on('click', function() {
        var number    = $(this).data('number');
        var modal     = $(this).attr('href');
        // var ajax_url  = $('#table-adv-src-result').data('ajax_table_order');

        var ajax_url      = $('#table-adv-src-result-wrap').data('ajax_table_order');

        $(modal).modal('show')

        setTimeout(() => {
            $.ajax({
                type      : ajax_method_POST,
                url       : ajax_url,
                data      : JSON.stringify({
                    so_number : number
                }),
                dataType    : 'json',
                contentType : 'application/json; charset=utf-8',
                success     : function (resp) {

                    var opts = {
                        data          : resp.data,
                        columns       : advSrcModal_order_columns,
                        columnDefs    : resp.columnDefs
                    }

                    var merge_opts = Object.assign(opts, dataTable_general_opts)

                    var table_order = $('#table-item-order').DataTable(merge_opts)
                }
            })
        }, 500);
    });
});


// --------------------
// OPEN SOURCING - DATATABLE WITHIN MODAL
// --------------------
$('#table-item-order').DataTable().on('draw', function() {
    $('.btn-open-sourcing-modal').on('click', function() {
        var number    = $(this).data('number');
        var itemsd    = $(this).data('itemsd');
        var modal     = $(this).attr('href');
        // var ajax_url  = $('#table-adv-src-result').data('ajax_table_sourcing');
  
        var ajax_url   = $('#table-adv-src-result-wrap').data('ajax_table_sourcing');
  
        $(modal).modal('show')
  
        setTimeout(() => {
            $.ajax({
                type      : ajax_method_POST,
                url       : ajax_url,
                data      : JSON.stringify({
                    salesDocumentNumber : number,
                    salesDocumentItem   : itemsd
                }),
                dataType    : 'json',
                contentType : 'application/json; charset=utf-8',
                success     : function (resp) {
  
                    var opts = {
                        data        : resp.data,
                        columns     : advSrcModal_sourcing_columns,
                        columnDefs  : resp.columnDefs
                    }
  
                    var merge_opts = Object.assign(opts, dataTable_general_opts)
    
                    var table_order = $('#table-item-sourcing').DataTable(merge_opts)
                }
            })
        }, 500);
    });
});

// -------------
// ADVANCE SEARCH - CANCEL AJAX ADVANCE FILTER 
// -------------
$('#btn-cancel-filter').on('click', function() {
    // runFilterAjax('abort');
});

// -------------
// 
// -------------
$('.btn-back-quick-item-to-summer').on('click', function(){
    var $table_target   = $('#table-quick-order');
    $table_target.DataTable().clear().destroy();
    $table_target.find('thead .cloned').remove()
    $table_target.find('thead tr, thead th').each(function() {
        var attributes = $.map(this.attributes, function(item) {
            return item.name;
        });
        var img = $(this);
        $.each(attributes, function(i, item) {
            img.removeAttr(item);
        });
    });
    $('#table-quick-result-wrap').show();
    $('#table-quick-order-wrap').hide();
});

$('.btn-back-summer-to-form').on('click', function(){
    var $table_target   = $('#table-quick-result');
    $table_target.DataTable().clear().destroy();
    $table_target.find('thead .cloned').remove()
    $table_target.find('thead tr, thead th').each(function() {
        var attributes = $.map(this.attributes, function(item) {
            return item.name;
        });
        var img = $(this);
        $.each(attributes, function(i, item) {
            img.removeAttr(item);
        });
    });
    $('#quick-search-form').show();
    $('#quick-search-result').hide();
});

function quickSearchValidation(){
    var qssono = $('#qs-so-no').val();
    var qscn = $('#qs-cn').val();
    var qscds = $('#quick-search-date_start').val();
    var qscde = $('#quick-search-date_end').val();
    if (qssono == '' && qscn == '' && qscds == '' && qscde == ''){
        Swal.fire({
            type  : 'error',
            html  : 'Please fill So Number or Customer Name or Creation Date before click button search !'
        });
        return false;
    }else{
        return true;
    }
}

// -------------
// AJAX SELECTPICKER
// -------------
$('.ajax-selectpicker').each(function(){
    $(this).selectpicker({
        liveSearch: true
    })
    .ajaxSelectPicker({
        ajax      : {
            url       : $(this).data('ajax-url'),
            type      : 'POST',
            dataType  : 'json',
            data    : {
                q       : '{{{q}}}'
            }
        },
        locale      : {
            emptyTitle  : 'Select and Begin Typing'
        },
        log             : 3,
        preprocessData  : function(feedback) {
            var i;
            var data  = feedback.data;
            var array = [];
            if (data.length) {
                for (i = 0; i < data.length; i++) {
                    array.push(
                      $.extend(true, data[i], {
                          text  : data[i].option,
                          value : data[i].value
                      })
                    );
                }
            }
            return array;
        }
    });
});
});

// ---------------------------------
// SO TRACKING - MANUAL TABLE
// ---------------------------------
var manual_table_so_tracking = function (ajax_url, number) {
    $('#tracking-so-loop').html('');

    var total_table     = 1;
    var resp_data       = '';

    var i = 0;
    setTimeout(() => {
        $.ajax({
            type      : ajax_method_POST,
            url       : ajax_url,
            data      : JSON.stringify({
                salesDocumentNumber : number
            }),
            dataType  : 'json',
            contentType : 'application/json; charset=utf-8',
            success   : function (resp) {
                var so_number   = resp.so_number;
                resp_data       = resp.data;
                total_table     = resp_data.length;

                $('#tracking-so-number').html(so_number);
            }
        });

    setTimeout(function () {
        console.log(resp_data)
        var id = 'table-tracking-so-' + i

        var thead = ''
        trackNumber.title.forEach(function(item, index){
            thead += '<th>'+item+'</th>'
        });

        var tbody       = ''
        var div         = ''
        var da_number   = ''
      
        resp_data.forEach(function(item_resp, index_resp){
            da_number = item_resp.DANumber57

            item_resp.Trakingdata.forEach(function(item_tr, index_tr){
                tbody += '<tr>'
                Object.keys(item_tr).forEach(function(item_td, index_td){
                    if (trackNumber.data.includes(item_td)) {
                        tbody += '<td>'+ item_tr[item_td]+'</td>'
                    }
                })
                tbody += '</tr>'
            })

            div += '<div class="row no-gutters">' +
                        '<h6>DA NUMBER '+da_number+'</h6>' +
                          '<div class="table-responsive mb-3">' +
                            '<table class="table table-striped table-bordered table-tracking table-text-nowrap" id="'+id+'">' +
                              '<thead class="thead-light">' +
                                '<tr class="small text-center align-middle">' +
                                  thead +
                                '</tr>' +
                              '</thead>' +
                              '<tbody>' +
                                  tbody +
                              '</tbody>' +
                            '</table>' +
                          '</div>' +
                        '</div>';
        
            tbody = ''
        })
        $('#tracking-so-loop').append(div);
    }, 500);
}, 500);
}


// ------------------------------
// VERTICAL TIMELINE LAYOUT
// ------------------------------
var vertical_timeline = function(collect_data, modal_target, ajax_url, error_tracking) {
    $(modal_target).find('.vertical-timeline').html('')

    var xhr = $.ajax({
        type        : ajax_method_POST,
        url         : ajax_url,
        data        : collect_data,
        dataType    : 'json',
        contentType : 'application/json; charset=utf-8',
        success     : function (resp) {
            if (resp.is_valid === true) {
                $(modal_target).modal('show');
                var total_data = resp.data.length

                for (var i=0; i<total_data; i++) {
          
                    var resp_data = {
                        id      : resp.data[i].id,
                        city    : resp.data[i].city,
                        status  : resp.data[i].status,
                        date    : resp.data[i].date,
                    };

                    $(modal_target)
                      .find('.vertical-timeline')
                      .append('<li>'+
                        '<div class="circle">'+
                          '<i class="fa fa-check icon-check"></i>' +
                          '<div class="item-left">'+resp_data.city+'</div>' +
                          '<div class="item-right">'+
                            '<span class="title">'+resp_data.status+'</span>' +
                            '<span class="subtitle">'+resp_data.date+'</span>' +
                          '</div>' +
                        '</div>' +
                      '</li>');
          
                    setTimeout(() => {
                        $(modal_target)
                          .find('.vertical-timeline li').popover('show');
                }, 500);
            }
        } else {
          Swal.fire({
        type  : 'error',
        html  : error_tracking
});
}
}
});
}
