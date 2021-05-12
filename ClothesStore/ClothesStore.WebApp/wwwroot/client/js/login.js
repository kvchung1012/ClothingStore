
function Login() {
    let data = $('.login').serialize();
    $.ajax({
        url: '/Admin/Admin/Login',
        type: 'post',
        dataType: 'json',
        cache: false,
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

function ChangePass(url) {
   /* debugger;*/
    let data = $('.changepass').serializeArray();
    var changePass = {};

    jQuery.map(data, function (value, index) {
        changePass[value.name] = [value.value];
    });


    if (JSON.stringify(changePass.NewPassword) !== JSON.stringify(changePass.ConfirmedPassword)) {
        swal("Fail", "New password must be the same!", "warning");
    } else {
        swal({
            title: "Are you sure with above infomation?",
            text: "",
            icon: "warning",
            buttons: true,
            dangerMode: false,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: '/Admin/Admin/ChangePassword',
                        type: 'post',
                        dataType: 'json',
                        cache:false,
                        data: changePass,
                        beforeSend: function () {

                        },
                        success: function (res) {
                            if (res) {
                                swal(`Your account is changed password!`, {
                                    icon: "success",
                                }).then((value) => {
                                    location.href = '/User/Login';
                                });

                            } else {
                                swal("Faild", "OldPassword is incorrect!", "error");
                            }
                        }
                    })

                } else {
                    swal("Failed!");
                }
            });
    }
}


function ForgotPassword() {
    debugger
    let data = $('.forgotpassword').serialize();
    $.ajax({
        url: '/User/ForgotPassword',
        type: 'post',
        dataType: 'json',
        data: data,
        beforeSend: function () {
            Loading();
        },
        success: function (res) {
            if (res == true) {
                swal("SUCCESS!", "An email have just sent to you. Please check it!", {
                    icon: "success",
                }).then((value) => {
                    location.href = '/User/Login';
                });
            }
            else {
                swal("Oops", "Something went wrong!", "error");
            }
        },
        complete: function () {
            CompleteLoading();
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

function Register() {
   
    let data = $('.register').serializeArray();
    var registerObj = {};

    jQuery.map(data, function (value, index) {
        registerObj[value.name] = [value.value];
    });


    //chua validate

    if (JSON.stringify(registerObj.Password) !== JSON.stringify(registerObj.ConfirmedPassword)) {
        swal("Fail", "Password must be the same!", "warning");
    } else {
        swal({
            title: "Are you sure with above infomation?",
            text: "",
            icon: "warning",
            buttons: true,
            dangerMode: false,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: '/Admin/Admin/Register',
                        type: 'post',
                        dataType: 'json',
                        data: registerObj,
                        beforeSend: function () {
                            swal("Oops!", "Are you sure with your infomation?", "question", {
                                button: "Yes",
                            });
                                },
                        success: function (res) {
                            if (res) {
                                swal(`Hello ${registerObj.Name}! Your account is registered!`, {
                                    icon: "success",
                                }).then((value) => {
                                    location.href = '/User/Login';
                                });
                            } else {
                                swal("OOps", "Something went wrong", "error");
                            }
                        }
                    })

                } else {
                    swal("Failed!");
                }
            });
    }
}


function Loading() {
    if ($('#overlay').hasClass("hide")) {
        $('#overlay').removeClass("hide");
    }
    $('#overlay').addClass("show");
}

function CompleteLoading() {
    if ($('#overlay').hasClass("show")) {
        $('#overlay').removeClass("show");
    }
    $('#overlay').addClass("hide");
}
