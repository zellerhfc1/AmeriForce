
$(document).ready(function () {

    // Validates that at least one item of a checkbox list is checked
    // Used with <div class="form-group options">
    $(function () {
        var requiredCheckboxes = $('.requiredOptions :checkbox[required]');
        requiredCheckboxes.change(function () {
            if (requiredCheckboxes.is(':checked')) {
                requiredCheckboxes.removeAttr('required');
            } else {
                requiredCheckboxes.attr('required', 'required');
            }
        });
    });

    //$("#modalAddFacility").on('shown', function () {
    //    console.log("I want this to appear after the modal has opened!");
    //});

    $('#modalAddFacility').on('shown.bs.modal', function (e) {
        var x = "";
        x = $('#FacilityID').val();
        console.log(x);
        console.log("BS3 -- I want this to appear after the modal has opened!");
    })

    $('#modalAddFacility').on('hide.bs.modal', function (e) {
        var x = $('#FacilityID').val("");
        console.log(x);
        console.log("BS3 -- I want this to appear after the modal has opened!");
    })

    function DoNothing(e) {
        e.PreventDefault();
    };
    
});