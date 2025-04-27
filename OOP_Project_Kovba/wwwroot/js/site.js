

    const profileLink = document.getElementById('profile-link');
    const profilePanel = document.getElementById('profile-panel');

    // Показываем панель при наведении на иконку профиля или саму панель
    profileLink.addEventListener('mouseover', function() {
        profilePanel.style.display = 'block';
        });

    profilePanel.addEventListener('mouseover', function() {
        profilePanel.style.display = 'block';
        });

    // Скрываем панель, когда курсор уходит с иконки профиля и самой панели
    profileLink.addEventListener('mouseleave', function() {
        setTimeout(function () {
            if (!profilePanel.matches(':hover')) {  // Если курсор не на самой панели
                profilePanel.style.display = 'none';
            }
        }, 100);  // Задержка, чтобы избежать преждевременного скрытия
        });

    profilePanel.addEventListener('mouseleave', function() {
        setTimeout(function () {
            if (!profileLink.matches(':hover')) {  // Если курсор не на иконке профиля
                profilePanel.style.display = 'none';
            }
        }, 100);  // Задержка, чтобы избежать преждевременного скрытия
        });

$(document).ready(function () {
    $('#createTripForm').validate({
        rules: {
            FromLocation: {
                required: true
            },
            ToLocation: {
                required: true
            },
            // добавь правила для остальных полей, если нужно
        },
        messages: {
            FromLocation: {
                required: "Це поле не може бути порожнім!"
            },
            ToLocation: {
                required: "Це поле не може бути порожнім!"
            },
            // добавь сообщения для остальных полей, если нужно
        }
    });
});






