(function() {
var common = (function () {

  var menu_responsivo = function() {
     $('.fa-bars').on('click', function(){
        $('nav').slideToggle();
     });
  };
 
  var Toggle = function() {
    $('.toggle').on('click', function() {
      $('.toggle').addClass('press');
      $('.menu').toggleClass('active');
      $('.menu_top').toggleClass('dn');
    });
  };


  var efectos = function(){
    
    $(document).ready(function() {

      if($(window).width() > 680 ){
        $('.top_home_text').delay(500).animate({left: 100, opacity: 1}, 1000);
      }

      if($(window).width() < 680 && $(window).width() > 480){
        $('.top_home_text').delay(500).animate({left: 0, opacity: 1}, 1000);
      }

      if($(window).width() <= 480 ){
        $('.top_home_text').delay(500).animate({left: 10, opacity: 1}, 1000);
      }
    });

  }

  var logo_transparente = function() {
  var logo = $('header'),sec0 = $('#section0'), win = $(window);
    win.scroll(function(){  
      if ((win.scrollTop() - 200) >= -68){
        $('.logo_top').css({opacity:0});
      }else{
        $('.logo_top').css({opacity:1});
      }
      if(!$('.toggle').hasClass('press')){
        if (win.scrollTop() == 0){
           $('.menu').toggleClass('active', false);
           $('.menu_top').toggleClass('dn', false);
        }else{
          if(!$('.menu').hasClass('active')){
            $('.menu').toggleClass('active', true);
            $('.menu_top').toggleClass('dn', true);
          }
        }
      }
    });
  };

  var initialize = function () {
    menu_responsivo();
    Toggle();
    efectos();
    logo_transparente();
  };

  return {
      init: initialize
  };
})();

common.init();

}).call(this);
