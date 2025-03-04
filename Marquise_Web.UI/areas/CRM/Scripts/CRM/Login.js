function handleLoginFormSubmit(event) {
    event.preventDefault();

    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    var rememberMe = document.getElementById('rememberMe').checked;

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
            Username: username,
            Password: password,
        };

        fetch('/api/CRM/AccountApi/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert("ورود با موفقیت انجام شد.");
                    if (rememberMe) {
                        setCookie('username', username, 7);
                        setCookie('password', password, 7);
                        setCookie('rememberMe', rememberMe, 7);
                    } else {
                        deleteCookie("username");
                        deleteCookie("password");
                        deleteCookie("rememberMe");
                    }
                    console.log(data.redirectUrl);
                    // هدایت به آدرس مشخص‌شده در پاسخ
                    window.location.href = data.redirectUrl;
                } else {
                    alert(data.message || "خطا! لطفا مجددا تلاش کنید");
                }
            })
            .catch(error => {
                console.error("Error:", error);
                alert("لطفا مجددا تلاش کنید.");
            });
    }


}


function handleForgetPassFormSubmit(event) {
    event.preventDefault();

    var phone = document.getElementById("phone").value;
    var phoneErrorDiv = document.getElementById("phone-error");

    phoneErrorDiv.innerHTML = "";

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



    var phonePattern = /^09(?!(\d)\1{8})\d{9}$/;

    if (!phonePattern.test(phone)) {
        phoneErrorDiv.innerHTML = "لطفاً یک شماره تلفن معتبر وارد کنید.";
        isValid = false;
    }


    if (isValid) {

        var data = {
            PhoneNumber: phone,
        }

        fetch('/api/CRM/LoginAPI/ForgetPassword', {
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


function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً  "${input.name}"خود را وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
    }
    return ""; // اگر خطایی وجود ندارد
}


function encryptData(data) {
    if (typeof data !== 'string') {
        console.error("Data must be a string!");
        return null;
    }

    var secretKey = "Marquise33";  // کلید رمزنگاری شما
    var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Base64);

    try {
        var encrypted = CryptoJS.AES.encrypt(data, hashedKey).toString();
        return encrypted;
    } catch (error) {
        console.error("Error during encryption:", error);
        return null;
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


function encryptData(data) {
    if (typeof data !== 'string') {
        console.error("Data must be a string!");
        return null;
    }

    var secretKey = "Marquise33";  // کلید رمزنگاری شما
    var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Base64);

    try {
        var encrypted = CryptoJS.AES.encrypt(data, hashedKey).toString();
        return encrypted;
    } catch (error) {
        console.error("Error during encryption:", error);
        return null;
    }
}


function setCookie(name, value, days) {
    try {
        // اطمینان از اینکه مقدار به رشته تبدیل شده است
        var stringValue = value.toString();  // تبدیل به رشته
        var encryptedValue = encryptData(stringValue);  // رمزگذاری مقدار

        if (!encryptedValue) {
            console.error("Failed to encrypt data. Cannot set cookie.");
            return;
        }

        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));  // مدت زمان انقضا
        var expires = "expires=" + date.toUTCString();
        document.cookie = name + "=" + encryptedValue + ";" + expires + ";path=/";  // ذخیره کوکی
    } catch (e) {
        console.error("Error in setCookie:", e);
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


function encryptData(data) {
    if (typeof data !== 'string') {
        console.error("Data must be a string!");
        return null;
    }

    var secretKey = "Marquise33";  // کلید رمزنگاری شما
    var hashedKey = CryptoJS.SHA256(secretKey).toString(CryptoJS.enc.Base64);

    try {
        var encrypted = CryptoJS.AES.encrypt(data, hashedKey).toString();
        return encrypted;
    } catch (error) {
        console.error("Error during encryption:", error);
        return null;
    }
}


function setCookie(name, value, days) {
    try {
        // اطمینان از اینکه مقدار به رشته تبدیل شده است
        var stringValue = value.toString();  // تبدیل به رشته
        var encryptedValue = encryptData(stringValue);  // رمزگذاری مقدار

        if (!encryptedValue) {
            console.error("Failed to encrypt data. Cannot set cookie.");
            return;
        }

        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));  // مدت زمان انقضا
        var expires = "expires=" + date.toUTCString();
        document.cookie = name + "=" + encryptedValue + ";" + expires + ";path=/";  // ذخیره کوکی
    } catch (e) {
        console.error("Error in setCookie:", e);
    }
}


function deleteCookie(name) {
    // حذف کوکی برای مسیر اصلی
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";

    // حذف کوکی برای مسیر جاری (در صورتی که مسیر متفاوت باشد)
    var pathParts = window.location.pathname.split('/');
    for (var i = 0; i < pathParts.length; i++) {
        var path = pathParts.slice(0, i + 1).join('/');
        document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=" + path + ";";
    }
}