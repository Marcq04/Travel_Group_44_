// Function to load comments
function loadComments(carRentalId) {
    $.ajax({
        url: '/TravelGroupManagement/CarRentalComment/GetComments?carRentalId=' + carRentalId,
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
    var carRentalId = $('#carRentalComments input[name="CarRentalId"]').val();
    // Call loadComments on page load
    loadComments(carRentalId);

    // Submit event addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            CarRentalId: carRentalId,
            Content: $('#carRentalComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/TravelGroupManagement/CarRentalComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData), // Serialize the formData object
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $('#carRentalComment textarea[name="Content"]').val('');
                    loadComments(carRentalId);
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


