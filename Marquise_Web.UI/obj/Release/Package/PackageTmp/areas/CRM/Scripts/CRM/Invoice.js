document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        const firstRow = rows[0];
        const firstInvoiceId = firstRow.getAttribute('data-invoice-id');
        firstRow.classList.add('selected-row'); // رنگ دادن به اولین ردیف
        loadInvoiceDetail(firstInvoiceId);
    }

    rows.forEach(row => {
        row.addEventListener('click', function () {
            // حذف رنگ قبلی از تمام ردیف‌ها
            rows.forEach(r => r.classList.remove('selected-row'));

            // رنگ دادن به ردیف کلیک‌شده
            this.classList.add('selected-row');

            const invoiceId = this.getAttribute('data-invoice-id');
            loadInvoiceDetail(invoiceId);
        });
    });

    function loadInvoiceDetail(invoiceId) {
        fetch('/CRM/Invoice/Detail?invoiceId=' + invoiceId, {
            method: 'GET'
        })
            .then(response => response.text())
            .then(data => {
                document.querySelector('#DetailInvoice').innerHTML = data;
            })
            .catch(error => {
                console.error('خطا در بارگذاری جزئیات:', error);
            });
    }
});
