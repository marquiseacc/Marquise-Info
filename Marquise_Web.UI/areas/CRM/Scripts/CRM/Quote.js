document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        const firstRow = rows[0];
        const firstQuoteId = firstRow.getAttribute('data-quote-id');
        firstRow.classList.add('selected-row'); // افزودن کلاس به اولین ردیف
        loadQuoteDetail(firstQuoteId);
    }

    rows.forEach(row => {
        row.addEventListener('click', function () {
            // حذف کلاس انتخاب از همه ردیف‌ها
            rows.forEach(r => r.classList.remove('selected-row'));

            // افزودن کلاس به ردیف کلیک‌شده
            this.classList.add('selected-row');

            const quoteId = this.getAttribute('data-quote-id');
            loadQuoteDetail(quoteId);
        });
    });

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
