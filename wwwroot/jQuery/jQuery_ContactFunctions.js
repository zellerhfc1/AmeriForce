

$(document).ready(function () {


    $("#contactSpinner").hide();
    $("#nextContactCallSpinner").hide();
    $("#mailMergeSpinner").hide();
    $("#ownerReassignSpinner").hide();

    $("#NewContactNoteSuccess").hide();
    $("#NewMailMergeSuccess").hide();
    $("#NewReassignSuccess").hide();
    $("#NextContactCallSuccess").hide();

    $("#NextContactCallError").hide();
    $("#NewMailMergeError").hide();
    $("#NewReassignError").hide();
    $("#NewContactNoteError").hide();


    var today = new Date();

    $('#taskData_ActivityDate').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });

    $('#contactData_Update_Needed_Date').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });

    $('#contactData_EmailOptOutDate').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });


    $('#contactData_EmailOptOutDate').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });


    $('#contactData_Referral_Date').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });


    $('#contactData_Referral_Partner_Agmnt_Date').datepicker({
        dateFormat: "mm/dd/yy",
        startDate: today,
        showOtherMonths: true,
        selectOtherMonths: true,
        autoclose: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0",
        //gotoCurrent: true,
        orientation: "bottom" // add this
    });



    $("#SubmitAddAContactNote").submit(function (e) {

        e.preventDefault();

        $("#contactSpinner").show();


        var contactID = $("#contactNote_Id").val();
        var nextNoteType = $("#contactNote_Type").val();
        var nextNoteOwnerID = $("#contactNote_OwnerId").val();
        var nextNoteActivityDate = $("#contactNote_ActivityDate").val();
        var nextNoteDescription = $("#contactNote_Description").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Contacts/UpdateContactNote",
            data: {
                ContactID: contactID,
                NextNoteType: nextNoteType,
                NextNoteOwnerID: nextNoteOwnerID,
                NextNoteActivityDate: nextNoteActivityDate,
                NextNoteDescription: nextNoteDescription
            },

            success: function (data) {
                setTimeout(function () {

                    $("#NewContactNoteSuccess").show();
                    $("#NewContactNoteError").hide();
                    $("#contactSpinner").hide();
                    $("#NewContactNoteSuccess").html("<strong>Note has been updated successfully!</strong>")

                    var returnToClient = "/Contacts/Details/" + contactID;
                    window.location.href = 'http://' + $(location).attr('host') + returnToClient;
                },
            4000);
            },
            fail: function (data) {
                $("#NewContactNoteSuccess").hide();
                $("#NewContactNoteError").show();
                $("#contactSpinner").hide();
            }
        });
    });


    $("#SubmitUpdateNextScheduledContactCall").submit(function (e) {

        e.preventDefault();

        $("#nextContactCallSpinner").show();

        var contactID = $("#contact_Id").val();
        var nextCallID = $("#contactNextTask_Id").val();
        var nextCallType = $("#contactNextTask_Type").val();
        var nextCallOwnerID = $("#contactNextTask_OwnerId").val();
        var nextCallActivityDate = $("#contactNextTask_ActivityDate").val();
        var nextCallDescription = $("#contactNextTask_Description").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Contacts/UpdateNextScheduledCall",
            data: {
                NextCallID: nextCallID,
                NextCallType: nextCallType,
                NextCallOwnerID: nextCallOwnerID,
                NextCallActivityDate: nextCallActivityDate,
                NextCallDescription: nextCallDescription

            },

            success: function (data) {
                setTimeout(function () {
                    console.log("success");
                    $("#NextContactCallSuccess").show();
                    $("#NextContactCallError").hide();
                    $("#nextContactCallSpinner").hide();
                    $("#NextContactCallSuccess").html("<strong>Next Scheduled Call has been updated successfully!</strong>")

                    var returnToClient = "/Contacts/Details/" + contactID;
                    window.location.href = 'https://' + $(location).attr('host') + returnToClient;

                },
            4000);
            },
            fail: function (data) {
                console.log("error");
                $("#NextContactCallSuccess").hide();
                $("#NextContactCallError").show();
                $("#nextContactCallSpinner").hide();
            }
        });
    });

    $("#SubmitContactMailMerge").submit(function (e) {

        e.preventDefault();

        $("#mailMergeSpinner").show();


        var contactMailMergeId = $("#contactMailMerge_Id").val();
        var mailMergeTemplate = $("#mailMerge_Template").val();
        var contactMailMergeOwnerId = $("#contactMailMerge_OwnerId").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Contacts/MailMergeWordDoc",
            data: {
                ContactID: contactMailMergeId,
                TemplateType: mailMergeTemplate,
                OwnerID: contactMailMergeOwnerId
            },

            success: function (data) {
                setTimeout(function () {

                    $("#NewMailMergeSuccess").show();
                    $("#NewMailMergeError").hide();
                    $("#mailMergeSpinner").hide();
                    $("#NewMailMergeSuccess").html("<strong>Mail Merge has been created successfully!</strong>")

                    var returnToClient = "/Contacts/Details/" + contactMailMergeId;
                    window.location.href = 'https://' + $(location).attr('host') + returnToClient;
                },
                    4000);
            },
            fail: function (data) {
                $("#NewMailMergeSuccess").hide();
                $("#NewMailMergeError").show();
                $("#mailMergeSpinner").hide();
            }
        });
    });




    $("#SubmitReassignContact").submit(function (e) {

        e.preventDefault();

        $("#ownerReassignSpinner").show();


        var contactReassignId = $("#contactReassign_Id").val();
        var contactReassignNewOwnerId = $("#contactReassign_NewOwnerId").val();
        var contactReassignOwnerId = $("#contactReassign_OwnerId").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Contacts/ContactReassignToNewOwner",
            data: {
                ContactID: contactReassignId,
                NewOwnerID: contactReassignNewOwnerId,
                OwnerID: contactReassignOwnerId
            },

            success: function (data) {
                setTimeout(function () {

                    $("#NewReassignSuccess").show();
                    $("#NewReassignError").hide();
                    $("#ownerReassignSpinner").hide();
                    $("#NewReassignSuccess").html("<strong>Owner has been reassigned successfully!</strong>")

                    var returnToClient = "/Contacts/Details/" + contactMailMergeId;
                    window.location.href = 'https://' + $(location).attr('host') + returnToClient;
                },
                    4000);
            },
            fail: function (data) {
                $("#NewReassignSuccess").hide();
                $("#NewReassignError").show();
                $("#ownerReassignSpinner").hide();
            }
        });
    });






// #region COMPANY INFORMATION CHECK

    $("#CompanyName").change(function () {

        var selectedValue = "";
        $("#companyData_Description").val("");
        $("#companyData_SICCode").val("");
        $("#companyData_CharterState").val("");
        // #endregion




        $.ajax({
            type: 'POST',
            url: '/Contacts/GetCompanyID',
            dataType: 'json',

            data: { enteredText: $("#CompanyName").val() },

            success: function (companyInfo) {

                $.each(companyInfo, function (i, company) {
                    $("#companyData_Description").val(company.Description);
                    $("#companyData_SICCode").val(company.SICCode);
                    $("#companyData_CharterState").val(company.CharterState);
                    $("#clientData_Referring_Company").append('<option value="' + company.Value + '">' +
                     company.Text + '</option>');
                    selectedValue = company.ID;
                    $("#CompanyId").val(company.ID);
                    alert(company.ID);
                });
                $("#clientData_Referring_Company").val(selectedValue);
                $("#CompanyId").trigger("change");
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })



    $("#CompanyId").change(function () {
        var existingReferringContact = $("#ContactId").val();

        console.log(existingReferringContact);

        $("#clientData_Referring_Contact").empty();
        $.ajax({
            type: 'POST',
            url: '/Contacts/GetContactListByCompany',

            dataType: 'json',

            data: { id: $("#clientData_Referring_Company").val() },

            success: function (contacts) {

                $.each(contacts, function (i, contact) {
                    var isSelected = "";
                    if (existingReferringContact === contact.Value) {
                        isSelected = " selected ";
                    }
                    $("#clientData_Referring_Contact").append('<option value="' + contact.Value + '" ' + isSelected + '>' +
                     contact.Text + '</option>');
                });
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })


    $("#CompanyName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Contacts/GetAutocompleteFeature_Companies",
                type: "POST",
                dataType: "json",
                data: { enteredText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Value };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
    // #endregion



    // #region REFERRAL SOURCE CHECK - COMPANY

    $("#contactData_Referring_Company").change(function () {

        var selectedValue = "";

        $.ajax({
            type: 'POST',
            url: '/Contacts/GetCompanyID',
            dataType: 'json',

            data: { enteredText: $("#contactData_Referring_Company").val() },

            success: function (companyInfo) {

                $.each(companyInfo, function (i, company) {
                    $("#clientData_Referring_Company").append('<option value="' + company.Value + '">' +
                     company.Text + '</option>');
                    selectedValue = company.ID;
                    $("#ReferringCompanyId").val(company.ID);
                });
                $("#ReferringCompanyId").trigger("change");
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })



    $("#ReferringCompanyId").change(function () {
        var existingReferringContact = $("#ReferringCompanyId").val();

        console.log(existingReferringContact);

        $("#clientData_Referring_Contact").empty();
        $.ajax({
            type: 'POST',
            url: '/Contacts/GetContactListByCompany',

            dataType: 'json',

            data: { id: $("#clientData_Referring_Company").val() },

            success: function (contacts) {

                $.each(contacts, function (i, contact) {
                    var isSelected = "";
                    if (existingReferringContact === contact.Value) {
                        isSelected = " selected ";
                    }
                    $("#clientData_Referring_Contact").append('<option value="' + contact.Value + '" ' + isSelected + '>' +
                     contact.Text + '</option>');
                });
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })


    $("#contactData_Referring_Company").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Contacts/GetAutocompleteFeature_Companies",
                type: "POST",
                dataType: "json",
                data: { enteredText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Value };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
    // #endregion




    // #region REFERRAL SOURCE CHECK - CONTACT

    $("#contactData_Referring_Contact").change(function () {

        var selectedValue = "";

        $.ajax({
            type: 'POST',
            url: '/Contacts/GetContactID',
            dataType: 'json',

            data: { enteredText: $("#contactData_Referring_Contact").val() },

            success: function (contactInfo) {

                $.each(contactInfo, function (i, contact) {
                    //$("#clientData_Referring_Company").append('<option value="' + company.Value + '">' +
                    // company.Text + '</option>');
                    selectedValue = contact.Id;
                    $("#ReferringContactId").val(contact.Id);
                    var x = $("#ReferringContactId").val();
                    var y = 1;
                });
                $("#ReferringContactId").trigger("change");
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })



    $("#contactData_Referring_Contact").change(function () {
        var existingReferringContact = $("#ReferringContactId").val();

        console.log(existingReferringContact);

        //$("#clientData_Referring_Contact").empty();
        //$.ajax({
        //    type: 'POST',
        //    url: '/Contacts/GetContactListByCompany',

        //    dataType: 'json',

        //    data: { id: $("#clientData_Referring_Company").val() },

        //    success: function (contacts) {

        //        $.each(contacts, function (i, contact) {
        //            var isSelected = "";
        //            if (existingReferringContact === contact.Value) {
        //                isSelected = " selected ";
        //            }
        //            $("#clientData_Referring_Contact").append('<option value="' + contact.Value + '" ' + isSelected + '>' +
        //             contact.Text + '</option>');
        //        });
        //    },
        //    error: function (ex) {
        //        //alert('Failed to retrieve states.' + ex);
        //    }
        //});
        //return false;
    })


    $("#contactData_Referring_Contact").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Contacts/GetAutocompleteFeature_Contacts",
                type: "POST",
                dataType: "json",
                data: { enteredText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        var fullName = item.FirstName + " " + item.LastName;
                        return { label: fullName, value: item.Value };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
    // #endregion






    $("#companyData_SICCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Contacts/GetAutocompleteFeature_SICCodes",
                type: "POST",
                dataType: "json",
                data: { enteredText: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Text, value: item.Value };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });







});