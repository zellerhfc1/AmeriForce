
$(document).ready(function () {

    $("#facilitySpinner").hide();
    $("#FacilitySuccess").hide();
    $("#FacilityError").hide();

    $("#nextCallSpinner").hide();
    $("#NextCallSuccess").hide();
    $("#NextCallError").hide();

    $("#noteSpinner").hide();
    $("#NewNoteSuccess").hide();
    $("#NewNoteError").hide();

    $("#mailMergeSpinner").hide();
    $("#MailMergeSuccess").hide();
    $("#MailMergeError").hide();

    $("#ClientContactSuccess").hide();
    $("#ClientContactError").hide();
    $("#clientContactSpinner").hide();

    var today = new Date();

    //$('#clientData_CloseDate').datepicker({
    //    dateFormat: "mm/dd/yy",
    //    startDate: today,
    //    showOtherMonths: true,
    //    selectOtherMonths: true,
    //    autoclose: true,
    //    changeMonth: true,
    //    changeYear: true,
    //    yearRange: "-100:+0",
    //    //gotoCurrent: true,
    //    orientation: "bottom" // add this
    //});

    $('#clientData_Referral_Date').datepicker({
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
        orientation: "bottom",
        onSelect: function () {
            $(this).trigger("focus").trigger("blur");
        }
        //onSelect: function () {
        //    $("#clientCreateForm").valid();
        //}
    });







    $('body').on('focus', ".dateDropDown", function () {
        $(this).datepicker({
            dateFormat: "mm/dd/yy",
            startDate: today,
            showOtherMonths: true,
            selectOtherMonths: true,
            autoclose: true,
            changeMonth: true,
            changeYear: true,
            //gotoCurrent: true,
            orientation: "bottom" // add this
        });
    });


    $("#SubmitAddFacility").submit(function (e) {

        e.preventDefault();

        $("#facilitySpinner").show();

        var checkboxValues = [];
        $('input[type=checkbox]:checked').each(function () {
            checkboxValues.push(this.value);
        });

        var facilityCheckboxList = checkboxValues.toString();
        facilityCheckboxList = facilityCheckboxList.replace(/,/g, "|");


        var facilityID = $("#FacilityID").val();
        var facilityName = $("#FacilityName").val();
        var facilityClientID = $("#FacilityClientID").val();
        var facilityType = $("#FacilityType").val();
        var facilityCategories = facilityCheckboxList;

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/UpdateFacility",
            data: {
                FacilityName: facilityName, FacilityClientID: facilityClientID, FacilityType: facilityType, FacilityCategories: facilityCategories, FacilityID: facilityID
            },

            success: function (data) {
                setTimeout(function () {

                    $("#FacilitySuccess").show();
                    $("#FacilityError").hide();
                    $("#facilitySpinner").hide();
                    $("#FacilitySuccess").html("<strong>Facility " + facilityName + " has been updated successfully!</strong>")

                    var returnToClient = "/Clients/Details/" + facilityClientID;
                    window.location.href = 'http://' + $(location).attr('host') + returnToClient;
                },
            4000);
            },
            fail: function (data) {
                $("#FacilitySuccess").hide();
                $("#FacilityError").show();
                $("#facilitySpinner").hide();
            }
        });
    });





    // FUNCTIONS


    $(".facilityEntry").click(function () {

        $('#FacilityID').val("");
        $('#FacilityName').val("");
        $('#FacilityType').val("");
        $("#AccountsReceivable").prop('checked', false);
        $("#Inventory").prop('checked', false);
        $("#RealEstate").prop('checked', false);
        $("#ME").prop('checked', false);
        $("#Brokerage").prop('checked', false);
        $("#Other").prop('checked', false);

        var requiredCheckboxes = $('.requiredOptions :checkbox[required]');
        requiredCheckboxes.trigger("change");


        $('#FacilityID').val($(this).data('id'));

        console.log("Facility ID --- " + $('#FacilityID').val());



        var facilityID = $("#FacilityID").val();

        if (facilityID !== "") {

            $.ajax({
                type: "POST",
                cache: false,
                async: false,
                url: "/Clients/GetFacility",
                data: {
                    FacilityID: facilityID
                },
                success: function (data) {
                    setTimeout(function () {

                        $("#FacilityName").val(data.facilityName);
                        $("#FacilityType").val(data.facilityType);

                        const arrFacilityCategories = data.facilityCategories.split("|");

                        $('.form-group input[type=checkbox]').each(function () {

                            if (arrFacilityCategories.indexOf("Accounts Receivable") >= 0) {
                                $("#AccountsReceivable").prop('checked', true);
                            }
                            if (arrFacilityCategories.indexOf("Inventory") >= 0) {
                                $("#Inventory").prop('checked', true);
                            }
                            if (arrFacilityCategories.indexOf("Real Estate") >= 0) {
                                $("#RealEstate").prop('checked', true);
                            }
                            if (arrFacilityCategories.indexOf("M & E") >= 0) {
                                $("#ME").prop('checked', true);
                            }
                            if (arrFacilityCategories.indexOf("Brokerage") >= 0) {
                                $("#Brokerage").prop('checked', true);
                            }
                            if (arrFacilityCategories.indexOf("Other") >= 0) {
                                $("#Other").prop('checked', true);
                            }
                        });

                    },
                4000);
                },
                fail: function (data) {
                    console.log("error...");
                }
            });

        }

    });


    $("#ReferringCompanyName").change(function () {

        var selectedValue = "";
        $.ajax({
            type: 'POST',
            url: '/Clients/GetCompanyID',
            dataType: 'json',

            data: { enteredText: $("#ReferringCompanyName").val() },

            success: function (companyInfo) {

                $.each(companyInfo, function (i, company) {
                    $("#clientData_Referring_Company").append('<option value="' + company.ID + '">' +
                     company.Name + '</option>');
                    selectedValue = company.ID;
                });
                $("#clientData_Referring_Company").val(selectedValue);
                $("#CompanyId").val(selectedValue);
                $("#CompanyId").trigger("change");
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })



    $("#CompanyId").change(function () {
        var existingReferringCompany = $("#CompanyId").val();

        console.log(existingReferringCompany);

        $("#clientData_Referring_Contact").empty();
        $.ajax({
            type: 'POST',
            url: '/Clients/GetContactListByCompany',

            dataType: 'json',

            data: { id: existingReferringCompany },

            success: function (contacts) {

                $.each(contacts, function (i, contact) {
                    var isSelected = "";
                    if (existingReferringCompany === contact.Value) {
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


    $("#ReferringCompanyName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Clients/GetAutocompleteFeature_Companies",
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


    $("#companyData_SICCode").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Clients/GetAutocompleteFeature_SICCodes",
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



    $("#SubmitUpdateNextScheduledCall").submit(function (e) {

        e.preventDefault();

        $("#nextCallSpinner").show();

        var clientID = $("#client_Id").val();
        var nextCallID = $("#clientNextTask_Id").val();
        var nextCallType = $("#clientNextTask_Type").val();
        var nextCallOwnerID = $("#clientNextTask_OwnerId").val();
        var nextCallActivityDate = $("#clientNextTask_ActivityDate").val();
        var nextCallDescription = $("#clientNextTask_Description").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/UpdateNextScheduledCall",
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
                    $("#NextCallSuccess").show();
                    $("#NextCallError").hide();
                    $("#nextCallSpinner").hide();
                    $("#NextCallSuccess").html("<strong>Next Scheduled Call has been updated successfully!</strong>")

                    var returnToClient = "/Clients/Details/" + clientID;
                    window.location.href = 'http://' + $(location).attr('host') + returnToClient;

                },
            4000);
            },
            fail: function (data) {
                console.log("error");
                $("#NextCallSuccess").hide();
                $("#NextCallError").show();
                $("#nextCallSpinner").hide();
            }
        });
    });





    $("#SubmitAddAClientNote").submit(function (e) {

        e.preventDefault();

        $("#noteSpinner").show();


        var clientID = $("#client_Id").val();
        var nextNoteType = $("#clientNote_Type").val();
        var nextNoteOwnerID = $("#clientNote_OwnerId").val();
        //var nextNoteActivityDate = $("#clientNote_ActivityDate").val();
        var nextNoteDescription = $("#clientNote_Description").val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/UpdateClientNote",
            data: {
                ClientId: clientID,
                NextNoteType: nextNoteType,
                NextNoteOwnerID: nextNoteOwnerID,
                //NextNoteActivityDate: nextNoteActivityDate,
                NextNoteDescription: nextNoteDescription
            },

            success: function (data) {
                setTimeout(function () {

                    $("#NewNoteSuccess").show();
                    $("#NewNoteError").hide();
                    $("#noteSpinner").hide();
                    $("#NewNoteSuccess").html("<strong>Note has been updated successfully!</strong>")

                    var returnToClient = "/Clients/Details/" + clientID;
                    window.location.href = 'http://' + $(location).attr('host') + returnToClient;
                },
            4000);
            },
            fail: function (data) {
                $("#NewNoteSuccess").hide();
                $("#NewNoteError").show();
                $("#noteSpinner").hide();
            }
        });
    });



    $(".contactEntry").click(function () {

        $('#clientContact_Title').val("");
        $('#clientContact_OwnershipPercentage').val("");
        $('#clientContact_Guarantor').val("");
        $('#clientContact_Phone').val("");
        $('#clientContact_MobilePhone').val("");
        $('#clientContact_Email').val("");

        $('#clientContact_Id').val($(this).data('id'));

        console.log("Client Contact ID --- " + $('#clientContact_Id').val());

        var clientContactID = $("#clientContact_Id").val();

        if (clientContactID !== "") {

            $.ajax({
                type: "POST",
                cache: false,
                async: false,
                url: "/Clients/GetContact",
                data: {
                    ClientContactID: clientContactID
                },
                success: function (data) {
                    setTimeout(function () {

                        $("#clientContact_Title").val(data.clientContactTitle);
                        $("#clientContact_OwnershipPercentage").val(data.clientContactOwnershipPercentage);
                        $("#clientContact_Guarantor").val(data.clientContactGuarantor.toString());
                        $("#clientContact_Phone").val(data.clientContactPhone);
                        $("#clientContact_MobilePhone").val(data.clientContactMobilePhone);
                        $("#clientContact_Email").val(data.clientContactEmail);

                        console.log(data.clientContactGuarantor);
                    },
                4000);
                },
                fail: function (data) {
                    console.log("error...");
                }
            });

        }

    });



    $("#SubmitUpdateClientContact").submit(function (e) {
        e.preventDefault();

        $("#clientContactSpinner").show();
        var clientID = $("#client_Id").val();
        var clientContactID = $("#clientContact_Id").val();
        var clientContactTitle = $('#clientContact_Title').val();
        var clientContactOwnershipPercentage = $('#clientContact_OwnershipPercentage').val();
        var clientContactGuarantor = $('#clientContact_Guarantor').val();
        var clientContactPhone = $('#clientContact_Phone').val();
        var clientContactMobilePhone = $('#clientContact_MobilePhone').val();
        var clientContactEmail = $('#clientContact_Email').val();

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/UpdateContact",
            data: {
                ClientContactID: clientContactID,
                clientContactTitle: clientContactTitle,
                clientContactOwnershipPercentage: clientContactOwnershipPercentage,
                clientContactGuarantor: clientContactGuarantor,
                clientContactPhone: clientContactPhone,
                clientContactMobilePhone: clientContactMobilePhone,
                clientContactEmail: clientContactEmail
            },

            success: function (data) {
                setTimeout(function () {

                    $("#ClientContactSuccess").show();
                    $("#ClientContactError").hide();
                    $("#clientContactSpinner").hide();
                    $("#ClientContactSuccess").html("<strong>Contact has been updated successfully!</strong>")

                    var returnToClient = "/Clients/Details/" + clientID;
                    window.location.href = 'http://' + $(location).attr('host') + returnToClient;
                },
            4000);
            },
            fail: function (data) {
                $("#ClientContactSuccess").hide();
                $("#ClientContactError").show();
                $("#clientContactSpinner").hide();
            }
        });
    });



    $("#SubmitCreateMailMergeDocument").submit(function (e) {

        e.preventDefault();

        $("#mailMergeSpinner").show();
        var clientID = $("#client_Id").val();
        var templateType = $('#templateType').val();
        var templateRecipient = $('#templateRecipient').val();


        if (clientID !== "") {

            $.ajax({
                type: "POST",
                cache: false,
                async: false,
                url: `/Clients/MailMerge/${clientID}`,
                data: {
                    ClientID: clientID,
                    TemplateType: templateType,
                    TemplateRecipient: templateRecipient
                },

                success: function (data) {
                    $("#MailMergeSuccess").show();
                    $("#MailMergeError").hide();

                    setTimeout(function () {
                        var returnToClient = "/Clients/Details/" + clientID;
                        window.location.href = 'http://' + $(location).attr('host') + returnToClient;
                    },
                4000);
                },
                fail: function (data) {
                    $("#MailMergeSuccess").hide();
                    $("#MailMergeError").show();
                    $("#mailMergeSpinner").hide();
                }
            });

        }

    });



    $("#SubmitEmailAttachments").submit(function (e)
    {

        var formData = new FormData();
        var totalFiles = document.getElementById("files").files.length;
        for (var i = 0; i < totalFiles; i++)
        {
            var file = document.getElementById("files").files[i];
            formData.append("files", file);
        }

        $.ajax({
            type: "POST",
            url: '/Clients/UploadEmailFiles',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                //alert('succes!!');
            },
            error: function (error) {
                //alert("Failed");
            }
        });


        //// Stops the form from reloading
        //e.preventDefault();

        //$.ajax({
        //    url: '/Clients/UploadEmailFiles',
        //    type: 'POST',
        //    contentType: false,
        //    processData: false,
        //    data: function () {
        //        var data = new FormData();
        //        jQuery.each(jQuery('#file')[0].files, function (i, file) {
        //            data.append('file-' + i, file);
        //        });
        //        data.append('body', $('#body').val());
        //        data.append('uid', $('#uid').val());
        //        return data;
        //    }(),
        //    success: function (result) {
        //        alert(result);
        //    },
        //    error: function (xhr, result, errorThrown) {
        //        alert('Request failed.');
        //    }
        //});
        //$('#picture').val('');
        //$('#body').val('');


        //$.ajax({
        //    url: '/Clients/UploadEmailFiles',
        //    type: 'POST',
        //    data: new FormData($('#SubmitEmailAttachments')[0]), // The form with the file inputs.
        //    processData: false,
        //    contentType: false                    // Using FormData, no need to process data.
        //}).done(function () {
        //    console.log("Success: Files sent!");
        //}).fail(function () {
        //    console.log("An error occurred, the files couldn't be sent!");
        //});


        //var data = new FormData();

        //$.each($("input[type='file']")[0].files, function (i, file) {
        //    data.append('file', file);
        //});

        //$.ajax({
        //    type: 'POST',
        //    url: '/Clients/UploadEmailFiles',
        //    cache: false,
        //    contentType: false,
        //    processData: false,
        //    data: data,
        //    success: function (result) {
        //        console.log(result);
        //    },
        //    error: function (err) {
        //        console.log(err);
        //    }
        //})
    
    });



    // #region REFERRAL SOURCE CHECK - CONTACT

    $("#CompanyName").change(function () {

        var selectedValue = ""; $("#companyData_Description").val("");
        $("#companyData_SICCode").val("");
        $("#companyData_CharterState").val("");

        $.ajax({
            type: 'POST',
            url: '/Clients/GetCompanyID',
            dataType: 'json',

            data: { enteredText: $("#CompanyName").val() },

            success: function (companyInfo) {

                $.each(companyInfo, function (i, company) {
                    $("#companyData_Description").val(company.Description);
                    $("#companyData_SICCode").val(company.SICCode);
                    $("#companyData_CharterState").val(company.CharterState);
                    $("#clientData_Referring_Company").append('<option value="' + company.ID + '">' +
                     company.Name + '</option>');
                    selectedValue = company.ID;
                });
                $("#clientData_Referring_Company").val(selectedValue);
                $("#CompanyNameId").val(selectedValue);
                $("#CompanyNameId").trigger("change");
            },
            error: function (ex) {
                //alert('Failed to retrieve states.' + ex);
            }
        });
        return false;
    })


    $("#CompanyNameId").change(function () {
        var existingReferringContact = $("#CompanyNameId").val();

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

    $("#CompanyName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Clients/GetAutocompleteFeature_Companies",
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



    $("#ReferringCompanyName").trigger("change");

});