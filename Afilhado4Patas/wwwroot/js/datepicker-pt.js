jQuery(function ($) {
    $.datepicker.regional['pt'] = {
        
        weekHeader: "Sem",
        dateFormat: "dd/mm/yy",
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    };
    $.datepicker.setDefaults($.datepicker.regional['pt']);
});