/*************************************************
Star Rating System
************************************************/

jQuery.fn.rating = function(callback, settings) {

    settings = jQuery.extend({
			propertyID: -1 
		},settings);
	
	if(callback == null) return;
   
    var container = jQuery(this);
    var stars = jQuery(container).children('li').children('a');
  
    stars.click(function(){
        var rating = stars.index(this) + 1;
        jQuery(container).children('.pa-current-rating').css('width', rating*20 + '%').end();
        eval(callback + "('" + rating + "-" + settings.propertyID + "')");
        jQuery(container).addClass('pa-star-rating pa-has-rated');
        return false;
    });
  
};
