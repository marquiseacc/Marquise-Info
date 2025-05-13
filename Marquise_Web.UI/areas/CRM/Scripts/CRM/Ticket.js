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
                if (data.IsSuccess) {
                    Swal.fire({
                        title: 'موفق',
                        text: data.Message || 'ثبت تیکت با موفقیت انجام شد.',
                        icon: 'success',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        const redirectUrl = data.Data?.redirectUrl || '/CRM/Ticket/Index';
                        window.location.href = redirectUrl;
                    });
                } else {
                    alert(data.message || "خطا! لطفا مجددا تلاش کنید.", 'error');
                }
            })
            .catch(error => { console.error("Error:", error); alert("لطفا مجددا تلاش کنید."); });
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
                if (data.IsSuccess) {
                    Swal.fire({
                        title: 'موفق',
                        text: data.Message || 'بروزرسانی اطلاعات با موفقیت انجام شد.',
                        icon: 'success',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        const redirectUrl = data.Data?.redirectUrl || `/CRM/Ticket/Detail?ticketId=${ticketId}`;
                        window.location.href = redirectUrl;
                    });
                } else {
                    alert(data.message || "خطا! لطفا مجددا تلاش کنید.", 'error');
                }
            })
            .catch(error => { console.error("Error:", error); alert("لطفا مجددا تلاش کنید."); });
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
                    if (data.IsSuccess) {
                        Swal.fire({
                            title: 'موفق',
                            text: data.Message || 'تیکت بسته شد.',
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        }).then(() => {
                            const redirectUrl = data.Data?.redirectUrl || '/CRM/Ticket/Index';
                            window.location.href = redirectUrl;
                        });
                    } else {
                        alert(data.message || "خطا! لطفا مجددا تلاش کنید.", 'error');
                    }
                })
                .catch(error => { console.error("Error:", error); alert("لطفا مجددا تلاش کنید."); });
        }
    });
}

