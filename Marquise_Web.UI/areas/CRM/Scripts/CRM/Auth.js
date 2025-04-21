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
        var data = {
            PhoneNumber: phoneNumber
        };

        
        var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch('/CRM/Auth/SendOtp', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token 
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok) throw new Error("HTTP error");
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    window.location.href = data.redirectUrl;
                }
                else if (data.redirectUrl) {
                    alert("لطفا برای پیوستن به همراهان مارکیز از طریق صفحه تماس با ما با تیم مارکیز در ارتباط باشید.");
                    window.location.href = decodeURIComponent(data.redirectUrl);
                }
                else {
                    alert(data.message || "خطایی رخ داده است");
                }
            })
            .catch(error => {
                console.error("Error:", error);
                alert("لطفا مجددا تلاش کنید.");
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

    if (isValid) {
        var data = {
            PhoneNumber: phoneNumber,
            Code: otpCode
        };

        // گرفتن توکن ضد جعل (AntiForgeryToken) از input hidden
        var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        fetch('/CRM/Auth/VerifyOtp', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok) throw new Error("HTTP error");
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    alert("ورود با موفقیت انجام شد.");
                    // انتقال کاربر به داشبورد با CRMId
                    window.location.href = '/Dashboard/Index?CrmId=' + data.crmId;
                } else {
                    alert(data.message || "خطا! لطفا مجددا تلاش کنید.");
                }
            })
            .catch(error => {
                console.error("Error:", error);
                alert("لطفا مجددا تلاش کنید.");
            });
    }
}


function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً  "${input.name}"خود را وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
    }
    return ""; // اگر خطایی وجود ندارد
}


function fetchCrmData(crmId) {
    return fetch('/CRM/Account/GetData/' + crmId)
        .then(response => {
            if (!response.ok) throw new Error("Failed to fetch CRM data");
            return response.json();
        })
        .then(data => {
            return data; // داده‌های CRM برگشتی
        })
        .catch(error => {
            console.error("CRM Data Fetch Error:", error);
            return null;
        });
}




    let timeRemaining = 2 * 60; // 2 دقیقه به ثانیه
    let timerInterval;

    // تابع برای به روز رسانی زمان تایمر
    function updateTimerDisplay() {
        let minutes = Math.floor(timeRemaining / 60);
        let seconds = timeRemaining % 60;
        document.getElementById("timer").textContent = `${minutes}:${seconds < 10 ? '0' + seconds : seconds}`;

        if (timeRemaining <= 0) {
            clearInterval(timerInterval);
            document.getElementById("timer").textContent = "زمان تمام شد";
            document.getElementById("resendButton").style.display = "inline-block"; // نمایش دکمه ارسال مجدد
        }

        timeRemaining--;
    }

    // شروع تایمر
    function startTimer() {
        timeRemaining = 2 * 60; // بازنشانی تایمر
        clearInterval(timerInterval); // تایمر قبلی را متوقف می‌کنیم
        timerInterval = setInterval(updateTimerDisplay, 1000); // تایمر جدید را شروع می‌کنیم
    }

    // ارسال کد مجدد
    function sendOtpAgain() {
        const phoneNumber = document.getElementById("PhoneNumber").value;  // شماره تماس مدل را به اینجا منتقل کنید

        // ارسال درخواست برای ارسال کد OTP مجدد به سرور با AJAX
        fetch('/CRM/Auth/SendOtp', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ PhoneNumber: phoneNumber })
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert("کد مجدد ارسال شد.");
                startTimer(); // تایمر را دوباره شروع می‌کنیم
                document.getElementById("resendButton").style.display = "none"; // مخفی کردن دکمه ارسال مجدد
            } else {
                alert("خطا در ارسال کد مجدد.");
            }
        })
        .catch(error => {
            alert("خطا در ارتباط با سرور.");
        });
    }

    // شروع تایمر در بارگذاری صفحه
    window.onload = function() {
        startTimer();
    };
