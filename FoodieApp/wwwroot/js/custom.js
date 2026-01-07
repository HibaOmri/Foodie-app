// to get current year
// getYear function removed as it was crashing (element #displayYear missing) and unnecessary (handled by Razor)


// isotope js
$(window).on('load', function () {
    var $grid = $(".grid").isotope({
        itemSelector: ".all",
        percentPosition: false,
        masonry: {
            columnWidth: ".all"
        }
    });

    $('.filters_menu li').click(function () {
        $('.filters_menu li').removeClass('active');
        $(this).addClass('active');

        var data = $(this).attr('data-filter');
        $grid.isotope({
            filter: data
        });
    });
});

// Smooth scrolling for "Voir le menu" and other anchor links
$(document).ready(function () {
    $('a[href^="#"]').on('click', function (event) {
        var target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 100 // Offset for header
            }, 1000);
        }
    });
});

// nice select
$(document).ready(function () {
    $('select').niceSelect();
});

/** google_map js **/
function myMap() {
    var mapProp = {
        center: new google.maps.LatLng(40.712775, -74.005973),
        zoom: 18,
    };
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
}

// client section owl carousel
$(".client_owl-carousel").owlCarousel({
    loop: true,
    margin: 0,
    dots: false,
    nav: true,
    navText: [],
    autoplay: true,
    autoplayHoverPause: true,
    navText: [
        '<i class="fa fa-angle-left" aria-hidden="true"></i>',
        '<i class="fa fa-angle-right" aria-hidden="true"></i>'
    ],
    responsive: {
        0: {
            items: 1
        },
        768: {
            items: 2
        },
        1000: {
            items: 2
        }
    }
});

// AJAX Add to Cart
$(document).on('click', '.add-to-cart-link', function (e) {
    e.preventDefault(); // Prevent default navigation
    var url = $(this).attr('href');

    $.ajax({
        url: url,
        type: 'GET', // or POST if your controller expects POST
        headers: { "X-Requested-With": "XMLHttpRequest" },
        success: function (response) {
            if (response.success) {
                // Create and show a toast/alert
                var alertHtml = '<div class="alert alert-success alert-dismissible fade show fixed-top m-3" style="z-index: 9999;" role="alert">' +
                    '<strong>Succès !</strong> ' + response.message +
                    '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                    '<span aria-hidden="true">&times;</span>' +
                    '</button>' +
                    '</div>';
                $('body').append(alertHtml);

                // Auto dismiss after 3 seconds
                setTimeout(function () {
                    $('.alert').alert('close');
                }, 3000);
            }
        },
        error: function () {
            alert('Une erreur est survenue lors de l\'ajout au panier.');
        }
    });
});