$(function () {
    $('#btn-addtocart').on('click', function (e) {
        e.preventDefault();
        console.log('add to cart');
        var data = $('#product-detail').serializeArray();
        var cartObj = {};

        jQuery.map(data, function (n, i) {
            cartObj[n.name] = n.value;
        });

        $('#ProductConfig select').each(function () {
            let key = $(this).attr('name');
            let value = $(this).find('option:selected').text() + '-' + $(this).find('option:selected').data('id');
            cartObj[key] = value;
        })
     
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'post',
            dataType: 'json',
            data: {
                CartItem: cartObj
            },
            success: function (res) {
               /* if (res.status == true) {
                    swal({
                        title: "Success",
                        text: "Add Successfully!",
                        icon: "success"
                    });
                }
                else {
                    swal({
                        title: "Failed",
                        text: "Something wrong!",
                        icon: "error"
                    });
                }*/
            }
        })

    });

})