
$(document).ready(function () {

    const statuses = ['All',
        'Referral Source',
        'Competitor',
        'Amerisource',
        'Client - Active',
        'Client - ZZ',
        'Prospect',
        'Referral Partner'];

    var contactGridData;

    $(function GetContactInfo() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Contacts/GetContactChartInfo",
            data: {

            },
            success: function (data) {
                contactGridData = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const companyGrid = $('#contactsIndexGrid').dxDataGrid({
            dataSource: contactGridData,
            loadPanel: {
                enabled: true,
                height: 300,
                width: 300,
            },
            filterRow: { visible: true },
            filterPanel: { visible: true },
            headerFilter: { visible: true },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: 'Search...',
            },
            //toolbar: {
            //    items: [
            //        {
            //            location: 'before',
            //            template() {
            //                return $('<div>')
            //                    .addClass('informer')
            //                    .append(
            //                        $('<h3>')
            //                            .addClass('nameSubHeaderNextCall')
            //                            .text(getGroupCount('name') + " Contacts"),
            //                    );
            //            },
            //        },],
            //},
            selection: {
                mode: 'single',
            },
            hoverStateEnabled: true,
            columns: [
                {
                    caption: 'Owner',
                    dataField: 'OwnerName',
                    width: 200,
                }, {
                    caption: 'Grade',
                    dataField: 'Grade'
                }, {
                    caption: 'Contact Name',
                    dataField: 'ContactName'
                },  {
                    caption: 'Company',
                    dataField: 'Company',
                }, {
                    caption: 'Phone',
                    dataField: 'Phone',
                }, {
                    caption: 'Email',
                    dataField: 'Email',
                },{
                    dataField: 'ID',
                    caption: '',
                    allowSorting: false,
                    width: 100,
                    allowFiltering: false,
                    cellTemplate: function (container, options) {
                        $('<a>View</a>')
                            .attr('href', "https://localhost:44381/Contacts/Details/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                        $("<span> | </span>")
                            .appendTo(container);
                        $('<a>Edit</a>')
                            .attr('href', "https://localhost:44381/Contacts/Edit/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                    }
                },
            ],
            showBorders: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: 'Search...',
            },
            paging: {
                pageSize: 10,
            },
            pager: {
                visible: true,
                allowedPageSizes: [5, 10, 25, 50, 100, 200, 'all'],
                showPageSizeSelector: true,
                showInfo: true,
                showNavigationButtons: true,
            },
        });
    });

    function getGroupCount(groupField) {
        return DevExpress.data.query(companyGridData)
            .groupBy(groupField)
            .toArray().length;
    }


});