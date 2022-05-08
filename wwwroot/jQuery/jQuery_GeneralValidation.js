$(document).ready(function () {

    // INPUT TYPES
    $('input.currencyField').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentVal = $(this).val();
        var testDecimal = testDecimals(currentVal);
        if (testDecimal.length > 1) {
            //console.log("You cannot enter more than one decimal point");
            currentVal = currentVal.slice(0, -1);
        }
        $(this).val(replaceCommas(currentVal));
    });


    $('.emailAddress').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentVal = $(this).val();
        var isEmailTrue = isEmail(currentVal);
        console.log(isEmailTrue);

        //var testDecimal = testDecimals(currentVal);
        //if (testDecimal.length > 1) {
        //    //console.log("You cannot enter more than one decimal point");
        //    currentVal = currentVal.slice(0, -1);
        //}
        //$(this).val(replaceCommas(currentVal));
    });


    $(".phone").keyup(function () {
        if (/\D/g.test(this.value)) {
            // Filter non-digits from input value.
            this.value = this.value.replace(/\D/g, '');
        }
        $(this).val($(this).val().replace(/(\d{3})\-?(\d{3})\-?(\d{4})/, '$1-$2-$3'));
    });


    $('.zip').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) {
            event.preventDefault();
        }

        var currentVal = $(this).val();
        var testDecimal = testDecimals(currentVal);
        if (testDecimal.length > 0) {
            //console.log("You cannot enter any decimal points");
            currentVal = currentVal.slice(0, -1);
        }
        $(this).val(replaceAlphaCharacters(currentVal));
    });

    $(".email").on('change', function () {
        var email = this.value;
        var isExistingUser = "false";
        $.ajax({
            data: { userEmail: email },
            type: 'POST',
            cache: false,
            async: false,
            url: '/Account/DoesUserAlreadyExist',
            success: function (data) {
                // SUCCESSFUL INQUIRY
                isExistingUser = data.isUser;
                return isExistingUser;
            },
            fail: function (data) {
                return "failed";
            }
        });

        if (isExistingUser == "true") {
            $("#PreExistingLogin").show();
            $("#SubmitRegistration").attr("disabled", true);
        } else {
            $("#PreExistingLogin").hide();
            $("#SubmitRegistration").attr("disabled", false);
        }
    });






    // FUNCTIONS
    function testDecimals(currentVal) {
        var count;
        currentVal.match(/\./g) === null ? count = 0 : count = currentVal.match(/\./g);
        return count;
    }

    function replaceCommas(yourNumber) {
        var components = yourNumber.toString().split(".");
        if (components.length === 1)
            components[0] = yourNumber;
        components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (components.length === 2)
            components[1] = components[1].replace(/\D/g, "");
        return components.join(".");
    }


    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }


    function replaceAlphaCharacters(yourNumber) {
        var components = yourNumber.toString().split(".");
        if (components.length === 1)
            components[0] = yourNumber;
        components[0] = components[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, "");
        if (components.length === 2)
            components[1] = components[1].replace(/\D/g, "");
        return components.join(".");
    }
});