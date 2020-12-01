
(function () {

        var defaultErrorMessage = "Handler Returned Non-Success Response Code";
        var failedActionResponse = { Success: false, Message: defaultErrorMessage }
        var isAsync = true;
        var ws;
        var OPTIONS;
        var timeout = 3;
        var Status = 0;
 
     this.WebSocketRequestBinding = function (options)
     {
         var defaults = {
             //onOpen: OnOpen,
             //onMessage: OnMessage,
             //onError: OnError,
             //onClose: OnClose,
             hostAddress: ""
         }
         if (arguments[0] && typeof arguments[0] === "object")
             this.options = extendDefaults(defaults, arguments[0]);
         else
             this.options = defaults;
         OPTIONS = this.options;
        // ws = new WebSocket(this.options.hostAddress);

         console.log("WebSocket object Created");
        // InitializeEvents(this.options);
         
     }
    
     WebSocketRequestBinding.prototype.SubscribeWebSocket = function (hostName, retry, agentId, accountId,conferenceRoom,CallId) {
         SubscribeWebSocket(hostName, retry);
         timeout = retry;
         //ws.onopen = function (evt) {
         //    Status = 1;
         //    UpdateStatus(agentId, accountId, conferenceRoom, Status,CallId);
         //    console.log("WebSocket Connection Opened");
         //    $("#lblWebsocket").html("WebSocket Status :  Open");
         //    ChangeVertoRegistrationStatus(1);
         //};
       
       
         //ws.onclose = function () {
         //    Status = 0;
         //    console.log("WebSocket Connection Closed");
         //    UpdateStatus(agentId, accountId, conferenceRoom, Status,CallId);
         //    $("#lblWebsocket").html("WebSocket Status :  Closed");
         //};
         return ws;
         
     }
     WebSocketRequestBinding.prototype.retryws = function (hostName, retry, agentId, accountId, conferenceRoom, CallId) {
         SubscribeWebSocket(hostName, retry, agentId, accountId, conferenceRoom, CallId);
         timeout = retry;

     }
     WebSocketRequestBinding.prototype.UpdateStatus = function (AgentId, AccountId, conferenceRoom, Status, CallId) {
         UpdateStatus(AgentId, AccountId, conferenceRoom, Status, CallId);
     }

     //function blink_text() {
     //    $('#lblWebsocket').fadeOut(500);
     //    $('#lblWebsocket').fadeIn(500);
     //}

     function SubscribeWebSocket(hostName, retry) {

         ws = new WebSocket(hostName);
         ws.onopen = (function (evt) {
             $("#lblWebsocket").html(" ");
             Status = 1;
             UpdateStatus(agentId, accountId, conferenceRoom, Status, CallId);
             console.log("WebSocket Connection Open");
             $("#lblWebsocket").html("WebSocket Status :  Open");
             if (roleId == 2 || roleId == 3)
                 $("#lblChannelName").html("Channel Name: manager_" + agentId)
             else if (roleId == 1)
                 $("#lblChannelName").html("Channel Name: Agent_" + agentId)
         });
             //setInterval(blink_text, 1000);
         //},3000);
         ws.onerror = function (evt) {
             console.log("WebSocket Connection Failed " + evt.message + '' + timeout);
             setTimeout(function () {
                 if (timeout > 1) {
                     SubscribeWebSocket(hostName);
                     timeout--;
                     console.log("Trying again");
                 }
             }, 5000);
             $("#lblWebsocket").html("WebSocket Status :  Closed");
         };
         ws.onclose = (function () {
             Status = 0;
             console.log("WebSocket Connection Closed");
             UpdateStatus(agentId, accountId, conferenceRoom, Status, CallId);
             $("#lblWebsocket").html("WebSocket Status :  Closed");
         });
         
        
     }

     function extendDefaults(source, properties) {
         var property;
         for (property in properties) {
             if (properties.hasOwnProperty(property)) {
                 source[property] = properties[property];
             }
         }
         return source;
     }
     function UpdateStatus(AgentId, AccountId, conferenceRoom, Status,CallId) {
         $.ajax({
             type: "GET",
             url: "Handlers/Calls.ashx",
             dataType: "JSON",
             async: false,
             data: { type: 23, agentId: AgentId, accountId: AccountId, ConferenceRoom: conferenceRoom, status: Status, CallId: CallId },
             success: function (res) {
                 $.unblockUI();
                 if (res.Success == "True") {
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $.unblockUI();
                 if (jqXHR.status == 401) {
                     window.location.href = "/Login.aspx?message=Session expired";
                 } else if (jqXHR.status == 406) {
                     $("#modalPreviousSession").modal("show");
                 } else {
                     console.log(errorThrown);
                 }
             }
         });
     }
     //function InitializeEvents(options)
     //{
     //    ws.onopen = options.onOpen;
     //    ws.onmessage = options.onMessage;
     //    ws.onerror = options.onError;
     //    ws.onclose = options.onClose;
     //}

     //function CanCallBack(callBackFunction) {
     //    if (isAsync && callBackFunction && typeof callBackFunction === "function")
     //        return true;
     //    else
     //        return false;
     //}

    // Web Socket onopen event
     function OnOpen(evt) {
         console.log(evt);
     }

    // Web Socket onmessage event
     function OnMessage(evt)
     {

         console.log("on message " + evt.data);

         var onMessageWebSocketResponse = "";

         if (typeof (evt.data) != "undefined" || evt.data != "")
         {
             onMessageResponse = evt.data;
         }

         return onMessageWebSocketResponse;
     }


    // Web Socket Connection Error
     function OnError(evt) {
         console.log("WebSocket Connection Failed" + evt.message);
     }
   
    // Web Socket Connection close
     function OnClose(evt) {
         console.log("WebSocket Connection Closed");
     }

  
     
 }());

