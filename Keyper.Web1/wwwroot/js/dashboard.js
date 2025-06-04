$(document).ready(function () {

    // Toggle Password
    $('.toggle-password').click(function () {
        const inputId = $(this).data('target');
        const input = document.getElementById(inputId);

        if (input.type === "password") {
            input.type = "text";
            $(this).text("🙈");
        } else {
            input.type = "password";
            $(this).text("👁");
        }
    });

    // Ekle
    $('#saveAddBtn').click(function () {
        const data = {
            userId: $('#addUserId').val(),
            siteName: $('#addSiteName').val(),
            siteUserName: $('#addUserName').val(),
            sitePassword: $('#addPassword').val()
        };

        $.ajax({
            url: 'http://localhost:5039/sites',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                $('#addModal').modal('hide');
                location.reload();
            }
        });
    });

    // Güncelle - modal aç
    $('.editBtn').click(function () {
        const row = $(this).closest('tr');

        $('#editId').val(row.data('id'));
        $('#editSiteName').val(row.data('name'));
        $('#editUserName').val(row.data('user'));
        $('#editPassword').val(row.data('pass'));

        $('#editModal').modal('show');
    });

    // Güncelle - kaydet
    $('#saveEditBtn').click(function () {
        const id = $('#editId').val();
        const data = {
            siteName: $('#editSiteName').val(),
            siteUserName: $('#editUserName').val(),
            sitePassword: $('#editPassword').val()
        };

        $.ajax({
            url: 'http://localhost:5039/sites/' + id,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                $('#editModal').modal('hide');
                location.reload();
            }
        });
    });

    // Sil - modal aç
    $('.deleteBtn').click(function () {
        const id = $(this).closest('tr').data('id');
        $('#deleteId').val(id);
        $('#confirmDeleteModal').modal('show');
    });

    // Sil - onayla
    $('#confirmDeleteBtn').click(function () {
        const id = $('#deleteId').val();
        console.log(id);
        $.ajax({
            url: 'http://localhost:5039/sites/' + id,
            type: 'DELETE',
            success: function () {
                $('#confirmDeleteModal').modal('hide');
                location.reload();
            }
        });
    });

});
