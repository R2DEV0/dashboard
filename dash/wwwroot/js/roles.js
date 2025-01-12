// Ensure the script runs after the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function () {
    // Open the modal for adding a role
    document.getElementById("addRoleBtn").addEventListener("click", function () {
        fetch("/Admin/AddEditRole")
            .then(response => response.text())
            .then(html => {
                document.getElementById("modalContent").innerHTML = html;
                const modal = new bootstrap.Modal(document.getElementById("addEditRoleModal"));
                modal.show();
            });
    });

    // Open the modal for editing a role
    document.querySelectorAll(".editRoleBtn").forEach(button => {
        button.addEventListener("click", function () {
            const roleId = this.getAttribute("data-id");
            fetch(`/Admin/AddEditRole/${roleId}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById("modalContent").innerHTML = html;
                    const modal = new bootstrap.Modal(document.getElementById("addEditRoleModal"));
                    modal.show();
                });
        });
    });

    // Remove a role
    document.querySelectorAll(".removeRoleBtn").forEach(button => {
        button.addEventListener("click", function () {
            const roleId = this.getAttribute("data-id");
            if (confirm("Are you sure you want to delete this role?")) {
                fetch(`/Admin/RemoveRole/${roleId}`, {
                    method: "POST"
                }).then(() => {
                    location.reload(); // Reload the page after deletion
                });
            }
        });
    });
});