// Ensure the script runs after the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    // Open the modal for adding a permission
    document.getElementById("addPermissionBtn").addEventListener("click", function () {
        fetch("/Admin/AddEditPermission")
            .then(response => response.text())
            .then(html => {
                document.getElementById("modalContent").innerHTML = html;
                const modal = new bootstrap.Modal(document.getElementById("addEditPermissionModal"));
                modal.show();
            });
    });

    // Open the modal for editing a permission
    document.querySelectorAll(".editPermissionBtn").forEach(button => {
        button.addEventListener("click", function () {
            const permissionId = this.getAttribute("data-id");
            fetch(`/Admin/AddEditPermission/${permissionId}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById("modalContent").innerHTML = html;
                    const modal = new bootstrap.Modal(document.getElementById("addEditPermissionModal"));
                    modal.show();
                });
        });
    });

    // Remove a permission
    document.querySelectorAll(".removePermissionBtn").forEach(button => {
        button.addEventListener("click", function () {
            const permissionId = this.getAttribute("data-id");
            if (confirm("Are you sure you want to delete this permission?")) {
                fetch(`/Admin/RemovePermission/${permissionId}`, {
                    method: "POST"
                }).then(() => {
                    location.reload(); // Reload the page after deletion
                });
            }
        });
    });
});
