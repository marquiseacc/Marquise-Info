document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("لطفاً ابتدا وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    fetch('/CRM/Account/AccountDetail', {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + token
        }
    })
        .then(response => {
            if (!response.ok) {
                console.error("❌ خطا:", error);
                const container = document.getElementById("account-detail-container");

                container.innerHTML = `
        <div class="card h-100 shadow-sm border-danger">
            <div class="card-body text-center">
                <img src="/Content/Images/error-page.png" alt="صفحه خطا" class="img-fluid mb-3" style="max-width: 200px;" />
                <h6 class="mb-2">خطا در دریافت اطلاعات حساب</h6>
                <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات پیش آمده است. لطفاً دوباره تلاش کنید.</p>
            </div>
        </div>
    `;
           
            }
            return response.text();
        })
        .then(html => {
            document.getElementById("account-detail-container").innerHTML = html;
        })
        .catch(error => {
            console.error("❌ خطا:", error);
            const container = document.getElementById("account-detail-container");

            container.innerHTML = `
        <div class="card h-100 shadow-sm border-danger">
            <div class="card-body text-center">
                <img src="/Content/Images/error-page.png" alt="صفحه خطا" class="img-fluid mb-3" style="max-width: 200px;" />
                <h6 class="mb-2">خطا در دریافت اطلاعات حساب</h6>
                <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات پیش آمده است. لطفاً دوباره تلاش کنید.</p>
            </div>
        </div>
    `;
        });

});

function handleUpdateAccountFormSubmit(event) {
    event.preventDefault();

    const token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("توکن یافت نشد. لطفاً دوباره وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    const form = event.target;
    const inputs = form.querySelectorAll("input");

    let isValid = true;
    inputs.forEach(input => {
        input.setCustomValidity("");

        if (!input.value.trim()) {
            input.setCustomValidity(`لطفاً فیلد "${input.name}" را پر کنید.`);
            input.reportValidity();
            isValid = false;
        } else {
            const customMessage = getCustomErrorMessage(input);
            if (customMessage) {
                input.setCustomValidity(customMessage);
                input.reportValidity();
                isValid = false;
            }
        }
    });

    if (!isValid) return;

    const data = {
        Name: form.name.value.trim(),
        IndustryCode: form.IndustryCode.value.trim(),
        ShippingAddress: form.shippingAddress.value.trim(),
        mahale__C: form.mahale.value.trim(),
        cituu__C: form.city.value.trim(),
        Telephone: form.phone.value.trim(),
        Mobile: form.mobile.value.trim()
    };

    fetch('/api/CRM/AccountApi/UpdateAccount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(result => {
            if (result?.IsSuccess) {
                Swal.fire({
                    title: 'موفق',
                    text: result.Message || 'بروزرسانی با موفقیت انجام شد.',
                    icon: 'success',
                    confirmButtonText: 'باشه'
                }).then(() => {
                    window.location.href = '/CRM/Account/Index';
                });
            } else {
                Swal.fire({
                    title: 'خطا',
                    text: result?.Message || 'بروزرسانی انجام نشد.',
                    icon: 'error',
                    confirmButtonText: 'باشه'
                });
            }
        })
        .catch(error => {
            console.error("❌ خطا:", error);
            Swal.fire({
                title: 'خطا',
                text: 'مشکلی در ارتباط با سرور رخ داده است.',
                icon: 'error',
                confirmButtonText: 'باشه'
            });
        });
}

function getCustomErrorMessage(input) {
    if (input.validity.typeMismatch) {
        return `لطفاً "${input.name}" را درست وارد کنید.`;
    }
    if (input.validity.patternMismatch) {
        return `الگوی واردشده برای "${input.name}" نادرست است.`;
    }
    return "";
}
