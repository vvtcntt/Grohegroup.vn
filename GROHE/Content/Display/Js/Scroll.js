// JavaScript Document
 $("document").ready(function($){
    var nav = $('#Sidebar');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
			$("#Sidebar").css("display", "block");
            nav.addClass("f-nav");
        } else {
			$("#Sidebar").css("display", "none");
            nav.removeClass("f-nav");
        }
    });
});