
$(document).ready(function () {

    var schedules;
    var employees;
    var chartNewUnassignedContacts;
    var chartFundedDealsYTD;
    var dataSource;
    var todaysDate = new Date();
    var appointmentContact;


    $(function GetUserInfo() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetUserInfo",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                employees = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(function GetUserTasks() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetUserTasks",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                schedules = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(function GetNewUnassignedContacts() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetNewUnassignedContacts",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                chartNewUnassignedContacts = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(function GetDealsByLeadSourceYTD() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetDealsByLeadSourceYTD",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                dataSource = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });

    $(function GetFundedDealsYTD() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetFundedDealsYTD",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                chartFundedDealsYTD = data;
                console.log(data);
            },
            fail: function (data) {
                console.log(data);
            }
        });
    });


    $(() => {
        $('.scheduler').dxScheduler({
           /* timeZone: 'America/Chicago',*/
            dataSource: schedules,
            views: ['week', 'month'],
            currentView: 'month',
            currentDate: todaysDate,
            firstDayOfWeek: 1,
            startDayHour: 1,
            endDayHour: 23,
            editing: {
                allowAdding: false,
                allowDeleting: false,
                allowUpdating: false,
                allowResizing: false,
                allowDragging: false,
            },
            showAllDayPanel: true,
            groups: ['employeeID'],
            resources: [
                {
                    fieldExpr: 'employeeID',
                    allowMultiple: false,
                    dataSource: employees,
                    label: 'Employee',
                },
            ], onAppointmentFormOpening: function (e) {
                e.popup.option('showTitle', true);
                e.popup.option('title', e.appointmentData.text ?
                    e.appointmentData.text :
                    'Create a new appointment');
                e.popup.option('contentTemplate', popupContentTemplate);
                appointmentContact = e.appointmentData;
            },
            //appointmentTooltipTemplate: function (model, index, element) {
            //    //element.append(`<div class='container-fluid'>`);
            //    //element.append(`<div class='row'>`);
            //    //element.append(`<div class='col-sm-9'><b>${model.appointmentData.text}</b><br>asfdasdfsad<br>asfdasdfsad<br>asfdasdfsad<br>asfdasdfsad</div>`);
            //    //element.append(`<div class='col-sm-3'>${model.appointmentData.taskIcon}</div>`);
            //    //element.append(`</div>`);
            //    //element.append(`</div>`);

            //    //element.append("<i>" + model.appointmentData.description + "<br>(" + model.appointmentData.startDate + ")</i>");
            //    //element.append("<p><img style='height: 80px' src='" + model.appointmentData.img + "' /></p>");
            //},
            dataCellTemplate(cellData, index, container) {
                const { employeeID } = cellData.groups;
                const apptDate = getCurrentEE(cellData.startDate.getDate(), employeeID);

                const wrapper = $('<div>')
                    .toggleClass(`employee-weekend-${employeeID}`, isWeekEnd(cellData.startDate)).appendTo(container)
                    .addClass(`employee-${employeeID}`)
                    .addClass('dx-template-wrapper');

                wrapper.append($('<div>')
                    .text(cellData.text)
                    .addClass(apptDate)
                    .addClass('day-cell'));
            },
            resourceCellTemplate(cellData) {
                const name = $('<div>')
                    .addClass('name')
                    .css({ backgroundColor: cellData.color })
                    .append($('<h2>')
                        .text(cellData.text));

                const avatar = $('<div>')
                    .addClass('avatar')
                    .html(`<img src=${cellData.data.avatar}>`)
                    .attr('title', cellData.text);

                const info = $('<div>')
                    .addClass('info')
                    .css({ color: cellData.color })
                    .html(`<br>Title: <b>${cellData.data.discipline}</b>`);

                return $('<div>').append([name, avatar, info]);
            },
        });

        function isWeekEnd(date) {
            const day = date.getDay();
            return day === 0 || day === 6;
        }

        function getCurrentEE(date, employeeID) {
            const result = (date + employeeID) % 3;
            const currentTraining = `training-background-${result}`;

            return currentTraining;
        }


        const popupContentTemplate = function () {
            var dateFormatted = formatDate(appointmentContact.startDate);
            return $('<div>').append(
                $(`<span><br>${appointmentContact.taskIcon}</span><br>`),
                $(`<span>${appointmentContact.taskType}</span><br>`),
                $(`<span>${dateFormatted}</span><br><br>`),
                $(`<span>Description: ${appointmentContact.description}</span><br><br>`),
                $(`<span><a href='https://localhost:44381/Contacts/Details/003b7-9419a982ca03' class="btn btn-sm btn-primary"><i class="fa fa-eye"></i> View Contact Info</a></span><br><br>`),
            );
        };

        function formatDate(incomingDate) {
            var d = new Date(incomingDate).toString('yyyy-MM-dd H:i:s')
            return d; 
        }

    });



    $(() => {
        $('#chart').dxChart({
            size: {
                height: 300,
            },
            dataSource: chartNewUnassignedContacts,
            title: "New Contacts",
            rotated: true,
            argumentAxis: {
                grid: {
                    visible: true
                }
            }, 
            series: {
                argumentField: 'bdo',
                valueField: 'valueCount',
                name: 'New Contacts',
                type: 'bar',
                color: '#ffaa66',
                showInLegend: false,
                label: {
                    visible: true,
                    backgroundColor: "#005eb8",
                    customizeText: function (pointInfo) {
                        return `${pointInfo.value}`;
                        //return pointInfo.argument + ': ' + pointInfoAsCurrency;
                    },
                    connector: {
                        visible: true
                    }
                }
            },
        });
    });


    $(() => {
        $('#chart2').dxChart({
            size: {
                height: 300,
            },
            dataSource: chartFundedDealsYTD,
            title: "Deals Funded YTD ($)",
            rotated: true,
            argumentAxis: {
                grid: {
                    visible: true
                }
            },
            series: {
                argumentField: 'bdo',
                valueField: 'valueCount',
                name: 'Deal Amount',
                type: 'bar',
                color: '#ffaa66',
                showInLegend: false,
                label: {
                    visible: true,
                    backgroundColor: "#005eb8",
                    customizeText: function (pointInfo) {
                        var pointInfoAsCurrency = formatMoney(pointInfo.value);
                        return `$${pointInfoAsCurrency}`;
                        //return pointInfo.argument + ': ' + pointInfoAsCurrency;
                    },
                    connector: {
                        visible: true
                    }
                }
            },
        });
    });


    //$(() => {
    //    $('#chart3').dxChart({
    //        dataSource: chartFundedDealsYTD,
    //        title: "Deals Funded YTD ($)",
    //        rotated: true,
    //        argumentAxis: {
    //            grid: {
    //                visible: true
    //            }
    //        },
    //        series: {
    //            argumentField: 'bdo',
    //            valueField: 'valueCount',
    //            name: 'Deal Amount',
    //            type: 'bar',
    //            color: '#ffaa66',
    //            showInLegend: false,
    //            label: {
    //                visible: true,
    //                backgroundColor: "#005eb8",
    //                customizeText: function (pointInfo) {
    //                    var pointInfoAsCurrency = formatMoney(pointInfo.value);
    //                    return `$${pointInfoAsCurrency}`;
    //                    //return pointInfo.argument + ': ' + pointInfoAsCurrency;
    //                },
    //                connector: {
    //                    visible: true
    //                }
    //            }
    //        },
    //    });
    //});



    $(() => {
        $('#chart3').dxPieChart({
            size: {
                height: 300,
            },
            palette: 'bright',
            dataSource,
            series: [
                {
                    argumentField: 'bdo',
                    valueField: 'valueCount',
                    showInLegend: false,
                    label: {
                        visible: true,
                        connector: {
                            visible: true,
                            width: 1,
                        },
                    },
                    //label: {
                    //    visible: true,
                    //    backgroundColor: "#005eb8",
                    //    customizeText: function (pointInfo) {
                    //        var pointInfoAsCurrency = formatMoney(pointInfo.value);
                    //        return `${pointInfo.argument} - $${pointInfoAsCurrency}`;
                    //        //return pointInfo.argument + ': ' + pointInfoAsCurrency;
                    //    },
                    //    connector: {
                    //        visible: true
                    //    }
                    //},
                },
            ],
            title: 'Lead Sources YTD',
            export: {
                enabled: true,
            },
            //onPointClick(e) {
            //    const point = e.target;

            //    toggleVisibility(point);
            //},
            //onLegendClick(e) {
            //    const arg = e.target;

            //    toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
            //},
        });

        function toggleVisibility(item) {
            if (item.isVisible()) {
                item.hide();
            } else {
                item.show();
            }
        }
    });

    //$(() => {
    //    $('#chart3').dxChart({
    //        dataSource: chartFundedDealsYTD,
    //        title: "YTD Leads",
    //        rotated: true,
    //        argumentAxis: {
    //            grid: {
    //                visible: true
    //            }
    //        },
    //        series: {
    //            argumentField: 'bdo',
    //            valueField: 'valueCount',
    //            name: 'Lead Count',
    //            type: 'bar',
    //            color: '#ffaa66',
    //        },
    //    });
    //});

    //const dataSource = [{
    //    bdo: 'Bill Herrington',
    //    valueCount: 3,
    //}, {
    //    bdo: 'Tuesday',
    //    valueCount: 2,
    //}, {
    //    bdo: 'Wednesday',
    //    valueCount: 3,
    //}, {
    //    bdo: 'Thursday',
    //    valueCount: 4,
    //}, {
    //    bdo: 'Friday',
    //    valueCount: 6,
    //}, {
    //    bdo: 'Saturday',
    //    valueCount: 11,
    //}, {
    //    bdo: 'Sunday',
    //    valueCount: 4,
    //    }];


    function formatMoney(number) {
        return number.toLocaleString('en-US');
    }


});