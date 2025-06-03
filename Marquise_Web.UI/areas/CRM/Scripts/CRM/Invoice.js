document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("لطفاً وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    fetchWithLoading('/CRM/Invoice/InvoiceList', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    }, '#invoice-list')
        .then(data => {
            if (!data) return;

            const invoiceListContainer = document.getElementById('invoice-list');
            const rows = invoiceListContainer.querySelectorAll("table tbody tr");

            if (rows.length > 0) {
                selectRow(rows[0]);
            }

            rows.forEach(row => {
                row.addEventListener('click', () => selectRow(row));
            });
        })
        .catch(error => {
            console.error('❌ خطا در دریافت لیست صورتحساب:', error);
            const container = document.getElementById('invoice-list');
            if (container) {
                container.innerHTML = `
            <div class="card h-100 shadow-sm border-danger">
                <div class="card-body text-center">
                    <img src="/Content/Images/error-page.png" alt="خطای صورتحساب" class="img-fluid mb-3" style="max-width: 200px;" />
                    <h6 class="mb-2">خطا در دریافت لیست صورتحساب</h6>
                    <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات صورتحساب‌ها پیش آمده است. لطفاً دوباره تلاش کنید.</p>
                </div>
            </div>
        `;
            }
        });

    function selectRow(row) {
        const rows = document.querySelectorAll("#invoice-list table tbody tr");
        rows.forEach(r => r.classList.remove('selected-row'));

        row.classList.add('selected-row');
        const invoiceId = row.getAttribute('data-invoice-id');

        loadInvoiceDetail(invoiceId);
    }

    function loadInvoiceDetail(invoiceId) {
        fetchWithLoading(`/CRM/Invoice/Detail?invoiceId=${encodeURIComponent(invoiceId)}`, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token
            }
        }, '#DetailInvoice')
            .catch(error => {
                console.error('❌ خطا در دریافت جزئیات صورتحساب:', error);
                const container = document.getElementById('DetailInvoice');
                if (container) {
                    container.innerHTML = `
                <div class="card h-100 shadow-sm border-danger">
                    <div class="card-body text-center">
                        <img src="/Content/Images/error-page.png" alt="خطای صورتحساب" class="img-fluid mb-3" style="max-width: 200px;" />
                        <h6 class="mb-2">خطا در دریافت جزئیات صورتحساب</h6>
                        <p class="font-13">متأسفانه مشکلی در بارگیری اطلاعات صورتحساب پیش آمده است. لطفاً دوباره تلاش کنید.</p>
                    </div>
                </div>
            `;
                }
            });
    }
});
