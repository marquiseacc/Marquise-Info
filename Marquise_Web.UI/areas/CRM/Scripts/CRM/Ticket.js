function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً مقدار صحیحی برای "${input.name}" وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `لطفاً الگوی صحیح برای "${input.name}" را رعایت کنید.`;
    }
    return "";
}

function handleNewTicketFormSubmit(event) {
    event.preventDefault();

    const title = document.getElementById("title").value.trim();
    const description = document.getElementById("description").value.trim();

    let isValid = true;
    const form = event.target;
    const inputs = form.querySelectorAll("input, textarea");

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

    if (!isValid) return;

    const data = {
        Title: title,
        Description: description
    };

    fetchWithLoading('/api/CRM/TicketApi/NewTicket', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem("jwtToken")
        },
        body: JSON.stringify(data)
    }, '#formNewTicket')  // لودر روی فرم یا بخش مشخص
        .then(response => response.json())
        .then(data => {
            if (data.IsSuccess) {
                Swal.fire({
                    title: 'موفق',
                    text: data.Message || 'ثبت تیکت با موفقیت انجام شد.',
                    icon: 'success',
                    confirmButtonText: 'باشه'
                }).then(() => {
                    window.location.href = '/CRM/Ticket/Index';
                });
            } else {
                Swal.fire({
                    title: 'خطا',
                    text: data.Message || "خطایی رخ داد. لطفاً دوباره تلاش کنید.",
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

function handleNewAnswerFormSubmit(event) {
    event.preventDefault();

    const message = document.getElementById("message").value.trim();
    const ticketId = document.getElementById("ticketId").value.trim();

    let isValid = true;
    const form = event.target;
    const inputs = form.querySelectorAll("input, textarea");

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

    if (!isValid) return;

    const data = {
        Message: message,
        TicketId: ticketId
    };

    fetchWithLoading('/api/CRM/TicketApi/NewAnswer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem("jwtToken")
        },
        body: JSON.stringify(data)
    }, '#formNewAnswer')
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                Swal.fire({
                    title: 'موفق',
                    text: data.message || 'پاسخ با موفقیت ثبت شد.',
                    icon: 'success',
                    confirmButtonText: 'باشه'
                }).then(() => {
                    window.location.href = '/CRM/Ticket/DetailPage?ticketId=' + ticketId;
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

function handleCloseTicket(id) {
    Swal.fire({
        title: 'پیام',
        text: 'از بستن این تیکت مطمئن هستید؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'بله، تیکت بسته شود',
        cancelButtonText: 'لغو'
    }).then(result => {
        if (result.isConfirmed) {
            const data = { TicketId: id };

            fetchWithLoading('/api/CRM/TicketApi/CloseTicket', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem("jwtToken")
                },
                body: JSON.stringify(data)
            }, '#ticketListContainer')
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({
                            title: 'موفق',
                            text: data.message || 'تیکت بسته شد.',
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        }).then(() => {
                            window.location.href = '/CRM/Ticket/DetailPage?ticketId=' + id;
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
