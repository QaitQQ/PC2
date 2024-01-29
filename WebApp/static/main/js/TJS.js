
document.addEventListener('DOMContentLoaded', function () {
    var modalButtons = document.querySelectorAll('#js-open-modal');
    modalButtons.forEach(function (item) {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            var modalId = this.getAttribute('data-modal');
            modalElem = document.querySelector('.my_modal[data-modal="' + modalId + '"]');
            modalElem.classList.add('active');
            //  modalElem.classList.remove('modal');
        }); 
    });
}); 
document.addEventListener('DOMContentLoaded', function () {
    closeButtons = document.querySelectorAll('#js-modal-close');
    closeButtons.forEach(function (item) {
        item.addEventListener('click', function (e) {
            var parentModal = this.closest('.my_modal');
            parentModal.classList.remove('active');
        });
    });
}); 




//$(document).ready(function () {
//    // ����������� ������� �������� �����
//    $('#id_username').keyup(function () {
//        // ������� AJAX-�����
//        $.ajax({
//            data: $(this).serialize(), // ��������� ������ �����
//            url: "{% url 'validate_username' %}",
//            // ���� �������, ��
//            success: function (response) {
//                if (response.is_taken == true) {
//                    $('#id_username').removeClass('is-valid').addClass('is-invalid');
//                    $('#id_username').after('<div class="invalid-feedback d-block" id="usernameError">This username is not available!</div>')
//                }
//                else {
//                    $('#id_username').removeClass('is-invalid').addClass('is-valid');
//                    $('#usernameError').remove();

//                }
//            },
//            // ���� ������, ��
//            error: function (response) {
//                // ����������� �� ������
//                console.log(response.responseJSON.errors)
//            }
//        });
//        return false;
//    });
//})
