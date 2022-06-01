
$(document).ready(function () {

    const statuses = ['All',
        'Referral Source',
        'Competitor',
        'Amerisource',
        'Client - Active',
        'Client - ZZ',
        'Prospect',
        'Referral Partner'];

    var clientGridData;
    var newClientGridData;

    $(function GetClientInfo() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/GetClientChartInfo",
            data: {

            },
            success: function (data) {
                clientGridData = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(function GetNewClientInfo() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Clients/GetNewClientChartInfo",
            data: {

            },
            success: function (data) {
                newClientGridData = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    // REGULAR CLIENTS DATA
    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const companyGrid = $('#clientsIndexGrid').dxDataGrid({
            dataSource: clientGridData,
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
                mode: 'multiple',
            },
            hoverStateEnabled: true,
            export: {
                enabled: true,
                allowExportSelectedData: true,
            },
            onExporting(e) {
                const workbook = new ExcelJS.Workbook();
                const worksheet = workbook.addWorksheet('Clients');

                DevExpress.excelExporter.exportDataGrid({
                    worksheet: worksheet,
                    component: e.component,
                    autoFilterEnabled: true,
                }).then(() => {
                    workbook.xlsx.writeBuffer().then((buffer) => {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Clients.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    caption: 'Owner',
                    dataField: 'OwnerName',
                    width: 200,
                }, {
                    caption: 'Client Name',
                    dataField: 'Name'
                }, {
                    caption: 'Company',
                    dataField: 'Company',
                }, {
                    caption: 'Stage',
                    dataField: 'StageName',
                }, {
                    caption: 'Amount ($)',
                    dataField: 'Amount',
                    format: { style: "currency", currency: "USD", useGrouping: true, minimumSignificantDigits: 3 },
                }, {
                    caption: 'Close Date',
                    dataField: 'CloseDate',
                }, {
                    dataField: 'ID',
                    caption: '',
                    allowSorting: false,
                    width: 100,
                    allowFiltering: false,
                    cellTemplate: function (container, options) {
                        $('<a>View</a>')
                            .attr('href', "https://localhost:44381/Clients/Details/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                        $("<span> | </span>")
                            .appendTo(container);
                        $('<a>Edit</a>')
                            .attr('href', "https://localhost:44381/Clients/Edit/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                    }
                },
            ],
            summary: {
                totalItems: [{
                    column: 'Name',
                    summaryType: 'count',
                },  {
                    column: 'Amount',
                    summaryType: 'sum',
                    valueFormat: 'currency',
                }],
            },
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



    // NEW CLIENTS DATA
    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const companyGrid = $('#newClientsIndexGrid').dxDataGrid({
            dataSource: newClientGridData,
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
                mode: 'multiple',
            },
            hoverStateEnabled: true,
            export: {
                enabled: true,
                allowExportSelectedData: true,
            },
            onExporting(e) {
                const workbook = new ExcelJS.Workbook();
                const worksheet = workbook.addWorksheet('Clients');

                DevExpress.excelExporter.exportDataGrid({
                    worksheet: worksheet,
                    component: e.component,
                    autoFilterEnabled: true,
                }).then(() => {
                    workbook.xlsx.writeBuffer().then((buffer) => {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Clients.xlsx');
                    });
                });
                e.cancel = true;
            },
            columns: [
                {
                    caption: 'Owner',
                    dataField: 'OwnerName',
                    width: 200,
                }, {
                    caption: 'Client Name',
                    dataField: 'Name'
                }, {
                    caption: 'Company',
                    dataField: 'Company',
                }, {
                    caption: 'Stage',
                    dataField: 'StageName',
                }, {
                    caption: 'Amount ($)',
                    dataField: 'Amount',
                    format: { style: "currency", currency: "USD", useGrouping: true, minimumSignificantDigits: 3 },
                }, {
                    caption: 'Close Date',
                    dataField: 'CloseDate',
                }, {
                    dataField: 'ID',
                    caption: '',
                    allowSorting: false,
                    width: 100,
                    allowFiltering: false,
                    cellTemplate: function (container, options) {
                        $('<a>View</a>')
                            .attr('href', "https://localhost:44381/Clients/Details/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                        $("<span> | </span>")
                            .appendTo(container);
                        $('<a>Edit</a>')
                            .attr('href', "https://localhost:44381/Clients/Edit/" + options.value)
                            .attr('target', '_self')
                            .appendTo(container);
                    }
                },
            ],
            summary: {
                totalItems: [{
                    column: 'Name',
                    summaryType: 'count',
                }, {
                    column: 'Amount',
                    summaryType: 'sum',
                    valueFormat: 'currency',
                }],
            },
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
        return DevExpress.data.query(clientGridData)
            .groupBy(groupField)
            .toArray().length;
    }


});