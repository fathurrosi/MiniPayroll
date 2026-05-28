document.addEventListener('DOMContentLoaded', function () {
    document.addEventListener('submit', function (e) {
        const form = e.target;
        if (!(form instanceof HTMLFormElement)) return;
        form.querySelectorAll('input[type="text"], textarea').forEach(el => {
            if (!el.value) return;
            el.value = el.value
                .trim()
                .replace(/\s{2,}/g, ' ');
        });
    }, true);
});
$(document).ajaxError(function (event, xhr) {
    if (xhr.status === 403) {
        var message = 'You are not authorized to perform this action.';
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'warning',
            title: 'Warning',
            title: message,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    }
    if (xhr.status === 401) {
        window.location.href = '/Home/Logout';
    }
});
window.setReadonly = function (readonly) {
    $('[asp-key="true"]').prop('readonly', readonly)
        .toggleClass('asp-key-readonly', readonly);

    const $mode = $('#Mode');
    if ($mode.length) {
        $mode.val(readonly ? 'Edit' : 'Create');
    }
};
window.getEditMode = function () {
    var $keyFields = $('[asp-key="true"]');

    if ($keyFields.length === 0) return false;

    return $keyFields.is('[readonly]') ||
        $keyFields.hasClass('asp-key-readonly');
};
window.notify = {
    success: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'success',
            title: message,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    },
    error: function (message) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: message,
            confirmButtonColor: '#d33'
        });
    },
    showHtml: function (message) {
        Swal.fire({
            draggable: true,
            html: message,
            confirmButtonColor: '#d33'
        });
    },
    warning: function (message) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            icon: 'warning',
            title: message,
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true
        });
    },
    info: function (message) {
        Swal.fire({
            icon: 'info',
            title: 'Info',
            text: message
        });
    }
};
function confirmDelete(callback) {
    Swal.fire({
        title: 'Delete Confirmation',
        text: 'Are you sure you want to delete this',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel',
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}

function confirmCallback(callback) {
    Swal.fire({
        title: 'Confirmation',
        text: 'Are you sure you want to do this',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel',
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-secondary'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}

function initDataTable(selector, ajaxUrl, columns, title) {
    return $(selector).DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: ajaxUrl,
            type: 'POST',
            contentType: 'application/json',
            data: d => JSON.stringify(d),
            dataSrc: 'data'
        },
        columns: columns,
        dom: 'Biftlp',
        buttons: [
            {
                extend: 'csvHtml5',
                title: title,
                className: 'd-none',
                exportOptions: { columns: ':visible' }
            },
            {
                extend: 'pdfHtml5',
                title: title,
                className: 'd-none',
                orientation: 'portrait',
                pageSize: 'A4',
                exportOptions: { columns: ':visible' }
            }
        ]
    });
}