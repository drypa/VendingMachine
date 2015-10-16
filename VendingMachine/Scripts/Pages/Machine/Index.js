(function ($, document,VM) {
    $(document).ready(function () {
        $('[data-id]').click(function () {
            var productId = $(this).data('id');
            $.ajax({
                method: 'POST',
                url: VM.BuyUrl,
                data: { productId: productId },
                success: function (data, textStatus, jqXHR) {
                    if (data.status == 'ok') {
                        window.location = window.location;
                    } else {
                        alert(data.message);

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('error');
                }
            });
            return false;
        });

        $('[data-nominal]').click(function () {
            var nominal = $(this).data('nominal');
            $.ajax({
                method: 'POST',
                url: VM.InsertCoin,
                data: { nominal: nominal },
                success: function (data, textStatus, jqXHR) {
                    if (data.status == 'ok') {
                        window.location = window.location;
                    } else {
                        alert(data.message);

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('textStatus');
                }
            });
            return false;
        });

    });
})(jQuery, document, window.VM);
