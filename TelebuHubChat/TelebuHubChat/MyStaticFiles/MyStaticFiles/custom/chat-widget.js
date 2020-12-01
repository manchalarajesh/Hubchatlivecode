var url = "http://hubchat.com/";

//var url = "http://localhost:65000";

var _args = {};
var TelebuPOP = TelebuPOP || (function () {

  return {
    load: function (Args) {
      if (Args) {
        _args = Args;
        if (_args.length > 0) {
          var animate_css = document.createElement("link")
          animate_css.setAttribute("rel", "stylesheet")
          animate_css.setAttribute("type", "text/css")
          animate_css.setAttribute("href", url + '/StaticFiles/animate.min.css');
          document.getElementsByTagName("head")[0].appendChild(animate_css);
		  
		  var custom_css = document.createElement("link")
          custom_css.setAttribute("rel", "stylesheet")
          custom_css.setAttribute("type", "text/css")
          custom_css.setAttribute("href", url + '/StaticFiles/Custom.css');
          document.getElementsByTagName("head")[0].appendChild(custom_css);
		  
		  var framestyle_css = document.createElement("link")
          framestyle_css.setAttribute("rel", "stylesheet")
          framestyle_css.setAttribute("type", "text/css")
          framestyle_css.setAttribute("href", url + '/StaticFiles/FrameStyles.css');
		  //framestyle_css.setAttribute("href", 'http://qa.press3.com/css/FrameStyles.css');
          document.getElementsByTagName("head")[0].appendChild(framestyle_css);
		  
          var aid = _args[0];
	  var pop_isAgent = _args[1];
	  var pop_isPingAgent = _args[2];
          var a = document.referrer;
          var host = window.location.hostname;
          if ((localStorage.getItem("h") != null) && (localStorage.getItem("h") != "")) { a = ""; }
          else if ((localStorage.getItem("AEname") != null) && (localStorage.getItem("AEname") != "")) { a = ""; }
          //var e = url + _args[0] + "/chat?domain=" + host + "&referrer=" + a;
	  var e = url + "/Chat/chat/" + _args[0] + "/" + _args[1] + "/" + _args[2] + "?httpreferrer=" + a;
	  //var e = url + "/Chat/chat/8yJEIAhiYViWgMrLZx21tM0tQvxZD6A0/0/1?referrer=" + a;
          var chatContent = '<div id="TelebuPOP_frame" class="frameBlock animated" style="display:none;"><iframe src="' + e + '" id="pop_frame" name="pop_frame" class="pop_frame" width="100%" height="100%" border="0"></iframe></div>';
          chatContent = chatContent + '<div id="TelebuChat-icon" class="iconBlock"><div class="notification animated"><div class="message"> <div class="semiBold"> Hi there, </div><div> We are here to help you out, ask us anything you want</div> </div></div> <div class="chatIcon" ><span class="chat_notify animated zoomIn" style="display:none;"  id="notificationCount"></span><div class="iconDots animated zoomIn"><span> '
		  chatContent = chatContent + '<img src="' + url + '/StaticFiles/images/chat_icn.png" alt=""> </span></div><div class="iconCount animated zoomIn" style="display:none">2</div><div class="iconClose animated rotateIn" style="display:none"><img src="' + url + '/StaticFiles/images/close-blue.png" alt="close-blue"></div></div></div>';
          var f = document.createElement("div");
          f.className = "mainSection";f.id="blue"; f.innerHTML = chatContent;
          document.body.appendChild(f);
          document.getElementById("TelebuChat-icon").addEventListener("click", function () {
           
            var x = document.getElementById("TelebuPOP_frame");
            if (x.style.display === "none") {
			  x.classList.add("fadeInRight");
			  x.classList.remove("fadeOutRight");
			  x.classList.add("show");
			  x.style.display = "block";
			  document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].classList.remove('show');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].classList.add('show');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.add('fadeOutRight');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.remove('fadeInRight');
              document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].style.display = "none";
              document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].style.display = "block";
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].style.display = "none";
              window.frames['pop_frame'].postMessage('open', '*');
            } else {
			  x.classList.add("fadeOutRight");
			  x.classList.remove("fadeInRight");
			  x.classList.remove("show");
			  x.style.display = "none";
			  document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].classList.add('show');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].classList.remove('show');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.add('fadeInRight');
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.remove('fadeOutRight');
              document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].style.display = "block";			  
              document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].style.display = "none";			  
			  document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].style.display = "block";
              window.frames['pop_frame'].postMessage('close', '*');
            }

          });
          window.addEventListener("message", receiveMessage, false);
        }
        else { console.log("Widget UUId is missing"); }

      }
      else { console.log("Widget UUId is missing"); }
    }

  };
}());

function receiveMessage(event) {
  if (event.data["type"] == "load") {
    document.getElementById('telebu-welcomemsg').innerHTML = event.data["msg"];
    if(event.data["cnt"] >0){ 
      document.getElementById('iconCount').innerHTML=event.data["cnt"]; 
      document.getElementById("TelebuPOP_frame").style.display = "block"; }
    else{     
      document.getElementById("TelebuPOP_frame").style.display = "none"; 
    }   
  }
  else if (event.data["type"] == "close") {
    document.getElementById("TelebuPOP_frame").style.display = "none";
	document.getElementById("TelebuPOP_frame").classList.remove('show');
	document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.add('fadeInRight');
	document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].classList.remove('fadeOutRight');
	document.getElementById("TelebuPOP_frame").classList.add("fadeOutRight");
	document.getElementById("TelebuPOP_frame").classList.remove("fadeInRight");
	document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].classList.remove('show');
	document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].classList.add('show');
    document.getElementById("TelebuChat-icon").getElementsByClassName("iconDots")[0].style.display = "block";
    document.getElementById("TelebuChat-icon").getElementsByClassName("iconClose")[0].style.display = "none";	
	document.getElementById("TelebuChat-icon").getElementsByClassName("notification")[0].style.display = "block";
	
    if(event.data["cnt"] >0){ 
      document.getElementById('iconCount').innerHTML=event.data["cnt"]; 
      document.getElementById("TelebuPOP_frame").style.display = "block"; }
    else{     
      document.getElementById("TelebuPOP_frame").style.display = "none"; 
    } 
  }

}