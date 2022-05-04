$(document).ready(function () {

    $(".searchTextbox").on("keyup", function () {
        var searchResults = $(this).val();
        $(".searchTable tr").each(function (results) {
            if (results !== 0) {
                var id1 = $(this).find("td:nth-child(1)").text().trim();
                var id3 = $(this).find("td:nth-child(3)").text().trim();
                var id4 = $(this).find("td:nth-child(4)").text().trim();
                var id6 = $(this).find("td:nth-child(6)").text().trim();

                if (id1.toLowerCase().indexOf(searchResults.toLowerCase()) < 0
                    && id3.toLowerCase().indexOf(searchResults.toLowerCase()) < 0
                    && id4.toLowerCase().indexOf(searchResults.toLowerCase()) < 0
                    && id6.toLowerCase().indexOf(searchResults.toLowerCase()) < 0) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
            }
        });
    });

});