// Ensure the script runs after the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    // Open the modal for adding a user
    document.getElementById("addUserBtn").addEventListener("click", function () {
        fetch("/Admin/AddEditUser")
            .then(response => response.text())
            .then(html => {
                document.getElementById("modalContent").innerHTML = html;
                const modal = new bootstrap.Modal(document.getElementById("addEditUserModal"));
                modal.show();
            });
    });

    // Open the modal for editing a user
    document.querySelectorAll(".editUserBtn").forEach(button => {
        button.addEventListener("click", function () {
            const userId = this.getAttribute("data-id");
            fetch(`/Admin/AddEditUser/${userId}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById("modalContent").innerHTML = html;
                    const modal = new bootstrap.Modal(document.getElementById("addEditUserModal"));
                    modal.show();
                });
        });
    });

    // Remove a user
    document.querySelectorAll(".removeUserBtn").forEach(button => {
        button.addEventListener("click", function () {
            const userId = this.getAttribute("data-id");
            if (confirm("Are you sure you want to delete this user?")) {
                fetch(`/Admin/RemoveUser/${userId}`, {
                    method: "POST"
                }).then(() => {
                    location.reload(); // Reload the page after deletion
                });
            }
        });
    });
});