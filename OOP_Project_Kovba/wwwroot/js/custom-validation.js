$(document).ready(function () {
    $.extend($.validator.messages, {
        required: "Це поле є обов'язковим.",
        number: "Введіть коректне число.",
        min: $.validator.format("Значення повинно бути більше або дорівнювати {0}."),
    });

    // Перезапуск валидации (если меняются поля динамически)
    $("form").each(function () {
        $(this).removeData("validator");
        $(this).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($(this));
    });
});
