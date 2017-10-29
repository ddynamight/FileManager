/*
* Author:      Marco Kuiper (http://www.marcofolio.net/)
*/

// Speed of the automatic slideshow
var slideshowSpeed = 15000;

// Variable to store the images we need to set as background
// which also includes some text and url's.
var photos = [{
    "title": "Through well researched and developed solutions. We redefine your business, ideas and create an enabling environment for you to actualize the realities others call fiction.",
    "image": "banner1.png",
    "url": "/Services/WebDev",
    "firstline": "The WEB",
    "secondline": "re-Imagined"
}, {
    "title": "With custom and tailor made Software as a Service (SaaS) and Infrastructure as a Service (IaaS) we deliver and provide various services to both individuals and organizations.",
    "image": "banner2.png",
    "url": "/Services/ServerSol",
    "firstline": "Effective & Affordable",
    "secondline": "Cloud Solution"
}, {
    "title": "We build systems that goes beyond clients expectation and improve greatly on the requirements with the aim of creating that perfect matching service for and your clients.",
    "image": "banner3.png",
    "url": "/Services/Software",
    "firstline": "Professional & Thoughtful",
    "secondline": "Software Design"
}, {
    "title": "The evolution of technology has allowed for some many possibilities of todays world which is why we have been able to leverage and harness the power in providing great benefits and solutions.",
    "image": "banner4.png",
    "url": "/Services/Network",
    "firstline": "Easy & Affordable",
    "secondline": "Network Solution"
}, {
    "title": "With the rate at which the Internet is growing and the velocity of technological advancement the need to have a secure system, network and information flow cannot be over-enversized.",
    "image": "banner5.png",
    "url": "/Services/Security",
    "firstline": "Easy & Affordable",
    "secondline": "Security Solution"
}];



$(document).ready(function() {
		
	// Backwards navigation
	$("#back").click(function() {
		stopAnimation();
		navigate("back");
	});
	
	// Forward navigation
	$("#next").click(function() {
		stopAnimation();
		navigate("next");
	});
	
	var interval;
	$("#control").toggle(function(){
		stopAnimation();
	}, function() {
		// Change the background image to "pause"
		$(this).css({ "background-image" : "url(/Content/images/btn_pause.png)" });
		
		// Show the next image
		navigate("next");
		
		// Start playing the animation
		interval = setInterval(function() {
			navigate("next");
		}, slideshowSpeed);
	});
	
	
	var activeContainer = 1;	
	var currentImg = 0;
	var animating = false;
	var navigate = function(direction) {
		// Check if no animation is running. If it is, prevent the action
		if(animating) {
			return;
		}
		
		// Check which current image we need to show
		if(direction == "next") {
			currentImg++;
			if(currentImg == photos.length + 1) {
				currentImg = 1;
			}
		} else {
			currentImg--;
			if(currentImg == 0) {
				currentImg = photos.length;
			}
		}
		
		// Check which container we need to use
		var currentContainer = activeContainer;
		if(activeContainer == 1) {
			activeContainer = 2;
		} else {
			activeContainer = 1;
		}
		
		showImage(photos[currentImg - 1], currentContainer, activeContainer);
		
	};
	
	var currentZindex = -1;
	var showImage = function(photoObject, currentContainer, activeContainer) {
		animating = true;
		
		// Make sure the new container is always on the background
		currentZindex--;
		
		// Set the background image of the new active container
		$("#headerimg" + activeContainer).css({
			"background-image" : "url(/Content/Images/" + photoObject.image + ")",
			"display" : "block",
			"z-index" : currentZindex
		});
		
		// Hide the header text
		$("#headertxt").css({"display" : "none"});
		
		// Set the new header text
		$("#firstline").html(photoObject.firstline);
		$("#secondline")
			.attr("href", photoObject.url)
			.html(photoObject.secondline);
		$("#pictureduri")
			.attr("href", photoObject.url)
			.html(photoObject.title);
		
		
		// Fade out the current container
		// and display the header text when animation is complete
		$("#headerimg" + currentContainer).fadeOut(function() {
			setTimeout(function() {
				$("#headertxt").css({"display" : "block"});
				animating = false;
			}, 500);
		});
	};
	
	var stopAnimation = function() {
		// Change the background image to "play"
	    $("#control").css({ "background-image": "url(/Content/images/btn_play.png)" });
		
		// Clear the interval
		clearInterval(interval);
	};
	
	// We should statically set the first image
	navigate("next");
	
	// Start playing the animation
	interval = setInterval(function() {
		navigate("next");
	}, slideshowSpeed);
	
});