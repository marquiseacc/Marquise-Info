document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem("jwtToken");

    fetch('/CRM/Account/AccountDetail', {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + token
        }
    })
        .then(response => {
            if (!response.ok) {
                document.getElementById("account-detail-container").innerText = "❌ خطا در دریافت اطلاعات حساب کاربری";
                return '';
            }
            return response.text();
        })
        .then(html => {
            if (html) {
                document.getElementById("account-detail-container").innerHTML = html;
            }
        })
        .catch(error => {
            console.error("❌ خطا:", error);
        });
});

function handleUpdateAccountFormSubmit(event) {
    event.preventDefault();

    const token = localStorage.getItem("jwtToken"); // دریافت توکن از localStorage

    var name = document.getElementById("name").value;
    var industryCode = document.getElementById("IndustryCode").value;
    var shippingAddress = document.getElementById("shippingAddress").value;
    var city = document.getElementById("city").value;
    var mahale = document.getElementById("mahale").value;
    var phone = document.getElementById("phone").value;
    var mobile = document.getElementById("mobile").value;

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
            Name: name,
            IndustryCode: industryCode,
            ShippingAddress: shippingAddress,
            mahale__C: mahale,
            cituu__C: city,
            Telephone: phone,
            Mobile: mobile
        };

        fetch('/api/CRM/AccountApi/UpdateAccount', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token // ارسال توکن در هدر
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(data => {
                console.log(data);
                if (data.IsSuccess) {
                    Swal.fire({
                        title: 'موفق',
                        text: data.Message || 'بروزرسانی اطلاعات با موفقیت انجام شد.',
                        icon: 'success',
                        confirmButtonText: 'باشه'
                    }).then(() => {
                        window.location.href = '/CRM/Account/Index';
                    });
                } else {
                    alert(data.Message || "خطا! لطفا مجددا تلاش کنید.");
                }
            })
            .catch(error => {
                console.error("Error:", error);
                alert("لطفا مجددا تلاش کنید.");
            });
    }

    function getCustomErrorMessage(input) {
        if (input.validity.typeMismatch) {
            return `لطفاً "${input.name}" را درست وارد کنید.`;
        }
        if (input.validity.patternMismatch) {
            return `لطفاً الگوی صحیح برای "${input.name}" را وارد کنید.`;
        }
        return "";
    }
}


