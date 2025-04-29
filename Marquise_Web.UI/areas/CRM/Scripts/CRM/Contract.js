document.addEventListener('DOMContentLoaded', function () {
    // گرفتن همه ردیف‌های جدول
    const rows = document.querySelectorAll("table tbody tr");

    if (rows.length > 0) {
        // گرفتن اولین ردیف
        const firstRow = rows[0];
        const firstContractId = firstRow.getAttribute('data-contract-id');

        // فراخوانی دیتیل برای اولین ردیف
        loadQuoteDetail(firstContractId);
    }

    // افزودن رویداد کلیک برای هر ردیف
    rows.forEach(row => {
        row.addEventListener('click', function () {
            const contractId = this.getAttribute('data-contract-id');
            loadQuoteDetail(contractId);
        });
    });

    // تابع مشترک برای دریافت دیتیل
    function loadQuoteDetail(contractId) {
        fetch('/CRM/Contract/Detail?contractId=' + contractId, {
            method: 'GET'
        })
            .then(response => response.text())
            .then(data => {
                document.querySelector('#DetailContract').innerHTML = data;
            })
            .catch(error => {
                console.error('خطا در بارگذاری جزئیات:', error);
            });
    }
});
