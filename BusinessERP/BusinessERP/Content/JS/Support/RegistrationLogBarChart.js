$(document).ready(function () {
    $.ajax({
        type: "Post",
        url: "/Support/RegistrationLogBarChart",
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
            type: 'bar',
            data: {
                labels: jsondata['category'],
                datasets: [{
                    label: 'Accepted',
                    data: jsondata['acceptedvalue'],
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)'
                    ],
                    borderWidth: 1
                },
                    {
                        label: 'Rejected',
                        data: jsondata['rejectedvalue'],
                        backgroundColor: [
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)'
                        ],
                        borderWidth: 1
                    }
                ]
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