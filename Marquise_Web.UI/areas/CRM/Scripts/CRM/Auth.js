// جایگزینی alert با SweetAlert
window.alert = function (message, icon = 'info') {
    Swal.fire({
        title: 'پیام',
        text: message,
        icon,
        confirmButtonText: 'باشه'
    });
};

// فرم ارسال OTP
function handleSentOTPFormSubmit(event) {
    event.preventDefault();

    if (!validateForm(event.target)) return;

    const phoneNumber = document.getElementById("PhoneNumber").value;
    const formData = new FormData();
    formData.append("PhoneNumber", phoneNumber);

    fetch('/CRM/Auth/SendOtp', {
        method: 'POST',
        body: formData
    })
        .then(handleResponse)
        .then(data => {
            if (data.IsSuccess) {
                window.location.href = data.Data.redirectUrl;
            } else if (data.Data?.redirectUrl) {
                Swal.fire({
                    title: 'پیام',
                    text: data.Message || 'خطایی رخ داده است.',
                    icon: 'info',
                    confirmButtonText: 'باشه'
                }).then(() => window.location.href = decodeURIComponent(data.Data.redirectUrl));
            } else {
                window.alert(data.Message || 'خطایی رخ داده است.', 'error');
            }
        })
        .catch(handleError);
}

// فرم تایید OTP
function handleVerifyOTPFormSubmit(event) {
    event.preventDefault();

    if (!validateForm(event.target)) return;

    const formData = new FormData(event.target);

    fetch('/CRM/Auth/VerifyOtp', {
        method: 'POST',
        body: formData
    })
        .then(handleResponse)
        .then(data => {
            if (data.IsSuccess) {
                Swal.fire({
                    title: 'موفق',
                    text: data.Message || 'ورود با موفقیت انجام شد.',
                    icon: 'success',
                    confirmButtonText: 'باشه'
                }).then(() => {
                    const redirectUrl = data.Data?.redirectUrl || '/CRM/Dashboard/Index';
                    window.location.href = redirectUrl;
                });
            } else {
                window.alert(data.Message || 'کد اشتباه یا منقضی شده است.', 'error');
            }
        })
        .catch(handleError);
}

// ارسال مجدد OTP
function sendOtpAgain() {
    const phoneNumber = document.getElementById("PhoneNumber").value;

    fetch('/CRM/Auth/SendOtp', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ PhoneNumber: phoneNumber })
    })
        .then(handleResponse)
        .then(data => {
            if (data.IsSuccess) {
                window.alert(data.Message || 'کد یک‌بار مصرف مجدداً ارسال شد.', 'info');
                startTimer();
                document.getElementById("resendButton").style.display = "none";
            } else {
                window.alert(data.Message || 'ارسال مجدد با مشکل مواجه شد.', 'error');
            }
        })
        .catch(handleError);
}

// اعتبارسنجی فرم به صورت عمومی
function validateForm(form) {
    let isValid = true;
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

    return isValid;
}

// پیام‌های سفارشی برای ورودی‌ها
function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً مقدار معتبر برای "${input.name}" وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
    }
    return "";
}

// مدیریت پاسخ JSON
function handleResponse(response) {
    if (!response.ok) throw new Error("خطا در ارتباط با سرور");
    return response.json();
}

// مدیریت خطاهای عمومی
function handleError(error) {
    console.error("Error:", error);
    window.alert("خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.", "error");
}

// مدیریت تایمر
let timeRemaining = 120;
let timerInterval;

function updateTimerDisplay() {
    const timerElement = document.getElementById("timer");

    if (!timerElement) {
        clearInterval(timerInterval);
        return;
    }

    const minutes = Math.floor(timeRemaining / 60);
    const seconds = timeRemaining % 60;
    timerElement.textContent = `${minutes}:${seconds < 10 ? '0' + seconds : seconds}`;

    if (timeRemaining <= 0) {
        clearInterval(timerInterval);
        timerElement.textContent = "زمان تمام شد";
        const resendButton = document.getElementById("resendButton");
        if (resendButton) resendButton.style.display = "inline-block";
    }

    timeRemaining--;
}

function startTimer() {
    timeRemaining = 120;
    clearInterval(timerInterval);
    updateTimerDisplay(); // بار اول
    timerInterval = setInterval(updateTimerDisplay, 1000);
}

window.onload = startTimer;
