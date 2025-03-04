function handleContactFormSubmit(event) {
    event.preventDefault();  // جلوگیری از ارسال فرم به صورت پیش‌فرض


    // گرفتن مقادیر فیلدهای ورودی
    var email = document.getElementById("email").value;
    var phone = document.getElementById("phone").value;
    var name = document.getElementById("name").value;
    var message = document.getElementById("message").value;

    // گرفتن div‌های پیام خطا
    var emailErrorDiv = document.getElementById("email-error");
    var phoneErrorDiv = document.getElementById("phone-error");

    // ابتدا پیام‌های خطا را پاک می‌کنیم
    emailErrorDiv.innerHTML = "";
    phoneErrorDiv.innerHTML = "";

    var isValid = true;
    const form = event.target;
    const inputs = form.querySelectorAll("input");
    inputs.forEach(input => {
        // پاک کردن پیام خطا برای شروع جدید
        input.setCustomValidity("");

        if (input.validity.valueMissing) {
            // پیام خاص برای فیلدهایی که required هستند
            input.setCustomValidity(`لطفاً فیلد "${input.name}" را پر کنید.`);
            input.reportValidity(); // نمایش پیام خطا
            isValid = false;
        } else {
            // سایر اعتبارسنجی‌ها
            const errorMessage = getCustomErrorMessage(input);
            if (errorMessage) {
                input.setCustomValidity(errorMessage);
                input.reportValidity();
                isValid = false;
            }
        }
    });
    // بررسی اعتبار ایمیل
    var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailPattern.test(email)) {
        emailErrorDiv.innerHTML = "لطفاً یک ایمیل معتبر وارد کنید.";
        isValid = false;
    }

    // بررسی اعتبار شماره تلفن
    var phonePattern = /^09(?!(\d)\1{8})\d{9}$/;

    if (!phonePattern.test(phone)) {
        phoneErrorDiv.innerHTML = "لطفاً یک شماره تلفن معتبر وارد کنید.";
        isValid = false;
    }

    // اگر همه فیلدها معتبر بودند، داده‌ها را به مدل مپ می‌کنیم
    if (isValid) {

        var data = {
            Name: name,
            Email: email,
            PhoneNumber: phone,
            Message: message
        }

        fetch('/api/MessageApi/Contact', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert("پیام شما با موفقیت ثبت شد.");
                }
                else {
                    alert("پیام شما با موفقیت ثبت شد.");
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
        return ""; // اگر خطایی وجود ندارد
    }
}