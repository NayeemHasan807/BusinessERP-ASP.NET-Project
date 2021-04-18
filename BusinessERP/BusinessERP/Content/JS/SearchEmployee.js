$(document).ready(function(){

	$('input[type=text]').keyup(function () {
		let json = {
			"key": $('#searchkey').val()
		}
		$.ajax({
			url: '/Employee/SearchEmployeeByName',
			type: 'GET',
			dataType:'json',
			data: json,
			success: function (data) {
				$('#searchresult').html = data.val;
			},
			error: function (error) {
			}
		});

	});			
});