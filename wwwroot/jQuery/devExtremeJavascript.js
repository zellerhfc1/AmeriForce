
$(document).ready(function () {

    var schedules;
    var employees;
    var chartInfo;
    var todaysDate = new Date();

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

    $(function GetChartInfo() {

        $.ajax({
            type: "POST",
            cache: false,
            async: false,
            url: "/Home/GetChartInfo",
            data: {
                //OwnerID: ownerID
            },
            success: function (data) {
                chartInfo = data;
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


    $(() => {
        $('.scheduler').dxScheduler({
            timeZone: 'America/Los_Angeles',
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
            ],
            dataCellTemplate(cellData, index, container) {
                const { employeeID } = cellData.groups;
                const currentTraining = getCurrentTraining(cellData.startDate.getDate(), employeeID);

                const wrapper = $('<div>')
                    .toggleClass(`employee-weekend-${employeeID}`, isWeekEnd(cellData.startDate)).appendTo(container)
                    .addClass(`employee-${employeeID}`)
                    .addClass('dx-template-wrapper');

                wrapper.append($('<div>')
                    .text(cellData.text)
                    .addClass(currentTraining)
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

        function getCurrentTraining(date, employeeID) {
            const result = (date + employeeID) % 3;
            const currentTraining = `training-background-${result}`;

            return currentTraining;
        }
    });


    //var newText = String(employeee["textes"]);

    //const employees = [{
    //    text: `${employeee.color}`, //employeee.text.toString(),
    //    id: employeee.id,
    //    color: employeee.color,
    //    avatar: employeee.avatar,
    //    discipline: employeee.discipline,
    //}];

    //const employees = [{
    //    text: 'asdf', //employeee.text.toString(),
    //    id: 1,
    //    color: "#ff0000",
    //    avatar: "images/loriSquare.png",
    //    discipline: "BDO",
    //}];


    $(() => {
        $('#chart').dxChart({
            dataSource:chartInfo,
            title: "YTD Leads",
            rotated: true,
            argumentAxis: {
                grid: {
                    visible: true
                }
            }, 
            series: {
                argumentField: 'bdo',
                valueField: 'valueCount',
                name: 'Lead Count',
                type: 'bar',
                color: '#ffaa66',
            },
        });
    });

    $(() => {
        $('#chart2').dxChart({
            dataSource: chartInfo,
            title: "YTD Leads",
            rotated: true,
            argumentAxis: {
                grid: {
                    visible: true
                }
            },
            series: {
                argumentField: 'bdo',
                valueField: 'valueCount',
                name: 'Lead Count',
                type: 'bar',
                color: '#ffaa66',
            },
        });
    });

    $(() => {
        $('#chart3').dxChart({
            dataSource: chartInfo,
            title: "YTD Leads",
            rotated: true,
            argumentAxis: {
                grid: {
                    visible: true
                }
            },
            series: {
                argumentField: 'bdo',
                valueField: 'valueCount',
                name: 'Lead Count',
                type: 'bar',
                color: '#ffaa66',
            },
        });
    });

    const dataSource = [{
        bdo: 'Bill Herrington',
        valueCount: 3,
    }, {
        bdo: 'Tuesday',
        valueCount: 2,
    }, {
        bdo: 'Wednesday',
        valueCount: 3,
    }, {
        bdo: 'Thursday',
        valueCount: 4,
    }, {
        bdo: 'Friday',
        valueCount: 6,
    }, {
        bdo: 'Saturday',
        valueCount: 11,
    }, {
        bdo: 'Sunday',
        valueCount: 4,
        }];



});