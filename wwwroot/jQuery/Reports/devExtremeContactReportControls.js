
$(document).ready(function () {

    var contactGridData;

    // CONTACTS IN SPECIFIC STATES
    $(() => {
        //$('#selectStatus').dxSelectBox({
        //    dataSource: statuses,
        //    value: statuses[0],
        //    onValueChanged(data) {
        //        if (data.value === 'All') { companyGrid.clearFilter(); } else { companyGrid.filter(['companytype', '=', data.value]); }
        //    },
        //});
        const contactsInStatesGridData = $('#contactsInSpecificStatesGrid').dxDataGrid({
            dataSource: "https://localhost:44381/api/ContactsDevExtremeAPI/GetReferralPartnersInSpecificStates",
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
                    caption: 'First Name',
                    dataField: 'FirstName',
                }, {
                    caption: 'Last Name',
                    dataField: 'LastName',
                },{
                    caption: 'Company Name',
                    dataField: 'CompanyName'
                }, {
                    caption: 'Mailing Street',
                    dataField: 'MailingStreet'
                }, {
                    caption: 'Mailing City',
                    dataField: 'MailingCity'
                }, {
                    caption: 'Mailing State',
                    dataField: 'MailingState',
                    groupIndex: 0,
                }, {
                    caption: 'Mailing Zip Code',
                    dataField: 'MailingPostalCode'
                }, {
                    caption: 'Phone',
                    dataField: 'Phone',
                }, {
                    caption: 'Mobile',
                    dataField: 'Mobile',
                }, {
                    caption: 'Email',
                    dataField: 'Email',
                }, {
                    caption: 'Tag/Grade/Sort',
                    dataField: 'Rating_Sort',
                }, {
                    caption: 'Relationship Status',
                    dataField: 'RelationshipStatus',
                }, {
                    caption: 'Company Type',
                    dataField: 'CompanyType',
                }, {
                    caption: 'Opt Out',
                    dataField: 'OptOut',
                }, {
                    caption: 'Email Opt Out',
                    dataField: 'EmailOptOut',
                }, {
                    caption: 'Contact Owner',
                    dataField: 'OwnerName',
                }, {
                    dataField: 'Id',
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
            summary: {
                totalItems: [{
                    column: 'Id',
                    summaryType: 'count',
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