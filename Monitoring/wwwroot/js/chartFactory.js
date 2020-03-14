class ChartFactory
{
    constructor() {
        this.charts = {};
    }
    generate(elementName) {
        var ctx = $("#" + elementName)[0].getContext("2d");
        var asd = [];
        for (var i = 0; i < 60; i++) {
            asd.push(''+i);
        }
        var chart = new Chart(ctx, {
            // The type of chart we want to create
            type: 'line',

            // The data for our dataset
            data: {
                labels: asd,
                datasets: [{
                    label: elementName +' sensor',
                    backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: [0, 10, 5, 2, 20, 30, 45, 0, 10, 5, 2, 20, 30, 45, 0, 10, 5, 2, 20, 400]
                }]
            },

            // Configuration options go here
            options: {}
        });
        this.charts[elementName] = chart;
        return chart;
    }
    updateChart(elementName, freshData) {
        if (this.charts[elementName].data.datasets[0].data.length === 60) {
            console.log("splice");
            this.charts[elementName].data.datasets[0].data.splice(0, 20);
        }
        
        this.charts[elementName].data.datasets[0].data = this.charts[elementName].data.datasets[0].data.concat(freshData);
        console.log(this.charts[elementName].data.datasets[0].data);
    }
}