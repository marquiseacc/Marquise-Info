document.addEventListener('DOMContentLoaded', function () {
    // گرفتن همه ردیف‌های جدول
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        // گرفتن اولین ردیف
        const firstRow = rows[0];
        const firstQuoteId = firstRow.getAttribute('data-quote-id');

        // فراخوانی دیتیل برای اولین ردیف
        loadQuoteDetail(firstQuoteId);
    }

    // افزودن رویداد کلیک برای هر ردیف
    rows.forEach(row => {
        row.addEventListener('click', function () {
            const quoteId = this.getAttribute('data-quote-id');
            loadQuoteDetail(quoteId);
        });
    });

    // تابع مشترک برای دریافت دیتیل
    function loadQuoteDetail(quoteId) {
        fetch('/CRM/PreInvoice/Detail?quoteId=' + quoteId, {
            method: 'GET'
        })
            .then(response => response.text())
            .then(data => {
                document.querySelector('#DetailQuote').innerHTML = data;
            })
            .catch(error => {
                console.error('خطا در بارگذاری جزئیات:', error);
            });
    }
});
