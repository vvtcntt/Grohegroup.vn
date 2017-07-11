// JavaScript Document
 $("document").ready(function($){
    var nav = $('#nVar');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
			$("#nVar").css("display", "block");
            nav.addClass("f-nav");
        } else {
			 
            nav.removeClass("f-nav");
        }
    });
});
