$(document).ready(function () {
    $("#addPermissionBtn").on("click", function () {
        $.get("/Admin/AddPermission", function (data) {
            $("#modalContent").html(data);
            $("#addPermissionModal").modal("show");
        });
    });

    // Handle form submission
    $(document).on("submit", "form", function (e) {
        e.preventDefault();
        const form = $(this);

        $.ajax({
            type: form.attr("method"),
            url: form.attr("action"),
            data: form.serialize(),
            success: function () {
                // Close the modal and reload the permissions table
                $("#addPermissionModal").modal("hide");
                location.reload();
            },
            error: function (xhr) {
                // Display validation errors inside the modal
                //$("#modalContent").html(xhr.responseText);
                $("#js-error").html("Something went wrong.");
            }
        });
    });
});
