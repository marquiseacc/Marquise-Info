function handleForgetPassFormSubmit(event) {
    event.preventDefault();

    var phone = document.getElementById("phone").value;

    var phoneErrorDiv = document.getElementById("phone-error");


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

        var phonePattern = /^09(?!(\d)\1{8})\d{9}$/;

        if (!phonePattern.test(phone)) {
            phoneErrorDiv.innerHTML = "لطفاً یک شماره تلفن معتبر وارد کنید.";
            isValid = false;
        }


        if (isValid) {
            var data = {
                phone: phone
            }

            fetch('/CRM/Account/GetPhoneNumber', {
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
                })
                .catch(error => {
                    console.error("Error:", error); alert("لطفا مجددا تلاش کنید.");
                });
        }

    });
}