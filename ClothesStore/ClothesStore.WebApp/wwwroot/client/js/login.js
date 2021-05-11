
function Login() {
    let data = $('.login').serialize();
    $.ajax({
        url: '/Admin/Admin/Login',
        type: 'post',
        dataType: 'json',
        data: data,
        beforeSend: function () {
            $('body').append(`<div class="loader"></div>`);
            $('body').addClass('overlayout');
        },
        success: function (res) {
            if (res.status == true) {
                if (res.isAdmin == null || res.isAdmin == false)
                    location.href = '/';
                else
                    location.href = '/Admin/Home/Index';
            }
            else {
                swal("Fail!", "Email or password is incorrect!", "warning");
            }
        },
        complete: function () {
            $('.loader').remove();
            $('body').removeClass('overlayout');
        }
    })
}


function GetView(url) {
    $.ajax({
        url: url,
        type: 'get',
        beforeSend: function () {
            Loading();
        },
        success: function () {

        },
        error: function () {

        },
        complete: function () {
            CompleteLoading();
        }
    })
}

function Loading() {
    $('#overlay').addClass("show");
    $('#overlay').removeClass("hide");
}

function CompleteLoading() {
    $('#overlay').removeClass("show");
    $('#overlay').addClass("hide");
}
