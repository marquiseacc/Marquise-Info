function handleNewTicketFormSubmit(event) {
    event.preventDefault();  

    var title = document.getElementById("title").value;
    var description = document.getElementById("description").value;

    var isValid = true;
    const form = event.target;
    const inputs = form.querySelectorAll("input");
    inputs.forEach(input => {

        input.setCustomValidity("");

        if (input.validity.valueMissing) {

            input.setCustomValidity(`لطفاً فیلد "${input.name}" را پر کنید.`);
            input.reportValidity(); 
            isValid = false;
        } else {
            const errorMessage = getCustomErrorMessage(input);
            if (errorMessage) {
                input.setCustomValidity(errorMessage);
                input.reportValidity();
                isValid = false;
            }
        }
    });
    
    if (isValid) {

        var data = {
            Title: title,
            Description: description
        }

        fetch('/api/CRM/TicketApi/NewTicket', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) { // توجه: خروجی Web API باید "success" باشه نه "IsSuccess"
                    Swal.fire({
                        title: 'موفق',
                        text: data.message || 'ثبت تیکت با موفقیت انجام شد.',
                        icon: 'success',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        const redirectUrl = '/CRM/Ticket/Index';
                        window.location.href = redirectUrl;
                    });
                } else {
                    Swal.fire({
                        title: 'خطا',
                        text: data.message || "خطایی رخ داد. لطفاً دوباره تلاش کنید.",
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                }
            })
            .catch(error => {
                console.error("Error:", error);
                Swal.fire({
                    title: 'خطا',
                    text: "مشکلی در ارتباط با سرور پیش آمد. لطفاً دوباره تلاش کنید.",
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            });

    }

    function getCustomErrorMessage(input) {
        if (input.validity.typeMismatch) {
            return `لطفاً  "${input.name}"خود را وارد کنید.`;
        }
        if (input.validity.patternMismatch) {
            return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
        }
        return "";
    }
}

function handleNewAnswerFormSubmit(event) {
    event.preventDefault();

    var message = document.getElementById("message").value;
    var ticketId = document.getElementById("ticketId").value;

    var isValid = true;
    const form = event.target;
    const inputs = form.querySelectorAll("input");
    inputs.forEach(input => {

        input.setCustomValidity("");

        if (input.validity.valueMissing) {

            input.setCustomValidity(`لطفاً فیلد "${input.name}" را پر کنید.`);
            input.reportValidity();
            isValid = false;
        } else {
            const errorMessage = getCustomErrorMessage(input);
            if (errorMessage) {
                input.setCustomValidity(errorMessage);
                input.reportValidity();
                isValid = false;
            }
        }
    });

    if (isValid) {

        var data = {
            Message: message,
            TicketId: ticketId
        }

        fetch('/api/CRM/TicketApi/NewAnswer', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({
                        title: 'موفق',
                        text: data.message || 'پاسخ با موفقیت ثبت شد.',
                        icon: 'success',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        const redirectUrl = '/CRM/Ticket/Detail?ticketId=' + ticketId;
                        window.location.href = redirectUrl;
                    });
                } else {
                    Swal.fire({
                        title: 'خطا',
                        text: data.message || 'ثبت پاسخ با خطا مواجه شد.',
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                }
            })
            .catch(error => {
                console.error('Error:', error);
                Swal.fire({
                    title: 'خطا',
                    text: 'مشکلی در ارتباط با سرور پیش آمد.',
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            });

    }

    function getCustomErrorMessage(input) {
        if (input.validity.typeMismatch) {
            return `لطفاً  "${input.name}"خود را وارد کنید.`;
        }
        if (input.validity.patternMismatch) {
            return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
        }
        return "";
    }
}

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".close-ticket-btn").forEach(function (btn) {
        btn.addEventListener("click", function () {
            var ticketId = this.getAttribute("data-ticketId");
            console.log(ticketId);
            closeTicket(ticketId);
        });
    });
});

function closeTicket(id) {

    Swal.fire({
        title: 'پیام',
        text: 'از بستن این تیکت مطمئن هستید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، تیکت بسته بشه',
        cancelButtonText: 'لغو'
    }).then((result) => {
        if (result.isConfirmed) {
            var data = {
                TicketId: id
            }
            fetch('/api/CRM/TicketApi/CloseTicket', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({
                            title: 'موفق',
                            text: data.message || 'تیکت بسته شد.',
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        }).then(() => {
                            const redirectUrl = '/CRM/Ticket/Detail?ticketId=' + id;
                            window.location.href = redirectUrl;
                        });
                    } else {
                        Swal.fire({
                            title: 'خطا',
                            text: data.message || 'بستن تیکت با خطا مواجه شد.',
                            icon: 'error',
                            confirmButtonText: 'باشه'
                        });
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    Swal.fire({
                        title: 'خطا',
                        text: "مشکلی در ارتباط با سرور پیش آمد. لطفاً دوباره تلاش کنید.",
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                });

        }
    });
}

