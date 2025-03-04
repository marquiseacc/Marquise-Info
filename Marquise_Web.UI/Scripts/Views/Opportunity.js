function handleOpportunityFormSubmit(event) {
    event.preventDefault();

    var email = document.getElementById("email").value;
    var phone = document.getElementById("phone").value;
    var name = document.getElementById("name").value;
    var message = document.getElementById("message").value;
    var address = document.getElementById("address").value;
    var birthday = document.getElementById("birthday").value;
    const fileInput = document.getElementById('file');
    const file = fileInput.files[0];

    var emailErrorDiv = document.getElementById("email-error");
    var phoneErrorDiv = document.getElementById("phone-error");
    var birthdayErrorDiv = document.getElementById("birthday-error");

    if (emailErrorDiv) emailErrorDiv.innerHTML = "";
    if (phoneErrorDiv) phoneErrorDiv.innerHTML = "";
    if (birthdayErrorDiv) birthdayErrorDiv.innerHTML = "";

    var isValid = true;

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

    var emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    if (!emailPattern.test(email)) {
        if (emailErrorDiv) emailErrorDiv.innerHTML = "لطفاً یک ایمیل معتبر وارد کنید.";
        isValid = false;
    }

    var phonePattern = /^09\d{9}$/;
    if (!phonePattern.test(phone)) {
        if (phoneErrorDiv) phoneErrorDiv.innerHTML = "لطفاً یک شماره تلفن معتبر وارد کنید.";
        isValid = false;
    }

    var birthdayPattern = /^(13[4-9][0-9]|14[0-9][0-9])\/(0[1-9]|1[0-2])\/(0[1-9]|[12][0-9]|3[01])$/;
    if (!birthday || !birthdayPattern.test(birthday.trim())) {
        if (birthdayErrorDiv) birthdayErrorDiv.innerHTML = "لطفاً یک تاریخ معتبر وارد کنید.";
        isValid = false;
    }

    if (isValid) {
        if (file) {
            var reader = new FileReader();
            reader.onloadend = function (event) {
                const base64File = event.target.result.split(',')[1];
                var data = {
                    Name: name,
                    Email: email,
                    PhoneNumber: phone,
                    Message: message,
                    Address: address,
                    Birthday: birthday.trim(),
                    File: base64File,
                    FileName: file.name,
                    FileType: file.type
                };

                fetch('/api/MessageApi/Opportunity', {
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
                        } else {
                            alert("خطایی رخ داد. لطفاً مجدداً تلاش کنید.");
                        }
                    })
                    .catch(error => {
                        console.error("Error:", error);
                        alert("لطفاً مجدداً تلاش کنید.");
                    });
            };
            reader.readAsDataURL(file);
        } else {
            var data = {
                Name: name,
                Email: email,
                PhoneNumber: phone,
                Message: message,
                Address: address,
                Birthday: birthday.trim()
            };

            fetch('/api/Message/Opportunity', {
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
                    } else {
                        alert("خطایی رخ داد. لطفاً مجدداً تلاش کنید.");
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    alert("لطفاً مجدداً تلاش کنید.");
                });
        }
    }

    function getCustomErrorMessage(input) {
        if (input.validity.typeMismatch) {
            return `لطفاً "${input.name}" را صحیح وارد کنید.`;
        }
        if (input.validity.patternMismatch) {
            return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
        }
        return "";
    }
}
