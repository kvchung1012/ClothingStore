// khai báo cho các biến dùng chung
// các url
var urlGetData = '';

// các biến để getData
var submitData = {
    listFilter: [],
    pageSize: 20,
    pageNumber: 1,
    orderBy: "",
    isAsc: true
}
// định nghĩa các function hiển thị dữ liệu

function Search(el) {
    let filter = [];
    $('#FormSearch input').each(function () {
        let key = $(this).attr('name');
        let value = $(this).val();
        if (value !== '') {
            filter.push({ key: key, value: value })
        }
    })

    $('#FormSearch select').each(function () {
        let key = $(this).attr('name');
        let value = $(this).val();
        if (value != 0) {
            filter.push({ key: key, value: value })
        }
    })

    submitData.listFilter = filter;
    GetDataToTable(urlGetData)
}

function CancelSearch() {
    $('#FormSearch input').each(function () {
        $(this).val('');
    })
    submitData.listFilter = [];
    GetDataToTable(urlGetData)
    $('#FormSearch').collapse('hide');
}

function ChangePageSize() {
    $('.pageSize').on('change', function () {
        let val = $(this).val();
        submitData.pageSize = val;
        GetDataToTable(urlGetData)
    })
}

function ChangePageNumber() {
    $('.pagination li a').on('click', function (e) {
        let val = $(this).attr('data-index');
        submitData.pageNumber = val;
        GetDataToTable(urlGetData)
    })
}

function SortByKey() {
    $('thead tr th').click(function () {
        let key = $(this).attr('data-name');
        let orderBy = $(this).attr('data-order');
        if (key == orderBy) {
            let sort = $(this).attr('data-sort');
            if (sort == "True") { // sắp xếp giảm
                sort = false;
                $(this).attr('data-sort', "False")
            }
            else if (sort == "False") {  // reset
                key = "";
            }
            submitData.orderBy = key;
            submitData.isAsc = sort;
            GetDataToTable(urlGetData);
        }
        else {  // sắp xếp tăng
            let sort = true;
            submitData.orderBy = key;
            submitData.isAsc = sort;
            GetDataToTable(urlGetData);
        }

    });
}
// get data from server 
function GetDataToTable(url) {
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'html',
        data: {
            requestData: submitData
        },
        beforeSend: function () {
            $('body').append(`<div class="loader"></div>`);
            $('body').addClass('overlayout');
        },
        success: function (res) {
            $('.main-center-table').html('');
            $('.main-center-table').append(res);
        },
        error: function (error) {
            console.log("error");
            console.log(error);
        },
        complete: function () {
            ChangePageNumber();
            SortByKey();
            $('.loader').remove();
            $('body').removeClass('overlayout');
        }
    })
}

function GetFormAddOrEdit(url, Id) {
    debugger;
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'html',
        data: {
            Id: Id
        },
        beforeSend: function () {
        },
        success: function (res) {
            $('.modal-add .modal-body').html('');
            $('.modal-add .modal-body').append(res);
            console.log(res);
            $('.btn-save').html('');
            $('.btn-save').removeClass('d-none');
            if (Id == 0) {
                $('.btn-save').append('<i class="fa fa-spinner fa-spin d-none"></i><i class="bi bi-check2 d-none"></i >Thêm mới');
            }
            else {
                $('.btn-save').append('<i class="fa fa-spinner fa-spin d-none"></i><i class="bi bi-check2 d-none"></i >Cập nhật');
            }
            $('.modal-add').modal('show');
        },
        error: function (error) {
            console.log("error");
            console.log(error);
        },
        complete: function () {
            ChangePageNumber();
            SortByKey();
            ConvertToSlug();
            $('.loader').remove();
            $('body').removeClass('overlayout');
        }
    })
}

// post record
function AddOrUpdate(el,url) {
    if (ValidationInput()) {  // kiểm tra dữ liệu đầu vào
        // read data in form
        let formArray = $('form.form-input').serializeArray();
        var object = {};
        jQuery.map(formArray, function (n, i) {
            object[n.name] = n.value;
        });
        $('.form-input textarea').each(function (index, item) {
            if ($(this).hasClass('ckeditor') || $(this).hasClass('ckeditor')) {
                let id = $(this).attr('id');
                object[$(this).attr('name')] = CKEDITOR.instances[id].getData();
            }
        })
        swal({
            title: $('#Id').val() == 0 ? "Bạn có chắc chắn muốn thêm" : "Bạn có chắc muốn sửa dữ liệu này?",
            text: $('#Id').val() == 0 ? "" : "Sau khi sửa thì không thể quay lại!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                // submit data
                $.ajax({
                    url: url,
                    type: 'post',
                    dataType: 'json',
                    data: {
                        obj: object
                    },
                    beforeSend: function () {
                        $(el).addClass('buttonload');
                    },
                    success: function (res) {
                        if (res.success) {
                            $(el).children('.bi-check2').removeClass('d-none')
                            GetDataToTable(urlGetData)
                        }
                        else {
                            if (res.error != null) {
                                $('.help-block').css('display', 'inline-block');
                                res.error.map(x => {
                                    $('.form-input input[name=' + x.key + ']').siblings('.help-block').html('');
                                    $('.form-input input[name=' + x.key + ']').siblings('.help-block').append(x.value);
                                })
                            }
                        }
                    },
                    error: function (error) {

                    },
                    complete: function () {
                        $(el).removeClass('buttonload');
                    }
                })
            }
        })
    }
}

//get record
function GetObjectById(url, id) {
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'json',
        data: {
            id: id
        },
        success: function (res) {
            $.each(res, function (key, value) {
                key = key[0].toUpperCase() + key.slice(1, key.length);   // lấy ra key object
                let elName = ".form-input " + "#" + key   // lấy ra string el
                let el = $(elName); // get element by key
                if (el != null && el != undefined && el.length > 0) {
                    let tagName = el.prop('tagName').toLowerCase();  // get tag name element
                    if (tagName == 'input') {
                        let type = el.prop('type');  // type of input
                        if (type == 'text' || type == 'number' || type == 'hidden' || type == 'email') {
                            el.val(value);
                            if (key == "Image") {
                                el.siblings('img').attr('src', value)
                            }
                        }
                        else if (type == 'radio') {
                            let obj = ".form-input " + "input[name=" + key + "][value=" + value + "]"
                            let radio = $(obj);
                            radio.prop('checked', true);
                        }
                        else if (type == 'date') {
                            let date = value.slice(0, 10);
                            el.val(date);
                        }
                    }
                    else if (tagName == 'select') {
                        el.val(value);
                        if (el.hasClass('select2')) {
                            el.val(value).select2();
                        }
                    }
                    else if (tagName == 'textarea') {
                        if (el.hasClass('ckeditor') || el.hasClass('p-ckeditor')) {
                            CKEDITOR.instances[key].setData(value);
                        }
                        else {
                            el.val(value);
                        }
                    }
                }

            })
        },
        error: function (error) {
            console.log(error)
        },
        complete: function () {
        }
    })
}

// view detail
function GetViewDetail(url, id) {
    debugger;
    $.ajax({
        url: url,
        type: 'post',
        dataType: 'html',
        data: {
            id: id
        },
        success: function (res) {
            debugger;
            $('.modal-add .modal-body').html('');
            $('.modal-add .modal-body').append(res);
            $('.btn-save').addClass('d-none');
            $('.modal-add').modal('show');
        },
        error: function (error) {
            console.log(error)
        }
    })
}

// delete record
function DeleteById(url, id) {
    swal({
        title: "Xóa dữ liệu này?",
        text: "Dữ liệu xóa sẽ không thể khôi phục!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: 'post',
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (res) {
                    if (res) {
                        swal("Thành công!", "Bạn vừa xóa dữ liệu thành công!", "success");
                        GetDataToTable(urlGetData)
                    }
                    else {
                        alert("Đã có lỗi xảy ra !");
                    }
                },
                error: function (error) {
                    console.log(error)
                }
            })
        }
    })
}

// xóa đi dữ liệu đang nhập trên form
function RefreshForm() {
    $('.text-danger').html('');
    $('.form-input input').each(function () {
        if ($(this).attr('type') == 'text' || $(this).attr('type') == 'date' || $(this).attr('type') == 'password' || $(this).attr('type') == 'email') {
            $(this).val("");
            if ($(this).attr('name') == "Image") {
                $(this).val('/Uploads/Image/no-image.jpg');
                $(this).siblings('img').attr('src', '/Uploads/Image/no-image.jpg');
            }
        }
        else if ($(this).attr('type') == 'number') {
            $(this).val(0);
        }
        else if ($(this).attr('type') == 'radio') {
            let name = $(this).attr('name');
            $("#" + name + "").prop('checked', true)
        }
    })

    $('.form-input select').each(function () {
        $(this).val(0);
        if ($(this).hasClass('select2')) {
            $(this).val(0).select2();
        }
    })
    $('.form-input textarea').each(function () {
        if ($(this).hasClass('ckeditor') || $(this).hasClass('p-ckeditor')) {
            let id = $(this).attr('id');
            CKEDITOR.instances[id].setData('')
        }
        else {
            $(this).val('')
        }
    })
}

// kiểm tra dữ liệu đầu vào
function ValidationInput() {
    let check = true;
    $('.help-block').html('');
    $('.help-block').css('display','none')
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

function DeleteImage(el) {
    $(el).parent().remove();
}

function ConvertToSlug() {
    $('input#Name').on('keyup', function () {
        console.log("ccc")
        let val = $(this).val();
        $('input#Slug').val(convertToSlug(val))

        function convertToSlug(str) {
            //Đổi chữ hoa thành chữ thường
            str = str.toLowerCase();
            str = str.trim();
            str = str.replace(/\s+/gi, '-')
            //Đổi ký tự có dấu thành không dấu
            str = str.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
            str = str.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
            str = str.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
            str = str.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
            str = str.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
            str = str.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
            str = str.replace(/đ/gi, 'd');
            str = str.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '-');
            str = str.replace(/\@\-|\-\@|\@/gi, '-');
            return str;
        }
    })
}

function validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
}


// choose main image
function OpenFolderImage() {
    $.ajax({
        url: "/Admin/Home/ReadFileBrower",
        data: {},
        type: "get",
        dataType: 'html',
        success: function (res) {
            $('#myModal .modal-body').html('');
            $('#myModal .modal-body').append(res);
            if (!$('#myModal').hasClass('show')) {
                $('#myModal').modal('show');
                ChooseImage();
            }
        }
    })
}

function ChooseImage() {
    $('#fileExploer').on('click', 'img', function () {
        debugger
        var fileUrl = '/Image/Upload/' + $(this).attr('title');
        $('#Image').val(fileUrl);
        $('#Image').siblings('img').attr('src', fileUrl);
        // ẩn modal đi
        $('#myModal').modal('hide');
    }).hover(function () {
        $(this).css('cursor', 'pointer')
    })
}


// add image to list
function OpenFolderImageList() {
    $.ajax({
        url: "/Admin/Home/ReadFileBrower",
        data: {},
        type: "get",
        dataType: 'html',
        success: function (res) {
            $('#myModal .modal-body').html('');
            $('#myModal .modal-body').append(res);
            if (!$('#myModal').hasClass('show')) {
                $('#myModal').modal('show');
                ChooseImageList();
            }
        }
    })
}

function ChooseImageList() {
    $('#fileExploer').on('click', 'img', function () {
        var fileUrl = '/Image/Upload/' + $(this).attr('title');

        let row = `<div class="image-item w-50">
                            <img src="${fileUrl}" alt="Alternate Text" class="w-100 border show-image" style="object-fit:contain" />
                            <button class="btn-danger" type="button" onclick="DeleteImage(this)"><i class="bi bi-trash"></i></button>
                        </div>`;
        $('.list-image > div:nth-child(1)').append(row);

        // ẩn modal đi
        $('#myModal').modal('hide');
    }).hover(function () {
        $(this).css('cursor', 'pointer')
    })
}

