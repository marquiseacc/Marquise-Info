
$(function() {
    "use strict";
     
	 
//sidebar menu js
$.sidebarMenu($('.sidebar-menu'));

// === toggle-menu js
$(".toggle-menu").on("click", function(e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });	 
	   
// === sidebar menu activation js

$(function() {
        for (var i = window.location, o = $(".sidebar-menu a").filter(function() {
            return this.href == i;
        }).addClass("active").parent().addClass("active"); ;) {
            if (!o.is("li")) break;
            o = o.parent().addClass("in").parent().addClass("active");
        }
    }), 	   
	   

/* Top Header */

$(document).ready(function(){ 
    $(window).on("scroll", function(){ 
        if ($(this).scrollTop() > 60) { 
            $('.topbar-nav .navbar').addClass('bg-dark'); 
        } else { 
            $('.topbar-nav .navbar').removeClass('bg-dark'); 
        } 
    });

 });


/* Back To Top */

$(document).ready(function(){ 
    $(window).on("scroll", function(){ 
        if ($(this).scrollTop() > 300) { 
            $('.back-to-top').fadeIn(); 
        } else { 
            $('.back-to-top').fadeOut(); 
        } 
    }); 

    $('.back-to-top').on("click", function(){ 
        $("html, body").animate({ scrollTop: 0 }, 600); 
        return false; 
    }); 
});	   
	    
   
$(function () {
  $('[data-toggle="popover"]').popover()
})


$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})


	 // theme setting
	 $(".switcher-icon").on("click", function(e) {
        e.preventDefault();
        $(".right-sidebar").toggleClass("right-toggled");
    });
	
	$('#theme1').click(theme1);
    $('#theme2').click(theme2);
    $('#theme3').click(theme3);
    $('#theme4').click(theme4);
    $('#theme5').click(theme5);
    $('#theme6').click(theme6);
    $('#theme7').click(theme7);
    $('#theme8').click(theme8);
    $('#theme9').click(theme9);
    $('#theme10').click(theme10);
    $('#theme11').click(theme11);
    $('#theme12').click(theme12);
    $('#theme13').click(theme13);
    $('#theme14').click(theme14);
    $('#theme15').click(theme15);
    $('#theme16').click(theme16);
    $('#theme17').click(theme17);
    $('#theme18').click(theme18);

    function theme1() {
        $('body').attr('class', 'bg-theme bg-theme1 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme1');
    }

    function theme2() {
        $('body').attr('class', 'bg-theme bg-theme2 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme2');
    }

    function theme3() {
        $('body').attr('class', 'bg-theme bg-theme3 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme3');
    }

    function theme4() {
        $('body').attr('class', 'bg-theme bg-theme4 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme4');
    }
	
	function theme5() {
        $('body').attr('class', 'bg-theme bg-theme5 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme5');
    }
	
	function theme6() {
        $('body').attr('class', 'bg-theme bg-theme6 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme6');
    }

    function theme7() {
        $('body').attr('class', 'bg-theme bg-theme7 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme7');
    }

    function theme8() {
        $('body').attr('class', 'bg-theme bg-theme8 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme8');
    }

    function theme9() {
        $('body').attr('class', 'bg-theme bg-theme9 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme9');
    }

    function theme10() {
        $('body').attr('class', 'bg-theme bg-theme10 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme10');
    }

    function theme11() {
        $('body').attr('class', 'bg-theme bg-theme11 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme11');
    }

    function theme12() {
        $('body').attr('class', 'bg-theme bg-theme12 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme12');
    }
	
	function theme13() {
        $('body').attr('class', 'bg-theme bg-theme13 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme13');
    }
	
	function theme14() {
        $('body').attr('class', 'bg-theme bg-theme14 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme14');
    }
	
	function theme15() {
        $('body').attr('class', 'bg-theme bg-theme15 dir-rtl d-flex flex-column min-vh-100');
        $('#logo-icon').attr('src', '/Content/Images/logo-white.png');
        setThemeCookie('theme15');
    }

    function theme16() {
        $('body').attr('class', 'bg-theme bg-theme16 dir-rtl d-flex flex-column min-vh-100 black-color');
        $('#logo-icon').attr('src', '/Content/Images/logo-black.png');
        setThemeCookie('theme16');
    }

    function theme17() {
        $('body').attr('class', 'bg-theme bg-theme17 dir-rtl d-flex flex-column min-vh-100 black-color');
        $('#logo-icon').attr('src', '/Content/Images/logo-black.png');
        setThemeCookie('theme17');
    }

    function theme18() {
        $('body').attr('class', 'bg-theme bg-theme18 dir-rtl d-flex flex-column min-vh-100 black-color');
        $('#logo-icon').attr('src', '/Content/Images/logo-black.png');
        setThemeCookie('theme18');
    }
});

function setThemeCookie(themeName) {
    document.cookie = "userTheme=" + themeName + "; path=/; max-age=" + (60 * 60 * 24 * 30); // 30 ???
}