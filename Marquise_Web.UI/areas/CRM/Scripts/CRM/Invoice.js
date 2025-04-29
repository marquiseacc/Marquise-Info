document.addEventListener('DOMContentLoaded', function () {
    // گرفتن همه ردیف‌های جدول
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        // گرفتن اولین ردیف
        const firstRow = rows[0];
        const firstQuoteId = firstRow.getAttribute('data-invoice-id');

        // فراخوانی دیتیل برای اولین ردیف
        loadQuoteDetail(firstQuoteId);
    }

    // افزودن رویداد کلیک برای هر ردیف
    rows.forEach(row => {
        row.addEventListener('click', function () {
            const invoiceId = this.getAttribute('data-invoice-id');
            loadQuoteDetail(quoteId);
        });
    });

    // تابع مشترک برای دریافت دیتیل
    function loadQuoteDetail(invoiceId) {
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
