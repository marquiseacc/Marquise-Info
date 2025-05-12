window.alert = function (message,icon) {
    Swal.fire({
        title: 'پیام',
        text: message,
        icon: icon,
        confirmButtonText: 'باشه'
    });
};


function handleSentOTPFormSubmit(event) {
    event.preventDefault();

    var phoneNumber = document.getElementById("PhoneNumber").value;

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
        var formData = new FormData();
        formData.append("PhoneNumber", phoneNumber);
        
        fetch('/CRM/Auth/SendOtp', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) throw new Error("HTTP error");
                return response.json();
            })
            .then(data => {
                if (data.IsSuccess) {
                    // موفقیت در ارسال کد
                    window.location.href = data.Data.redirectUrl;
                }
                else if (data.Data && data.Data.redirectUrl) {
                    // مثلا کاربر مجاز به دریافت کد نیست => نمایش پیام و ریدایرکت بعد از تأیید
                    Swal.fire({
                        title: 'پیام',
                        text: data.Message || 'خطایی رخ داده است.',
                        icon: 'info',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        window.location.href = decodeURIComponent(data.Data.redirectUrl);
                    });
                }
                else {
                    // نمایش پیام خطا بدون ریدایرکت
                    Swal.fire({
                        title: 'خطا',
                        text: data.Message || 'خطایی رخ داده است.',
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                }
            })
            .catch(error => {
                console.error("Error:", error);
                Swal.fire({
                    title: 'خطا',
                    text: 'لطفا مجدداً تلاش کنید.',
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            });

    }

}
function handleVerifyOTPFormSubmit(event) {
    event.preventDefault();

    var phoneNumber = document.getElementById("PhoneNumber").value;
    var otpCode = document.getElementById("OtpCode").value;

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

    if (!isValid) return;

    const formData = new FormData();
    formData.append("PhoneNumber", phoneNumber);
    formData.append("Code", otpCode);

    fetch('/CRM/Auth/VerifyOtp', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) throw new Error("HTTP error");
            return response.json();
        })
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
                Swal.fire({
                    title: 'خطا',
                    text: data.Message || "کد اشتباه یا منقضی شده است.",
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            }
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                title: 'خطا',
                text: "لطفا مجددا تلاش کنید.",
                icon: 'error',
                confirmButtonText: 'باشه'
            });
        });
}


// تابع ارسال مجدد OTP
function sendOtpAgain() {
    const phoneNumber = document.getElementById("PhoneNumber").value;

    fetch('/CRM/Auth/SendOtp', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ PhoneNumber: phoneNumber })
    })
        .then(response => response.json())
        .then(data => {
            if (data.IsSuccess) {
                Swal.fire({
                    title: 'ارسال مجدد',
                    text: data.Message || 'کد یک‌بار مصرف مجدداً ارسال شد.',
                    icon: 'info',
                    confirmButtonText: 'باشه'
                });
                startTimer();
                document.getElementById("resendButton").style.display = "none";
            } else {
                Swal.fire({
                    title: 'خطا',
                    text: data.Message || 'ارسال مجدد با مشکل مواجه شد.',
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            }
        })
        .catch(error => {
            Swal.fire({
                title: 'خطا',
                text: "ارتباط با سرور برقرار نشد.",
                icon: 'error',
                confirmButtonText: 'باشه'
            });
        });
}


// تایمر OTP
let timeRemaining = 2 * 60;
let timerInterval;

function updateTimerDisplay() {
    let timerElement = document.getElementById("timer");

    if (!timerElement) {
        clearInterval(timerInterval); // تایمر را متوقف کنید چون عنصر وجود ندارد
        return;
    }

    let minutes = Math.floor(timeRemaining / 60);
    let seconds = timeRemaining % 60;
    timerElement.textContent = `${minutes}:${seconds < 10 ? '0' + seconds : seconds}`;

    if (timeRemaining <= 0) {
        clearInterval(timerInterval);
        timerElement.textContent = "زمان تمام شد";

        const resendButton = document.getElementById("resendButton");
        if (resendButton) {
            resendButton.style.display = "inline-block";
        }
    }

    timeRemaining--;
}


function startTimer() {
    timeRemaining = 2 * 60;
    clearInterval(timerInterval);
    updateTimerDisplay(); // برای نمایش اولیه
    timerInterval = setInterval(updateTimerDisplay, 1000);
}

window.onload = function () {
    startTimer();
};


function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً  "${input.name}"خود را وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
    }
    return ""; // اگر خطایی وجود ندارد
}


