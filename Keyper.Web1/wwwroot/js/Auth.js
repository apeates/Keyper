$(document).ready(function () {
    $('#toLogin').click(function () {
        $('#formTitle').text('Giriş Yap');
        $('#registerForm').addClass('d-none');
        $('#loginForm').removeClass('d-none');
    });

    $('#toRegister').click(function () {
        $('#formTitle').text('Kayıt Ol');
        $('#loginForm').addClass('d-none');
        $('#registerForm').removeClass('d-none');
    });

    $('#registerBtn').click(function () {
        const data = {
            userName: $('#regUsername').val(),
            password: $('#regPassword').val(),
            email: $('#regEmail').val(),
            firstName: $('#regFirstName').val(),
            lastName: $('#regLastName').val() 
        };

        if (!data.userName || !data.password || !data.email || !data.firstName || !data.lastName) {
            $('#registerMessage').html('<div class="text-danger">Lütfen tüm alanları doldurun.</div>');
            return;
        }

        $.ajax({
            url: 'http://localhost:5039/account/register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                $('#registerMessage').html('<div class="text-success">Kayıt başarılı!</div>');
                setTimeout(function () {
                    $('#toLogin').click();
                }, 1000);
            },
            error: function (error) {
                console.log(error);
                $('#registerMessage').html('<div class="text-danger">Kayıt başarısız.</div>');
            }
        });
    });

    $('#loginBtn').click(function (e) {
        e.preventDefault();

        const data = {
            UserName: $('#loginUsername').val(),
            Password: $('#loginPassword').val()
        };

        if (!data.UserName?.trim() || !data.Password?.trim()) {
            $('#loginMessage').html('<div class="text-danger">Boş alan bırakmayın.</div>');
            return;
        }

        $.ajax({
            url: '/Account/Login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            xhrFields: {
                withCredentials: true // Cookie gönderimi için gerekli
            },
            success: function (res) {
                let result = res;

                // Eğer response JSON değilse, yani string gibi geldiyse
                if (typeof res === "string") {
                    try {
                        result = JSON.parse(res);
                    } catch (e) {
                        // JSON parse edilemedi, yine sabit mesaj göster
                        $('#loginMessage').html('<div class="text-danger">Kullanıcı adı veya Şifre hatalı</div>');
                        return;
                    }
                }

                if (result.success) {
                    $('#loginMessage').html('<div class="text-success">Giriş başarılı!</div>');
                    setTimeout(function () {
                        window.location.href = '/Account/Dashboard';
                    }, 500);
                } else {
                    $('#loginMessage').html('<div class="text-danger">Kullanıcı adı veya Şifre hatalı</div>');
                }
            }

        });
    });
});
