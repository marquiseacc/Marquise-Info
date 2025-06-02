document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem("jwtToken");

    if (!token) {
        alert("لطفاً وارد شوید.");
        window.location.href = "/CRM/Auth/SendOtp";
        return;
    }

    // دریافت لیست صورتحساب‌ها با نمایش لودر ساده (در #invoice-list)
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
        }, '#DetailInvoice');
    }
});
