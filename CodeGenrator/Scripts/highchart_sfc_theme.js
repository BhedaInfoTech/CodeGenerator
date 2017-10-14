Highcharts.theme = {
    //colors: [ '#425073', '#999999', '#754d70', '#e67f46', '#e7b334', '#a3456b', '#dd5f5b', '#f29c21', '#dac848'],
    colors: ['#FFdb58', '#8db600', '#465b00', '#324200', '#000000'],

    chart: {
        backgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
            stops: [
                [0, 'rgb(255, 255, 255)'],
                [1, 'rgb(255, 255, 255)']
            ]
        },
        plotBackgroundColor: null,
        plotShadow: false,
        plotBorderWidth: 0
    },
    credits: {
        enabled: false
    },
    title: {
        style: {
            color: '#373f51',
            font: '14px Arial, sans-serif'
        }
    },
    subtitle: {
        style: {
            color: '#425073',
            font: '12px Arial, sans-serif'
        }
    },
    xAxis: {
        gridLineWidth: 0,
        lineColor: '#aab1bd',
        tickColor: '#aab1bd',
        labels: {
            style: {
                color: '#425073',
                font: '12px Arial, sans-serif'
            }
        },
        title: {
            style: {
                color: '#373f51',
                font: '12px Arial, sans-serif'
            }
        }
    },
    yAxis: {
        alternateGridColor: null,
        minorTickInterval: null,
        gridLineColor: '#aab1bd',
        minorGridLineColor: 'rgba(170, 177, 189, .1)',
        lineWidth: 0,
        tickWidth: 0,
        labels: {
            style: {
                color: '#425073',
                font: '12px Arial, sans-serif'
            }
        },
        title: {
            style: {
                color: '#373f51',
                font: '12px Arial, sans-serif'
            }
        }
    },
    legend: {
        itemStyle: {
                color: '#373f51',
                font: '12px Arial, sans-serif'
        },
        itemHoverStyle: {
                color: '#616e8f',
                font: '12px Arial, sans-serif'
        },
        itemHiddenStyle: {
                color: '#373f51',
                font: '12px Arial, sans-serif'
        },
        borderWidth: 0
    },
    labels: {
        style: {
                color: '#425073',
                font: '12px Arial, sans-serif'
        }
    },
    plotOptions: {
        series: {
            nullColor: '#444444'
        },
        line: {
            dataLabels: {
                color: '#CCC'
            },
            marker: {
                lineColor: '#333'
            }
        },
        spline: {
            marker: {
                lineColor: '#333'
            }
        },
        scatter: {
            marker: {
                lineColor: '#333'
            }
        },
        candlestick: {
            lineColor: 'white'
        }
    },
    toolbar: {
        itemStyle: {
            color: '#CCC'
        }
    },
    navigation: {
        buttonOptions: {
            symbolStroke: '#DDDDDD',
            hoverSymbolStroke: '#FFFFFF',
            theme: {
                fill: {
                    linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                    stops: [
                        [0.4, '#606060'],
                        [0.6, '#333333']
                    ]
                },
                stroke: '#000000'
            }
        }
    },

    // scroll charts
    rangeSelector: {
        buttonTheme: {
            fill: {
                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                stops: [
                    [0.4, '#888'],
                    [0.6, '#555']
                ]
            },
            stroke: '#000000',
            style: {
                color: '#CCC',
                fontWeight: 'bold'
            },
            states: {
                hover: {
                    fill: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0.4, '#BBB'],
                            [0.6, '#888']
                        ]
                    },
                    stroke: '#000000',
                    style: {
                        color: 'white'
                    }
                },
                select: {
                    fill: {
                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                        stops: [
                            [0.1, '#000'],
                            [0.3, '#333']
                        ]
                    },
                    stroke: '#000000',
                    style: {
                        color: 'yellow'
                    }
                }
            }
        },
        inputStyle: {
            backgroundColor: '#333',
            color: 'silver'
        },
        labelStyle: {
            color: 'silver'
        }
    },

    navigator: {
        handles: {
            backgroundColor: '#666',
            borderColor: '#AAA'
        },
        outlineColor: '#CCC',
        maskFill: 'rgba(16, 16, 16, 0.5)',
        series: {
            color: '#7798BF',
            lineColor: '#A6C7ED'
        }
    },

    scrollbar: {
        barBackgroundColor: {
                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                stops: [
                    [0.4, '#888'],
                    [0.6, '#555']
                ]
            },
        barBorderColor: '#CCC',
        buttonArrowColor: '#CCC',
        buttonBackgroundColor: {
                linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
                stops: [
                    [0.4, '#888'],
                    [0.6, '#555']
                ]
            },
        buttonBorderColor: '#CCC',
        rifleColor: '#FFF',
        trackBackgroundColor: {
            linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
            stops: [
                [0, '#000'],
                [1, '#333']
            ]
        },
        trackBorderColor: '#666'
    },
};


// Apply the theme
var highchartsOptions = Highcharts.setOptions(Highcharts.theme);



// Pie Chart
$(function () {
    // Build the chart
    $('#piechart').highcharts({
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Total Pending Forms',
            data: [
                ['Pending', 45.2],
                ['Created', 54.8],
                
            ]
        }]
    });
});
$(function () {
    // Build the chart
    $('#piechart').highcharts({
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Total Pending Forms',
            data: [
                ['Pending', 54.8],
                ['Created', 42.2],
                
            ]
        }]
    });
});


$(function () {
    // Build the chart
    $('#piechart1').highcharts({
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Total Pending Quotes',
            data: [
                ['Pending', 22.7],
                ['Created', 77.3],
                
            ]
        }]
    });
});


$(function () {
    // Build the chart
    $('#piechart2').highcharts({
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Total Pending Quotes',
            data: [
                ['Pending', 36.2],
                ['Created', 63.8],
                
            ]
        }]
    });
});


// Bar Chart
$(function () {
    $('#barchart1').highcharts({
        chart: {
            type: 'bar'
        },
        title: {
            text: null
        },
        xAxis: {
            categories: ['Quotes Pending', 'Quotes Submitted', 'Quotes Approved', 'Quotes Cancelled','Quotes Lost'],
        },
        tooltip: {
            formatter: function() {
                return '<b>'+ this.x +'</b><br/>'+
                this.series.name +': '+ this.y +'<br/>'+
                'Total: '+ this.point.stackTotal;
            }
        },
       series: [{
            stacking: 'normal',
            name: 'Quote Count',
            data: [47, 65, 55, 2,20]
        }]
    });
});
// Bar Chart
$(function () {
    $('#barchart2').highcharts({
        chart: {
            type: 'bar'
        },
        title: {
            text: null
        },
        xAxis: {
            categories: ['Sent', 'Not Sent',],
        },
        tooltip: {
            formatter: function() {
                return '<b>'+ this.x +'</b><br/>'+
                this.series.name +': '+ this.y +'<br/>'+
                'Total: '+ this.point.stackTotal;
            }
        },
       series: [{
            stacking: 'normal',
            name: 'Quote Count',
            data: [47, 65,]
        }]
    });
});
$(function () {
    // Build the chart
    $('#piechart3').highcharts({
        title: {
            text: null
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            type: 'pie',
            name: 'Total Pending Order for Payment',
            data: [
                ['Pending', 22.7],
                ['Created', 77.3],
                
            ]
        }]
    });
});