function handleUpdateAccountFormSubmit(event) {
    event.preventDefault();

    var name = document.getElementById("name").value;
    //var managementName = document.getElementById("managementName").value;
    var industryCode = document.getElementById("IndustryCode").value;
    console.log(industryCode);
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
            /*ManagementName: managementName,*/
            IndustryCode: industryCode,
            ShippingAddress: shippingAddress,
            mahale__C: mahale,
            cituu__C: city,
            Telephone: phone,
            Mobile: mobile
        }

        fetch('/api/CRM/AccountApi/UpdateAccount', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
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
                        const redirectUrl = '/CRM/Account/Index';
                        window.location.href = redirectUrl;
                    });
                } else {
                    alert(data.message || "خطا! لطفا مجددا تلاش کنید.", 'error');
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
        return "";
    }
}


