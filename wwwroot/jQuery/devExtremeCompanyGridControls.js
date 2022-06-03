
$(document).ready(function () {

    const statuses = ['All',
        'Referral Source',
        'Competitor',
        'Amerisource',
        'Client - Active',
        'Client - ZZ',
        'Prospect',
        'Referral Partner'];

    var companyGridData;

    $(function GetUserTasks() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Companies/GetCompanyChartInfo",
            data: {

            },
            success: function (data) {
                companyGridData = data;
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
        const companyGrid = $('#companiesIndexGrid').dxDataGrid({
            dataSource: companyGridData,
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
                    caption: 'Name',
                    dataField: 'name',
                    width: 300,
                }, {
                    caption: 'Company Type',
                    dataField: 'companytype'
                }, {
                    caption: 'Description',
                    dataField: 'description'
                },  {
                    caption: 'Charter State',
                    dataField: 'state',
                }, {
                    caption: 'Last Modified',
                    dataField: 'lastmodifieddate',
                }, {
                    dataField: 'ID',
                    caption: '',
                    allowSorting: false,
                    width: 100,
                    allowFiltering: false,
                    cellTemplate: function (container, options) {
                        $('<a>View</a>')
                            .attr('href', "https://localhost:44381/Companies/Details/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                        $("<span> | </span>")
                            .appendTo(container);
                        $('<a>Edit</a>')
                            .attr('href', "https://localhost:44381/Companies/Edit/" + options.value)
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