
$(document).ready(function () {

    $("form[name='clientEditForm']").validate({
        rules: {
            "clientData.OwnerId": "required",
            "clientData.StageName": "required"
            //clientData_Name: "required",
            //companyData_Name: "required",
            //companyData_Description: "required",
            //companyData_SICCode: "required",
            //contactData_LastName: "required",
            //"clientData.OwnerId": "required",
            //"taskData.Type": "required",
            //"taskData.OwnerId": "required",
            //"taskData_ActivityDate": "required",
            //"taskData_Description": "required",
            //"CompanyName": "required",
            //email: {
            //    required: true,
            //    email: true
            //},
            //password: {
            //    required: true,
            //    minlength: 5
            //}
        },
        messages: {
            "clientData.OwnerId": "",
            "clientData.StageName": ""
            //clientData_Name: "",
            //companyData_Name: "",
            //companyData_Description: "",
            //companyData_SICCode: "",
            //contactData_LastName: "",
            //"clientData.OwnerId": "",
            //"taskData.Type": "",
            //"taskData.OwnerId": "",
            //"taskData_ActivityDate": "",
            //"taskData_Description": "",
            //"CompanyName": "",
            //password: {
            //    required: "Please provide a password",
            //    minlength: "Your password must be at least 5 characters long"
            //},
            //email: "Please enter a valid email address"
        },
        highlight: function (element) {
            var elementName = "IG_" + $(element).attr("name");
            elementName = elementName.replace(".", "_");
            $('#' + elementName)
                            .removeClass("validatedClass")
                            .removeClass("requiredClass")
                           .addClass("text-validation");
        },
        unhighlight: function (element) {
            var elementName = "IG_" + $(element).attr("name");
            elementName = elementName.replace(".", "_");
            $(element).removeClass("text-validation");
            $(element.form).find("label[for=" + elementName + "]")
                           .removeClass("text-validation");
        },

        submitHandler: function (form) {
            form.submit();
        }
    });

});