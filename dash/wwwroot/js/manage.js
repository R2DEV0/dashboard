
document.addEventListener("DOMContentLoaded", function () {
    // Open the modal for editing a user account
    document.querySelectorAll(".editAccountBtn").forEach(button => {
        button.addEventListener("click", function () {
            const userId = this.getAttribute("data-id");
            fetch(`/Account/EditAccount/${userId}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById("modalContent").innerHTML = html;
                    const modal = new bootstrap.Modal(document.getElementById("editAccountModal"));
                    modal.show();
                });
        });
    });
});