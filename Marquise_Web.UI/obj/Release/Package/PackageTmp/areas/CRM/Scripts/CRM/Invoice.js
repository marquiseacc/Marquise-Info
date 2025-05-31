document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    fetch('/CRM/Invoice/InvoiceList', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    })
        .then(response => {
            if (response.status === 401 || response.status === 403) {
                console.error(`❌ خطا در احراز هویت (${response.status}): دسترسی غیرمجاز یا توکن نامعتبر است.`);
                return '';
            }

            if (!response.ok) {
                console.error(`❌ خطای ناشناخته (${response.status}) هنگام دریافت اطلاعات.`);
                return '';
            }

            return response.text();
        })
        .then(data => {
            if (data) {
                document.getElementById('invoice-list').innerHTML = data;

                // انتخاب ردیف‌ها بعد از بارگذاری دیتا
                const rows = document.querySelectorAll("#invoice-list table tbody tr");

                if (rows.length > 0) {
                    const firstRow = rows[0];
                    const firstInvoiceId = firstRow.getAttribute('data-invoice-id');
                    firstRow.classList.add('selected-row');
                    loadInvoiceDetail(firstInvoiceId);
                }

                rows.forEach(row => {
                    row.addEventListener('click', function () {
                        rows.forEach(r => r.classList.remove('selected-row'));
                        this.classList.add('selected-row');

                        const invoiceId = this.getAttribute('data-invoice-id');
                        loadInvoiceDetail(invoiceId);
                    });
                });
            }
        })
        .catch(error => {
            console.error('❌ خطا در ارسال درخواست:', error);
        });

    function loadInvoiceDetail(invoiceId) {
        fetch('/CRM/Invoice/Detail?invoiceId=' + invoiceId, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("خطا در دریافت جزئیات صورتحساب");
                }
                return response.text();
            })
            .then(data => {
                document.querySelector('#DetailInvoice').innerHTML = data;
            })
            .catch(error => {
                console.error('❌ خطا در بارگذاری جزئیات:', error);
            });
    }
});
