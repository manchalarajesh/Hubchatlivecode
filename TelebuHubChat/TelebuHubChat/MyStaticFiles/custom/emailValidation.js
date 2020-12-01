
function isValidEmail(email) {
    // If no email or wrong value gets passed in (or nothing is passed in), immediately return false.
    if (typeof email === 'undefined') return null;
    if (typeof email !== 'string' || email.indexOf('@') === -1) return false;

    // Get email parts
    var emailParts = email.split('@'),
        emailName = emailParts[0],
        emailDomain = emailParts[1],
        emailDomainParts = emailDomain.split('.'),
        validChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.0123456789_-';

    // There must be exactly 2 parts
    if (emailParts.length !== 2) {
        return false;
    }

    // Must be at least one char before @ and 3 chars after
    if (emailName.length < 1 || emailDomain.length < 3) {
        return false;
    }

    // Domain must include but not start with .
    if (emailDomain.indexOf('.') <= 0) {
        return false;
    }

    // emailName must only include valid chars
    for (var i = emailName.length - 1; i >= 0; i--) {
        if (validChars.indexOf(emailName[i]) < 0) {
            return false;
        }
    };

    // emailDomain must only include valid chars
    for (var i = emailDomain.length - 1; i >= 0; i--) {
        if (validChars.indexOf(emailDomain[i]) < 0) {
          return false;
        }
    };

    // Domain's last . should be 2 chars or more from the end
    if (emailDomainParts[emailDomainParts.length - 1].length < 2) {
        return false;
    }

    console.log("Email address seems valid");
    return true;
}


$(document).on("input", "#txtCallerMobile", function () {
    this.value = this.value.replace(/[^\d]/g, '');
});
$(document).on("input", ".Numeric", function () {
    this.value = this.value.replace(/[^\d\.\-]/g, '');
});

$(document).on("input", "#txtCallerName", function () {

    this.value = this.value.replace(/[^a-zA-Z0-9\-\s]/g, '');
});
$(document).on("input", ".AlphaNumerics", function () {
    this.value = this.value.replace(/[^a-zA-Z0-9]/g, '');
});
$(document).on("input", ".onlynumbers", function () {
    this.value = this.value.replace(/[^\d]/g, '');
});

$(document).on("keypress", "input", function (e) {
    if (e.which == 13) {
        e.preventDefault();
        $("#btnCreateCaller").click();
    }
});


function isEmail(email) {
    // var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    // var regex = /^([\w\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    var regex = /^([\w\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (regex.test(email)) {
        return true;
    }
    else {
        return false;
    }
}





$(document).delegate("#Submit", "click", function () {

    var detailsObj = {};
    $(".lblEditErrorMsg").html("");
    var isValid = 1;
    $(".metaData .txtRequired").each(function (index) {
        if ($(this).val() == "") {
            $(".lblEditErrorMsg").html($(this).attr("key") + " is Mandatory");
            isValid = 0;
            return false;
        }
    });
    $(".metaData .NoSpecialChars").each(function (index) {
        if ($(this).val() != "") {

            var re = /[$-/:-?{-~!@"^_`\[\]]/;
            if (re.test($(this).val())) {
                $(".lblEditErrorMsg").html("Special Characters Not Allowed In " + $(this).attr("key"));
                isValid = 0;
                return false;
            }
        }
    });


    if (isValid == 0) {
        return false;
    }
    $(".metaData .ddlRequiredField").each(function (index) {
        if ($(this).val() == "Select") {
            $(".lblEditErrorMsg").html("Please select " + $(this).attr("key"))
            isValid = 0;
            return false;
        }
    });

    if (isValid == 0) {
        return false;
    }
     var callerName = "";
    var callerMobile = "";
    var flag = 0;
    
    $(".metaData .field").each(function () {

        var Key = $(this).attr("Key");
        var fieldType = $(this).attr("FieldType");
        var value = $(this).val();
        if (value == "Select") {
            value = "";
        }

        if (Key == "Name") {
            callerName = value;

        } else if (Key == "Mobile") {
            var moblen = value.length;
            if (moblen > 15 || moblen < 7) {
                $(".lblEditErrorMsg").html("Mobile Number length should be Min 7 and Max 15");
                flag = 1;
                return false;
            } else {
                callerMobile = value;
            }
        } else if (Key.toLowerCase() == "email") {
            if (!isValidEmail(value)) {
                flag = 1;
                $(".lblEditErrorMsg").html("Please Enter Valid Email");
                return false;
            } else {
                detailsObj[Key] = value;
            }
        } else {
            detailsObj[Key] = value;
        }


       
    });

    var details = JSON.stringify(detailsObj);
    var Alternatenumberstrings = "";
    var numberId;
    var indNumber; 
    


    if (flag == 0) {

        $(this).attr("disabled", "disabled");
        $(this).css("cursor", "not-allowed");
        console.log(details + "  " + callerName + "   " + callerMobile);
        //  CreateCaller(details, callerMobile, callerName, callerMobile, Caller_Id, Alternatenumberstrings);

        if (localStorage.getItem("TimeToDisplayWelcomeFrom") == "At the start of the conversation") {
            msg = "success";
            localStorage.setItem("success", "1");
            InsertCustomerInformation(AccountId, callerName, callerMobile, details);
            $("#submitForm").addClass("hide");
            $("#chatWindow").removeClass("hide");
            $("#chatWindow").addClass("animated fadeInRight show");

        } else if (localStorage.getItem("TimeToDisplayWelcomeFrom") == "Before connecting to an agent") {
            InsertCustomerInformation(AccountId, callerName, callerMobile, details);
            $(".typeArea").show();
        } else if (localStorage.getItem("TimeToDisplayWelcomeFrom") == "After agent is connected") {

        }

    }
    else {
        return false;
    }   
   
    



});




