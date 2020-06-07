

/// <reference path="jquery-1.8.2.js" />
jQuery.fn.extend({
    //width ==> Slider Width default=500px;
    //speed ==> slider animation duration in msec default=5000
    //autoplay ==> autoplay slider animations //true=>autoplay or false default=true
    //navigationType ==> "arrow","bullet","both" default=arrow
    //  QvSlider: function (width,speed,autoPlay,navigationType) {
    QvSlider: function (prop) {
        var QvSlider;
        var sliderInterval;
        var loopSpeed;
        var autoPlay;
        var width;
        var navType;
        var alignment;
        return this.each(function () {
            QvSlider = this;
            if ($(QvSlider).hasClass('qvs')) { return false;}
            if (typeof (prop) == "undefined") {
                prop = { speed: 5000, autoPlay: true, navigationType: "arrow", width: $(QvSlider).parent().width(), alignment: "Right" };
            }
            loopSpeed = prop.speed;
            autoPlay = prop.autoPlay;
            width = prop.width;
            navType = prop.navigationType;
            alignment = prop.alignment;
            debugger;
            //alert(alignment);
            $(QvSlider).addClass('qvs');
            var minHeight = Math.min.apply(null, $(QvSlider).find('>li').map(function () { return $(this).height(); }).get());
            var minWidth = Math.min.apply(null, $(QvSlider).find('>li').map(function () { return $(this).width(); }).get());
            if (typeof (autoPlay) == "string") { autoPlay = autoPlay.toLowerCase() == "true" ? true : false; }
            if (typeof (loopSpeed) == "undefined" || loopSpeed == null || loopSpeed == 0) { loopSpeed = 5000; }
            if (typeof (autoPlay) == "undefined" || autoPlay == null) { autoPlay = true; }
            if (typeof (navType) == "undefined" || navType == null) { navType = "arrow"; }
            if (typeof (alignment) == "undefined" || alignment == null) { alignment = "Right"; }
            if (typeof (width) == "undefined" || width == null || width == 0) { width = $(QvSlider).parent().width(); }
            $(QvSlider).attr("speed", loopSpeed);
            $(QvSlider).attr("auto", autoPlay);
            $(QvSlider).attr("navtype", navType);
            $(QvSlider).attr("alignment", alignment);
            var tag = $(this).prop("tagName");
            if (tag.toLowerCase() == "ul" && $(QvSlider).find('>li').length > 0) {
                addNavigation();
                if ($(QvSlider).parent().width() < minWidth) {
                    minWidth = $(QvSlider).parent().width();
                }
                else if (width > $(QvSlider).parent().width()) {
                    width = $(QvSlider).parent().width();
                }
                if (minWidth < width) { width = minWidth; }
                width = width / $(QvSlider).parent().width() * 100;

                if (alignment.toLowerCase().indexOf("mid") > -1) {
                    $(QvSlider).attr("style", $(QvSlider).attr("style") + "margin:auto !important");
                    //alert("fff");
                    $(QvSlider).css("margin", "auto");
                }
                else { $(QvSlider).css("float", alignment.toLowerCase()); }
                $(QvSlider).css("width", width.toString() + "%").css("height", minHeight);
                for (var i = 0; i < $(QvSlider).find(">li").length; i++) {
                    $(QvSlider).find(">li:eq(" + i + ")").css("width", "width", width.toString() + "%");
                    // $(QvSlider).find(">li:eq(" + i + ") img").css("width", width);
                }
                $(QvSlider).find(">ol.qv_arrow_nav").css("width", width).hide();
                $(QvSlider).find(">li").first().show();
                $(QvSlider).find(">ol.qv_bullets_nav .qv_bullet#0").addClass("active");
                //goToItem(0);
                prevEvent();
                nextEvent();
                bulletEvent();
                sliderHoverEvent();

                if (autoPlay) {
                    playSlider();
                }
            }
            else { console.log('not allowed tag'); }
        });
        function addNavigation() {

            if (navType.toLowerCase().trim() == "arrow" || navType.toLowerCase().trim() == "both")
                $(QvSlider).find(".qv_arrow_nav").remove();
                if ($(QvSlider).find(".qv_arrow_nav").length == 0) {
                    { $('<ol class="qv_arrow_nav"><div class="arrow-left"></div><div class="arrow-right"></div></ol>').prependTo(QvSlider); }
                }
                if (navType.toLowerCase().trim() == "bullet" || navType.toLowerCase().trim() == "both") {
                    $(QvSlider).find(".qv_bullets_nav").remove();
                if ($(QvSlider).find(".qv_bullets_nav").length == 0) {
                    var bullets = "";
                    for (var i = 0; i < $(QvSlider).find(">li").length; i++) {
                        bullets += '<span class="qv_bullet" id=' + i + '></span>';
                    }
                    $('<ol class="qv_bullets_nav">' + bullets + '</ol>').prependTo(QvSlider);
                }
            }
        }
        function goNext() {
            var cur = $(QvSlider).find(">li:visible");
            var curIndex = $(QvSlider).find(">li").index(cur);
            var next = $(cur).next("li");
            if (next.length > 0) {
                goToItem(curIndex + 1);
            } else {
                goToItem(0);
            }

        }
        function goPrev() {

            var cur = $(QvSlider).find(">li:visible");
            var curIndex = $(QvSlider).find(">li").index(cur);
            var prev = $(cur).prev("li");
            if (prev.length > 0) {
                goToItem(curIndex - 1, "prev");
            } else {
                goToItem($(QvSlider).find(">li").length - 1, "prev");
            }

        }
        function goToItem(index, dir) {

            var cur = $(QvSlider).find(">li:visible");
            var curIndex = $(QvSlider).find(">li").index(cur);
            var margin = $(QvSlider).width() * 2;
            if (index != curIndex && curIndex > -1) {
                //$(cur).fadeOut(1000);
                //$(QvSlider).find(">li:eq(" + index + ")").fadeIn(1000);
                if (dir == "prev") {
                    $(cur).animate({ "left": "-" + margin + "" }, "slow", function () {
                        $(cur).css("left", "auto").hide();
                    });
                }
                else {
                    $(cur).animate({ "right": "-" + margin + "" }, "slow", function () {
                        $(cur).css("right", "auto").hide();
                    });
                }
                $(QvSlider).find(">li:eq(" + index + ")").fadeIn(1000);

                $(QvSlider).find(">ol.qv_bullets_nav .qv_bullet").removeClass("active");

                $(QvSlider).find(">ol.qv_bullets_nav .qv_bullet#" + index + "").addClass("active");
            }


        }
        function prevEvent() {

            $(QvSlider).find(">ol.qv_arrow_nav .arrow-left").on("click", null, function (e) {
                pauseSlider();
                goPrev();
                if (autoPlay) {
                    playSlider();
                }
            });
        }
        function nextEvent() {

            $(QvSlider).find(">ol.qv_arrow_nav .arrow-right").on("click", null, function (e) {
                pauseSlider();
                goNext();
                if (autoPlay) {

                    playSlider();
                }
            });
        }
        function bulletEvent() {
            $(QvSlider).find(">ol.qv_bullets_nav .qv_bullet").on("click", null, function (e) {
                var index = parseInt($(this).attr("id"));
                pauseSlider();
                goToItem(index);
                if (autoPlay) {

                    playSlider();
                }
            });

        }
        function sliderHoverEvent() {
            $(QvSlider).hover(function () {
                pauseSlider();
                $(QvSlider).find(">ol.qv_arrow_nav").fadeIn(200);
            });
            $(QvSlider).mouseleave(function () {
                if (autoPlay) {
                    playSlider();
                }
                $(QvSlider).find(">ol.qv_arrow_nav").fadeOut(200);
            });
        }
        function playSlider() {
            sliderInterval = setInterval(function () { goNext(); }, loopSpeed);
        }
        function pauseSlider() {
            clearInterval(sliderInterval);
        }
    }


});


$(document).ready(function () {

    function setSliders() {
        for (var i = 0 ; i < $(".qv_slider").length ; i++) {
            var id ="#"+ $($(".qv_slider")[i]).attr("id")
            console.log($("" + id + ""));
            debugger;
            var speedVal = parseInt($("" + id + "").attr("speed"));
            var autoPalyVal = $("" + id + "").attr("auto");
            var navtypeVal = $("" + id + "").attr("navtype");
            var alignmentVal = $("" + id + "").attr("alignment");

            var intv = setInterval(function () {
                if ($("" + id + "").hasClass("qvs")) { clearInterval(intv); }
                else { $("" + id + "").QvSlider({width:833, speed: speedVal, autoPlay: autoPalyVal, navigationType: navtypeVal, alignment: "Mid" }); }
            }, 100);
        };

    }
    setTimeout(function () {
     //   $(".qv_slider").height($(".qv_slider >li >img").first().height());
        var images = $(".qv_slider >li >img");
        var count = images.length;
        var loaded = 0;

        images.each(function () {
            var image = new Image();
            image.onload = function () {
                loaded += 1;

                if (loaded == count) {
                    setSliders();
                }
            }

            image.src = $(this).attr('src');
        });

        //preload(imagesArray, setSliders);
    }, 1000);



})

