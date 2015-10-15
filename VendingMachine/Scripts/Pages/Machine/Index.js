$(document).ready(function () {
    $('a').click(function () {
        var productId = $(this).data('id');
        $.ajax({
            method: 'POST',
            url: 'Machine/Buy',
            data: {productId:productId},
            success: function (data, textStatus, jqXHR) {
                if (data.status == 'ok') {
                    alert('ok');
                } else {
                    alert(data.message);

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('error');
            }
    });
});
});