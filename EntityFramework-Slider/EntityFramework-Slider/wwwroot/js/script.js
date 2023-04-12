$(document).ready(function () {

    $(document).on("click", ".load-more", function () {

        let parent = $(".parent-products");
        //skipCount=htmlde olan clasin childrenlerin sayi,lazimdi her appen eliyende negeder skip=yani  buraxaq deye
        let skipCount = $(parent).children().length;

        let dataCount = $(parent).attr("data-count");
        //ajax-vasitesi ile request atiriq Urlere
        $.ajax({

            //urlde:-contoleri ve actionu yaziriq ve skip-adli varebla deyer gonderik,ordan gebul edib skip elesin deye
            url: `shop/loadmore?skip=${skipCount}`,
            //type:-datani gotururuk deye type=get
            type: "Get",

            //succsesden sonra hansi function islesin
            success: function (res) { 

                $(parent).append(res);

                //skipCount-burada cagirirsan cunku yuxarda caqirib methoda gonderirik,ama appenden sonra reqem deyisir deye bura yeniden cagirmali oluruq
                skipCount = $(parent).children().length;
                if (skipCount >= dataCount) {
                    $(".load-more").addClass("d-none")
                    $(".show-less").removeClass("d-none")
                }
            }
        })



    })

    $(document).on("click", ".show-less", function () {

       //show-lesse-basanda butun productlar itsin esas olanlar gorsensin
        let skipCount = 0;

        
        $.ajax({

            url: `shop/loadmore?skip=${skipCount}`,

            type: "Get",

        
            success: function (res) {

                //html-""-bos edirikki kohnenin ustune yazdirmayaq esaslari
                $(".parent-products").html("");
                $(".parent-products").append(res);
               
                    $(".load-more").removeClass("d-none")
                    $(".show-less").addClass("d-none")
                
            }
        })

    })

    $.ajax({
        url: `card/index`,

        type: "Get",

        success: function (res) {

            

        }

        })


    // HEADER

    $(document).on('click', '#search', function () {
        $(this).next().toggle();
    })

    $(document).on('click', '#mobile-navbar-close', function () {
        $(this).parent().removeClass("active");

    })
    $(document).on('click', '#mobile-navbar-show', function () {
        $('.mobile-navbar').addClass("active");

    })

    $(document).on('click', '.mobile-navbar ul li a', function () {
        if ($(this).children('i').hasClass('fa-caret-right')) {
            $(this).children('i').removeClass('fa-caret-right').addClass('fa-sort-down')
        }
        else {
            $(this).children('i').removeClass('fa-sort-down').addClass('fa-caret-right')
        }
        $(this).parent().next().slideToggle();
    })

    // SLIDER

    $(document).ready(function(){
        $(".slider").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });

    // PRODUCT

    $(document).on('click', '.categories', function(e)
    {
        e.preventDefault();
        $(this).next().next().slideToggle();
    })

    $(document).on('click', '.category li a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.product-item');
        
        products.each(function () {
            if(category == $(this).attr('data-id'))
            {
                $(this).parent().fadeIn();
            }
            else
            {
                $(this).parent().hide();
            }
        })
        if(category == 'all')
        {
            products.parent().fadeIn();
        }
    })

    // ACCORDION 

    $(document).on('click', '.question', function()
    {   
       $(this).siblings('.question').children('i').removeClass('fa-minus').addClass('fa-plus');
       $(this).siblings('.answer').not($(this).next()).slideUp();
       $(this).children('i').toggleClass('fa-plus').toggleClass('fa-minus');
       $(this).next().slideToggle();
       $(this).siblings('.active').removeClass('active');
       $(this).toggleClass('active');
    })

    // TAB

    $(document).on('click', 'ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().next().children('p.active').removeClass('active');

        $(this).parent().next().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    $(document).on('click', '.tab4 ul li', function()
    {   
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function()
        {
            if(dataId == $(this).attr('data-id'))
            {
                $(this).addClass('active')
            }
        })
    })

    // INSTAGRAM

    $(document).ready(function(){
        $(".instagram").owlCarousel(
            {
                items: 4,
                loop: true,
                autoplay: true,
                responsive:{
                    0:{
                        items:1
                    },
                    576:{
                        items:2
                    },
                    768:{
                        items:3
                    },
                    992:{
                        items:4
                    }
                }
            }
        );
      });

      $(document).ready(function(){
        $(".say").owlCarousel(
            {
                items: 1,
                loop: true,
                autoplay: true
            }
        );
      });
})