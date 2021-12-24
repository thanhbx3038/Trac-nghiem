$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        if (this.name == "largeModal") {
            $('#myModalContentLarge').load(this.href, function () {
                $(":input").inputmask();
                // this is the first add
                $.validator.unobtrusive.parse($('form'));
                //định dạng cho kiểu nhập ngày tháng cho tất cả các kiểu ngày
                $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true, yearRange: '1950:2030' }, $.datepicker.regional["vi"]);

                $('#myModalLarge').modal({
                    /*backdrop: 'static',*/
                    keyboard: true
                }, 'show');
                bindLargeForm(this);
            });
        }else{
            $('#myModalContent').load(this.href, function () {
                $(":input").inputmask();
                // this is the first add
                $.validator.unobtrusive.parse($('form'));
                //định dạng cho kiểu nhập ngày tháng cho tất cả các kiểu ngày
                $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true, yearRange: '1950:2030' }, $.datepicker.regional["vi"]);

                $('#myModal').modal({
                    /*backdrop: 'static',*/
                    keyboard: true
                }, 'show');

                bindForm(this);
            });            
        }
        return false;
    });

});
function bindForm(dialog) {
    $('form', dialog).submit(function () {
        // this is the second addition
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#myModal').modal('hide');
                        location.reload();
                    } else {
                        $('#myModalContent').html(result);
                        bindForm();
                    }
                }
            });
            return false;
        }
    });
}
function bindLargeForm(dialog) {
    $('form', dialog).submit(function () {
        // this is the second addition
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        $('#myModalLarge').modal('hide');
                        location.reload();
                    } else {
                        $('#myModalContentLarge').html(result);
                        bindForm();
                    }
                }
            });
            return false;
        }
    });
}
function loadFormModal(href,idDestination) {
    $('#myModalContent').load(href, function () {
        // this is the first add
        $.validator.unobtrusive.parse($('form'));
        $(".datefield").datepicker({ dateFormat: 'dd/mm/yy', changeYear: true }, $.datepicker.regional["vi"]);
        $('#myModal').modal({
            /*backdrop: 'static',*/
            keyboard: true
        }, 'show');
        bindFormModal(this, idDestination);
    });
};
function bindFormModal(dialog,idDestination) {
    $('form', dialog).submit(function () {
        var data = $(this).serialize();
        // this is the second addition
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: data,
                success: function (result) {
                    $("#myModal").modal('hide');
                    //load lại bảng
                    $("#" + idDestination).html(result);
                }
            });
            return false;
        }
    });
}
