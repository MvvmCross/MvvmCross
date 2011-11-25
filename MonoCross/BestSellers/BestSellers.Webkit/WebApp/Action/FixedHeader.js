WA.AddEventListener("endslide", function() { myScrollRefreshable = true; myScroll.refresh(); isLazyLoading=true;});
WA.AddEventListener("beginslide", function() { if (currentFocusElement != null) { currentFocusElement.blur(); currentFocusElement = null; } myScroll.scrollTo(0, '100ms'); });
WA.AddEventListener("endasync", function() { myScrollRefreshable = true; myScroll.refresh(); });

var myScroll;
var currentHeight = 0;
var myScrollRefreshable = true;
var currentFocusElement = null;

window.onload = handleResize;
window.setInterval(handleResize, 400);

WA.AddEventListener('load', loaded, true);

function loaded() {
    setTimeout(
	    function() {
	        document.ontouchmove = function(e) { e.preventDefault(); return false; }
	        myScroll = new iScroll(document.getElementById('scroller'));
	    }, 100);
	}

	function handleResize() {
	    
	    if (myScrollRefreshable) {
	        if (window.innerHeight != currentHeight) {
	           //alert(window.innerHeight + ":" + currentHeight);
            currentHeight = window.innerHeight;
            window.scrollTo(0, 0);
            document.getElementById('scroller').style.width = window.innerWidth + "px";
            document.getElementById('wrapper').style.height = (window.innerHeight - 40) + "px";
            if (myScroll)
                myScroll.refresh();
            }
        }
    }
    
function myScrollDoRefresh(toRefresh) {    
        myScrollRefreshable = toRefresh;
    }

window.setInterval(handleLazyLoading,2000);

var isLazyLoading = false;
var currentMaxPosition = 0;
function handleLazyLoading(){
    if(isLazyLoading){      
      myScroll.refresh();      
      if(currentMaxPosition != myScroll.getMaxScrollPos()){
         currentMaxPosition = myScroll.getMaxScrollPos();        
      }else{         
	isLazyLoading = false;         
      }
    }
}

function processFocusEvent(id) {
    currentFocusElement = document.getElementById(id);
    myScrollDoRefresh(false);
    return true;
}
function processBlurEvent(id) {
    var inputElement = document.getElementById(id);
    if (inputElement != null) {
        inputElement.blur();
    }
    myScrollDoRefresh(true);
    return true;
}



