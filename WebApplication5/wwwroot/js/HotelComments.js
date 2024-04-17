// Function to load comments
function loadComments(hotelId) {
    $.ajax({
        url: '/TravelGroupManagement/HotelComment/GetComments?hotelId=' + hotelId,
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
    var hotelId = $('#hotelComments input[name="HotelId"]').val();
    // Call loadComments on page load
    loadComments(hotelId);

    // Submit event addCommentForm
    $('#addCommentForm').submit(function (e) {
        e.preventDefault();
        var formData = {
            HotelId: hotelId,
            Content: $('#hotelComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/TravelGroupManagement/HotelComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData), // Serialize the formData object
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $('#hotelComments textarea[name="Content"]').val('');
                    loadComments(hotelId);
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


