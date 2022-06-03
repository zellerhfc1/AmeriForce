
$(document).ready(function () {


    $("form[name='contactCreateForm']").validate({
        rules: {
            contactData_LastName: "required",
            "contactData.OwnerId": "required",
            "taskData.Type": "required",
            "taskData.OwnerId": "required",
            "taskData_ActivityDate": "required",
            "taskData_Description": "required",
            "CompanyName": "required",
            "contactData_PreferredName": "required",
            "contactData.Relationship_Status": "required",
            "contactData.Rating_Sort": "required",
            //"contactData.Update_Needed": "required",
            "contactData_Initial_Meeting_Details": "required",
            email: {
                required: true,
                email: true
            },
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            contactData_LastName: "",
            "contactData.OwnerId": "",
            "taskData.Type": "",
            "taskData.OwnerId": "",
            "taskData_ActivityDate": "",
            "taskData_Description": "",
            "CompanyName": "",
            "contactData_PreferredName": "",
            "contactData.Relationship_Status": "",
            "contactData.Rating_Sort": "",
            //"contactData.Update_Needed": "",
            "contactData_Initial_Meeting_Details": "",
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
            email: "Please enter a valid email address"
        },
        highlight: function (element) {
            if (element.id.indexOf("LastName") > 0) {
                let elementName = "IG_" + $(element).attr("name");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
                console.log(elementName + " --- " + element.id.indexOf("LastName") + " --- BAD");
            };
            if (element.id.indexOf("OwnerId") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                if (elementName.indexOf("client") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
                if (elementName.indexOf("task") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
                if (elementName.indexOf("contact") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
            };
            if (element.id.indexOf("Type") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("ActivityDate") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Description") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id === "CompanyName") {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("CompanyName") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("PreferredName") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Relationship_Status") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Rating_Sort") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Initial_Meeting_Details") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            //IG_taskData_Type
        }, unhighlight: function (element) {
            if (element.id.indexOf("LastName") > 0) {
                let elementName = "IG_" + $(element).attr("name");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
                console.log(elementName + " --- " + element.id.indexOf("LastName") + " --- GOOD");
            };
            if (element.id.indexOf("OwnerId") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                if (elementName.indexOf("client") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
                if (elementName.indexOf("task") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
                if (elementName.indexOf("contact") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
            };
            if (element.id.indexOf("Type") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("ActivityDate") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Description") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id === "CompanyName") {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("CompanyName") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("PreferredName") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Relationship_Status") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Rating_Sort") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Initial_Meeting_Details") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
        },

        submitHandler: function (form) {
            form.submit();
        }
    });



    $("form[name='contactEditForm']").validate({
        rules: {
            contactData_LastName: "required",
            "contactData.OwnerId": "required",
            "taskData.Type": "required",
            "taskData.OwnerId": "required",
            "taskData_ActivityDate": "required",
            "taskData_Description": "required",
            "CompanyName": "required",
            "contactData_PreferredName": "required",
            "contactData.Relationship_Status": "required",
            "contactData.Rating_Sort": "required",
            //"contactData.Update_Needed": "required",
            "contactData_Initial_Meeting_Details": "required",
            email: {
                required: true,
                email: true
            },
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            contactData_LastName: "",
            "contactData.OwnerId": "",
            "taskData.Type": "",
            "taskData.OwnerId": "",
            "taskData_ActivityDate": "",
            "taskData_Description": "",
            "CompanyName": "",
            "contactData_PreferredName": "",
            "contactData.Relationship_Status": "",
            "contactData.Rating_Sort": "",
            //"contactData.Update_Needed": "",
            "contactData_Initial_Meeting_Details": "",
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
            email: "Please enter a valid email address"
        },
        highlight: function (element) {
            if (element.id.indexOf("LastName") > 0) {
                let elementName = "IG_" + $(element).attr("name");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
                console.log(elementName + " --- " + element.id.indexOf("LastName") + " --- BAD");
            };
            if (element.id.indexOf("OwnerId") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                if (elementName.indexOf("client") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
                if (elementName.indexOf("task") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
                if (elementName.indexOf("contact") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                    $('#' + elementName).removeClass("validatedClass");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).addClass("text-validation");
                };
            };
            if (element.id.indexOf("Type") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("ActivityDate") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Description") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id === "CompanyName") {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("CompanyName") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("PreferredName") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Relationship_Status") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Rating_Sort") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            if (element.id.indexOf("Initial_Meeting_Details") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- BAD");
                $('#' + elementName).removeClass("validatedClass");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).addClass("text-validation");
            };
            //IG_taskData_Type
        }, unhighlight: function (element) {
            if (element.id.indexOf("LastName") > 0) {
                let elementName = "IG_" + $(element).attr("name");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
                console.log(elementName + " --- " + element.id.indexOf("LastName") + " --- GOOD");
            };
            if (element.id.indexOf("OwnerId") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                if (elementName.indexOf("client") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
                if (elementName.indexOf("task") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
                if (elementName.indexOf("contact") > 0) {
                    console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                    $('#' + elementName).removeClass("requiredClass");
                    $('#' + elementName).removeClass("text-validation");
                    $('#' + elementName).addClass("validatedClass");
                };
            };
            if (element.id.indexOf("Type") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("OwnerId") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("ActivityDate") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Description") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id === "CompanyName") {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("CompanyName") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("PreferredName") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Relationship_Status") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Rating_Sort") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
            if (element.id.indexOf("Initial_Meeting_Details") > 0) {
                let elementName = "IG_" + $(element).attr("name").toString();
                elementName = elementName.replace(".", "_");
                console.log(elementName + " --- " + element.id.indexOf("ActivityDate") + " --- GOOD");
                $('#' + elementName).removeClass("requiredClass");
                $('#' + elementName).removeClass("text-validation");
                $('#' + elementName).addClass("validatedClass");
            };
        },

        submitHandler: function (form) {
            form.submit();
        }
    });

});