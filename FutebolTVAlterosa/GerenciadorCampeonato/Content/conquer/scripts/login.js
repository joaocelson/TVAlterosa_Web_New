var Login = function () {

	var handleLogin = function() {

	    $('.login-form input').keypress(function (e) {
	        if (e.which == 13) {
                $('.login-form').submit();
	            return false;
	        }
	    });
	}
    
    return {

        init: function () {
        	
            handleLogin();

			$.backstretch([
		        "/Content/conquer/img/bg/1.jpg",
		        "/Content/conquer/img/bg/2.jpg",
		        "/Content/conquer/img/bg/3.jpg",
		        "/Content/conquer/img/bg/4.jpg",
				"/Content/conquer/img/bg/5.jpg",
				"/Content/conquer/img/bg/6.jpg",
				"/Content/conquer/img/bg/7.jpg",
				"/Content/conquer/img/bg/8.jpg"
		        ], {
		          fade: 1000,
		          duration: 8000
		    });
			
	       
        }

    };

}();