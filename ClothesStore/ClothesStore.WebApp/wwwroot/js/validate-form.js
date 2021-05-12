

function ValidationInputVer2() {
    let check = true;
    $('.help-block').html('');
    $('.help-block').css('display', 'none')
    $('.form-input input').each(function () {
        if ($(this).hasClass('validation')) {
            let val = $(this).val();
            if (val == '') {
                let label = $(this).parent().siblings('label').text();
                $(this).siblings('.help-block').append(label + ' không được để trống');
                check = false;
            }
            else if ($(this).prop('type') == 'email') {
                if (!validateEmail(val)) {
                    let label = $(this).parent().siblings('label').text();
                    $(this).siblings('.help-block').append(label + ' không đúng định dạng');
                    check = false;
                }
            }
            else if ($(this).attr('name').includes('Phone')) {
                if (val.length < 10 || val.length > 11) {
                    let label = $(this).parent().siblings('label').text();
                    $(this).siblings('.help-block').append(label + ' không đúng định dạng');
                    check = false;
                }
            }
        }
    })
    $('.form-input select').each(function () {
        if ($(this).hasClass('validation')) {
            let val = $(this).val();
            if (val == 0) {
                let label = $(this).parent().siblings('label').text();
                $(this).siblings('.help-block').append(label + ' không được để trống');
                check = false;
            }
        }
    })

    $('.form-input textarea').each(function () {
        if ($(this).hasClass('validation')) {
            let val = $(this).val();
            if ($(this).hasClass('ckeditor') || $(this).hasClass('p-ckeditor')) {
                val = CKEDITOR.instances[$(this).attr('id')].getData();
            }
            if (val == '') {
                let label = $(this).parent().siblings('label').text();
                $(this).siblings('.help-block').append(label + ' không được để trống');
                check = false;
            }
        }
    })
    if (!check)
        $('.help-block').css('display', 'inline-block')
    return check;
}


function isVietnamesePhoneNumber(number) {
    return /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(number);
}

function validateUsername(name) {
    let pattern = /^[a-zA-Z0-9]+$/;
    return pattern.test(name);
}

function validatePassword(password) {
    let pattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/;
    return pattern.test(password);
}


function validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
}

