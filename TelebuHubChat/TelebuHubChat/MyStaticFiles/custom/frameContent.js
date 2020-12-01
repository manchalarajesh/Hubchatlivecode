$(document).ready(function () {
    $(".scroll").mCustomScrollbar({
        theme: "minimal"
    });

});
$(".error").parent().find(".errMsg").show();
$("#Submit").click(function () {
    $("#submitForm").addClass("hide");
    $("#chatWindow").addClass("animated fadeInRight show");
});
//$("#send").click(function () {
//    var msg = $("#message").val();
//    var text = '<div class="speechItem right"><div class="speechRight">';
//    text = text + '<p>' + msg + '</p></div><div class="time">02:30 PM</div></div>';

//    $("#conversation").html(text);
//    $("#message").val('')
//})
