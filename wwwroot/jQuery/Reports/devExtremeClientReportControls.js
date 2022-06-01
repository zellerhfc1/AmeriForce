
$(document).ready(function () {

    var clientGridData;


    // PIPELINE REPORT
    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const pipelineGridData = $('#clientsCurrentPipelineGrid').dxDataGrid({
            dataSource: "https://localhost:44381/api/ClientsDevExtremeAPI/GetPipeline",
            filterRow: { visible: true },
            filterPanel: { visible: true },
            headerFilter: { visible: true },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: 'Search...',
            },
            grouping: {
                autoExpandAll: true,
            },
            groupPanel: {
                visible: true,
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
                    caption: 'Client Name',
                    dataField: 'Name',
                    width: 400,
                }, {
                    caption: 'Type',
                    dataField: 'Type'
                }, {
                    caption: 'Amount ($)',
                    dataField: 'Amount',
                    format: { style: "currency", currency: "USD", useGrouping: true, minimumSignificantDigits: 3 },
                }, {
                    caption: 'Stage',
                    dataField: 'Stage',
                    groupIndex: 0,
                }, {
                    caption: 'Submitted',
                    dataField: 'CreatedDate',
                }, {
                    caption: 'Close Date',
                    dataField: 'CloseDate',
                }, {
                    caption: 'Owner',
                    dataField: 'OwnerName',
                }, {
                    caption: 'Description',
                    dataField: 'Description',
                }, {
                    caption: 'Days Since Submission',
                    dataField: 'DaysSinceSubmission',
                }, {
                    caption: 'Days Since TS Issued',
                    dataField: 'DaysSinceTSIssued',
                }, {
                    caption: 'Days Since TS Received',
                    dataField: 'DaysSinceTSReceived',
                }, {
                    dataField: 'Id',
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
                pageSize: 50,
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




    // TERM SHEETS PENDING REPORT
    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const pipelineGridData = $('#clientsTermSheetsPending').dxDataGrid({
            dataSource: "https://localhost:44381/api/ClientsDevExtremeAPI/GetTermSheetsPending",
            filterRow: { visible: true },
            filterPanel: { visible: true },
            headerFilter: { visible: true },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: 'Search...',
            },
            grouping: {
                autoExpandAll: true,
            },
            groupPanel: {
                visible: true,
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
                    caption: 'Client Name',
                    dataField: 'Name',
                    width: 400,
                },  {
                    caption: 'Amount ($)',
                    dataField: 'Amount',
                    format: { style: "currency", currency: "USD", useGrouping: true, minimumSignificantDigits: 3 },
                },{
                    caption: 'Owner',
                    dataField: 'OwnerName',
                }, {
                    caption: 'Type',
                    dataField: 'Type'
                }, {
                    caption: 'Lead Source',
                    dataField: 'LeadSource',
                },{
                    caption: 'Close Date',
                    dataField: 'CloseDate',
                }, {
                    caption: 'Stage',
                    dataField: 'Stage',
                }, {
                    caption: 'Fiscal Period',
                    dataField: 'FiscalPeriod',
                }, {
                    caption: 'Age',
                    dataField: 'Age',
                }, {
                    caption: 'Created Date',
                    dataField: 'CreatedDate',
                }, {
                    caption: 'Company Name',
                    dataField: 'CompanyName',
                },
                //{
                //    caption: 'Days Since Submission',
                //    dataField: 'DaysSinceSubmission',
                //}, {
                //    caption: 'Days Since TS Issued',
                //    dataField: 'DaysSinceTSIssued',
                //}, {
                //    caption: 'Days Since TS Received',
                //    dataField: 'DaysSinceTSReceived',
                //},
                {
                    dataField: 'Id',
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
                pageSize: 50,
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