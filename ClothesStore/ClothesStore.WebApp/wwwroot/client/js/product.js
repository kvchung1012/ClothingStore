var filter = {
    colors: [],
    brands: [],
    sort: 0,
    search: "",
    categoryId : 0
}
function Search(el) {
    filter.search = $(el).val();
    LoadProduct(filter);
}

function Sort(el, Id) {
    $('.sort-product').removeClass('filter-link-active');
    $(el).addClass('filter-link-active')
    filter.sort = Id;
    LoadProduct(filter);
}

function ChooseCate(Id) {
    filter.categoryId = Id;
    LoadProduct(filter);
}

function ChooseBrand(el,Id) {
    if (filter.brands.filter(x => x == Id).length == 0) {
        filter.brands.push(Id);
        $(el).addClass('active')
        LoadProduct(filter);
    }
    else {
        $(el).removeClass('active')
        let index = filter.brands.indexOf(Id);
        filter.brands.splice(index, 1);
        LoadProduct(filter);
    }
    
}

function ChooseColor(el, Id) {
    if (filter.colors.filter(x => x == Id).length == 0) {
        filter.colors.push(Id);
        $(el).children('a').addClass('active')
        LoadProduct(filter);
    }
    else {
        $(el).children('a').removeClass('active')
        let index = filter.colors.indexOf(Id);
        filter.colors.splice(index, 1);
        LoadProduct(filter);
    }
}
// get product by filter
function LoadProduct(fitler) {
    $.ajax({
        url: '/Home/GetProduct',
        dataType: 'html',
        type: 'post',
        data: {
            filter: filter
        },
        success: function (res) {
            $('.list-product').html('');
            $('.list-product').append(res);
        }
    })
}

function ProductDetail(Id) {
    $.ajax({
        url: '/Product/GetInforProduct',
        dataType: 'html',
        type: 'post',
        data: {
            Id: Id
        },
        success: function (res) {
            $('.main-product').html('');
            $('.main-product').append(res);
            SetSlick();
            $('.wrap-modal1').addClass('show-modal1');
        }
    })
}

function SetSlick() {
    //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    //    var nameTab = $(e.target).attr('href');
    //    $(nameTab).find('.slick2').slick('reinit');
    //});

    /*==================================================================
    [ Slick3 ]*/
    $('.wrap-slick3').each(function () {
        $(this).find('.slick3').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            fade: true,
            infinite: true,
            autoplay: false,
            autoplaySpeed: 6000,

            arrows: true,
            appendArrows: $(this).find('.wrap-slick3-arrows'),
            prevArrow: '<button class="arrow-slick3 prev-slick3"><i class="fa fa-angle-left" aria-hidden="true"></i></button>',
            nextArrow: '<button class="arrow-slick3 next-slick3"><i class="fa fa-angle-right" aria-hidden="true"></i></button>',

            dots: true,
            appendDots: $(this).find('.wrap-slick3-dots'),
            dotsClass: 'slick3-dots',
            customPaging: function (slick, index) {
                var portrait = $(slick.$slides[index]).data('thumb');
                return '<img src=" ' + portrait + ' "/><div class="slick3-dot-overlay"></div>';
            },
        });
    });

}

function ChooseSize(el) {
    let sizeId = $(el).val();
    let productId = $('#Id').val();
    if (sizeId > 0) {
        $.ajax({
            url: '/Product/GetColor',
            dataType: 'json',
            type: 'post',
            data: {
                productId: productId,
                sizeId: sizeId
            },
            success: function (res) {
                console.log(res);
                let row = `<option value="0" data-price="0">Chọn màu sản phẩm</option>`;
                $(res).each(function (index, item) {
                    row += `<option value="${item.color.id}" data-stock="${item.config.stock}" data-id="${item.config.id}" data-price="${item.config.price}" data-color="${item.color.name}">${item.color.name} (Số lượng : ${item.config.stock})</option>`
                })
                $('.choose-color').html('');
                $('.choose-color').append(row);
            }
        })
    }
}

function SetPrice(el) {
    let price = $(el).children('option:selected').attr('data-price');
    if (price != "0") {
        $('.item_price').html('');
        $('.item_price').append(price);
    }
}

function SubStock() {
    let stock = $('input[name=Stock]').val();
    if (stock > 0) {
        stock--;
        $('input[name=Stock]').val(stock);
    }
    else {
        alert("Bạn cần chọn số lượng sản phẩm lón hơn 0")
    }
}

function AddStock() {
    let stock = $('input[name=Stock]').val();
    let max = $('.choose-color').children('option:selected').attr('data-stock');
    if (max == undefined) {
        alert("Bạn cần chọn đủ size và màu của sản phẩm");
        return;
    }
    if (stock < max) {
        stock++;
        $('input[name=Stock]').val(stock);
    }
    else {
        alert("Bạn chỉ được mua tối đa " + max + " sản phẩm");
    }
}

function AddCart() {
    // order detail
    // img, name,price,size,color,
    if ($('.choose-color').val() > 0 && $('input[name=Stock]').val() > 0) {
        let img = $('#Image').val();
        let productName = $('.product-name').text();
        let price = $('.choose-color').children('option:selected').attr('data-price');
        let size = $('.product-size').children('option:selected').text();
        let color = $('.choose-color').children('option:selected').attr('data-color');
        let configId = $('.choose-color').children('option:selected').attr('data-id');
        let stockMax = $('.choose-color').children('option:selected').attr('data-stock');
        let stock = $('input[name=Stock]').val();
        let obj = {
            img: img,
            productName: productName,
            price: price,
            size: size,
            color: color,
            configId: configId,
            stock: stock,
            stockMax : stockMax
        };
        let cartStr = localStorage.getItem("Cart");
        if (cartStr == null)
            cartStr = JSON.stringify([]);
        let cartJson = JSON.parse(cartStr);
        let check = cartJson.filter(x => x.configId == obj.configId).length > 0;
        if (!check) {
            cartJson.push(obj);
            localStorage.setItem("Cart", JSON.stringify(cartJson));
            alert("Thêm giỏ hàng thành công !!!");
            $('.icon-header-item').attr('data-notify', cartJson.length);
        }
        else {
            alert("Đã có trong giỏ hàng !!!");
        }
    }
    else {
        alert("Vui lòng chọn size, màu sản phẩm và số lượng")
    }
}

function ShowCart() {
    let cartStr = localStorage.getItem("Cart");
    if (cartStr == null)
        cartStr = JSON.stringify([]);
    let cartJson = JSON.parse(cartStr);

    let totalPrice = 0;
    $('.header-cart-wrapitem').html('');
    $(cartJson).each(function (i, e) {
        totalPrice += e.stock * e.price;
        let row = `<li class="header-cart-item flex-w flex-t m-b-12">
                    <div class="header-cart-item-img">
                        <img src="${e.img}" alt="IMG">
                    </div>

                    <div class="header-cart-item-txt">
                        <a href="#" class="header-cart-item-name hov-cl1 trans-04">
                            ${e.productName}
                        </a>
                         <a class="header-cart-item-name hov-cl1 trans-04">
                            Size: ${e.size}, Color :  ${e.color}
                         </a>
                        <span class="header-cart-item-info">
                            ${e.stock} x ${e.price} VNĐ
                        </span>
                    </div>
                </li>`;
        $('.header-cart-wrapitem').append(row);
    })
    $('.wrap-header-cart').addClass('show-header-cart')
    $('.header-cart-total').text("Tổng hóa đơn: "+totalPrice+" VNĐ");
}