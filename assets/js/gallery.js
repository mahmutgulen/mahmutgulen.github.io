var lastBackgroundColor = "";
var lastClickBox = 0;
var isFullScreen = true;

var downloadUrl = "";
var itemQuantity = 5;
var page = 1;

$(document).ready(function () {
    loadScreen();
    loadBackgroundColors();
});

function loadScreen() {
    scrollToControlRange();
    var onBtn = $('.closeScreen .onBtn');
    onBtn.css('display', 'block');
    onBtn.animate({
        'opacity': '1',
    }, 500);
}

function btnScreenOn() {
    var closeScreen = $('.closeScreen');
    var input = $('.closeScreen input');
    var onBtn = $('.closeScreen .onBtn');

    if (input.val().length && Number(input.val()) != 0) {

        //sayıyı al isteğe çık
        if (input.val() > 100) {
            input.val('100');
        }
        itemQuantity = Number(input.val());
        dataSeeding(itemQuantity, page);

        //iş yükünü kontrol et, yüklenene kadar açma
        if (true) {
            closeScreen.animate({
                'opacity': '0',
                'z-index': '-1'
            }, 500);

            onBtn.animate({
                'opacity': '0',
            }, 500);
        }
    }
    else {
        alert('Görüntülmek istediğiniz resim sayısını girin.');
    }
}

function btnScreenOff() {
    var closeScreen = $('.closeScreen');
    var onBtn = $('.closeScreen .onBtn');
    closeScreen.css("z-index", 2);

    closeScreen.animate({
        "opacity": "1"
    }, 500);

    onBtn.animate({
        'opacity': '1',
    }, 500);

    $('.galleryContainer').html('');
    $('.closeScreen input').val('');
}



function btnShuffle() {
    var galleryContainer = $('.galleryContainer');
    var galleryBox = galleryContainer.children('.galleryBox');

    galleryBox.fadeOut(200, function () {
        galleryBox.sort(function () {
            return Math.random() - 0.5;
        });
        galleryContainer.html(galleryBox);
        galleryBox.fadeIn(200);
    });
}

function btnRefresh() {
    var galleryContainer = $('.galleryContainer');
    //Drain Container 
    galleryContainer.html('');
    //Data Load
    dataSeeding(itemQuantity, Math.floor(Math.random() * 10) + 1);
}

function dataSeeding(itemQuantity, page) {

    $('.loadingIcon').show();

    getImages(itemQuantity, page)
        .then(function (response) {

            var imagesLoaded = 0;
            var totalImages = response.length;

            $.each(response, function (index, item) {

                var imgElement = $('<img>', {
                    src: item.download_url,
                    alt: item.author,
                    title: item.author,
                    loading: 'lazy',
                    'data-download': item.url
                }).on('load', function () {
                    imagesLoaded++;
                    if (imagesLoaded === totalImages) {
                        $('.loadingIcon').hide();
                    }
                });


                $('.galleryContainer').append(`<div class="galleryBox" data-id="${index}" onclick="showThis($(this))"></div>`).find('div:last').append(imgElement);
            });

        })
        .catch(function (error) {
            console.error('Oppps. Bir Sorun Oluşmuş :/', error);
            $('.loadingIcon').hide();
        });
}


// function getImages(itemQuantity, page) {
//     return new Promise((resolve, reject) => {
//         $.ajax({
//             url: `https://picsum.photos/v2/list?page=${page}&limit=${itemQuantity}`,
//             type: 'GET',
//             dataType: 'json',
//             success: function (response) {
//                 resolve(response);  // Başarılı olduğunda yanıtı resolve et
//             },
//             error: function (xhr, status, error) {
//                 console.error('Hata:', error);
//                 alert('API isteğinde bir hata oluştu!');
//                 reject(error);  // Hata durumunda reject et
//             }
//         });
//     });
// }

function getImages(itemQuantity, page) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: `https://picsum.photos/v2/list?page=${page}&limit=${itemQuantity}`,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                // Resimlerin boyutlarını küçültmek için URL'leri yeniden düzenle
                const resizedImages = response.map(image => ({
                    ...image,
                    url: image.download_url,
                    download_url: `${image.download_url.split('/id/')[0]}/id/${image.id}/200/200`
                }));
                resolve(resizedImages);
            },
            error: function (xhr, status, error) {
                console.error('Hata:', error);
                alert('API isteğinde bir hata oluştu!');
                reject(error);
            }
        });
    });
}



function scrollToControlRange() {
    //scroll to range
    // const $scrollableDiv = $(".galleryContainer");
    const $scrollableDiv = $(".galleryContainer");
    const $scrollSlider = $("#scroll-slider");

    // Scrollable div'in maksimum scroll yüksekliğini belirle
    const maxScrollHeight = $scrollableDiv[0].scrollHeight - $scrollableDiv.height();

    // Slider'ın "input" olayı
    $scrollSlider.on("input", function () {
        const scrollPercentage = $(this).val(); // Slider'daki değer
        const scrollPosition = (maxScrollHeight * scrollPercentage) / 100; // Scroll pozisyonunu hesapla

        $scrollableDiv.scrollTop(scrollPosition); // Div'i kaydır
    });

    // Scrollable div'de bir scroll olayı olursa slider'ı güncelle
    $scrollableDiv.on("scroll", function () {
        const scrollPercentage = ($(this).scrollTop() / maxScrollHeight) * 100;
        $scrollSlider.val(scrollPercentage);
    });
}

function loadBackgroundColors() {
    var colors = [
        { "id": 1, "code": "#ff4d00" },
        { "id": 2, "code": "#00325b" },
        { "id": 3, "code": "#8B0000" },
        { "id": 4, "code": "#008000" }
    ];
    var backgroundColors = $('.backgroundColors');

    $.each(colors, function (index, item) {
        backgroundColors.append(`
              <div class="colorItem" style="background-color: ${item.code};" onclick="changeBackgroundColorButton($(this).css('background-color'))"></div>
            `);
    });

    backgroundColors.append(`
        <div class="colorItem randomColor" style="background-color: #fff;" onclick="changeRandomColorButton()"></div>
      `);
}

function changeBackgroundColorButton(color) {
    $('body').css({
        'background-image': `linear-gradient(179deg, rgba(0, 0, 0, 1) 9.2%, ${color} 103.9%)`
    });

    lastBackgroundColor = `linear-gradient(179deg, rgba(0, 0, 0, 1) 9.2%, ${color} 103.9%)`;
}

function changeRandomColorButton() {
    $('body').css({
        'background-image': `linear-gradient(179deg, rgba(0, 0, 0, 1) 9.2%, ${getRandomColor()} 103.9%)`
    });
    lastBackgroundColor = `linear-gradient(179deg, rgba(0, 0, 0, 1) 9.2%, ${getRandomColor()} 103.9%)`;
}

function getRandomColor() {
    let r = Math.floor(Math.random() * 256);
    let g = Math.floor(Math.random() * 256);
    let b = Math.floor(Math.random() * 256);
    return `rgb(${r}, ${g}, ${b})`;
}

function showThis(item) {
    lastClickBox = Number(item.attr('data-id'));

    //fade out buttons
    $('header').fadeOut(300);
    $('#scroll-slider').fadeOut(300);
    $('.backgroundColors').fadeOut(300);
    $('.galleryContainer').fadeOut(300);

    //fade in buttons
    $('.prevButton').fadeIn(300);
    $('.nextButton').fadeIn(300);
    $('.closeBigImageBoxButton').fadeIn(300);
    $('.screenSizeButtons').fadeIn(300);
    $('.downloadButton').fadeIn(300);

    var bigImageBox = $('.bigImageBox');

    bigImageBox.animate({
        "width": "75%",
        "height": "600px"
    }, 500);

    downloadUrl = item.children('img').attr('data-download');

    bigImageBox.append(`
        <image src="${resizeImage(item.children('img').attr('src'))}" title="${item.children('img').attr('title')}" alt="${item.children('img').attr('alt')}"/>
        `);


    $('body').css({
        'background-image': 'unset',
        'background-color': '#000'
    }, 1000);
}

function closeBigImageBoxButton() {
    isFullScreen = true;
    //fade in buttons
    $('header').fadeIn(300);
    $('#scroll-slider').fadeIn(300);
    $('.backgroundColors').fadeIn(300);
    $('.galleryContainer').fadeIn(300);

    //fade out buttons
    $('.prevButton').fadeOut(300);
    $('.nextButton').fadeOut(300);
    $('.closeBigImageBoxButton').fadeOut(300);
    $('.screenSizeButtons').fadeOut(300);
    $('.downloadButton').fadeOut(300);

    var bigImageBox = $('.bigImageBox');

    bigImageBox.animate({
        "width": "0",
        "height": "0"
    }, 500);

    bigImageBox.children('img').animate({
        "opacity": "0",
    }, 500);


    setTimeout(() => {
        bigImageBox.html(`
            <div class="prevButton" onclick="prevButton()">
                <i class="fa-solid fa-chevron-left"></i>
            </div>
            <div class="nextButton" onclick="nextButton()">
                <i class="fa-solid fa-chevron-right"></i>
            </div>
        `);
    }, 500);


    $('body').css({
        'background-image': lastBackgroundColor
    }, 1000);

}

function nextButton() {
    if ($('.galleryBox').length - 1 != lastClickBox) {
        var nextImage = $(`[data-id="${lastClickBox + 1}"]`).children('img').attr('src');

        $('.bigImageBox img').attr('src', resizeImage(nextImage));

        lastClickBox = lastClickBox + 1;

    }
}

function prevButton() {
    if (lastClickBox > 0) {
        var prevImage = $(`[data-id="${lastClickBox - 1}"]`).children('img').attr('src');
        $('.bigImageBox img').attr('src', resizeImage(prevImage));
        lastClickBox = lastClickBox - 1;

    }
}

function btnScreenSizeToggle() {
    var screenSizeButtons = $('.screenSizeButtons');
    var img = $('.bigImageBox img');
    if (isFullScreen) {
        screenSizeButtons.children('i').attr('class', 'fa-solid fa-expand');
        isFullScreen = false;
        img.css('object-fit', 'contain');
    }
    else {
        screenSizeButtons.children('i').attr('class', 'fa-solid fa-compress');
        isFullScreen = true;
        img.css('object-fit', 'cover');
    }
}

function downloadThis() {
    var downloadLink = document.createElement('a');
    downloadLink.href = downloadUrl;
    downloadLink.download = 'download.jpg';
    downloadLink.target = '_blank';
    downloadLink.click();
}


function resizeImage(src) {
    return src.replace(/\/\d+\/\d+$/, '/1000/1000');
}