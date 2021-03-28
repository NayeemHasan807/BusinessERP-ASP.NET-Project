$(document).ready(function () {
    $.ajax({
        type: "Post",
        url: "/Support/SupportLogPieChart",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            successFunc(response);
        }
    });

    function successFunc(jsondata) {
        var ctx = document.getElementById('myChart').getContext('2d');
        console.log(jsondata.value);
        var myChart = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: jsondata['category'],
                datasets: [{
                    data: jsondata['value'],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    }
});