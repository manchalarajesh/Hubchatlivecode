
    $(document).ready(function () {
        $(".scroll").mCustomScrollbar({
            theme: "minimal"
        });
      $("#submitForm").addClass("hide");
        $("#chatWindow").addClass("animated fadeInRight show");
});
$(".error").parent().find(".errMsg").show();
        $("#Submit").click(function () {
        $("#submitForm").addClass("hide");
    $("#chatWindow").addClass("animated fadeInRight show");
});
        // $(document).ready(function () {
        $('.chatIcon').click(function () {
            //debugger;

            $('#iframeBlock').toggleClass('show', 'hide');
            $('.notification').addClass("fadeOutRight");
            //$(".iconCount").addClass("hide");
            $(".iconDots").toggleClass('hide');
            $(".iconClose").toggleClass('show', 'hide');
        });
                           // });
        $("#send").click(function () {
              var msg = $("#message").val();
            var text = '<div class="speechItem right"><div class="speechRight">';
             text = text + '<p>' + msg + '</p></div><div class="time">02:30 PM</div></div>';
   $("#conversation").append(text);
   $("#message").val('');
   var g=@x;
   var WidgetId =@data.WidgetId;
   var AccountId =@data.AccountId;
   var url="@data.url";
            var Data = JSON.stringify({
        "StatusId": "1"
});
var url1 = url+ "/accounts/" + AccountId + "/widgets/" + WidgetId + "/conversations/";
            $.ajax({
        type: "POST",
    url: url1,
    dataType: "JSON",
    contentType: "application/json",
    async: false,
    data: Data,
                success: function (res) {

                    var conversationId = res.Id;
    BotInteraction(conversationId,msg)
   // alert(conversationId);
},
                error: function (jqXHR, textStatus, errorThrown) {
        alert(textStatus);
}
});
});
        function BotInteraction(conversationId, msg) {
               var g=@x;
 var WidgetId =@data.WidgetId;
 var AccountId =@data.AccountId;
 var url = "@data.url";
 var imgUrl = url + "/StaticFiles/images/Sender-sm.png";
  var url2 = url+ "/accounts/" + AccountId + "/widgets/" + WidgetId + "/conversations/"+ conversationId + "/replies/";
 //var url2 = "http://localhost:65000/accounts/5/widgets/1/conversations/" + conversationId + "/replies/";
            var Data2 = JSON.stringify({
        "MessageTypeId": "1",
    "Message":msg
});
            $.ajax({
        type: "POST",
    url: url2,
    dataType: "JSON",
    contentType: "application/json",
    async: false,
    data: Data2,
                success: function (res) {
                    var butt = "";
    var t = JSON.parse(res.Message);
    //alert(t.length)
                    for (var j = 0; j < t.length; j++) {
                        var retValue = JSON.parse(res.Message)[j];
    console.log(retValue)
    var resMsg = retValue.text;
                        try {
                            if (retValue.buttons.length > 0) {
        butt = '<div class="mt-2">';
                                for (var i = 0; i < retValue.buttons.length; i++) {
        butt = butt + '<button type="button" onclick="btnAction(this)" value="' + retValue.buttons[i].title + '" class="btnSubmit-sm mr-2 mb-2 ">' + retValue.buttons[i].title + '</button>';
}
                                butt = butt + '</div>';
                            }
                        }
                        catch (e) { console.log(e); }
var text = '<div class="speechItem"><div class="clearfix"><img id=imgUrl alt=""><div class="speechLeft">';
text = text + '<p>' + resMsg + '</p>' + butt + '</div><div class="time">02:30 PM</div></div>';
$("#conversation").append(text);
document.getElementById("imgUrl").src = imgUrl;
                    }
                 
                },
error: function (jqXHR, textStatus, errorThrown) {
    alert(textStatus);
}
            });
        }
$(".btnSubmit-sm").on("click", function () {
    var buttval = $(".btnSubmit-sm").text();
    $("#message").val(buttval);
    $("#send").click();

})
function btnAction(obj) {
    var a = $(obj).attr('value');
    // alert(a);

    // var buttval = $(".btnSubmit-sm").text();

    $("#message").val(a);
    $("#send").click();
}
