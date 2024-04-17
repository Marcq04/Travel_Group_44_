// Function to load comments
function loadComments(flightId) {
    $.ajax({
        url: '/TravelGroupManagement/FlightComment/GetComments?flightId=' + flightId,
        method: 'GET',
        success: function (data) {
            var commentsHtml = '';
            for (var i = 0; i < data.length; i++) {
                commentsHtml += '<div class="comment">';
                commentsHtml += '<p>' + data[i].Content + '</p>';
                commentsHtml += '<span>Posted on: ' + new Date(data[i].createdDate).toLocaleString() + '</span>';
                commentsHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        },
        error: function (xhr, status, error) {
            console.error('Error loading comments:', error);
        }
    });
}


$(document).ready(function () {
    var flightId = $('#flightComments input[name="FlightId"]').val();
    // Call loadComments on page load
    loadComments(flightId);

    // Submit event addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            FlightId: flightId,
            Content: $('#flightComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/TravelGroupManagement/FlightComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData), // Serialize the formData object
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $('#flightComments textarea[name="Content"]').val('');
                    loadComments(flightId);
                }
                else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                alert("Error: " + error);
            }
        })
    })
})


